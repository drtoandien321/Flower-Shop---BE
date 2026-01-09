using AnnFlowerProject.Repositories;

namespace AnnFlowerProject.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // Specific Repositories
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IUserRepository Users { get; }
        
        // Generic Repository for other entities
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        
        // Transaction Management
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
