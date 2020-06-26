using Akirs.client.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akirs.client.repository
{
   public interface IUserRoleRepository : IRepository<AspNetRole>
    {
        AspNetRole GetRoleById(string Id);
    }
}
