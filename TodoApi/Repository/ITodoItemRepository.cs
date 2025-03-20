using TodoApi.Models;

namespace TodoApi.Repository
{
  public interface ITodoItemRepository
  {
    Task<IEnumerable<TodoItem>> GetAllAsync(TodoItemsFilter filter);
    Task<TodoItem?> GetAsync(long id);
    Task<TodoItem> AddAsync(TodoItem item);
    Task UpdateAsync(TodoItem item);
    Task DeleteAsync(TodoItem item);
  }
}
