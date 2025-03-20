# TodoApi

A simple ASP.NET Core Web API for managing Todo items. This application demonstrates:

- **CRUD** (Create, Read, Update, Delete) operations
- **Filtering & Sorting** based on query parameters
- **SQLite** database for persistence
- **Repository & Service layers** to adhere to SOLID principles
- **Swagger** documentation and **xUnit** tests (unit + integration)

---

## Getting Started

### Prerequisites

- [.NET 9 or later](https://dotnet.microsoft.com/download) (the project targets `net9.0`, which requires .NET 9 SDK)
- SQLite is automatically included as a dependency (no separate install required)

### Installation & Setup

1. **Restore dependencies**:
   ```bash
   dotnet restore
   ```
2. **Build the solution**:
   ```bash
   dotnet build
   ```
3. **Apply Migrations (Optional)**:
   - If you want EF Core migrations instead of `EnsureCreated()`, run:
     ```bash
     dotnet ef migrations add InitialCreate --project TodoApi
     dotnet ef database update --project TodoApi
     ```
   - This step will create a local `todo.db` SQLite file.

### Running the Application

```bash
dotnet run --project TodoApi/TodoApi.csproj
```

- By default, the API starts on `http://localhost:5299` (or a random port).
- Navigate to `http://localhost:5299/swagger` to explore the Swagger UI.

---

## Usage

- **Swagger UI**:  
  Go to `[host]/swagger` to see interactive documentation. You can test each endpoint (GET, POST, PATCH, DELETE) from your browser.

- **Sample Requests**:
  - **Get All**:  
    `GET /api/TodoItems`
  - **Get By ID**:  
    `GET /api/TodoItems/1`
  - **Create**:  
    `POST /api/TodoItems` (send JSON body)
  - **Update**:  
    `PATCH /api/TodoItems/1`
  - **Delete**:  
    `DELETE /api/TodoItems/1`

---

## Project Structure

```
TodoApi
├─ Controllers
│   └─ TodoItemsController.cs   # Defines API endpoints for Todo items
├─ Migrations                   # (Optional) EF Core migration files
├─ Models
│   ├─ TodoItem.cs             # Entity model stored in the database
│   ├─ TodoItemDTO.cs          # Data Transfer Object exposed to clients
│   ├─ TodoStatus.cs           # Enum for item status
│   └─ TodoContext.cs          # EF Core DbContext
├─ Repository
│   ├─ ITodoItemRepository.cs  # Interface for data access operations
│   ├─ TodoItemRepository.cs   # Concrete implementation using EF Core
│   └─ TodoItemQueryExtensions.cs # Filtering & sorting extension methods
├─ Services
│   ├─ ITodoItemService.cs     # Interface for business logic
│   └─ TodoItemService.cs      # Concrete implementation using repository
├─ Program.cs                  # App entry point & DI configuration
└─ ...
```

**Highlights**:

- **Controllers** handle HTTP requests/responses.
- **Models** define data structures (entities, DTOs, enums, EF Core context).
- **Repository** layer encapsulates data access logic.
- **Services** layer handles business logic and orchestrates repository calls.

---

## Testing

- **Unit Tests** (Service layer) and **Integration Tests** (entire app) are located in `TodoApi.Tests`.
- Run all tests via:
  ```bash
  dotnet test
  ```
- Integration tests use an in-memory or SQLite database for realistic end-to-end scenarios.
