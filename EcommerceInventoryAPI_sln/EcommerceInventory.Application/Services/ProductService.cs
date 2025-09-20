using EcommerceInventory.Application.DTOs.Product;
using EcommerceInventory.Application.Interfaces;
using EcommerceInventory.Domain.Entities;
using EcommerceInventory.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Hosting;

namespace EcommerceInventory.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _env;

        public ProductService(IUnitOfWork uow, IWebHostEnvironment env)
        {
            _uow = uow;
            _env = env;
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            // save image if base64 provided
            string? imagePath = null;
            if (!string.IsNullOrWhiteSpace(dto.ImageBase64))
            {
                var bytes = Convert.FromBase64String(dto.ImageBase64!);
                var fileName = $"{Guid.NewGuid()}.jpg";
                var folder = Path.Combine(_env.ContentRootPath, "uploads");
                Directory.CreateDirectory(folder);
                var full = Path.Combine(folder, fileName);
                await File.WriteAllBytesAsync(full, bytes);
                imagePath = $"/uploads/{fileName}";
            }

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description ?? "",
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId,
                ImagePath = imagePath
            };

            await _uow.Products.AddAsync(product);
            await _uow.CommitAsync();

            return MapToDto(product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var p = await _uow.Products.GetByIdAsync(id);
            if (p == null) return false;
            _uow.Products.Remove(p);
            await _uow.CommitAsync();
            return true;
        }

        public async Task<(IEnumerable<ProductDto> items, int total)> GetAllAsync(int page, int limit, int? categoryId, decimal? minPrice, decimal? maxPrice, string? q)
        {
            var (items, total) = await _uow.Products.GetPagedAsync(page, limit, categoryId, minPrice, maxPrice, q);
            var dtos = items.Select(MapToDto);
            return (dtos, total);
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var p = await _uow.Products.GetWithCategoryAsync(id);
            if (p == null) return null;
            return MapToDto(p);
        }

        public async Task<bool> UpdateAsync(int id, CreateProductDto dto)
        {
            var p = await _uow.Products.GetByIdAsync(id);
            if (p == null) return false;

            p.Name = dto.Name;
            p.Description = dto.Description ?? "";
            p.Price = dto.Price;
            p.Stock = dto.Stock;
            p.CategoryId = dto.CategoryId;

            // ignore image update for brevity; could handle base64 similarly
            _uow.Products.Update(p);
            await _uow.CommitAsync();
            return true;
        }

        private ProductDto MapToDto(Product p) =>
            new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name,
                ImageUrl = p.ImagePath
            };
    }
}
