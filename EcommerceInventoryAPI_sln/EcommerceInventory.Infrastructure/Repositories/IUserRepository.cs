using EcommerceInventory.Domain.Entities;

namespace EcommerceInventory.Infrastructure.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
    }
}
