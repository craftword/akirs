using Akirs.client.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Implementation
{
    public class DbFactory: Disposable, IDbFactory
    {
        AKIRSTAXEntities dbContext;

        public AKIRSTAXEntities Init()
        {
            return dbContext ?? (dbContext = new AKIRSTAXEntities());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}