using AnnFlowerProject.Data;
using AnnFlowerProject.Repositories.Implementations;
using AnnFlowerProject.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace AnnFlowerProject.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FlowerShopDbContext _context;
        private IDbContextTransaction? _transaction;
        private Dictionary<Type, object>? _repositories;

        public IProductRepository Products { get; }
        public ICategoryRepository Categories { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(
            FlowerShopDbContext context,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository)
        {
            _context = context;
            Products = productRepository;
            Categories = categoryRepository;
            Users = userRepository;
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            _repositories ??= new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);
                var repositoryInstance = Activator.CreateInstance(
                    repositoryType.MakeGenericType(typeof(TEntity)), _context);
                
                if (repositoryInstance != null)
                {
                    _repositories.Add(type, repositoryInstance);
                }
            }

            return (IRepository<TEntity>)_repositories[type];
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                if (_transaction != null)
                {
                    await _transaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    await _transaction.DisposeAsync();
                    _transaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
