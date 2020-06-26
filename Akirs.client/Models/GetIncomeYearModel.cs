using Akirs.client.DL;
using Akirs.client.Implementation;
using Akirs.client.Persistence;
using Akirs.client.Persistence.Repositories;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Akirs.client.Models
{
    public class GetIncomeYearModel
    {
        private readonly IIncomeSourceModelRepository repoIcome;
        private readonly IDbFactory idbfactory;

        public GetIncomeYearModel()
        {
            idbfactory = new DbFactory();
            repoIcome = new IncomeSourceModelRepository(idbfactory);
        }

        public IEnumerable<SelectListItem> ListOfYear()
        {
            IEnumerable<System.Web.Mvc.SelectListItem> items = repoIcome.GetAll.Where(p => p.Status == "A").AsEnumerable()
                .Select(p => new System.Web.Mvc.SelectListItem
                {
                    Text = p.IncomeYear.ToString(),
                    Value = p.IncomeYear.ToString()
                });
            return items;
        }
       public yearIcome YearIcome { get; set; }


        public class yearIcome
        {
            public int ItbID { get; set; }
            public string EnrollmentID { get; set; }
            public int SourceOfIncomeID { get; set; }
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
            public IEnumerable<SelectListItem> Iyear { get; set; }
        }
    }
}