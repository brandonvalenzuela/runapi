using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly MyDbContext _dbContext;
    
    public SubscriptionRepository(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Subscription?> VerifySubscription(int userId)
    {
        return await _dbContext.Subscriptions
            .Where(s => s!.UserId == userId && s.IsActive && s.EndDate >= DateTime.UtcNow)
            .FirstOrDefaultAsync();
    }
    
    // 1) Verifica si el usuario ya usó el FreeTrial
    //    Si el usuario está intentando crear FreeTrial de nuevo, no se lo permites.
    

    public async Task AddSubscriptionAsync(Subscription subscription)
    {
        _dbContext.Subscriptions.Add(subscription);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Subscription> GetSubscriptionAsync(int subscriptionId)
    {
        return await _dbContext.Subscriptions
            .FirstOrDefaultAsync(s => s.SubscriptionId == subscriptionId);
    }

    public async Task UpadateSubscriptionAsync(Subscription subscription)
    {
        _dbContext.Subscriptions.Update(subscription);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Subscription> ActivateSubscriptionAsync(int userId, DateTime expiration)
    {
        return await _dbContext.Subscriptions
            .Where(s => s.UserId == userId && s.IsActive && s.EndDate >= expiration)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> VerifyFreeTrialHistorySubscription(int userId, SubscriptionType subscriptionType)
    {
        return await _dbContext.Subscriptions.AnyAsync(s =>
            s.UserId == userId &&
            (s.SubscriptionHistory.Contains(SubscriptionType.FreeTrial) // o s.Type == SubscriptionType.FreeTrial
             || s.Type == SubscriptionType.FreeTrial)
            );
    }
}