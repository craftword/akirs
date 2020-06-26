using Akirs.client.DL;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence.Repositories
{
    public class RolesRepository : Repository<tbl_Roles>, IRolesRepository
    {
        public RolesRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }

        public IEnumerable<tbl_Roles> GetRoles()
        {
            var ret = PlutoContext.tbl_Roles.Where(p => p.Roleid == 13).ToList();
            return ret;
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}