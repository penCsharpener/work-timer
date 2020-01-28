using System.Data;

namespace WorkTimer.Contracts {
    public interface IDatabaseConnection<T> where T : class, IDbConnection {
        T Get();
    }
}
