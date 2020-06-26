using Akirs.client.DL;
using Akirs.client.Enums;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence.Repositories
{
    public class SalaryHistoryRepository : Repository<SALARYUPLOAD>, ISalaryHistoryRepository
    {
        public SalaryHistoryRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }
        
        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }

        public IEnumerable<SALARYUPLOAD> GetSalaryUpload(string EnrollId)
        {
            var ret = PlutoContext.SALARYUPLOADs.Where(p => p.EnrollmentID == EnrollId 
                                                        && p.PayrollStatus == PayrollStatus.APPROVED.ToString() 
                                                        && p.IsDeleted == false).ToList();
            return ret;
        }
    }
}