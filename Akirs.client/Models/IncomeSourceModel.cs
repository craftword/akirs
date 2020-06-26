using Akirs.client.DL;
using Akirs.client.Persistence.Repositories;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Akirs.client.Models
{
    public class IncomeSourceModel
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
        public string IsNew { get; set; }
        public IEnumerable<SelectListItem> IncomeYr { set; get; }

    }
}