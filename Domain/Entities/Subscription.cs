
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    [Table("Subscriptions")]  // Nombre de la tabla en la base de datos
    public class Subscription
    {
        [Key]
        public int SubscriptionId { get; set; }           // Identificador único de la suscripción

        [ForeignKey("User")]
        public int UserId { get; set; }                   // Llave foránea a Usuario

        public SubscriptionType Type { get; set; }        // Enum o tipo para identificar el tipo de suscripción
        
        public SubscriptionPeriod Period { get; set; }    // Enum o tipo para identificar el periodo de suscripción

        public decimal Price { get; set; }                // Precio según el tipo de suscripción

        public DateTime StartDate { get; set; }           // Fecha de inicio de la suscripción

        public DateTime EndDate { get; set; }             // Fecha de fin de la suscripción

        public bool IsActive { get; set; }                // Estado de la suscripción (Activa, Expirada, etc.)

        // Lleva el conteo de cuántas veces el usuario se ha suscrito
        public int SubscriptionCount { get; set; }

        // Historial de tipos de suscripción
        public List<SubscriptionType> SubscriptionHistory { get; set; } = new List<SubscriptionType>();

        // Navegación
        public User? User { get; set; }

        // Constructor
        public Subscription()
        {
            // Constructor vacío necesario para Entity Framework
        }

        public Subscription(int userId, SubscriptionType type, decimal price, DateTime startDate, DateTime endDate, bool isActive, User? user = null)
        {
            UserId = userId;
            Type = type;
            Price = price;
            StartDate = startDate;
            EndDate = endDate;
            IsActive = isActive;
            User = user;
            SubscriptionCount = 1;   // Se inicializa el conteo en 1
            SubscriptionHistory.Add(type); // Se agrega el tipo actual al historial
        }

        // Método para actualizar suscripciones
        public void RenewSubscription(SubscriptionType newType, decimal newPrice, DateTime newEndDate)
        {
            // Actualiza el tipo, precio y fecha de fin
            Type = newType;
            Price = newPrice;
            EndDate = newEndDate;
            IsActive = true;

            // Incrementa el conteo de suscripciones
            SubscriptionCount++;

            // Agrega el nuevo tipo de suscripción al historial
            SubscriptionHistory.Add(newType);
        }
    }

    // Enum para definir los tipos de suscripción
    [Flags]
    public enum SubscriptionType
    {

        Freemium,       // Gratis con funcionalidades limitadas
        FreeTrial,      // Prueba gratuita de 7 dias
        Premium,     // De pago
        Discounted,     // De pago con descuento
        ContentCreator, // Para creadores
    }
    
    public enum SubscriptionPeriod
    {
        OneMonth,
        ThreeMonths,
        SixMonths,
        OneYear
    }
}

