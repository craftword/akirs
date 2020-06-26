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
    public class RentPriceRepository : Repository<RentPrice>, IRentPriceRepository
    {
        public RentPriceRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }
        public IEnumerable<RentPrice> GetRentPrices()
        {

            var ret = PlutoContext.RentPrices.ToList();
            return ret;
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}