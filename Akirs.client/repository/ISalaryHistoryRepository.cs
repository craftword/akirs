using Akirs.client.DL;
using Akirs.client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Akirs.client.repository
{
    public interface ISalaryHistoryRepository : IRepository<SALARYUPLOAD>
    {
        IEnumerable<SALARYUPLOAD> GetSalaryUpload(string EnrollId);

    }
}