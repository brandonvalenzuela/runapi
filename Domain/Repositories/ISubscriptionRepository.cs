using Domain.Entities;

namespace Domain.Repositories;

public interface ISubscriptionRepository
{
    Task<Subscription?> VerifySubscription(int userId);
    Task AddSubscriptionAsync(Subscription subscription);
    Task<Subscription> GetSubscriptionAsync(int subscriptionId);
    Task UpadateSubscriptionAsync(Subscription subscription);
    Task<Subscription> ActivateSubscriptionAsync(int userId, DateTime expiration);
    Task<bool> VerifyFreeTrialHistorySubscription(int userId, SubscriptionType subscriptionType);
}