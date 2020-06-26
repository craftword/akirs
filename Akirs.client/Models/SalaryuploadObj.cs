using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Models
{
    public partial class SalaryuploadObj
    {
        public long ItbID { get; set; }
        public string EnrollmentID { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<decimal> AnnualBasic { get; set; }
        public Nullable<decimal> AnnualHousing { get; set; }
        public Nullable<decimal> AnnualTransport { get; set; }
        public Nullable<decimal> AnnualMeal { get; set; }
        public Nullable<decimal> AnnualOthers { get; set; }
        public Nullable<decimal> GrossPay { get; set; }
        public Nullable<decimal> NHFContribution { get; set; }
        public Nullable<System.DateTime> Last_Modified_Date { get; set; }
        public string Last_Modified_Authid { get; set; }
        public string Last_Modified_Uid { get; set; }
        public string Status { get; set; }
        public bool VALIDATIONERRORSTATUS { get; set; }
        public string ValidationStatusClass
        {
            get
            {
                return VALIDATIONERRORSTATUS ? "btn btn-warning btn-xs" : "btn btn-success btn-xs";
            }

        }
        public string ValidationStatusIcon
        {
            get
            {
                return VALIDATIONERRORSTATUS ? "fa fa-times" : "fa fa-check";
            }

        }

        public string VALIDATIONERRORMESSAGE { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string AuthUserID { get; set; }
    }
}