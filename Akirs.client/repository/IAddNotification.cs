using Akirs.client.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akirs.client.repository
{
    public interface IAddNotification : IRepository<proc_Notification_Result>
    {
        proc_Notification_Result AddAlert(string NotificationType,
         string Email,
         string phoneno,
         string notificationmsg,
         string userid);
    }
}
