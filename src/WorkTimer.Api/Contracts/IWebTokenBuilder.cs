using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkTimer.Api.Contracts {
    public interface IWebTokenBuilder {
        string GenerateToken();
    }
}
