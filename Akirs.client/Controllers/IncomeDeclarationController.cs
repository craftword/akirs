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
    [SessionExpire]
    public class IncomeDeclarationController : Controller
    {
        // string EnrollId = string.Empty;
        public IncomeDeclarationController()
        {

        }
        [HttpPost]
        // [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IncomeSource model)
        {
            try
            {
                var errorMsg = "";
                if (ModelState.IsValid)
                {
                    IncomeSourceModel Familydata = new IncomeSourceModel();
                    if (model.ItbID == 0)
                    {
                        model.EnrollmentID = Session["EnrollID"].ToString();
                        model.Status = "P";
                        model.CreateDate = DateTime.Now;
                        model.CreatedBy = User.Identity.Name;
                        model.IsNew = "Y";

                        using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                        {
                            unitOfWork.IncomeDeclarartion.Add(model);
                            unitOfWork.Complete();

                            Familydata = unitOfWork.IncomeModel.GetIncomeSourceTypeDetailsById(model.ItbID);
                        }

                        return Json(new { data = Familydata, RespCode = 0, RespMessage = "Record Created Successfully" });


                    }
                    else
                    {
                        using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                        {
                            unitOfWork.IncomeDeclarartion.Add(model);
                            unitOfWork.IncomeDeclarartion.Update(model);
                            unitOfWork.Complete();

                            Familydata = unitOfWork.IncomeModel.GetIncomeSourceTypeDetailsById(model.ItbID);
                        }

                        return Json(new { data = Familydata, RespCode = 0, RespMessage = "Record Updated Successfully" });


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
            try
            {
                IncomeSource rec = null;
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    rec = unitOfWork.IncomeDeclarartion.Get(id);
                    if (rec == null)
                    {
                        var obj = new { RespCode = 1, RespMessage = "Record Not Found" };
                        return Json(obj, JsonRequestBehavior.AllowGet);

                    }


                   
                }
                var obj1 = new { model = rec, RespCode = 0, RespMessage = "Success" };
                return Json(obj1, JsonRequestBehavior.AllowGet);

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


                IncomeSource rec = null;
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {

                    rec = unitOfWork.IncomeDeclarartion.GetIncomeSourceById(id);
                    if (rec == null)
                    {
                        var obj2 = new { RespCode = 1, RespMessage = "Record Not Found" };
                        return Json(obj2, JsonRequestBehavior.AllowGet);

                    }
                    unitOfWork.IncomeDeclarartion.Remove(rec);
                    unitOfWork.Complete();
                    var obj = new { model = rec, RespCode = 0, RespMessage = "Success" };
                    return Json(obj, JsonRequestBehavior.AllowGet);


                  

                }
                var obj1 = new { model = rec, RespCode = 0, RespMessage = "Success" };
                return Json(obj1, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                var obj1 = new { RespCode = 2, RespMessage = ex.Message };
                return Json(obj1, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult IncomeTypeList()
        {
            List<IncomeSourceType> IncomeSource = new List<IncomeSourceType>();
            try
            {
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    IncomeSource = unitOfWork.IncomeSourceType.GetIncomeSourceType().ToList();

                }
                return Json(new { data = IncomeSource, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var obj1 = new { data = new List<IncomeSourceType>(), RespCode = 2, RespMessage = ex.Message };
                return Json(obj1, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult IncomeSourceList()
        {
            List<IncomeSourceModel> Familydata = new List<IncomeSourceModel>();
            try
            {

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    Familydata = unitOfWork.IncomeModel.GetIncomeSourceTypeDetails(Session["EnrollID"].ToString()).ToList();
                }
                return Json(new { data = Familydata, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<IncomeSource>(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Json(data);
        }

          

        public ActionResult DirectAssessmentDetails(string EnrollID)
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
        }
    }

}

