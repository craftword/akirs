using Akirs.client.DL;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence.Repositories
{
    public class IncomeSourceTypeRepository : Repository<IncomeSourceType>, IIncomeSourceTypeRepository
    {
        public IncomeSourceTypeRepository(AKIRSTAXEntities context) 
            : base(context)
        {
        }

        public IEnumerable<IncomeSourceType> GetIncomeSourceType()
        {
            var familydetails = PlutoContext.IncomeSourceTypes.Where(p => p.Status == "A").ToList();


            return familydetails; //PlutoContext.FamilyDetails.Where(p=>p.EnrollmentID == EnrollId).ToList();
        }

        
        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}