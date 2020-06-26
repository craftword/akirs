using Akirs.client.DL;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Akirs.client.Persistence.Repositories
{
    public class CompanyDataRepository : Repository<CompanyData>, ICompanyDataRepository
    {
        public CompanyDataRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }
        

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}