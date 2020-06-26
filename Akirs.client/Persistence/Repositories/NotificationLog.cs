using Akirs.client.DL;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence.Repositories
{
 
  public class NotificationLogRepository : Repository<NotificationLog>, INotificationLogRepository
    {
        public NotificationLogRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }
        public IEnumerable<NotificationLog> GetNotificationLog()
        {

            var ret = PlutoContext.NotificationLogs.Where(p => p.Status == "A").ToList();
            return ret;
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}