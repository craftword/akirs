using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Models
{
   
        public class IncomePaymentModel
    {
            public int ItbID { get; set; }
            public string EnrollmentID { get; set; }
            public string SourceOfIncome { get; set; }
            public decimal Amount { get; set; }
            public Nullable<System.DateTime> Last_Modified_Date { get; set; }
            public string Last_Modified_Authid { get; set; }
            public string Last_Modified_Uid { get; set; }
            public string Status { get; set; }
            public string CreatedBy { get; set; }
            public Nullable<System.DateTime> CreateDate { get; set; }
            public Nullable<int> NHFFlag { get; set; }
            public Nullable<int> PensionFlag { get; set; }
            public string IncomeYear { get; set; }
            //public IEnumerable<SelectListItem> IncomeYr { set; get; }
        }
    }
