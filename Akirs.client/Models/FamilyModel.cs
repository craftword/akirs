using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Models
{
    public class FamilyModel
    {
        public int ItbID { get; set; }
        public string EnrollmentID { get; set; }
        public string FullName { get; set; }
        public Nullable<int> Age { get; set; }
        public string RelationshipType { get; set; }
        public Nullable<System.DateTime> Last_Modified_Date { get; set; }
        public string Last_Modified_Authid { get; set; }
        public string Last_Modified_Uid { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string SchoolAttended { get; set; }
        public Nullable<bool> IsStudent { get; set; }
    }
}