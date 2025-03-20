using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace TodoApi.Models
{
  /// <summary>
  /// Data Transfer Object for a Todo item.
  /// Contains the properties that are exposed to clients.
  /// </summary>
  public class TodoItemDTO
  {
    /// <summary>
    /// Unique identifier of the Todo item.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Name of the Todo item.
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string? Name { get; set; }

    /// <summary>
    /// Description of the Todo item.
    /// </summary>
    [StringLength(250)]
    public string? Description { get; set; }

    /// <summary>
    /// Due date of the Todo item.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Status of the Todo item.
    /// </summary>
    [Required]
    public TodoStatus Status { get; set; }

    /// <summary>
    /// The ID of the user who owns this Todo item.
    /// </summary>
    [Required(ErrorMessage = "OwnerId is required.")]
    public long OwnerId { get; set; }

    /// <summary>
    /// When the Todo item was created (read-only).
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// When the Todo item was last updated (read-only).
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public DateTime UpdatedAt { get; set; }
  }
}
