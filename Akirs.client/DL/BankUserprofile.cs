
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace Akirs.client.DL
{

using System;
    using System.Collections.Generic;
    
public partial class BankUserprofile
{

    public long itbID { get; set; }

    public int RoleID { get; set; }

    public long BankID { get; set; }

    public string BranchCode { get; set; }

    public string UserID { get; set; }

    public string CoreBankUserID { get; set; }

    public string Surname { get; set; }

    public string Firstname { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public string Telephone { get; set; }

    public System.DateTime LastLogin { get; set; }

    public bool IsActive { get; set; }

    public bool IsFirstLogin { get; set; }

    public int FailedLogins { get; set; }

    public Nullable<bool> IsApproved { get; set; }

    public Nullable<long> ApprovedBy { get; set; }

    public Nullable<System.DateTime> DateApproved { get; set; }

    public Nullable<bool> IsTokenRequired { get; set; }

    public Nullable<decimal> LimitAmount { get; set; }

    public bool IsDeleted { get; set; }

    public string Status { get; set; }

    public System.DateTime Createdate { get; set; }

    public string AdminUserID { get; set; }

    public string AuthUserID { get; set; }

    public string TillAccountNo { get; set; }

}

}