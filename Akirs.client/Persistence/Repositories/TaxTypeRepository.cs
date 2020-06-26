using Akirs.client.DL;
using Akirs.client.Models;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akirs.client.Persistence.Repositories
{
    public class TaxTypeRepository : Repository<TaxType>, ITaxTypeRepository
    {
        public TaxTypeRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }
        public IEnumerable<TaxType> GetTaxType()
        {

            var ret = PlutoContext.TaxTypes.ToList();
            return ret;
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}