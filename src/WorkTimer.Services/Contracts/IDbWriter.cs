using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimer.Contracts {
    public interface IDbWriter<T> where T : class {
        Task<T> Insert(T item);
        Task Delete(T item);
        Task<T> Update(T item, string sql);

    }
}
