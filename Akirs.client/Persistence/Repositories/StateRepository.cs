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
    public class StateRepository : Repository<State>, IStateRepository
    {
        public StateRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }
        public IEnumerable<State> GetStates()
        {

            var ret = PlutoContext.States.Where(p => p.Status == "A").ToList();
            return ret;
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}