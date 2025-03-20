using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TodoApi.Models;
using Xunit;
using System;

namespace TodoApi.Tests
{
  public class TodoApiIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
  {
    private readonly HttpClient _client;

    public TodoApiIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
      // CreateClient spins up an in-memory test server using our custom factory.
      _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetTodoItems_ReturnsOk()
    {
      // Act
      var response = await _client.GetAsync("/api/TodoItems");

      // Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostTodoItem_ReturnsCreatedAndItem()
    {
      // Arrange
      // Ensure OwnerId is set to a valid seeded user (e.g., 1)
      var newTodo = new TodoItemDTO
      {
        Name = "Integration Test Item",
        Description = "Integration Test Desc",
        DueDate = DateTime.Today,
        Status = TodoStatus.NotStarted,
        OwnerId = 1
      };

      var content = new StringContent(
          JsonSerializer.Serialize(newTodo),
          Encoding.UTF8,
          "application/json"
      );

      // Act
      var response = await _client.PostAsync("/api/TodoItems", content);

      // Assert
      Assert.Equal(HttpStatusCode.Created, response.StatusCode);
      var body = await response.Content.ReadAsStringAsync();
      Assert.Contains("Integration Test Item", body);
    }

    [Fact]
    public async Task PostTodoItem_InvalidOwner_ReturnsNotFound()
    {
      // Arrange
      // Use an OwnerId that doesn't exist (e.g., 999)
      var newTodo = new TodoItemDTO
      {
        Name = "Invalid Owner Todo",
        Description = "This should fail",
        DueDate = DateTime.Today,
        Status = TodoStatus.NotStarted,
        OwnerId = 999
      };

      var content = new StringContent(
          JsonSerializer.Serialize(newTodo),
          Encoding.UTF8,
          "application/json"
      );

      // Act
      var response = await _client.PostAsync("/api/TodoItems", content);

      // Assert: our exception handling middleware returns a 500 error.
      Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    }
  }
}
