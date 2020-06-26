using Akirs.client.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akirs.client.repository
{
    public interface INotificationUpdateRepository : IRepository<Proc_UpdateNotification_Result>
    {
        Proc_UpdateNotification_Result UpdateNotification(long Itbid);
    }
}
