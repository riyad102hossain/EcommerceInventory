using EcommerceInventory.Domain.Entities;
using EcommerceInventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EcommerceInventory.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext ctx) : base(ctx) { }

        public async Task<Product?> GetWithCategoryAsync(int id) =>
            await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

        public async Task<(IEnumerable<Product> items, int total)> GetPagedAsync(
            int page, int limit, int? categoryId, decimal? minPrice, decimal? maxPrice, string? q)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (categoryId.HasValue) query = query.Where(p => p.CategoryId == categoryId.Value);
            if (minPrice.HasValue) query = query.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue) query = query.Where(p => p.Price <= maxPrice.Value);
            if (!string.IsNullOrWhiteSpace(q))
            {
                var kw = q.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(kw) || p.Description.ToLower().Contains(kw));
            }

            var total = await query.CountAsync();
            var items = await query
                .OrderByDescending(p => p.Id)
                .Skip((page - 1) * limit)
                .Take(limit)
                .AsNoTracking()
                .ToListAsync();

            return (items, total);
        }
    }
}
