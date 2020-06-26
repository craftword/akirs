using Akirs.client.DL;
using Akirs.client.Models;
using Akirs.client.Persistence;
using Akirs.client.utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Akirs.client.Controllers
{
    public class HomeController : Controller
    {
        DateTime time = DateTime.Now;
       
        //private object WebSecurity;

        public ActionResult Index()
        {
            List<TaxType> taxttype = new List<TaxType>();
            try
            {
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    taxttype = unitOfWork.TaxType.GetTaxType().ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return View(taxttype);
        }


        public ActionResult Login()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }


        public ActionResult SendNotification()
        {

            var Email = new EmailerNotification();
            //Email.SendHtmlFormattedEmail(model.Email, title, body);
            // var body2 = Email.PopulateUserBody("realitytaiwo2@gmail.com", "", "", "");
            //Email.SendHtmlFormattedEmail(model.Email, title, body);

            var companyname = ConfigurationManager.AppSettings["company"];

            List<NotificationAlert_Result> rec = new List<NotificationAlert_Result>();

            using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
            {
                rec = unitOfWork.NotificationAlert.GetNotificationAlert().ToList();
                if (rec == null)
                {

                    var obj = new { RespCode = 1, RespMessage = "Record Not Found" };
                    return Json(obj, JsonRequestBehavior.AllowGet);

                }

                foreach (var md in rec)
                {

                    var phoneno = md.smsaddress;
                    var itbidRec = md.itbid;
                    var email = md.emailaddress;
                    var notificationdefault = md.notificationdefaultmsg;
                    string parameter = md.notificationmsg;
                    string[] parameterarray = parameter.Split(new Char[] { ',', '|' });
                    // string[] parameterarray = parameter.Split(new Char[] { ' ', ',', '.', '-', '|', '\n', '\t' });
                    var template = md.notificationTemplate;
                    var provider = md.ProviderID;
                    var reqtemp = md.requireTemplate;
                    var sstype = "EMAIL";// md.Notificationclass;
                    var notificationType = md.notificationType;

                    if (sstype == "SMS")
                    {
                        try
                        {
                            var bosystring = string.Empty;
                            EmailerNotification emailNot = new EmailerNotification();
                            //bosystring = string.Format(body, parameterarray.Select(x=>x.ToString().ToArray()));
                            bosystring = string.Format(template, parameterarray[0], parameterarray[1]);
                            var res = bosystring;
                            //await emailNot.SendSMSAscync(phoneno, companyname, bosystring);
                        }
                        catch (Exception ex)
                        {

                        }

                    }

                    if (sstype == "EMAIL")
                    {
                        try
                        {
                            var bosystring = string.Empty;
                            var EmailBot = new EmailerNotification();
                            //bosystring = string.Format(body, parameterarray.Select(x=>x.ToString().ToArray()));
                            //bosystring = string.Format(template, parameterarray[0], parameterarray[1], parameterarray[2]);
                            var enrollid = unitOfWork.Users.GetByUserName(parameterarray[0]);

                            //if (ctE <= 0)
                            //{
                            //    return Json(new { RespCode = 1, RespMessage = "E-Mail Address does not exist" });
                            //}

                            var pass = SmartObj.Decrypt(enrollid.PasswordHash);
                            var body = EmailBot.PopulateUserBody(parameterarray[0], SmartObj.Decrypt(enrollid.PasswordHash), parameterarray[2], template);

                            EmailBot.SendEmailAscync(email, notificationType + " Notification ", body);


                        }
                        catch (Exception ex)
                        {

                        }

                    }

                    ////var Notification = unitOfWork.NotificationLog.GetAll().Where(y => y.itbid == itbidRec && y.NotificationType == notificationType).FirstOrDefault();
                    ////if (Notification.itbid>0)
                    ////{
                    ////    Notification.Status = "S";
                    ////    unitOfWork.NotificationLog.Update(Notification);

                    ////}

                    unitOfWork.Notification.UpdateNotification(md.itbid);

                }

                // Notification.




                ////unitOfWork.Complete();

            }



            return Json(0, JsonRequestBehavior.AllowGet);
        }

        private string PopulateUserBodyRec(string UserName, string Password, string FullName, string Template)
        {
            string body = string.Empty;
            ////StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(Template));
            ////body = reader.ReadToEnd();
            ////body = body.Replace("{0}", UserName);
            ////body = body.Replace("{1}", Password);
            ////body = body.Replace("{2}", FullName);

            ////return body;

            using (StreamReader reader = new StreamReader(Server.MapPath(Template)))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{0}", UserName);
            body = body.Replace("{1}", Password);
            body = body.Replace("{2}", FullName);

            return body;
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordModel pwdchangemodel)
        {
            using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
            {
                try
                {
                    var users = unitOfWork.Users.GetUserById(pwdchangemodel.Itbid);
                    users.PasswordHash = SmartObj.Encrypt(pwdchangemodel.ChangePassword);
                    users.EnforcePasswordChange = "N";
                    unitOfWork.Users.Update(users);
                    unitOfWork.Complete();
                    return Json(new { RespCode = 0, RespMessage = "Password Changed Successfully" });
                }
                catch (Exception ex)
                {

                }
            }
            return Json(new { RespCode = 1, RespMessage = "An error occured" });

        }
        [HttpPost]
        [AllowAnonymous]
        // [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
            {
                try
                {
                    var enrollid = unitOfWork.Users.GetByUserName(model.UserName);

                    //if (ctE <= 0)
                    //{
                    //    return Json(new { RespCode = 1, RespMessage = "E-Mail Address does not exist" });
                    //}

                    var pass = SmartObj.Encrypt(model.Password);

                    var result1 = unitOfWork.Users.ValidateEnrollment(enrollid.EnrollmentID, pass);
                    if (result1 == null)
                    {
                        return Json(new { RespCode = 1, RespMessage = "Login Password/User Name is InCorrect" });

                    }
                    if (result1.EnforcePasswordChange == "Y")
                    {
                        return Json(new { RespCode = 7, RespMessage = "Password change", Itbid = result1.ItbId });
                    }
                    Session["UserName"] = model.UserName;
                    Session["EnrollID"] = result1.EnrollmentID;
                    Session["RoleId"] = result1.RoleId;
                    var enrolltype = unitOfWork.EnrollLog.GetEnrollDetails(result1.EnrollmentID);
                    Session["Taxtype"] = enrolltype.TaxtypeID;
                    Session["RoleName"] = result1.RoleName;
                    Session["Email"] = result1.Email;

                    return Json(new { RespCode = 0, RespMessage = "" });

                }
                catch (SqlException ex)
                {
                    return Json(new { RespCode = 1, RespMessage = ex.Message });
                    //return RedirectToAction("ErrorPage", "Account", new { RouteMessage = ex.Message });

                }
                catch (Exception ex)
                {
                    return Json(new { RespCode = 1, RespMessage = ex.Message });
                    // return RedirectToAction("ErrorPage", "Account", new { RouteMessage = ex.Message });

                }
            }
        }


        [HttpPost]
        public ActionResult ForgotPassword(string UserName)
        {
            if (ModelState.IsValid)
            {

                if (UserExists(UserName))
                {
                    string To = UserName, UserID, Password, SMTPPort, Host;
                    //string token = GeneratePasswordResetToken(UserName);
                    Random rdn = new Random();
                    int password = rdn.Next(111111, 999999);
                   
                    if (UserName == null)
                    {
                        // If user does not exist or is not confirmed.  

                        return Json(new { RespCode = 1, RespMessage = "Email does not exist" }, JsonRequestBehavior.AllowGet);

                    }
                    else {

                        var emailBot = new EmailerNotification();

                        //Create URL with above token  

                        var lnkHref = "<a href='" + Url.Action("ResetPassword", "Home", new { email = UserName, PasswordHash = password, time = DateTime.Now.ToString() }, "http") + "'>Reset Password</a>";


                        //HTML Template for Send email  




                        string subject = "Your changed password";

                        string body = "<b>Please find the Password Reset Link. </b><br/>" + lnkHref + "<br/> #kindly Note that the link expires if no action is taken after One hour ";
                      


                        //Get and set the AppSettings using configuration manager.  



                        //EmailManager.AppSettings(out UserID, out Password, out SMTPPort, out Host);

                        emailBot.SendEmailAscync(UserName, subject, body);
                        //Call send email methods.  

                        //EmailManager.SendEmail(UserID, subject, body, To, UserID, Password, SMTPPort, Host);
                        

                        //}
                        return Json(new { RespCode = 0, RespMessage = "Email sent, pls check your mail.." }, JsonRequestBehavior.AllowGet);

                    }

                }
                else
                {
                    return Json(new { RespCode = 1, RespMessage = "Email does not exist" }, JsonRequestBehavior.AllowGet);

                }

            }
            return View();

        }


    public Boolean UserExists( string UserName)
        {
            AspNetUser user = new AspNetUser();
            using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
            {
                user = unitOfWork.Users.GetByUserEmail(UserName);
                if (user == null)
                    return false;
                else
                    return true;

            }

        }

    
        public ActionResult ResetPassword(string email, string PasswordHash , string time)
        {
            DateTime linkTime;
            DateTime.TryParse(time, out linkTime);

            var today = DateTime.Now;

            if (today.Subtract(linkTime) >= TimeSpan.FromMinutes(60))
            {
                //60 minutes were passed from start
                return RedirectToAction("ExpiredLink", "Home");
            }
            else
            {

                Session["ResetEmail"] = email;
                return View();
            }


        }


        public ActionResult ExpiredLink() {
            return View();
        }



        [HttpPost]
        public JsonResult UpdatePassword(string email, string password)
        {
            try
            {
                string passwordhash = SmartObj.Encrypt(password);
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    var result = unitOfWork.Users.ResetPassword(email, passwordhash);
                    if(result == true)
                    {
                        return Json(new { RespCode = 0, RespMessage = "Password Changed Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new { RespCode = 1, RespMessage = "Internal Server Error Occured" }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { RespCode = 1, RespMessage = "Internal Server Error Occured" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssessmentDetails(string EnrollID)
        {

            try
            {
                List<proc_computeAssessment_Result> Assessmentdata = new List<proc_computeAssessment_Result>();
                List<SALARYUPLOAD> salary = new List<SALARYUPLOAD>();
                //returns a list of salary
                // list of salary in step 2 and pass enrol_id and salary_count into function below
                //return list and add to an assessment data list, then return the assement data list to jquery
                //from jquery loop returned list above for each div elements 


                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {


                    salary = unitOfWork.SalaryHistory.GetSalaryUpload(EnrollID).ToList();

                    Assessmentdata = unitOfWork.Assessment.GetAssessmentSingleWork(EnrollID, string.Format("{0:yyyy}",DateTime.Now)).ToList();

                    //   Assessmentdata = unitOfWork.Assessment.GetAssessmentSingle(EnrollID);
                    //   Assessmentdata = unitOfWork.Assessment.GetAssessment(EnrollID,counter).ToList();
                    // Assessmentdata = unitOfWork.Assessment.GetAssessment(EnrollID).ToList();
                }

                return Json(new { data = Assessmentdata, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new proc_computeAssessment_Result(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Json(data);
        }
    }    
}