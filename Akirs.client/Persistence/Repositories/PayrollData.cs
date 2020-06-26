using Akirs.client.DL;
using Akirs.client.Enums;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence.Repositories
{
   
        public class PayrollDataRepository : Repository<SALARYUPLOAD>, IPayrollDataRepository
    {
            public PayrollDataRepository(AKIRSTAXEntities context)
                : base(context)
            {

            }
            public IEnumerable<SALARYUPLOAD> GetPendingPayrollData(string EnrollId)
            {
                var ret = PlutoContext.SALARYUPLOADs.Where(p => p.EnrollmentID == EnrollId &&
                                                           p.PayrollStatus == PayrollStatus.PENDING.ToString()
                                                           && p.IsDeleted == false).ToList();
                return ret;
            }
            public IEnumerable<SALARYUPLOAD> GetPayrollData(string EnrollId)
            {
                var ret = PlutoContext.SALARYUPLOADs.Where(p => p.EnrollmentID == EnrollId 
                                                            && p.PayrollStatus == PayrollStatus.APPROVED.ToString()
                                                            && p.IsDeleted == false).ToList();
                return ret;
            }
            public SALARYUPLOAD GetPayrollDataById(int Itbid)
            {
                var ret = PlutoContext.SALARYUPLOADs.Where(p => p.ItbID == Itbid && p.IsDeleted == false).FirstOrDefault();
                return ret;
            }

        public int GetCountPayrollData(string EnrollId)
        {
            int ret = PlutoContext.SALARYUPLOADs.Where(x => x.EnrollmentID == EnrollId
                                                        && x.PayrollStatus == PayrollStatus.APPROVED.ToString()
                                                        && x.IsDeleted == false).Count();
            return ret;
        }

        public AKIRSTAXEntities PlutoContext
            {
                get { return Context as AKIRSTAXEntities; }
            }
        }
    }