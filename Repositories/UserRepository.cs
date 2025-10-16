using Microsoft.EntityFrameworkCore;
using SmartTaskPro.Data;
using SmartTaskPro.Models;

namespace SmartTaskPro.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmail(string email);
    }
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public IQueryable<User> Query() => _context.Users.AsQueryable();
        public async Task<User?> GetByIdAsync(int id) => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        public async Task AddAsync(User entity) => await _context.Users.AddAsync(entity);
        public void Update(User entity) => _context.Users.Update(entity);
        public void Remove(User entity) => _context.Users.Remove(entity);
        public Task SaveChangesAsync() => _context.SaveChangesAsync();
        public Task<User> GetByEmail(string email) => _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
