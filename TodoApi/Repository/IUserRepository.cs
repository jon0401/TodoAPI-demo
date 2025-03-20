using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Repository
{
  public interface IUserRepository
  {
    Task<User?> GetUserByIdAsync(long id);
  }
}
