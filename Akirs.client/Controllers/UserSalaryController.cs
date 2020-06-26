using Akirs.client.DL;
using Akirs.client.Persistence;
using Akirs.client.utility;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Akirs.client.Models;
using Akirs.client.Enums;
using Akirs.client.ViewModel;
using System.Threading.Tasks;

namespace Akirs.client.Controllers
{
    [SessionExpire]
    public class UserSalaryController : Controller
    {

      
        //string _EnrollId = string.Empty;
        public UserSalaryController()
        {
            
            //_EnrollId = Session["EnrollID"].ToString();
        }
        // GET: Salary
        public ActionResult Index()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    var model = new SALARYUPLOAD();
                    var rec = unitOfWork.SalaryUpload.GetPendingSalaryUpload(Session["EnrollID"].ToString()).ToList();

                    var entity = new MultipleSalaryUploadViewModel
                    {
                        Salary = model,
                        SalaryList = rec
                    };
                   
                    return View(entity);
                }
            }
            catch (Exception ex)
            {
                return Json(new { RespCode = 1, RespMessage = "Problem Processing Validation. " }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult DetailsForEdit(SALARYUPLOAD model)
        {
            var EnrollId = Session["EnrollId"].ToString();

            using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
            {
                var rec = unitOfWork.SalaryUpload.GetDetailsForEdit(EnrollId, model);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult ApprovePayrollEmployee()
        {
            using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
            {
                //IList<SalaryuploadObj> model = null;
                var EnrollId = Session["EnrollId"].ToString();
                try
                {
                 var result = unitOfWork.SalaryUpload.ApprovePayrollByEmployeeId(EnrollId);

                    TempData["AlertMessage"] = result;
                    return RedirectToAction("VerifyList");
                }
                catch (Exception ex)
                {
                    TempData["AlertMessage"] = "Error while approving " + ex.Message;
                    return RedirectToAction("VerifyList");
                }
            }
           
        }

        [HttpPost]
        public ActionResult SendPayeeForApproval()
        {
            using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
            {
                //IList<SalaryuploadObj> model = null;
                var EnrollId = Session["EnrollId"].ToString();
                try
                {
                    var result = unitOfWork.SalaryUpload.SendPayeeForApproval(EnrollId);

                    TempData["AlertMessage"] = result;
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["AlertMessage"] = "Error while approving " + ex.Message;
                    return RedirectToAction("Index");
                }
            }

        }

        public ActionResult RejectPayrollEmployee()
        {
            using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
            {
                //IList<SalaryuploadObj> model = null;
                var EnrollId = Session["EnrollId"].ToString();
                try
                {
                    var result = unitOfWork.SalaryUpload.RejectPayrollEmployee(EnrollId);

                    TempData["AlertMessage"] = result;
                    return RedirectToAction("VerifyList");
                }
                catch (Exception ex)
                {
                    TempData["AlertMessage"] = "Error while approving " + ex.Message;
                    return RedirectToAction("VerifyList");
                }
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SALARYUPLOAD model)
        {
            try
            {
                var errorMsg = "";
                if (ModelState.IsValid)
                {
                    if (model.ItbID == 0)
                    {
                        model.EnrollmentID = Session["EnrollID"].ToString();
                        model.PayrollStatus = PayrollStatus.APPROVED.ToString() ;
                        model.CreateDate = DateTime.Now;
                        model.CreatedBy = User.Identity.Name;

                        using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                        {
                            unitOfWork.SalaryUpload.Add(model);
                            unitOfWork.Complete();
                        }

                        return Json(new { data = model, RespCode = 0, RespMessage = "Record Created Successfully" });


                    }
                    else
                    {
                        using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                        {
                            model.PayrollStatus = PayrollStatus.APPROVED.ToString();
                            model.EnrollmentID = Session["EnrollID"].ToString();
                            unitOfWork.SalaryUpload.Add(model);
                            unitOfWork.SalaryUpload.Update(model);
                            unitOfWork.Complete();
                        }

                        return Json(new { data = model, RespCode = 0, RespMessage = "Record Updated Successfully" });


                    }
                }
                // If we got this far, something failed, redisplay form
                return Json(new { RespCode = 1, RespMessage = errorMsg });
            }
            catch (SqlException ex)
            {
                return Json(new { RespCode = 1, RespMessage = ex.Message });
            }
            catch (Exception ex)
            {
                return Json(new { RespCode = 1, RespMessage = ex.Message });
            }
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
                var rec = new SALARYUPLOAD();
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    rec = unitOfWork.SalaryUpload.GetSalaryUploadById(id);
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

        public JsonResult DeleteDetail(int id = 0)
        {
            if (id == 0)
            {
                return Json(new { RespCode = 99, RespMessage = "Bad Request" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                var rec = new SALARYUPLOAD();
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    rec = unitOfWork.SalaryUpload.GetSalaryUploadById(id);
                    if (rec == null)
                    {
                        var obj = new { RespCode = 1, RespMessage = "Record Not Found" };
                        return Json(obj, JsonRequestBehavior.AllowGet);

                    }
                    unitOfWork.SalaryUpload.Remove(rec);
                    unitOfWork.Complete();
                    var obj1 = new { model = rec, RespCode = 0, RespMessage = "Success" };
                    return Json(obj1, JsonRequestBehavior.AllowGet);

                }

            }
            catch (Exception ex)
            {
                var obj1 = new { RespCode = 2, RespMessage = ex.Message };
                return Json(obj1, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult SalList()
        {
            var salarydata = new List<SALARYUPLOAD>();
            try
            {

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    salarydata = unitOfWork.SalaryUpload.GetPendingSalaryUpload(Session["EnrollID"].ToString()).ToList();
                }
                return Json(new { data = salarydata, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<Salaryupload_temp>(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Json(data);
        }
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult UploadFiles()
        {
            IList<SalaryuploadObj> model = null;
            List<SALARYUPLOAD> saveuplList = null;
            var SeesionID = Session["EnrollID"].ToString();
            try
            {
                var rc = Request.Files;
                //  var dd = Request.Form["requestType"];
                if (rc != null)
                {
                    var file = rc[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        var stream = file.InputStream;
                        var fileName = Path.GetFileName(file.FileName);
                        var ext = Path.GetExtension(file.FileName);
                        if (ext != ".xlsx")
                        {
                            return Json(new { RespCode = 1, RespMessage = "Please Upload Using .xlsx file" });
                        }
                        using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                        {
                            saveuplList = unitOfWork.SalaryUpload.UploadPayroll(file, SeesionID);
                        }

                        Session["Salary"] = saveuplList;

                        int cnt = saveuplList.Count();
                        if (cnt > 0)
                        {
                            var html = PartialView("_salaryUpld", saveuplList).RenderToString();
                            return Json(new { data_html = html, RespCode = 0, RespMessage = "File Uploaded Succesfully" });
                        }
                        else
                        {
                            var html = PartialView("_salaryUpld").RenderToString();
                            return Json(new { RespCode = 1, RespMessage = "Problem processing file upload." });
                        }
                    }
                }
                // If we got this far, something failed, redisplay form
                //return Json(new { RespCode = 1, RespMessage = errorMsg });
            }
            catch (SqlException ex)
            {
                return Json(new { data = model, RespCode = 1, RespMessage = "Error Occured While Uploading. Please Try Again" });
            }
            catch (Exception ex)
            {
                return Json(new { data = model, RespCode = 1, RespMessage = "Error Occured While Uploading." + ex.Message });
            }
            return Json(new { data = model, BatchId = "", RespCode = 0, RespMessage = "File Uploaded Successfully" });
        }

        public JsonResult GetPaymentSampleTemplate(short Itbid)
        {
            return Json(new { Itbid = Itbid });
        }
       
        [HttpPost]
        public ActionResult VerifyRecord()
        {
            try
            {
                SALARYUPLOAD salup = null;
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    var rec = unitOfWork.SalaryUpload.GetSalaryUpload(Session["EnrollID"].ToString()).ToList();
                    foreach (var item in rec)
                    {
                        salup = new SALARYUPLOAD();


                        salup.AnnualBasic = item.AnnualBasic;
                        salup.AnnualHousing = item.AnnualHousing;
                        salup.AnnualMeal = item.AnnualMeal;
                        salup.AnnualOthers = item.AnnualOthers;
                        salup.AnnualTransport = item.AnnualTransport;
                        salup.AuthUserID = item.AuthUserID;
                        salup.CreateDate = item.CreateDate;
                        salup.CreatedBy = item.CreatedBy;
                        salup.EmployeeID = item.EmployeeID;
                        salup.EmployeeName = item.EmployeeName;
                        salup.GrossPay = item.GrossPay;
                        salup.Last_Modified_Authid = item.Last_Modified_Authid;
                        salup.Last_Modified_Date = item.Last_Modified_Date;
                        salup.Last_Modified_Uid = item.Last_Modified_Uid;
                        salup.NHFContribution = item.NHFContribution;
                        salup.PayrollStatus = PayrollStatus.APPROVED.ToString();
                        salup.VALIDATIONERRORSTATUS = item.VALIDATIONERRORSTATUS;
                        salup.EnrollmentID = item.EnrollmentID;

                        item.PayrollStatus = PayrollStatus.PENDING.ToString();

                        unitOfWork.SalaryUpload.Add(item);
                        unitOfWork.SalaryUpload.Update(item);
                        //repoSalarytemp.Update(item);
                        unitOfWork.SalaryHistory.Add(salup);                        //repoSalary.Insert(salup);
                    }
                    unitOfWork.Complete();

                    var dr = unitOfWork.SalaryUpload.GetPendingSalaryUpload(Session["EnrollID"].ToString()).ToList();
                    var html = PartialView("_verifysalary", dr).RenderToString();
                    return Json(new { data_html = html, RespCode = 0, RespMessage = "Record " }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { RespCode = 1, RespMessage = "Problem Processing Validation. " });
            }
        }
       
        public ActionResult VerifyList()
        {
            return View();
        }
        [HttpGet]
        public ActionResult VerifySalary()
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    var rec = unitOfWork.SalaryUpload.GetSalaryUpload(Session["EnrollID"].ToString()).ToList();
                    var html = PartialView("_verifySalary", rec).RenderToString();
                    return Json(new { data_html = html, RespCode = 0, RespMessage = "Record " }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(new { RespCode = 1, RespMessage = "Problem Processing Validation. " }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public virtual ActionResult PaymentTemplateProcess(short Itbid)
        {

            string file = HostingEnvironment.MapPath("~/Template/salaryupload.xlsx");
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(file, contentType, Path.GetFileName(file));
        }
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult Validate(string EnrollID)
        {
            try
            {
                EnrollID = Session["EnrollID"].ToString();
                var rec = new List<SALARYUPLOAD>();
                var errCount = 0;
                var sucCount = 0;
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    errCount = ValidateUpload(EnrollID);
                    rec = unitOfWork.SalaryUpload.GetSalaryUpload(EnrollID).ToList();
                    sucCount = rec.Count - errCount;
                }
                var html = PartialView("_salaryUpld", rec).RenderToString();
                return Json(new { data_html = html, RespCode = 0, RespMessage = "Record ", SucCount = sucCount, FailCount = errCount });
            }
            catch (Exception ex)
            {
                return Json(new { RespCode = 1, RespMessage = "Problem Processing Validation. " });
            }
        }
        protected int ValidateUpload(string enrollID)
        {
            var rv = new Salaryupload_temp();

            //try
            //{
            //  int enrollID;
            int CompanyID;
            string Companyname = "";
            var rec = (List<SALARYUPLOAD>)Session["Salary"];
            int totalErrorCount = 0;
            List<SALARYUPLOAD> lst = new List<SALARYUPLOAD>();

            int midNameLength = 150;
            using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
            {
                var t = unitOfWork.EnrollLog.GetEnrollDetails(enrollID);
                if (t != null)
                {
                    CompanyID =(int)  t.CompanyID;
                    Companyname = t.Companyname;
                }


               


                //var rec = repoSalarytemp.Query().Where(m => m.CreatedBy == User.Identity.Name && m.EnrollmentID == enrollID && m.Status == "A");

                
                foreach (var tm in rec)
                {
                    int errorCount = 0;
                    var validationErrorMessage = new List<string>();
                    decimal amountrec;
                    int specialCount = 0;
                    // validating merchant id  (1)


                    // Match match = Regex.Match(t.MERCHANTID, "^a-z0-9", RegexOptions.IgnoreCase);
                    if (Regex.IsMatch(tm.EmployeeID, "[^a-z0-9]", RegexOptions.IgnoreCase))
                    {
                        specialCount++;
                        //match.NextMatch();
                    }
                    if (specialCount > 0)
                    {
                        errorCount++;
                        // totalErrorCount++;
                        validationErrorMessage.Add(string.Format("Special Character is not allowed for Employee ID"));
                    }

                    if (Regex.IsMatch(tm.EmployeeName, "[^a-z0-9]", RegexOptions.IgnoreCase))
                    {
                        specialCount++;
                        //match.NextMatch();
                    }
                    if (specialCount > 0)
                    {
                        errorCount++;
                        // totalErrorCount++;
                        validationErrorMessage.Add(string.Format("Special Character is not allowed for Employee name"));
                    }



                    if (!string.IsNullOrEmpty(tm.EmployeeName))
                    {

                        if (tm.EmployeeName.Length > midNameLength)
                        {
                            errorCount++;
                            //  totalErrorCount++;
                            validationErrorMessage.Add(string.Format("Merchant Id Lenght must not be more than {0}", midNameLength));

                        }

                    }

                    if (!decimal.TryParse(tm.AnnualBasic.ToString(), out amountrec))
                    {
                        errorCount++;
                        //  totalErrorCount++;
                        validationErrorMessage.Add(string.Format("Employee Annual Basic Salary must be a number"));
                    }

                    if (!decimal.TryParse(tm.AnnualHousing.ToString(), out amountrec))
                    {
                        errorCount++;
                        //  totalErrorCount++;
                        validationErrorMessage.Add(string.Format("Employee Annual Housing Salary must be a number"));
                    }

                    if (!decimal.TryParse(tm.AnnualMeal.ToString(), out amountrec))
                    {
                        errorCount++;
                        //  totalErrorCount++;
                        validationErrorMessage.Add(string.Format("Employee Annual Meal Salary must be a number"));
                    }

                    if (!decimal.TryParse(tm.AnnualTransport.ToString(), out amountrec))
                    {
                        errorCount++;
                        //  totalErrorCount++;
                        validationErrorMessage.Add(string.Format("Employee Annual Transport Salary must be a number"));
                    }

                    if (!decimal.TryParse(tm.AnnualOthers.ToString(), out amountrec))
                    {
                        errorCount++;
                        //  totalErrorCount++;
                        validationErrorMessage.Add(string.Format("Employee Annual Other Salary must be a number"));
                    }


                    if (errorCount == 0)
                    {
                        tm.VALIDATIONERRORSTATUS = false;
                       // tm.VALIDATIONERRORMESSAGE = "";
                        tm.PayrollStatus = PayrollStatus.APPROVED.ToString();
                    }
                    else
                    {
                        totalErrorCount++;
                        tm.VALIDATIONERRORSTATUS = true;
                       // tm.VALIDATIONERRORMESSAGE = GetStringFromList(validationErrorMessage);
                    }

                    unitOfWork.SalaryUpload.Add(tm);
                }
                unitOfWork.Complete();
            }


            return totalErrorCount;

        }
        string GetStringFromList(List<string> val)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"<div style=""color:red;font-size:11px"">");
            foreach (var d in val)
            {
                sb.AppendLine(@"<i class=""fa-arrow-right fa""> </i> " + d + "<br/>");
            }
            sb.AppendLine("</div>");
            var l = sb.ToString();
            return l;
        }

        public JsonResult GetUserById()
          {
           var EnrollID = Session["EnrollID"].ToString();
            try
            {
                var user = new AspNetUser();
                var userM = new UserModel();


                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities())) {

                    user = unitOfWork.Users.GetUser(EnrollID);

                    var userModel = new UserModel
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNo = user.MobileNo,
                        EnrollmentId = user.EnrollmentID

                    };


                    if (user != null)
                    {
                        return Json(new { data = userModel, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { data = new List<UserModel>(), RespCode = -2, RespMessage = "Null/No Data" }, JsonRequestBehavior.AllowGet);
                    }

                }
              
            }
            catch (Exception ex)
            {
                var obj1 = new { data = new List<UserModel>(), RespCode = -1, RespMessage = ex.Message };
                return Json(obj1, JsonRequestBehavior.AllowGet);
            }

            //System.Threading.Thread.Sleep(200);
           
        }

        public ActionResult CreateWitholdTaxUser()
        {
            return View();
        }

        public ActionResult CreatePayeeUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(CreatePayeeUserViewModel model)
        {
            string entity = null;

            if (ModelState.IsValid)
            {
                try
                {
                    using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                    {
                        entity = unitOfWork.SalaryUpload.CreatePayeeUser(model);
                    }
                }
                catch(Exception ex)
                {
                    TempData["AlertMessage"] = "Error while Creating User";
                    return RedirectToAction("CreatePayeeUser");
                }
            }
            TempData["AlertMessage"] = entity;
            return RedirectToAction("CreatePayeeUser");
        }


        public ActionResult GetPayeeUserForDelete()
        {

            var EnrollId = Session["EnrollId"].ToString();

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    var entity = unitOfWork.SalaryUpload.GetPayeeUserForDelete(EnrollId);
                     return View(entity);
                }
        }

        [HttpPost]
        public ActionResult DeletePayeeUser(string emailAddress)
        {
            //string emailAddress = Session["Email"].ToString();
            using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
            {
                var entity = unitOfWork.SalaryUpload.DeletePayeeUser(emailAddress);

                TempData["AlertMessage"] = entity;
                return RedirectToAction("GetPayeeUserForDelete");
            }
        }
    }
}