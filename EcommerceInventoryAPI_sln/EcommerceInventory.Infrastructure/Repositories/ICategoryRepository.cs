using EcommerceInventory.Domain.Entities;

namespace EcommerceInventory.Infrastructure.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<bool> HasProductsAsync(int categoryId);
        Task<IEnumerable<(Category category, int productCount)>> GetAllWithCountsAsync();
    }
}
