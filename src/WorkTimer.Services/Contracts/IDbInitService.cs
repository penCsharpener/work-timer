using System.Threading.Tasks;

namespace WorkTimer.Contracts {
    public interface IDbInitService {
        void CreateDatabase();
        Task CreateTable();
    }
}
