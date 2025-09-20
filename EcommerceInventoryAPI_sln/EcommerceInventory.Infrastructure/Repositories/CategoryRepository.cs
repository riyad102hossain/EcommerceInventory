using EcommerceInventory.Domain.Entities;
using EcommerceInventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInventory.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext ctx) : base(ctx) { }

        public async Task<bool> HasProductsAsync(int categoryId) =>
            await _context.Products.AnyAsync(p => p.CategoryId == categoryId);

        public async Task<IEnumerable<(Category category, int productCount)>> GetAllWithCountsAsync()
        {
            var list = await _context.Categories
                .Include(c => c.Products)
                .AsNoTracking()
                .ToListAsync();

            return list.Select(c => (c, c.Products?.Count ?? 0));
        }
    }
}
