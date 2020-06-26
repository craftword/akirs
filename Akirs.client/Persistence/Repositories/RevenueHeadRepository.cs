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
    public class RevenueHeadRepository : Repository<RevenueHead>, IRevenueHeadRepository
    {
        public RevenueHeadRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }
        public IEnumerable<RevenueHead> GetRevenueHead(string Ministry)
        {

            var ret = PlutoContext.RevenueHeads.Where(p => p.Status == "A" && p.MinistryCode== Ministry).ToList().OrderBy(f=> f.RevenueHead1);
            return ret;
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}