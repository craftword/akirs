using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Models
{
    public class UserModel
    {
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string EnrollmentId { get; set; }
        public DateTime? CreateDate { get; internal set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public Nullable<int> NoOfChildren { get; set; }
        public string Status { get; set; }
        public string MaritalStatus { get; set; }
        public string City { get; set; }
       public string CompanyName { get; set; }

    }
}