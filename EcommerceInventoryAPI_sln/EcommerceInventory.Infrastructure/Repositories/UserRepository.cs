using EcommerceInventory.Domain.Entities;
using EcommerceInventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInventory.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext ctx) : base(ctx) { }

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }
}
