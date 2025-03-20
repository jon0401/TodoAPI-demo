using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using TodoApi.Models;
using TodoApi.Repository;
using TodoApi.Services;
using Xunit;

namespace TodoApi.Tests
{
  public class TodoItemServiceTests
  {
    [Fact]
    public async Task GetAllAsync_ReturnsMappedDTOs()
    {
      // Arrange
      var mockTodoRepo = new Mock<ITodoItemRepository>();
      var mockUserRepo = new Mock<IUserRepository>();
      var filter = new TodoItemsFilter();

      var items = new List<TodoItem>
            {
                new TodoItem
                {
                    Id = 1,
                    Name = "Item1",
                    Description = "Desc1",
                    Status = TodoStatus.NotStarted,
                    OwnerId = 1,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new TodoItem
                {
                    Id = 2,
                    Name = "Item2",
                    Description = "Desc2",
                    Status = TodoStatus.Completed,
                    OwnerId = 2,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

      // Setup repository to return our test data.
      mockTodoRepo.Setup(r => r.GetAllAsync(filter)).ReturnsAsync(items);

      var service = new TodoItemService(mockTodoRepo.Object, mockUserRepo.Object);

      // Act
      var result = await service.GetAllAsync(filter);

      // Assert
      Assert.Equal(2, result.Count());
      Assert.Contains(result, dto => dto.Id == 1 && dto.Name == "Item1");
      Assert.Contains(result, dto => dto.Id == 2 && dto.Name == "Item2");
    }

    [Fact]
    public async Task GetAllAsync_WhenRepositoryThrows_ExceptionIsPropagated()
    {
      // Arrange
      var mockTodoRepo = new Mock<ITodoItemRepository>();
      var mockUserRepo = new Mock<IUserRepository>();
      var filter = new TodoItemsFilter();

      // Simulate a repository failure.
      mockTodoRepo.Setup(r => r.GetAllAsync(filter))
                  .ThrowsAsync(new Exception("Repository failure"));

      var service = new TodoItemService(mockTodoRepo.Object, mockUserRepo.Object);

      // Act & Assert: Exception should propagate.
      var ex = await Assert.ThrowsAsync<Exception>(() => service.GetAllAsync(filter));
      Assert.Equal("Repository failure", ex.Message);
    }

    [Fact]
    public async Task CreateAsync_AddsItemAndReturnsDTO()
    {
      // Arrange
      var mockTodoRepo = new Mock<ITodoItemRepository>();
      var mockUserRepo = new Mock<IUserRepository>();

      // Create a valid DTO with an existing OwnerId.
      var newDto = new TodoItemDTO
      {
        Name = "New Item",
        Description = "New Desc",
        DueDate = DateTime.Today,
        Status = TodoStatus.NotStarted,
        OwnerId = 1
      };

      // Setup user repository to return a valid user for OwnerId = 1.
      mockUserRepo.Setup(u => u.GetUserByIdAsync(1))
                  .ReturnsAsync(new User
                  {
                    Id = 1,
                    Name = "John Doe",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                  });

      // Simulate repository creating an item and assigning an ID.
      var createdItem = new TodoItem
      {
        Id = 99,
        Name = newDto.Name,
        Description = newDto.Description,
        DueDate = newDto.DueDate,
        Status = newDto.Status,
        OwnerId = newDto.OwnerId,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
      };

      mockTodoRepo.Setup(r => r.AddAsync(It.IsAny<TodoItem>()))
                  .ReturnsAsync(createdItem);

      var service = new TodoItemService(mockTodoRepo.Object, mockUserRepo.Object);

      // Act
      var result = await service.CreateAsync(newDto);

      // Assert
      Assert.Equal(99, result.Id);
      Assert.Equal("New Item", result.Name);
    }

    [Fact]
    public async Task CreateAsync_WithInvalidOwner_ThrowsKeyNotFoundException()
    {
      // Arrange
      var mockTodoRepo = new Mock<ITodoItemRepository>();
      var mockUserRepo = new Mock<IUserRepository>();

      // Create a DTO with an OwnerId that does not exist.
      var newDto = new TodoItemDTO
      {
        Name = "New Item",
        Description = "New Desc",
        DueDate = DateTime.Today,
        Status = TodoStatus.NotStarted,
        OwnerId = 10  // Assume no user with this ID exists.
      };

      // Setup user repository to return null for OwnerId = 10.
      mockUserRepo.Setup(u => u.GetUserByIdAsync(10))
                  .ReturnsAsync((User?)null);

      var service = new TodoItemService(mockTodoRepo.Object, mockUserRepo.Object);

      // Act & Assert: Expect a KeyNotFoundException with a meaningful message.
      var ex = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.CreateAsync(newDto));
      Assert.Equal("User with ID 10 does not exist.", ex.Message);
    }
  }
}
