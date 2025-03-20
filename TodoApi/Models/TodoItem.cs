using System;

namespace TodoApi.Models
{
  public class TodoItem
  {
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public TodoStatus Status { get; set; }
    public long OwnerId { get; set; }
    public User? Owner { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}
