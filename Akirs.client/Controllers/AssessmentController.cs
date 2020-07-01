using Akirs.client.DL;
using Akirs.client.Models;
using Akirs.client.Persistence;
using Akirs.client.utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Akirs.client.Controllers
{
    public class AssessmentController : Controller
    {

      
        AKIRSTAXEntities db = new AKIRSTAXEntities();
        private object IncomeYear;
        
        public string _ConnectionString { get; private set; }
        public object _context { get; private set; }
        //public object NewCheck { get; private set; }

        public AssessmentController()
        {
            //EnrollId = Session["EnrollID"].ToString();
        }
        // GET: Assessment
        public ActionResult Index()
        {
            //var enrollId = Session["EnrollID"];

            //if( enrollId != null)
            //{
            //    ViewBag.HasRecord = 0;
            //    using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
            //    {
            //        var Cass = unitOfWork.PayrollData.GetCountPayrollData(Session["EnrollID"].ToString());
            //        if (Cass > 0)
            //        {
            //            ViewBag.HasRecord = 1;
            //        }
            //        var taxttype = unitOfWork.EnrollLog.GetEnrollDetails(Session["EnrollID"].ToString());
            //        ViewBag.Taxtype = taxttype;

            //    }
            //}
          
            return View();
        }

        public JsonResult checkCompanyData(string email)
        {
            System.Threading.Thread.Sleep(200);
            var SearchData = db.CompanyDatas.Where(x => x.CompanyEmail == email).FirstOrDefault();
            if (SearchData != null)
            {
                return Json(new { data = SearchData, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = new JsonResult(), RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult checkUserData(string EnrollId)

       {
            //System.Threading.Thread.Sleep(200);
            var SearchData = db.AspNetUsers.Where(x => x.EnrollmentID == EnrollId).FirstOrDefault();
            if (SearchData != null)
            {
                return Json(new { data = SearchData, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { data = new JsonResult(), RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetEmail(string EnrollID)
        {
            //stem.Threading.Thread.Sleep(200);
            var SearchData = db.AspNetUsers.Where(x => x.EnrollmentID == EnrollID).FirstOrDefault();
            if (SearchData != null)
            {
                var email = SearchData.Email;
                return Json(new { data = email, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                
                return Json(new { data = new JsonResult(), RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult AdditionalAssessment()
        {
            return View();
        }


        public JsonResult GetNetTaxPaid(string yearValue)
        {
            IncomePayment PaymentMsg = new IncomePayment();
            string eID = Session["EnrollID"].ToString();
            PaymentMsg = db.IncomePayments.Where(j => j.EnrollmentID == eID && j.IncomeYear == yearValue).OrderByDescending(m => m.Count).FirstOrDefault();

            try
            {

                var NetTaxPaid = db.IncomePayments.Where(x => x.EnrollmentID == eID && x.Count == PaymentMsg.Count && x.IncomeYear == yearValue).Select(c => c.AmountPaid).Distinct().FirstOrDefault();
                //return Json(new { data = NetTaxPaid, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);

                if (NetTaxPaid != null)
                {
                    //var email = SearchData.Email;
                    return Json(new { data = NetTaxPaid, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    return Json(new { data = new JsonResult(), RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //stem.Threading.Thread.Sleep(200);
           
        }


        public ActionResult AssessmentDetailsTax()
        {
           List<AssessmentRecord> Assessmentdata = new List<AssessmentRecord>();
            try
            {
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    string eID = Session["EnrollID"].ToString();
                    Assessmentdata = unitOfWork.Assessment.GetAssessmentSingleFirst(Session["EnrollID"].ToString()).ToList();


                }
                return Json(new { data = Assessmentdata, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new proc_computeAssessment_Result(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Json(data);
        }


        public ActionResult GetDirectAssessmentDetailsTax()
        {
            AssessmentRecord Assessmentdata = new AssessmentRecord();
            EnrollmentLog EnrollmentLogRecord = new EnrollmentLog();

            try
            {
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    string eID = Session["EnrollID"].ToString();
                    Assessmentdata = unitOfWork.Assessment.GetDirectAssessmentSingleFirst(Session["EnrollID"].ToString());
                    EnrollmentLogRecord = unitOfWork.EnrollLog.GetEnrollDetails(Session["EnrollID"].ToString());
                    Assessmentdata.EmployeeName = EnrollmentLogRecord.Firstname + " " + EnrollmentLogRecord.Lastname;
                    //Assessmentdata.
                    //Assessmentdata. = EnrollmentLogRecord.Phonenumber


                }
                return Json(new { data = Assessmentdata, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new AssessmentRecord(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Json(data);
        }

        [HttpGet]
        public ActionResult IncomeYears()
        {
            
            return View();
        }


        public ActionResult GetIncomeYear()
        {
            //GetIncomeYearModel model = new GetIncomeYearModel();
            //return View(model);
            //proc_computeAssessment_Result Assessmentdata = new proc_computeAssessment_Result();
            try
            {
                List<string> IncomeYear = new List<string>();

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {

                    string eID = Session["EnrollID"].ToString();
                    //Assessmentdata = unitOfWork.Assessment.GetAssessmentSingle(Session["EnrollID"].ToString());

                    IncomeYear = db.IncomeSources.Where(j => j.EnrollmentID == eID && j.Status == "P").Select(c => c.IncomeYear).Distinct().ToList();
                                       
                }
                
                //return View(IncomeYear);
                return Json(new { data = IncomeYear, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult AssessmentDetails(string yearValue)
        {
            string eID = Session["EnrollID"].ToString();


             var NewCheck = db.IncomeSources.Where(j => j.EnrollmentID == eID && j.Status == "P" && j.IncomeYear == yearValue && j.IsNew == "Y").ToList();
           
            if (NewCheck.Count >= 1) {

                proc_computeAssessment_Result Assessmentdata = new proc_computeAssessment_Result();
                try
                {
                    using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                    {

                        Assessmentdata = unitOfWork.Assessment.GetAssessmentSingle(Session["EnrollID"].ToString(), yearValue);
                    }
                    return Json(new { data = Assessmentdata, RespCode = 0, isNew = true, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { data = new proc_computeAssessment_Result(), RespCode = 2, isNew = true, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                //return Json(data);
            }
            else
            {
                proc_computeAssessment_modification_Result Assessmentdata = new proc_computeAssessment_modification_Result();
                try
                {
                    using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                    {

                        Assessmentdata = unitOfWork.Assessment.GetAssessmentSingle2(Session["EnrollID"].ToString(), yearValue);
                    }
                    if(Assessmentdata == null)
                    {
                        return Json(new { data = Assessmentdata, RespCode = -2, isNew = false, RespMessage = "lower Income declaired" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { data = Assessmentdata, RespCode = 0, isNew = false, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
                    }
                   
                }
                catch (Exception ex)
                {
                    return Json(new { data = new proc_computeAssessment_modification_Result(), RespCode = 2, isNew = false, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                //return Json(data);
            }


        }
        public ActionResult AssessmentDetailsTaxPayee()
        {
            List<AssessmentRecord> Assessmentdata = new List<AssessmentRecord>();
            AssessmentModelPayee assessmentpayee = new AssessmentModelPayee();
            EnrollmentLog enrollmentdetails = new EnrollmentLog();
            try
            {
                string html = string.Empty;
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    string eID = Session["EnrollID"].ToString();
                    Assessmentdata = unitOfWork.Assessment.GetAssessmentSingleFirst(Session["EnrollID"].ToString()).ToList();

                    assessmentpayee.AssessmentRecord = Assessmentdata;
                    
                    html = PartialView("_PayeeAssessment", assessmentpayee).RenderToString();
                }
                return Json(new { data = Assessmentdata,  html_div = html, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new proc_computeAssessment_Result(), enrollmentdetails = new EnrollmentLog(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult MakeSalaryPayment(string netTax)
        {
            try
            {
                List<string> IncomeYear = new List<string>();

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    string eID = Session["EnrollID"].ToString();
                    var response = 0;
                    if (response == 0)
                    {
                        var salaryhistory = unitOfWork.SalaryUploadSecondary.GetSalaryUploadSecondaryCount(Session["EnrollID"].ToString());
                        var salarydata = unitOfWork.SalaryUpload.GetSalaryUpload(Session["EnrollID"].ToString());

                        int count = (int)salaryhistory++;
                        foreach (var item in salarydata)
                        {
                            unitOfWork.SalaryUploadSecondary.Add(new SALARYUPLOAD_HISTORY()
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
                                GrossPay = item.GrossPay,
                                IsDeleted = item.IsDeleted,
                                NHFContribution = item.NHFContribution,
                                NHIS = item.NHIS,
                                Others = item.Others,
                                PayrollStatus = "PAID",
                                Pension = item.Pension,
                                Premium = item.Premium,
                                UploadMonthIndex = item.UploadMonthIndex,
                                UploadYear = item.UploadYear,
                                IndexCount = count
                            });
                            var salaryrecord = unitOfWork.SalaryUpload.Find(p => p.ItbID == item.ItbID).FirstOrDefault();
                            unitOfWork.SalaryUpload.Remove(salaryrecord);



                        }

                    }

                    var retValue = unitOfWork.Complete() > 0 ? true : false;
                    if (retValue)
                    {
                        var assessment = unitOfWork.AssessmentRecord.GetAssessmentRecord(Session["EnrollID"].ToString());

                        foreach(var item in assessment)
                        {
                            var salaryrecord = unitOfWork.AssessmentRecord.Find(p => p.Itbid == item.Itbid).FirstOrDefault();
                            unitOfWork.AssessmentRecord.Remove(salaryrecord);
                            var returnValue = unitOfWork.Complete() > 0 ? true : false;
                        }


                        return Json(new { RespCode = 0, RespMessage = "Payment successful" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { RespCode = -1, RespMessage = "An error occured" }, JsonRequestBehavior.AllowGet);
                    }

                }

                //return View(IncomeYear);
                ///return Json(new { data = IncomeYear, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult AssessmentList()
        {
            List<AssessmentRecord> Assessmentdata = new List<AssessmentRecord>();
            try
            {

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    Assessmentdata = unitOfWork.AssessmentRecord.GetAssessmentRecord(Session["EnrollID"].ToString()).ToList();
                }
                return Json(new { data = Assessmentdata, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new proc_computeAssessment_Result(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //dummy posting from incomeSource to incomPayment on click of verify pay
        [HttpPost]
        public ActionResult IncomePaymentSave(string yearValue)
        {
            IncomePayment model = new IncomePayment();
            List<IncomeSource> Incomedetails = new List<IncomeSource>();
            IncomeSource incS = new IncomeSource();

            try
            {
                string eID = Session["EnrollID"].ToString();
                var errorMsg = "";
                if (ModelState.IsValid)
                {
                    if (model.ItbID == 0)
                    {

                        using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                        {
                            //get details of company from enrollment using enrollid
                            //pass fthose details eg name, address etc to payment gateway
                            ////pass amount pasyable taxpayable
                            var interswitchresponse = 0;
                            if (interswitchresponse == 0)//response from interswitch
                            {
                                var rec = new IncomeSource();
                                List<IncomePayment> Paymentdetails = new List<IncomePayment>();
                                IncomePayment PaymentMsg = new IncomePayment();
                                int PaymentMsgCnt = 0;
                                //incS = db.IncomeSources.Where(j => j.EnrollmentID == eID && j.IncomeYear == yearValue);

                                var incomepayment_object = db.IncomePayments.Where(j => j.EnrollmentID == eID && j.IncomeYear == yearValue).ToList();

                                var PaymentMsg1 = incomepayment_object == null|| incomepayment_object.Count==0 ? 0 : incomepayment_object.Max(p => p.Count);
                               
                                ErrorManager.SaveLog($"PaymentMsg1 {Newtonsoft.Json.JsonConvert.SerializeObject(PaymentMsg1)}");



                                Incomedetails = db.IncomeSources.Where(j => j.EnrollmentID == eID).ToList();
                                int count = 0;
                                count = PaymentMsg1+1;
                                foreach (IncomeSource item in Incomedetails) // Loop through List with foreach
                                {
                                    //model.ItbID = item.ItbID;
                                    model = new IncomePayment();
                                    model.EnrollmentID = item.EnrollmentID;
                                    model.SourceOfIncomeID = item.SourceOfIncomeID;
                                    model.Status = "S";
                                    model.CreateDate = item.CreateDate;
                                    model.Amount = item.Amount;
                                    model.IncomeYear = item.IncomeYear;
                                    model.Count = count;
                                    unitOfWork.IncomePayment.Add(model);

                                    ErrorManager.SaveLog($"Itbid {item.ItbID}");
                                    var removeobject = unitOfWork.IncomeDeclarartion.Find(p=>p.ItbID==item.ItbID).SingleOrDefault();
                                    ErrorManager.SaveLog($"removeobject {Newtonsoft.Json.JsonConvert.SerializeObject(removeobject)}");

                                    unitOfWork.IncomeDeclarartion.Remove(removeobject);
                                    ErrorManager.SaveLog($"Itbid {item.ItbID}");

                                }
                                var retv = unitOfWork.Complete() > 0 ? true : false; ;
                                ErrorManager.SaveLog($"retv {Newtonsoft.Json.JsonConvert.SerializeObject(retv)}");

                                if (retv)
                                {
                                    //unitOfWork.IncomeDeclarartion.RemoveRange(Incomedetails);
                                    //unitOfWork.Complete();
                                    //rec = unitOfWork.IncomeDeclarartion.GetIncomeSourceById(item.ItbID);
                                    return Json(new { RespCode = 0, RespMessage = "Payment Successfully" });
                                    //unitOfWork.Complete();
                                }
                            }


                            //++;





                        }

                        


                    }
                    else
                    {
                        

                        return Json(new {  RespCode = -1, RespMessage = "Error" });


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


        [HttpPost]
        // [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AssessmentRecord model)
        {
            try
            {
                var errorMsg = "";
                if (ModelState.IsValid)
                {
                    if (model.Itbid == 0)
                    {

                        using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                        {
                            unitOfWork.AssessmentRecord.Add(model);
                            unitOfWork.Complete();
                        }

                        return Json(new { data = model, RespCode = 0, RespMessage = "Record Created Successfully" });


                    }
                    else
                    {
                        using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                        {
                            unitOfWork.AssessmentRecord.Add(model);
                            unitOfWork.AssessmentRecord.Update(model);
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
    }
}