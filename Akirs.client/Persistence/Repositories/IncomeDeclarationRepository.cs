using Akirs.client.DL;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Akirs.client.Persistence.Repositories
{
    public class IncomeDeclarationRepository : Repository<IncomeSource>, IIncomeDeclarationRepository
    {
        public IncomeDeclarationRepository(AKIRSTAXEntities context) 
            : base(context)
        {
        }

        public IEnumerable<IncomeSource> GetIncomeSource(string EnrollId)
        {
            var familydetails = PlutoContext.IncomeSources.Where(p => p.EnrollmentID == EnrollId).ToList();


            return familydetails; //PlutoContext.FamilyDetails.Where(p=>p.EnrollmentID == EnrollId).ToList();
        }
        public IncomeSource GetIncomeSourceById(int Itbid)
        {
            var ret = PlutoContext.IncomeSources.Where(p => p.ItbID == Itbid).FirstOrDefault();
            return ret;
        }
        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}