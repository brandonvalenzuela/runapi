using Domain.Entities;

namespace Application.Interfaces;

public interface ISubscriptionService
{
    Task<Subscription> CreateSubscriptionAsync(int userId, SubscriptionType type, SubscriptionPeriod period);
    Task<Subscription> RenewSubscriptionAsync(int subscriptionId, SubscriptionType newType, SubscriptionPeriod period);
    Task CancelSubscriptionAsync(int subscriptionId);
    Task<Subscription?> GetActiveSubscriptionAsync(int userId);
}