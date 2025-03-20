using TodoApi.Models;

namespace TodoApi.Services
{
  public interface ITodoItemService
  {
    Task<IEnumerable<TodoItemDTO>> GetAllAsync(TodoItemsFilter filter);
    Task<TodoItemDTO?> GetAsync(long id);
    Task<TodoItemDTO> CreateAsync(TodoItemDTO dto);
    Task UpdateAsync(long id, TodoItemDTO dto);
    Task DeleteAsync(long id);
  }
}
