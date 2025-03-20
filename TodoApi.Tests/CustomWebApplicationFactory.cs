using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TodoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.Tests
{
  public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
      where TStartup : class
  {
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
      builder.ConfigureServices(services =>
      {
        // Build the service provider.
        var sp = services.BuildServiceProvider();

        // Create a scope to obtain a reference to the database context.
        using var scope = sp.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<TodoContext>();
        var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

        // Ensure the database is created.
        // Alternatively, you could apply migrations with db.Database.Migrate() if you have migrations set up.
        try
        {
          // Ensure the database is deleted so we start fresh.
          db.Database.EnsureDeleted();
          // Apply migrations to create the updated schema (tables, columns, etc.)
          db.Database.Migrate();
        }
        catch (Exception ex)
        {
          logger.LogError(ex, "An error occurred creating the database.");
          throw;
        }
      });
    }
  }
}
