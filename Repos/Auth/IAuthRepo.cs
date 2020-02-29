using System.Threading.Tasks;
using ToDo_exercise1.Models.Dtos;

namespace ToDo_exercise1.Repos.Auth
{
    public interface IAuthRepo
    {
        Task<(UserDto userCreds, string token)> Login(UserLogin loginCreds);
        Task<(UserDto userCreds, string token)> Register(UserRegister newUser);
    }
}