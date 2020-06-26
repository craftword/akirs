using Akirs.client.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akirs.client.Implementation
{
    public interface IDbFactory : IDisposable
    {
        AKIRSTAXEntities Init();
    }
}
