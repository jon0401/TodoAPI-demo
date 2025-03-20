using System.Linq;
using TodoApi.Models;

namespace TodoApi.Repository
{
  public static class TodoItemsQueryExtensions
  {
    public static IQueryable<TodoItem> ApplyFilter(this IQueryable<TodoItem> query, TodoItemsFilter filter)
    {
      if (!string.IsNullOrEmpty(filter.Status) && Enum.TryParse(filter.Status, out TodoStatus statusEnum))
      {
        query = query.Where(t => t.Status == statusEnum);
      }
      if (filter.DueBefore.HasValue)
      {
        query = query.Where(t => t.DueDate <= filter.DueBefore.Value);
      }
      return query;
    }

    public static IQueryable<TodoItem> ApplySorting(this IQueryable<TodoItem> query, TodoItemsFilter filter)
    {
      switch (filter.SortBy?.ToLower())
      {
        case "duedate":
          query = (filter.SortDirection?.ToLower() == "desc")
              ? query.OrderByDescending(t => t.DueDate)
              : query.OrderBy(t => t.DueDate);
          break;
        case "status":
          query = (filter.SortDirection?.ToLower() == "desc")
              ? query.OrderByDescending(t => t.Status)
              : query.OrderBy(t => t.Status);
          break;
        case "name":
          query = (filter.SortDirection?.ToLower() == "desc")
              ? query.OrderByDescending(t => t.Name)
              : query.OrderBy(t => t.Name);
          break;
        default:
          break;
      }
      return query;
    }
  }
}
