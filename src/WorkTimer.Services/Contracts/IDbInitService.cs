using System.Threading.Tasks;

namespace WorkTimer.Contracts {
    public interface IDbInitService {
        public string ConnectionString { get; }
        Task InitializeDatabase();
    }
}
