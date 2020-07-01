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
            return View();

        }
        public ActionResult ModifyList()
        {
            return View();
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
                bool recordsaved = false;
                var EnrollId = Session["EnrollId"].ToString();
                try
                {
                    var result = unitOfWork.SalaryUpload.GetPendingSalaryUpload(EnrollId);

                    foreach (var employee in result)
                    {
                        employee.PayrollStatus = PayrollStatus.APPROVED.ToString();

                        unitOfWork.SalaryUpload.Update(employee);

                    }
                    recordsaved = unitOfWork.Complete() > 0 ? true : false;
                    if (recordsaved)
                    {
                        var assessment = unitOfWork.Assessment.GetAssessmentSingleWork(EnrollId, null);

                        if (assessment.Count() > 0)
                            return Json(new { RespCode = 0, RespMessage = "Record approved. Go to the assessment" });
                    }
                    return Json(new { RespCode = -1, RespMessage = "Cannot approve at this time. Try again later" });
                }
                catch (Exception ex)
                {
                    return Json(new { RespCode = -1, RespMessage = ex.Message });
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
                bool recordsaved = false;
                if (ModelState.IsValid)
                {
                    if (model.ItbID == 0)
                    {
                        model.EnrollmentID = Session["EnrollID"].ToString();
                        model.PayrollStatus = PayrollStatus.APPROVED.ToString();
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
                            //model.PayrollStatus = PayrollStatus.APPROVED.ToString();
                            // model.EnrollmentID = Session["EnrollID"].ToString();
                            SALARYUPLOAD modeluspdate = unitOfWork.SalaryUpload.SingleOrDefault(p => p.ItbID == model.ItbID);
                            modeluspdate.NHFContribution = model.NHFContribution;
                            modeluspdate.AnnualBasic = model.AnnualBasic;
                            modeluspdate.AnnualHousing = model.AnnualHousing;
                            modeluspdate.AnnualTransport = model.AnnualTransport;
                            modeluspdate.AnnualMeal = model.AnnualMeal;
                            modeluspdate.AnnualOthers = model.AnnualOthers;


                            unitOfWork.SalaryUpload.Update(modeluspdate);
                            recordsaved = unitOfWork.Complete() > 0 ? true : false;
                        }
                        if (recordsaved)
                        {
                            return Json(new { data = model, RespCode = 0, RespMessage = "Record Updated Successfully" });
                        }
                        else
                        {
                            return Json(new { data = model, RespCode = -1, RespMessage = "Record Not Updated Successfully" });
                        }



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
        [HttpPost]
        public JsonResult SkipPay()
        {

            try
            {
                var rec = new List<SALARYUPLOAD>();
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    rec = unitOfWork.SalaryUpload.SkipPay(Session["EnrollID"].ToString());
                    if (rec == null)
                    {

                        var obj = new { RespCode = -1, RespMessage = "Record Not Found" };
                        return Json(obj, JsonRequestBehavior.AllowGet);

                    }
                    var savedvalue = false;

                    foreach (var item in rec)
                    {

                        unitOfWork.SalaryUploadSecondary.Add(new SALARYUPLOAD_HISTORY
                        {
                            AnnualBasic = item.AnnualBasic,
                            AnnualHousing = item.AnnualHousing,
                            AnnualMeal = item.AnnualMeal,
                            AnnualOthers = item.AnnualOthers,
                            AnnualTransport = item.AnnualTransport,
                            AuthUserID = item.AuthUserID,
                            Counter = item.Counter,
                            CreateDate = DateTime.Now,
                            CreatedBy = item.CreatedBy,
                            EmployeeID = item.EmployeeID,
                            EmployeeName = item.EmployeeName,
                            EnrollmentID = item.EnrollmentID,
                            GrossPay = item.GrossPay,
                            IsDeleted = item.IsDeleted,
                            Last_Modified_Authid = item.Last_Modified_Authid,
                            Last_Modified_Date = item.Last_Modified_Date,
                            Last_Modified_Uid = item.Last_Modified_Uid,
                            NextUploadDate = item.NextUploadDate,
                            NHFContribution = item.NHFContribution,
                            NHIS = item.NHIS,
                            Others = item.Others,
                            PayrollStatus = "SKIPPED",
                            Pension = item.Pension,
                            Premium = item.Premium,
                            UploadMonthIndex = item.UploadMonthIndex,
                            UploadYear = DateTime.Now.Year,
                            VALIDATIONERRORSTATUS = item.VALIDATIONERRORSTATUS
                        });
                    }


                    savedvalue = unitOfWork.Complete() > 0 ? true : false;
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
                return Json(new { data = salarydata, RespCode = salarydata.Count > 0 ? 0 : -1, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<Salaryupload_temp>(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Json(data);GetSalaryDate
        }
        [HttpGet]
        public ActionResult GetdefaultRecord()
        {
            var salarydata = new List<SALARYUPLOAD>();
            try
            {

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    salarydata = unitOfWork.SalaryUpload.GetDefaultSalaryList(Session["EnrollID"].ToString()).ToList();
                }
                return Json(new { data = salarydata, RespCode = salarydata.Count > 0 ? 0 : -1, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<Salaryupload_temp>(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Json(data);GetSalaryDate
        }
        [HttpPost]
        public ActionResult ModifySalary(string datemonth)
        {
            var salarydata = new List<SALARYUPLOAD_HISTORY>();
            var salary = new List<SALARYUPLOAD>();
            var months = new List<int>();
            try
            {
                var splitedstring = datemonth.Split('-');
                short month = Convert.ToInt16(splitedstring[0]);
                int year = Convert.ToInt32(splitedstring[1]);

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    salary = unitOfWork.SalaryUpload.GetSalaryUpload(Session["EnrollID"].ToString());
                    if (salary.Count > 0)
                    {
                        return Json(new { RespCode = -1, RespMessage = "You cannot modify while having a pending transaction" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        salarydata = unitOfWork.SalaryUploadSecondary.GetSalaryUploadSecondary(Session["EnrollID"].ToString(), month, year);


                        foreach (var item in salarydata)
                        {
                            unitOfWork.SalaryUpload.Add(new SALARYUPLOAD()
                            {
                                AnnualBasic = item.AnnualBasic,
                                AnnualHousing = item.AnnualHousing,
                                AnnualMeal = item.AnnualMeal,
                                AnnualOthers = item.AnnualOthers,
                                AnnualTransport = item.AnnualTransport,
                                AuthUserID = item.AuthUserID,
                                Counter = item.Counter,
                                CreateDate = DateTime.Now,
                                EmployeeID = item.EmployeeID,
                                EmployeeName = item.EmployeeName,
                                EnrollmentID = item.EnrollmentID,
                                //GrossPay = item.GrossPay,
                                IsDeleted = item.IsDeleted,
                                NHFContribution = item.NHFContribution,
                                NHIS = item.NHIS,
                                Others = item.Others,
                                PayrollStatus = "PENDING",
                                Pension = item.Pension,
                                Premium = item.Premium,
                                UploadMonthIndex = item.UploadMonthIndex,
                                UploadYear = item.UploadYear,
                            });
                        }

                    }
                    var retValue = unitOfWork.Complete() > 0 ? true : false;
                    if (retValue)
                    {
                        return Json(new { RespCode = 0, RespMessage = "Data ready to be modified. check in upload" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { RespCode = -1, RespMessage = "An error occured" }, JsonRequestBehavior.AllowGet);
                    }



                }

            }
            catch (Exception ex)
            {
                return Json(new { data = new List<int?>(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult GetSalaryhistory(string datemonth)
        {
            var salarydata = new List<SALARYUPLOAD_HISTORY>();
            var months = new List<int>();
            try
            {
                var splitedstring = datemonth.Split('-');
                short month = Convert.ToInt16(splitedstring[0]);
                int year = Convert.ToInt32(splitedstring[1]);

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    salarydata = unitOfWork.SalaryUploadSecondary.GetSalaryUploadSecondary(Session["EnrollID"].ToString(), month, year);


                }
                return Json(new { data = salarydata, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<int?>(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult GetSalaryDate()
        {
            var salarydata = new List<short?>();
            var months = new List<short>();
            var year = new List<int?>();
            List<SalaryDate> salarydate = new List<SalaryDate>();
            try
            {

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    for (short i = 1; i <= 12; i++)
                    {
                        salarydate.Add(new SalaryDate()
                        {
                            monthIndex = i,
                            year = DateTime.Now.Year
                        });
                        //months.Add(i);
                    }


                }
                return Json(new { data = salarydate, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<short?>(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Json(data);
        }
        [HttpGet]
        public ActionResult SalaryMonth()
        {
            var salarydata = new List<short?>();
            var months = new List<short>();
            try
            {

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    salarydata = unitOfWork.SalaryUploadSecondary.GetMonths(Session["EnrollID"].ToString());
                    if (salarydata != null)
                    {
                        for (short i = 1; i <= 12; i++)
                        {
                            if (!salarydata.Contains(i))
                                months.Add(i);
                        }


                    }
                    else
                    {
                        for (short i = 1; i <= 12; i++)
                        {
                            months.Add(i);
                        }
                    }

                }
                return Json(new { data = months, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<short?>(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Json(data);
        }
        [HttpGet]
        public ActionResult SalaryYear()
        {
            var salarydata = new List<int?>();
            var months = new List<int>();
            try
            {

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    salarydata = unitOfWork.SalaryUploadSecondary.GetYears(Session["EnrollID"].ToString());


                }
                return Json(new { data = salarydata, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<int?>(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Json(data);
        }

        [HttpPost]
        public ActionResult CompleteModification()
        {
            bool savedrecord = false;
            var salarydata = new List<SALARYUPLOAD>();
            try
            {

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    salarydata = unitOfWork.SalaryUpload.GetSalaryUpload(Session["EnrollID"].ToString());

                    foreach (var item in salarydata)
                    {
                        item.PayrollStatus = PayrollStatus.AWAITINGAPPROVAL.ToString();

                        unitOfWork.SalaryUpload.Update(item);
                    }

                    savedrecord = unitOfWork.Complete() > 0 ? true : false;

                }
                if (savedrecord)
                    return Json(new { RespCode = 0, RespMessage = "Record saved sucessfully" }, JsonRequestBehavior.AllowGet);
                else
                {
                    return Json(new { RespCode = -1, RespMessage = "Failed" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<int?>(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
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
                    var monthindex = Request.Form[0];
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
                            saveuplList = unitOfWork.SalaryUpload.UploadPayroll(file, SeesionID, Convert.ToInt16(monthindex));
                        }



                        int cnt = saveuplList.Count();
                        if (cnt > 0)
                        {
                            var html = PartialView("_salaryUpld", saveuplList).RenderToString();
                            Session["Salary"] = saveuplList;
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
                    CompanyID = (int)t.CompanyID;
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


                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {

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
                catch (Exception ex)
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