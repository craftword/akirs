using Akirs.client.DL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Akirs.client.Models
{
    public class AssessmentModelPayee
    {
        public List<AssessmentRecord> AssessmentRecord { get; set; }
        public string FormattedAmount(decimal amount)
        {
            return amount.ToString("N2", CultureInfo.InvariantCulture);
        }
    }
}