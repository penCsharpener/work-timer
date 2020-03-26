using WorkTimer.Api.Models;

namespace WorkTimer.Api.Contracts {
    public interface IWebTokenBuilder {
        string GenerateToken(User user);
    }
}
