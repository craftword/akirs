using Akirs.client.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.repository
{
    public interface IPayrollDataRepository : IRepository<SALARYUPLOAD>
    {

        IEnumerable<SALARYUPLOAD> GetPendingPayrollData(string EnrollId);
        IEnumerable<SALARYUPLOAD> GetPayrollData(string EnrollId);
        int GetCountPayrollData(string EnrollId);
        SALARYUPLOAD GetPayrollDataById(int Itbid);
    }
}