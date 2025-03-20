using TodoApi.Models;
using TodoApi.Repository;

namespace TodoApi.Services
{
  public class TodoItemService : ITodoItemService
  {
    private readonly ITodoItemRepository _repository;
    private readonly IUserRepository _userRepository;

    public TodoItemService(ITodoItemRepository repository, IUserRepository userRepository)
    {
      _repository = repository;
      _userRepository = userRepository;
    }

    public async Task<IEnumerable<TodoItemDTO>> GetAllAsync(TodoItemsFilter filter)
    {
      var items = await _repository.GetAllAsync(filter);
      return items.Select(t => MapToDTO(t));
    }

    public async Task<TodoItemDTO?> GetAsync(long id)
    {
      var item = await _repository.GetAsync(id);
      return item == null ? null : MapToDTO(item);
    }

    public async Task<TodoItemDTO> CreateAsync(TodoItemDTO dto)
    {
      // Verify that the user (owner) exists before creating the Todo item.
      var user = await _userRepository.GetUserByIdAsync(dto.OwnerId);
      if (user == null)
      {
        throw new KeyNotFoundException($"User with ID {dto.OwnerId} does not exist.");
      }

      var item = new TodoItem
      {
        Name = dto.Name,
        Description = dto.Description,
        DueDate = dto.DueDate,
        Status = dto.Status,
        OwnerId = dto.OwnerId,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
      };

      var createdItem = await _repository.AddAsync(item);
      return MapToDTO(createdItem);
    }

    public async Task UpdateAsync(long id, TodoItemDTO dto)
    {
      var item = await _repository.GetAsync(id);
      if (item == null)
      {
        throw new KeyNotFoundException("Todo item not found.");
      }

      // We do NOT allow updating OwnerId after creation, so we skip that field.
      item.Name = dto.Name;
      item.Description = dto.Description;
      item.DueDate = dto.DueDate;
      item.Status = dto.Status;

      item.UpdatedAt = DateTime.UtcNow;

      await _repository.UpdateAsync(item);
    }

    public async Task DeleteAsync(long id)
    {
      var item = await _repository.GetAsync(id);
      if (item == null)
      {
        throw new KeyNotFoundException("Todo item not found.");
      }

      await _repository.DeleteAsync(item);
    }

    private static TodoItemDTO MapToDTO(TodoItem item)
    {
      return new TodoItemDTO
      {
        Id = item.Id,
        Name = item.Name,
        Description = item.Description,
        DueDate = item.DueDate,
        Status = item.Status,
        OwnerId = item.OwnerId,
        CreatedAt = item.CreatedAt,
        UpdatedAt = item.UpdatedAt
      };
    }
  }
}
