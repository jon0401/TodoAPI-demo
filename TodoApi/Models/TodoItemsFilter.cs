using System;

namespace TodoApi.Models
{
  public class TodoItemsFilter
  {
    public string? Status { get; set; }
    public DateTime? DueBefore { get; set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
  }
}
