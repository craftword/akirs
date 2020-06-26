using Akirs.client.DL;
using Akirs.client.Models;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence.Repositories
{
    public class FamilyDetailsRepository : Repository<FamilyDetail>, IFamilyDetailsRepository
    {
        public FamilyDetailsRepository(AKIRSTAXEntities context) 
            : base(context)
        {
        }

        public IEnumerable<FamilyDetail> GetFamilyDetails(string EnrollId)
        {
            
            return PlutoContext.FamilyDetails.Where(p=>p.EnrollmentID == EnrollId).ToList();
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}