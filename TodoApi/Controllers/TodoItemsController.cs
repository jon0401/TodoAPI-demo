using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
  /// <summary>
  /// Controller for managing Todo items.
  /// </summary>
  [Route("api/[controller]")]
  [ApiController]
  public class TodoItemsController : ControllerBase
  {
    private readonly ITodoItemService _service;

    /// <summary>
    /// Initializes a new instance of the <see cref="TodoItemsController"/> class.
    /// </summary>
    /// <param name="service">The Todo item service instance.</param>
    public TodoItemsController(ITodoItemService service)
    {
      _service = service;
    }

    /// <summary>
    /// Retrieves all Todo items with optional filtering and sorting.
    /// </summary>
    /// <param name="filter">Optional filter parameters (status, due date, sort options).</param>
    /// <returns>A list of TodoItemDTO objects.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems([FromQuery] TodoItemsFilter filter)
    {
      var items = await _service.GetAllAsync(filter);
      return Ok(items);
    }

    /// <summary>
    /// Retrieves a specific Todo item by its ID.
    /// </summary>
    /// <param name="id">The ID of the Todo item to retrieve.</param>
    /// <returns>
    /// A <see cref="TodoItemDTO"/> if found; otherwise, a 404 Not Found response.
    /// </returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
      var item = await _service.GetAsync(id);
      if (item == null)
      {
        return NotFound();
      }
      return Ok(item);
    }

    /// <summary>
    /// Creates a new Todo item.
    /// </summary>
    /// <param name="todoDTO">The Todo item data to create.</param>
    /// <returns>
    /// A 201 Created response containing the created TodoItemDTO.
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO)
    {
      var createdItem = await _service.CreateAsync(todoDTO);
      return CreatedAtAction(nameof(GetTodoItem), new { id = createdItem.Id }, createdItem);
    }

    /// <summary>
    /// Updates an existing Todo item.
    /// </summary>
    /// <param name="id">The ID of the Todo item to update.</param>
    /// <param name="todoDTO">The updated Todo item data.</param>
    /// <returns>
    /// A 204 No Content response if the update is successful;
    /// a 400 Bad Request response if the URL ID and DTO ID do not match;
    /// or a 404 Not Found response if the Todo item is not found.
    /// </returns>
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchTodoItem(long id, TodoItemDTO todoDTO)
    {
      if (id != todoDTO.Id)
      {
        return BadRequest("URL id and DTO id do not match.");
      }

      try
      {
        await _service.UpdateAsync(id, todoDTO);
      }
      catch (KeyNotFoundException)
      {
        return NotFound();
      }
      return NoContent();
    }

    /// <summary>
    /// Deletes a specific Todo item by its ID.
    /// </summary>
    /// <param name="id">The ID of the Todo item to delete.</param>
    /// <returns>
    /// A 204 No Content response if the deletion is successful;
    /// or a 404 Not Found response if the Todo item is not found.
    /// </returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
      try
      {
        await _service.DeleteAsync(id);
      }
      catch (KeyNotFoundException)
      {
        return NotFound();
      }
      return NoContent();
    }
  }
}
