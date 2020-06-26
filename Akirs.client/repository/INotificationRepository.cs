﻿using Akirs.client.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akirs.client.repository
{
    public interface INotificationRepository : IRepository<NotificationAlert_Result>
    {
        IEnumerable<NotificationAlert_Result> GetNotificationAlert();
    }
}
