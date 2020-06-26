using Akirs.client.DL;
using Akirs.client.Models;
using Akirs.client.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Akirs.client.repository
{
    public interface ISalaryUploadRepository : IRepository<SALARYUPLOAD>
    {
        
        IEnumerable<SALARYUPLOAD> GetPendingSalaryUpload(string EnrollId);
        IEnumerable<SALARYUPLOAD> GetSalaryUpload(string EnrollId);
        SALARYUPLOAD GetSalaryUploadById(int Itbid);
        string ApprovePayrollByEmployeeId(string EnrollId);
        List<SALARYUPLOAD> UploadPayroll(HttpPostedFileBase file, string SessionISD);
        SALARYUPLOAD GetDetailsForEdit( string EnrolId,SALARYUPLOAD model);
        string CreatePayeeUser(CreatePayeeUserViewModel model);
        IEnumerable<AspNetUser> GetPayeeUserForDelete(string EnrollId);
        string DeletePayeeUser(string emailAddress);
        string RejectPayrollEmployee(string EnrollId);
        string SendPayeeForApproval(string EnrollId);

    }
}
