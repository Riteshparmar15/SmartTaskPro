using Microsoft.EntityFrameworkCore;
using SmartTaskPro.Data;
using SmartTaskPro.Models;

namespace SmartTaskPro.Repositories
{
    public interface ITaskRepository : IRepository<TaskItem>
    {
        Task<List<TaskItem>> GetForUserAsync(int userId);
    }

    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;
        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<TaskItem> Query() => _context.Tasks.Include(t => t.AssignedToUser).AsQueryable();

        public async Task<TaskItem> GetByIdAsync(int id) => await _context.Tasks.Include(t => t.AssignedToUser).FirstOrDefaultAsync(t => t.Id == id);

        public async Task AddAsync(TaskItem entity) => await _context.Tasks.AddAsync(entity);

        public void Update(TaskItem entity) => _context.Tasks.Update(entity);

        public void Remove(TaskItem entity) => _context.Tasks.Remove(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<List<TaskItem>> GetForUserAsync(int userId)
        {
            return await _context.Tasks.Where(t => t.AssignedToUserId == userId).ToListAsync();
        }
    }
}
