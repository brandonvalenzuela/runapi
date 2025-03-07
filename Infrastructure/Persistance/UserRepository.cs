using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistance
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _dbContext;

        public UserRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbContext.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> VerifyPassword(string password)
        {
            return await _dbContext.Users.AnyAsync(u => u.PasswordHash == password);
        }
    }
}
