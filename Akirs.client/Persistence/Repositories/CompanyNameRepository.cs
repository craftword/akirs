using Akirs.client.DL;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence.Repositories
{
    public class CompanyNameRepository : Repository<proc_GetCompanyName_Result>, ICompanyNameRepository
    {
        public CompanyNameRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }

        public proc_GetCompanyName_Result GetCustomerName(string CompanyName)
        {
            var ret = PlutoContext.proc_GetCompanyName(CompanyName).FirstOrDefault();
            return ret;
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}