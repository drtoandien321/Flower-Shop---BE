using AnnFlowerProject.Models;

namespace AnnFlowerProject.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdWithRoleAsync(int id);
        Task<IEnumerable<User>> GetUsersByRoleAsync(int roleId);
        Task<bool> IsEmailExistsAsync(string email);
    }
}
