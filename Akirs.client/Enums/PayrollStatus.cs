using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Akirs.client.Enums
{
    public enum PayrollStatus
    {
        [Description("Awaiting Approval")]
        AWAITINGAPPROVAL,
        [Description("Pending")]
        PENDING,
        [Description("Approved")]
        APPROVED,
        [Description("Rejected")]
        REJECTED,
    }
}