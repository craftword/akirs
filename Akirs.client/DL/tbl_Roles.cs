
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
    
public partial class tbl_Roles
{

    public long Roleid { get; set; }

    public string RoleName { get; set; }

    public string Status { get; set; }

    public System.DateTime Createdate { get; set; }

    public string AdminUserID { get; set; }

    public string AuthUserID { get; set; }

    public bool IsMainRole { get; set; }

    public Nullable<bool> ReportToFlag { get; set; }

    public bool CanPost { get; set; }

}

}