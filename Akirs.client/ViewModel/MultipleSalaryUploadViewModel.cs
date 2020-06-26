using System;
using Akirs.client.DL;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.ViewModel
{
    public class MultipleSalaryUploadViewModel
    {
        public List<SALARYUPLOAD> SalaryList { get; set; }

        public SALARYUPLOAD Salary { get; set; }
    }
}