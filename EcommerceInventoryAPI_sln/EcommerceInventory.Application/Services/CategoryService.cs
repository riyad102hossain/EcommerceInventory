using EcommerceInventory.Application.DTOs.Category;
using EcommerceInventory.Application.Interfaces;
using EcommerceInventory.Domain.Entities;
using EcommerceInventory.Infrastructure.UnitOfWork;

namespace EcommerceInventory.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;

        public CategoryService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<CategoryDto> CreateAsync(string name, string? description)
        {
            // ensure unique
            var exists = (await _uow.Categories.FindAsync(c => c.Name.ToLower() == name.ToLower())).FirstOrDefault();
            if (exists != null) throw new InvalidOperationException("Category with same name exists.");

            var cat = new Category { Name = name, Description = description ?? "" };
            await _uow.Categories.AddAsync(cat);
            await _uow.CommitAsync();

            return new CategoryDto { Id = cat.Id, Name = cat.Name, Description = cat.Description, ProductCount = 0 };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var c = await _uow.Categories.GetByIdAsync(id);
            if (c == null) return false;
            var has = await _uow.Categories.HasProductsAsync(id);
            if (has) throw new InvalidOperationException("Category has linked products.");

            _uow.Categories.Remove(c);
            await _uow.CommitAsync();
            return true;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var list = await _uow.Categories.GetAllWithCountsAsync();
            return list.Select(x => new CategoryDto
            {
                Id = x.category.Id,
                Name = x.category.Name,
                Description = x.category.Description,
                ProductCount = x.productCount
            });
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var c = await _uow.Categories.GetByIdAsync(id);
            if (c == null) return null;
            var count = await _uow.Categories.HasProductsAsync(id) ? (c.Products?.Count ?? 0) : (c.Products?.Count ?? 0);
            return new CategoryDto { Id = c.Id, Name = c.Name, Description = c.Description, ProductCount = count };
        }

        public async Task<bool> UpdateAsync(int id, string name, string? description)
        {
            var c = await _uow.Categories.GetByIdAsync(id);
            if (c == null) return false;

            // check uniqueness
            var same = (await _uow.Categories.FindAsync(x => x.Name.ToLower() == name.ToLower() && x.Id != id)).FirstOrDefault();
            if (same != null) throw new InvalidOperationException("Category name conflict.");

            c.Name = name;
            c.Description = description ?? "";
            _uow.Categories.Update(c);
            await _uow.CommitAsync();
            return true;
        }
    }
}
