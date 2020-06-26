using Akirs.client.DL;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence.Repositories
{
    public class UserRoleRepository : Repository<AspNetRole>, IUserRoleRepository
    {
        public UserRoleRepository(AKIRSTAXEntities context) : base(context)
        {

        }

        public AspNetRole GetRoleById(string Id)
        {
            var ret = PlutoContext.AspNetRoles.Where(p => p.Id == Id).FirstOrDefault();
            return ret;
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }

}