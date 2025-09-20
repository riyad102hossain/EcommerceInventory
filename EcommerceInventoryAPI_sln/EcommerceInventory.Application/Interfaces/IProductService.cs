using EcommerceInventory.Application.DTOs.Product;

namespace EcommerceInventory.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> CreateAsync(CreateProductDto dto);
        Task<(IEnumerable<ProductDto> items, int total)> GetAllAsync(int page, int limit, int? categoryId, decimal? minPrice, decimal? maxPrice, string? q);
        Task<ProductDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, CreateProductDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
