using EcommerceInventory.Infrastructure.Data;
using EcommerceInventory.Infrastructure.Repositories;

namespace EcommerceInventory.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _ctx;
        private ProductRepository? _productRepo;
        private CategoryRepository? _categoryRepo;
        private UserRepository? _userRepo;

        public UnitOfWork(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public IProductRepository Products => _productRepo ??= new ProductRepository(_ctx);
        public ICategoryRepository Categories => _categoryRepo ??= new CategoryRepository(_ctx);
        public IUserRepository Users => _userRepo ??= new UserRepository(_ctx);

        public async Task<int> CommitAsync() => await _ctx.SaveChangesAsync();

        public void Dispose() => _ctx.Dispose();
    }
}
