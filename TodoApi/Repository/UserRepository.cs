using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Repository
{
  public class UserRepository : IUserRepository
  {
    private readonly TodoContext _context;

    public UserRepository(TodoContext context)
    {
      _context = context;
    }

    public async Task<User?> GetUserByIdAsync(long id)
    {
      return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
  }
}
