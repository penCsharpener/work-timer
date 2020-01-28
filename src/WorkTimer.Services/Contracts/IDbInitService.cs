using System.Threading.Tasks;

namespace WorkTimer.Contracts {
    public interface IDbInitService {
        Task InitializeDatabase();
    }
}
