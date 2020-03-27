using System.Threading.Tasks;
using WorkTimer.Api.Models;

namespace WorkTimer.Api.Contracts {
    public interface IAuthProvider {
        Task<User> Login(LoginModel model);
        Task<User> Register(RegisterModel model);
        Task<bool> UserExists(LoginModel registerModel);
    }
}
