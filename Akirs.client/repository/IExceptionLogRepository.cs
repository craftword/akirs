using Akirs.client.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.repository
{
    

    public interface IExceptionLogRepository : IRepository<ExceptionLog>
    {
        IEnumerable<ExceptionLog> GetException(int ExceptionId);
    }
}