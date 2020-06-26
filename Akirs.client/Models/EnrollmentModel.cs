using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Akirs.client.DL;

namespace Akirs.client.Models
{
    public class EnrollmentModel
    {
        public EnrollmentViewModel EnrollmentViewModel { get; set; }
        public List<FamilyDetail> FamilyModel { get; set; }
        public  List<IncomeSource> IncomeSource { get; set; }
        public List<Salaryupload_temp> Salaryupload_temp { get; set; }
        public List<WHTUPLOAD> WHTUPLOAD { get; set; }
    }
}