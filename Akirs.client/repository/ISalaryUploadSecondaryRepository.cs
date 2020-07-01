using Akirs.client.DL;
using Akirs.client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Akirs.client.repository
{
    public interface ISalaryUploadSecondaryRepository : IRepository<SALARYUPLOAD_HISTORY>
    {
        List<SALARYUPLOAD_HISTORY> GetSalaryUploadSecondary(string EnrollId, short month, int year);
        List<short?> GetMonths(string EnrollId);
        List<int?> GetYears(string EnrollId);
        decimal GetSalaryUploadSecondaryCount(string EnrollId);

    }
}
