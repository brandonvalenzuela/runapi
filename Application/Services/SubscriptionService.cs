using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Domain.Services;

namespace Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionService(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }
        
        public async Task<Subscription> CreateSubscriptionAsync(int userId, SubscriptionType type, SubscriptionPeriod period)
        {
            // 1) Verificar si el usuario ya tiene alguna suscripción activa
            var existingActive = await _subscriptionRepository.VerifySubscription(userId);
            
            if (existingActive != null)
            {
                throw new InvalidOperationException("El usuario ya tiene una suscripción activa");
            }
            
            // 2) Verificar si el usuario ya uso el freeTrial
            if (type == SubscriptionType.FreeTrial)
            {
                if (await _subscriptionRepository.VerifyFreeTrialHistorySubscription(userId, type))
                {
                    throw new InvalidOperationException("Ya usaste tu periodo de prueba gratuito.");
                }
            }
            
            // 3) Calcular el precio de la Subscription 
            var price = CalculatePrice(type, period);
            
            // 4) Calcular fecha de inicio y fin
            var startDate = DateTime.UtcNow;
            
            var endDate = CalculateEndDate(startDate, period);
            
            // 5) Crear la suscripción
            var subscription = new Subscription(
                userId: userId,
                type: type,
                price: price,
                startDate: startDate,
                endDate: endDate,
                isActive: true
            );
    
            await _subscriptionRepository.AddSubscriptionAsync(subscription);
    
            return subscription;
        }
    
        public async Task<Subscription> RenewSubscriptionAsync(int subscriptionId, SubscriptionType newType, SubscriptionPeriod period)
        {
            // 1) Obtener la suscripción
            var subscription =   await _subscriptionRepository.GetSubscriptionAsync(subscriptionId);
            
            // 2) Calcular el precio de la Subscription 
            var newPrice = CalculatePrice(newType, period);
            
            if (subscription == null)
            {
                throw new KeyNotFoundException("La suscripción no existe.");
            }
    
            if (!subscription.IsActive || subscription.EndDate < DateTime.UtcNow)
            {
                // Podrías permitir renovar igual, dependiendo de la lógica
                // o forzar la creación de una nueva suscripción
                // Aqui lo permitimos y reactivamos
            }
    
            // 2) Calcular nueva fecha de fin
            var newEndDate = subscription.EndDate >= DateTime.UtcNow
                ? CalculateEndDate(subscription.EndDate, period)
                : CalculateEndDate(DateTime.UtcNow, period);
    
            // 3) Actualizar la suscripción
            subscription.RenewSubscription(newType, newPrice, newEndDate);
    
            await _subscriptionRepository.UpadateSubscriptionAsync(subscription);
    
            return subscription;
        }
    
        public async Task CancelSubscriptionAsync(int subscriptionId)
        {
            var subscription = await _subscriptionRepository.GetSubscriptionAsync(subscriptionId);
    
            if (subscription == null)
            {
                throw new KeyNotFoundException("La suscripción no existe.");
            }
    
            //subscription.IsActive = false; // Si la deseas "cancelar" con efecto inmediato:
            subscription.EndDate = DateTime.UtcNow; // O en la siguiente fecha de corte
            
            await _subscriptionRepository.UpadateSubscriptionAsync(subscription);
        }
    
        public async Task<Subscription?> GetActiveSubscriptionAsync(int userId)
        {
            var now = DateTime.UtcNow;
    
            return await _subscriptionRepository.ActivateSubscriptionAsync(userId, now); 
                
        }
    
        public async Task<bool> HasActiveSubscriptionAsync(int userId)
        {
            var sub = await GetActiveSubscriptionAsync(userId);
            return sub != null;
        }
        
        private DateTime CalculateEndDate(DateTime start, SubscriptionPeriod period)
        {
            return period switch
            {
                SubscriptionPeriod.OneMonth    => start.AddMonths(1),
                SubscriptionPeriod.ThreeMonths => start.AddMonths(3),
                SubscriptionPeriod.SixMonths   => start.AddMonths(6),
                SubscriptionPeriod.OneYear     => start.AddYears(1),
                _ => throw new NotImplementedException($"No se ha implementado la duración para {period}.")
            };
        }
        
        private decimal CalculatePrice(SubscriptionType type, SubscriptionPeriod period)
        {
            // 1) Partir de un precio base según el "type"
            var basePrice = type switch
            {
                SubscriptionType.Premium => 10.0m,  // Ejemplo: 10 USD al mes
                SubscriptionType.ContentCreator => 15.0m,
                // Freemium -> 0 o se ignora la duracion
                SubscriptionType.Freemium => 0.0m,
                // FreeTrial -> 0, asumiendo 7 días gratis
                SubscriptionType.FreeTrial => 0.0m,
                _ => 0.0m  // Por defecto
            };

            // 2) Ajustar el precio en función del "period"
            // (Ejemplo: si es OneYear, un descuento, etc.)
            // Por ejemplo, mes a mes 10, 3 meses a 28 (ahorras 2), 6 meses a 50, 1 año 90, etc.
            var finalPrice = period switch
            {
                SubscriptionPeriod.OneMonth    => basePrice,
                SubscriptionPeriod.ThreeMonths => basePrice * 3 * 0.95m,   // un 5% de descuento
                SubscriptionPeriod.SixMonths   => basePrice * 6 * 0.90m,   // 10% de descuento
                SubscriptionPeriod.OneYear     => basePrice * 12 * 0.75m,  // 25% de descuento
                _ => basePrice
            };
            
            return finalPrice;
        }

    }
}
