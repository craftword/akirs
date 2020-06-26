using Akirs.client.DL;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Akirs.client.Persistence.Repositories
{
    public class NotificationUpdateRepository : Repository<Proc_UpdateNotification_Result>, INotificationUpdateRepository
    {
        public NotificationUpdateRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }

        public Proc_UpdateNotification_Result UpdateNotification(long Itbid)
        {
            var ret = PlutoContext.Proc_UpdateNotification((int)Itbid).FirstOrDefault();
            return ret;
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}