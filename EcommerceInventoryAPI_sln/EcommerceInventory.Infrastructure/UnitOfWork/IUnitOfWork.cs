using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceInventory.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Repositories.IProductRepository Products { get; }
        Repositories.ICategoryRepository Categories { get; }
        Repositories.IUserRepository Users { get; }
        Task<int> CommitAsync();
    }
}
