using System;

namespace TodoApi.Models
{
  public class User
  {
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ICollection<TodoItem>? TodoItems { get; set; }
  }
}
