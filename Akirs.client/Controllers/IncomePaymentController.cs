using Akirs.client.DL;
using Akirs.client.Models;
using Akirs.client.Persistence;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Akirs.client.Controllers
{
    public class IncomePaymentController : Controller
    {
        AKIRSTAXEntities db = new AKIRSTAXEntities();
       

        // GET: IncomePayment
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IncomePaymentList( string yearValue)
        {
            //var data = "2020";
            List<IncomePayment> Paymentdetails = new List<IncomePayment>();
            List<IncomePaymentModel> PaymentModeldetails = new List<IncomePaymentModel>();
            IEnumerable<IncomePaymentModel> PaymentModelIenum= null;
            IncomePayment PaymentMsg = new IncomePayment();
            IncomePaymentModel PaymentModel = new IncomePaymentModel();
            try
            {

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities())) 
                {
                    string eID = Session["EnrollID"].ToString();

                    PaymentMsg = db.IncomePayments.Where(j => j.EnrollmentID == eID && j.IncomeYear == yearValue).OrderByDescending(m => m.Count).FirstOrDefault();

                    PaymentModelIenum = unitOfWork.IncomePaymentModel.GetIncomePaymentDetails(eID, yearValue, PaymentMsg.Count);
                    ErrorManager.SaveLog($"PaymentModelIenum {Newtonsoft.Json.JsonConvert.SerializeObject(PaymentModelIenum)}");
                    PaymentModeldetails = PaymentModelIenum.ToList();
                    ErrorManager.SaveLog($"PaymentModeldetails {Newtonsoft.Json.JsonConvert.SerializeObject(PaymentModeldetails)}");


                }
                return Json(new { data = PaymentModeldetails, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<IncomePaymentModel>(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Json(data);
        }


        [HttpGet]
        public ActionResult IncomePayYears()
        {

            return View();
        }
        public ActionResult GetIncomePayYear()
        {
         
            try
            {
                List<string> IncomePayYear = new List<string>();

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {

                    string eID = Session["EnrollID"].ToString();
                    //Assessmentdata = unitOfWork.Assessment.GetAssessmentSingle(Session["EnrollID"].ToString());

                    IncomePayYear = db.IncomePayments.Where(j => j.EnrollmentID == eID && j.Status == "S").Select(c => c.IncomeYear).Distinct().ToList();

                }

                //return View(IncomeYear);
                return Json(new { data = IncomePayYear, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult incomeModify(string yearValue)
        {
            IncomeSource model = new IncomeSource();
           List<IncomePayment> Paymentdetails = new List<IncomePayment>();
            IncomePayment PaymentMsg = new IncomePayment();

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
                            PaymentMsg = db.IncomePayments.Where(j => j.EnrollmentID == eID && j.IncomeYear == yearValue).OrderByDescending(m => m.Count).FirstOrDefault();
                            Paymentdetails = db.IncomePayments.Where(j => j.EnrollmentID == eID && j.IncomeYear == yearValue && j.Count == PaymentMsg.Count).ToList();
                           // var newPaymentdetails = Paymentdetails.OrderByDescending(x => x.Count).ToList();

                            try
                            {
                            
                                foreach (IncomePayment item in Paymentdetails) // Loop through List with foreach
                                {
                                   // model.ItbID = item.ItbID;
                                    model.EnrollmentID = item.EnrollmentID;
                                    model.SourceOfIncomeID = item.SourceOfIncomeID;
                                    model.Status = "P";
                                    model.CreateDate = item.CreateDate;
                                    model.Amount = item.Amount;
                                    model.IncomeYear = item.IncomeYear;
                                    model.IsNew = "N";
                                    unitOfWork.IncomeDeclarartion.Add(model);
                                    unitOfWork.Complete();
                                }
                            }
                            catch ( Exception ex)
                            {

                                return Json(new { RespCode = 1, RespMessage = ex.Message });
                            }
                            
                        }

                        return Json(new { data = model, RespCode = 0, RespMessage = "Record(s) ready for Modification!!!" });


                    }
                    else
                    {
                        using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                        {
                            unitOfWork.IncomeDeclarartion.Add(model);
                            unitOfWork.IncomeDeclarartion.Update(model);
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


        public ActionResult PaymentHistory()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetPaymentHistory(string yearValue)
        {
            //var data = "2020";
            List<IncomePayment> Paymentdetails = new List<IncomePayment>();
            List<IncomePaymentModel> PaymentModeldetails = new List<IncomePaymentModel>();
            IEnumerable<IncomePaymentModel> PaymentModelIenum = null;
            //IncomePayment PaymentMsg = new IncomePayment();
            IncomePaymentModel PaymentModel = new IncomePaymentModel();
            try
            {

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    string eID = Session["EnrollID"].ToString();

                    //PaymentMsg = db.IncomePayments.Where(j => j.EnrollmentID == eID && j.IncomeYear == yearValue).OrderByDescending(m => m.Count).FirstOrDefault();

                    PaymentModelIenum = unitOfWork.IncomePaymentModel.GetIncomePaymentDetails2(eID, yearValue);
                    PaymentModeldetails = PaymentModelIenum.ToList();

                }
                return Json(new { data = PaymentModeldetails, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<IncomePaymentModel>(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Json(data);
        }
    }
}