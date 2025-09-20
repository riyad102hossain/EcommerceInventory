using EcommerceInventory.Application.DTOs.Category;

namespace EcommerceInventory.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> CreateAsync(string name, string? description);
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(int id, string name, string? description);
        Task<bool> DeleteAsync(int id);
    }
}
