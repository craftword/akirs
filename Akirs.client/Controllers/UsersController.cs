using Akirs.client.DL;
using Akirs.client.Models;
using Akirs.client.Persistence;
using Akirs.client.utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Akirs.client.Controllers
{
    [SessionExpire]
    public class UsersController : Controller
    {
        //public object db;

        public UsersController()
        {
           
        }
        // GET: Users
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ViewDetail(int id = 0)
        {
            if (id == 0)
            {
                return Json(new { RespCode = 99, RespMessage = "Bad Request" }, JsonRequestBehavior.AllowGet);
            }
            //var d = _repo.GetSession(0, true);
            try
            {
                AspNetUser rec = new AspNetUser();
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    rec = unitOfWork.Users.GetUserById(id);
                    if (rec == null)
                    {
                        // return Json(null, JsonRequestBehavior.AllowGet);
                        var obj = new { RespCode = 1, RespMessage = "Record Not Found" };
                        return Json(obj, JsonRequestBehavior.AllowGet);

                    }
                    //  return Json(rec, JsonRequestBehavior.AllowGet);
                    var obj1 = new { model = rec, RespCode = 0, RespMessage = "Success" };
                    return Json(obj1, JsonRequestBehavior.AllowGet);

                }
                      //repoSession.FindAsync(id);
                

            }
            catch (Exception ex)
            {
                var obj1 = new { RespCode = 2, RespMessage = ex.Message };
                return Json(obj1, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UserList()
        {
            try
            {
                string EnrollmentID = Session["EnrollID"].ToString();
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    var rec =
                   (from a in unitOfWork.Users.GetUsers(EnrollmentID).ToList()
                    join r in unitOfWork.Roles.GetRoles().ToList() on a.RoleId equals r.Roleid
                    select new
                    {
                        ItbId = a.ItbId,
                        LastName = a.LastName,
                        FirstName = a.FirstName,
                        Email = a.Email,
                        Role = r.Roleid,
                        Status = a.Status == "A" ? "Active" : "Close",
                        CreateDate = a.CreateDate,
                    }).ToList();

                    return Json(new { data = rec, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
                }

                

                
            }
            catch (Exception ex)
            {
                var obj1 = new { data = new List<IncomeSource>(), RespCode = 2, RespMessage = ex.Message };
                return Json(obj1, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult ViewUser()
        {
            List<UserModel> Userdata = new List<UserModel>();
            try
            {

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                   // Userdata = unitOfWork.Users.GetUser(Session["EnrollID"].ToString()).ToList();
                }
                return Json(new { data = Userdata, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<User>(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult ViewProfile()
        {
            return View();
        }


        public ActionResult GetViewProfile()
        {
            var profile = new EnrollmentLog();
            try
            {

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    profile = unitOfWork.EnrollLog.GetEnrollDetails(Session["EnrollID"].ToString());
                    var userModel = new UserModel
                    {
                        EnrollmentId = profile.EnrollmentID,
                        Email = profile.Email,
                        FirstName = profile.Firstname,
                        LastName = profile.Lastname,
                        PhoneNo = profile.Phonenumber,
                        Gender = profile.Gender,
                        NoOfChildren = profile.Numberofchildren,
                        Status = profile.Status,
                        MaritalStatus = profile.MaritalStatus,                       
                        City = profile.CityCode,
                                               
                    };
                    return Json(new { data = userModel, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<UserModel>(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
               
        public ActionResult SaveUser(UserModel user)

        {
            string EID = Session["EnrollID"].ToString();
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                var sqlquery = "Update EnrollmentLog set Lastname = @LastName, Firstname = @FirstName, Email = @Email, EnrollmentID = @EnrollmentID, Phonenumber = @PhoneNo, Gender = @Gender, Numberofchildren = @NoOfChildren, Status = @Status, MaritalStatus = @MaritalStatus, CityCode = @City where EnrollmentID = @EnrollmentID";
                {
                    using (SqlCommand cmd = new SqlCommand(sqlquery, con))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@LastName", user.LastName);
                        cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@EnrollmentID", EID);
                        cmd.Parameters.AddWithValue("@PhoneNo", user.PhoneNo);
                        cmd.Parameters.AddWithValue("@Gender", user.Gender);
                        cmd.Parameters.AddWithValue("@NoOfChildren", user.NoOfChildren);
                        cmd.Parameters.AddWithValue("@Status", user.Status);
                        cmd.Parameters.AddWithValue("@MaritalStatus", user.MaritalStatus);
                        cmd.Parameters.AddWithValue("@City", user.City);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
            }
        }
               
            return Json(new { RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetPayHistory()
        {
            var history = new List<TransactionLogModel>();
           

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    history = unitOfWork.TransactionLog.GetTransactionLog(Session["EnrollID"].ToString());

                    if(history != null)
                {
                    return Json(new { data = history, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);

                }

                        return Json(new { data = new List<TransactionLogModel>(), RespCode = -1, RespMessage = "Failed" }, JsonRequestBehavior.AllowGet);


                }


            //catch (Exception ex)
            //{
            //    return Json(new { data = new List<UserModel>(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            //}


        }


        public ActionResult PayHistory()
        {
            return View();
        }


        public ActionResult RolesList()
        {
            try
            {
                var rec = new List<tbl_Roles>();
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    rec = unitOfWork.Roles.GetRoles().ToList();

                }
                return Json(new { data = rec, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var obj1 = new { data = new List<IncomeSourceType>(), RespCode = 2, RespMessage = ex.Message };
                return Json(obj1, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        // [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterViewModel model, string m)
            {
            try
            {

                string title = "";

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    var ct = unitOfWork.Users.GetCountByUserName(model.UserName);
                    if (ct > 0)
                    {
                        return Json(new { RespCode = 1, RespMessage = "UserName already exist" });
                        //return RedirectToAction("ErrorPage", "Account", new { RouteMessage = "Error: UserName already exist" });


                    }
                    var ctE = unitOfWork.Users.GetCountByEmail(model.Email);
                    if (ctE > 0)
                    {
                        return Json(new { RespCode = 1, RespMessage = "E-Mail Address already exist" });
                        // return RedirectToAction("ErrorPage", "Account", new { RouteMessage = "Error: E-Mail Address already exist" });


                    }

                    var NewUID = SmartObj.GenerateRandNum();
                    Random rdn = new Random();
                    int password = rdn.Next(111111, 999999);

                    var user = new AspNetUser
                    {
                        UserName = NewUID,
                        Id = Guid.NewGuid().ToString(),
                        Email = model.Email,
                        LastName = model.LastName,
                        FirstName = model.FirstName,
                        RoleId = model.RoleId,
                        UserId = User.Identity.Name,
                        CreateUserId = User.Identity.Name,
                        CreateDate = DateTime.Now,
                        EnforcePasswordChangeDays = 90,
                        FullName = model.FullName,
                        ForcePassword = true,
                        IsApproved = true,
                        MobileNo = model.MobileNo,
                        Status = "A",
                        DeptCode = null,
                        EnforcePasswordChange ="Y",
                        //PasswordHash = SmartObj.Encrypt(NewUID),
                        PasswordHash = SmartObj.Encrypt(password.ToString()),
                        DeptName = model.DeptName,
                        InstitutionId = model.InstitutionId.GetValueOrDefault(),
                        InstitutionName = model.InstitutionName,
                        RoleName = model.RoleName,
                        Supervisor = null,
                        EnrollmentID = Session["EnrollID"].ToString()//model.EnrollmentID
                    };

                    //var pass = SmartObj.Encrypt(SmartObj.GenerateRandomPassword(8));
                    var pass = SmartObj.Encrypt(NewUID);
                    //user.PasswordHash = pass;// ;
                    unitOfWork.Users.Add(user);
                    unitOfWork.Complete();

                    title = "New User Creation";

                    try
                    {

                        var Email = new EmailerNotification();
                        //Email.SendHtmlFormattedEmail(model.Email, title, body);
                        //  var body = Email.PopulateUserBody(model.Email, null, NewUID, model.FullName);
                        //  Email.SendHtmlFormattedEmail(model.Email, title, body);
                        proc_Notification_Result rec2 = new proc_Notification_Result();
                        rec2 = unitOfWork.AddNotification.AddAlert("ENROLLMENT", model.Email, model.MobileNo, model.UserName + "," + NewUID, model.UserName);
                        if (rec2 == null)
                        {

                            var obj = new { RespCode = 1, RespMessage = "Record Not Found" };
                            return Json(obj, JsonRequestBehavior.AllowGet);

                        }

                        var rec =
                          (from a in unitOfWork.Users.GetById(user.ItbId)
                           join r in unitOfWork.Roles.GetRoles().ToList() on a.RoleId equals r.Roleid
                           select new
                           {
                               ItbId = a.ItbId,
                               LastName = a.LastName,
                               FirstName = a.FirstName,
                               Email = a.Email,
                               RoleName = r.RoleName,
                               Status = a.Status == "A" ? "Active" : "Close",
                               CreateDate = a.CreateDate,
                           }).FirstOrDefault();

                                return Json(new { data = rec, RespCode = 0, RespMessage = "Record Created Successfully" });
                            }
                    catch (Exception ex)
                    {

                    }


                }




            }
            catch (SqlException ex)
            {
                return Json(new { RespCode = 1, RespMessage = ex.Message });
                // return RedirectToAction("ErrorPage", "Account", new { RouteMessage = ex.Message });

            }
            catch (Exception ex)
            {
                return Json(new { RespCode = 1, RespMessage = ex.Message });
                // return RedirectToAction("ErrorPage", "Account", new { RouteMessage = ex.Message });

            }
            return Json(new { RespCode = 1, RespMessage = "An error occured" });
        }
    }
}