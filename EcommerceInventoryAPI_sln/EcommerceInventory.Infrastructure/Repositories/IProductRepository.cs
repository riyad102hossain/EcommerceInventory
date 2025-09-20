using EcommerceInventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceInventory.Infrastructure.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<(IEnumerable<Product> items, int total)> GetPagedAsync(
            int page, int limit, int? categoryId, decimal? minPrice, decimal? maxPrice, string? q);
        Task<Product?> GetWithCategoryAsync(int id);
    }
}
