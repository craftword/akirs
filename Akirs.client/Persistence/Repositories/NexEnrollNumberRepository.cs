using Akirs.client.DL;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence.Repositories
{
    public class NexEnrollNumberRepository : Repository<proc_GetNextEnRoll_Result>, INexEnrollNumberRepository
    {
        public NexEnrollNumberRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }

        public proc_GetNextEnRoll_Result GetNextEnrollNumber()
        {
            var ret = PlutoContext.proc_GetNextEnRoll().FirstOrDefault();
            return ret;
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}