using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Models
{
    public class EnrollmentViewModel
    {
        public int? RentRangeId { get; set; }
        public string LGACode { get; set; }
        public string Companyname { get; set; }
        public string Position { get; set; }
        public int ItbID { get; set; }
        public string EnrollmentID { get; set; }
        public int TaxtypeID { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Address { get; set; }
        public string Phonenumber { get; set; }
        public string Email { get; set; }
        public string CountryID { get; set; }
        public string StateCode { get; set; }
        public string CityCode { get; set; }
        public string DateOfBirth { get; set; }
        public int ProfessionID { get; set; }
        public int CompanyID { get; set; }
        public string MaritalStatus { get; set; }
        public Nullable<int> Numberofchildren { get; set; }
        public Nullable<System.DateTime> Last_Modified_Date { get; set; }
        public string Last_Modified_Authid { get; set; }
        public string Last_Modified_Uid { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string AuthUserID { get; set; }
        public Nullable<decimal> AMOUNTDUE { get; set; }
        public Nullable<decimal> LASTPAYAMOUNT { get; set; }
        public Nullable<System.DateTime> LASTPAYDATE { get; set; }
        public Nullable<decimal> NETTAX { get; set; }
        public string Gender { get; set; }
    }
}