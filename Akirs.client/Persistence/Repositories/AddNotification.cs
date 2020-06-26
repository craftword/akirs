using Akirs.client.DL;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence.Repositories
{
    public class AddNotification : Repository<proc_Notification_Result>, IAddNotification
    {
        public AddNotification(AKIRSTAXEntities context)
            : base(context)
        {
        }

        public proc_Notification_Result AddAlert(string NotificationType ,
         string Email,
         string phoneno ,
         string notificationmsg ,
         string userid) 
        {
            var ret = PlutoContext.proc_Notification(NotificationType,Email, phoneno, notificationmsg, userid).FirstOrDefault();
            return ret;
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}