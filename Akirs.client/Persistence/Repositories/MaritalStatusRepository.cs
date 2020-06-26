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
    public class MaritalStatusRepository : Repository<MaritalStatu>, IMaritalStatusRepository
    {
        public MaritalStatusRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }
        public IEnumerable<MaritalStatu> GetMaritalStatus()
        {
            var ret = PlutoContext.MaritalStatus.ToList();
            return ret;
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}