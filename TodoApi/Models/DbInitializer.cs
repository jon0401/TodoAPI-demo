using Microsoft.EntityFrameworkCore;

namespace TodoApi.Models
{
  public class DbInitializer
  {
    private readonly ModelBuilder modelBuilder;

    public DbInitializer(ModelBuilder modelBuilder)
    {
      this.modelBuilder = modelBuilder;
    }

    public void Seed()
    {
      modelBuilder.Entity<User>().HasData(
        new User
        {
          Id = 1,
          Name = "John Doe",
          CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
          UpdatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new User
        {
          Id = 2,
          Name = "Jane Smith",
          CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
          UpdatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        }
      );
    }
  }
}