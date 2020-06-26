using Akirs.client.DL;
using Akirs.client.Models;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence.Repositories
{


    public class NotificationRepository : Repository<NotificationAlert_Result>, INotificationRepository
    {
        public NotificationRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }

        public IEnumerable<NotificationAlert_Result> GetNotificationAlert()
        {
            var ret = PlutoContext.NotificationAlert().ToList();
            return ret;
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}