using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Akirs.client.ViewModel
{
    public class CreateWitholdTaxViewModel
    {
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Last Name cannot be Empty")]
        public string LastName { get; set; }

        [DisplayName("First Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First Name cannot be Empty")]
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public int RoleId { get; set; }
        public string MobileNo { get; set; }
        public string DeptName { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> LastPasswordChangeDate { get; set; }
        public string CreateUserId { get; set; }
        public int ItbId { get; set; }

        [Required(ErrorMessage = "E-mail cannot be Empty")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }

        [DisplayName("Role Name")]
        [Required(ErrorMessage = "Role Name cannot be Empty")]
        public PayeeRoles RoleName { get; set; }
        public string Supervisor { get; set; }

        [DisplayName("Enrollment Id")]
        public string EnrollmentID { get; set; }
    }

    //public enum PayeeRoles
    //{
    //    Uploader,
    //    Approver
    //}
}
