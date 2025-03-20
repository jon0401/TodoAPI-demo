using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Repository
{
  public class TodoItemRepository : ITodoItemRepository
  {
    private readonly TodoContext _context;

    public TodoItemRepository(TodoContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<TodoItem>> GetAllAsync(TodoItemsFilter filter)
    {
      var query = _context.TodoItems.AsQueryable();
      query = query.ApplyFilter(filter);
      query = query.ApplySorting(filter);
      return await query.ToListAsync();
    }

    public async Task<TodoItem?> GetAsync(long id)
    {
      return await _context.TodoItems.FindAsync(id);
    }

    public async Task<TodoItem> AddAsync(TodoItem item)
    {
      _context.TodoItems.Add(item);
      await _context.SaveChangesAsync();
      return item;
    }

    public async Task UpdateAsync(TodoItem item)
    {
      // With EF Core, tracked entities are automatically updated.
      await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TodoItem item)
    {
      _context.TodoItems.Remove(item);
      await _context.SaveChangesAsync();
    }
  }
}
