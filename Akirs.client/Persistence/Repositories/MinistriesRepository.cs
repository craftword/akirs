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
    public class MinistriesRepository : Repository<Ministry>, IMinistriesRepository
    {
        public MinistriesRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }
        public IEnumerable<Ministry> GetMinistry()
        {

            var ret = PlutoContext.Ministries.Where(p => p.Status == "A").ToList().OrderBy(f=> f.MinistryDesc);
            return ret;
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}