using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int userId);
        Task<User> GetByEmailAsync(string email);
        Task AddAsync(User user);
        Task<bool> VerifyPassword(string password);
        Task<bool> ExistsByEmailAsync(string email);

    }
}
