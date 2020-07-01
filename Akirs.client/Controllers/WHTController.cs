using Akirs.client.DL;
using Akirs.client.Persistence;
using Akirs.client.utility;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Akirs.client.Controllers
{
    [SessionExpire]
    public class WHTController : Controller
    {
        
        public WHTController()
        {
            
        }

        public ActionResult VerifyList()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WHTUPLOAD model)
        {
            try
            {
                var errorMsg = "";
                if (ModelState.IsValid)
                {
                    if (model.ItbID == 0)
                    {
                        model.EnrollmentID = Session["EnrollID"].ToString();
                        model.Status = "A";
                        model.CreateDate = DateTime.Now;
                        model.CreatedBy = User.Identity.Name;

                        using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                        {
                            unitOfWork.WhtUpload.Add(model);
                            unitOfWork.Complete();
                        }

                        return Json(new { data = model, RespCode = 0, RespMessage = "Record Created Successfully" });


                    }
                    else
                    {
                        using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                        {
                            model.Status = "A";
                            model.EnrollmentID = Session["EnrollID"].ToString();
                            unitOfWork.WhtUpload.Add(model);
                            unitOfWork.WhtUpload.Update(model);
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
                WHTUPLOAD rec = new WHTUPLOAD();
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    rec = unitOfWork.WhtUpload.GetWhtUploadById(id);
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
                WHTUPLOAD rec = new WHTUPLOAD();
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    rec = unitOfWork.WhtUpload.GetWhtUploadById(id);
                    if (rec == null)
                    {
                        var obj = new { RespCode = 1, RespMessage = "Record Not Found" };
                        return Json(obj, JsonRequestBehavior.AllowGet);

                    }
                    unitOfWork.WhtUpload.Remove(rec);
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
        public JsonResult GetPaymentSampleTemplate(short Itbid)
        {
            return Json(new { Itbid = Itbid });
        }
        [HttpGet]
        public virtual ActionResult PaymentTemplateProcess(short Itbid)
        {
            string file = HostingEnvironment.MapPath("~/Template/WhtUpload.xlsx");
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(file, contentType, Path.GetFileName(file));
        }
        [HttpPost]
        public ActionResult UploadFiles()
        {
            IList<WHTUPLOAD> model = null;
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


                        List<WHTUPLOAD> saveuplList = new List<WHTUPLOAD>();
                        var returnSaved = false;
                        using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                        {
                            using (var package = new ExcelPackage(file.InputStream))
                            {
                                var currentSheet = package.Workbook.Worksheets;
                                var workSheet = currentSheet.First();
                                var noOfCol = workSheet.Dimension.End.Column;
                                var noOfRow = workSheet.Dimension.End.Row;

                                for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                                {
                                    var saveupl = new WHTUPLOAD();
                                    saveupl.EnrollmentID = Session["EnrollID"].ToString();
                                    saveupl.VendorTINNO = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    saveupl.VendorName = workSheet.Cells[rowIterator, 2].Value.ToString();
                                    //saveupl.TaxRate = decimal.Parse(workSheet.Cells[rowIterator, 3].Value.ToString());
                                    saveupl.TaxAmount = decimal.Parse(workSheet.Cells[rowIterator, 3].Value.ToString());

                                    saveupl.VALIDATIONERRORSTATUS = true;
                                    saveuplList.Add(saveupl);
                                    unitOfWork.WhtUpload.Add(saveupl);
                                    //repoSalarytemp.Insert(saveupl);

                                }

                            }
                            returnSaved = unitOfWork.Complete() > 0 ? true : false;
                        }

                        if (returnSaved)
                        {
                            var html = PartialView("_wthUpld", saveuplList).RenderToString();
                            return Json(new { data_html = html, RespCode = 0, RespMessage = "Upload Sucessfull" });

                        }
                        else
                        {
                            var html = PartialView("_wthUpld").RenderToString();
                            return Json(new { RespCode = 1, RespMessage = "Problem processing file upload." });
                        }
                    }
                }
                // If we got this far, something failed, redisplay form
                //return Json(new { RespCode = 1, RespMessage = errorMsg });
            }
            catch (SqlException ex)
            {
                return Json(new { data = model, RespCode = 1, RespMessage = ex.Message });
            }
            catch (Exception ex)
            {
                return Json(new { data = model, RespCode = 1, RespMessage = ex.Message });
            }
            return Json(new { data = model, BatchId = "", RespCode = 0, RespMessage = "File Uploaded Successfully" });
        }
        public ActionResult WhtList()
        {
            try
            {
                var rec = new List<WHTUPLOAD>();
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    rec = unitOfWork.WhtUpload.GetPendingWhtUpload(Session["EnrollID"].ToString()).ToList();

                }
                return Json(new { data = rec, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var obj1 = new { data = new List<IncomeSourceType>(), RespCode = 2, RespMessage = ex.Message };
                return Json(obj1, JsonRequestBehavior.AllowGet);
            }

        }
        protected int ValidateUpload(string enrollID)
        {
            var rv = new WHTUPLOAD();

            int CompanyID;
            string Companyname = "";
            var rec = (List<WHTUPLOAD>)Session["Salary"];
            int totalErrorCount = 0;
            //enrollID = "1450168";
            using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
            {
                var t = unitOfWork.EnrollLog.GetEnrollDetails(enrollID);//(f => f.ITBID == instSelected);
                if (t != null)
                {
                    CompanyID =(int) t.CompanyID;
                    Companyname = t.Companyname;
                }



                int midNameLength = 150;


                //var rec = repoSalarytemp.Query().Where(m => m.CreatedBy == User.Identity.Name && m.EnrollmentID == enrollID && m.Status == "A");

                
                foreach (var tm in rec)
                {
                    int errorCount = 0;
                    var validationErrorMessage = new List<string>();
                    decimal amountrec;
                    int specialCount = 0;
                    // validating merchant id  (1)


                    // Match match = Regex.Match(t.MERCHANTID, "^a-z0-9", RegexOptions.IgnoreCase);
                    if (Regex.IsMatch(tm.VendorTINNO, "[^a-z0-9]", RegexOptions.IgnoreCase))
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

                    if (Regex.IsMatch(tm.VendorName, "[^a-z0-9]", RegexOptions.IgnoreCase))
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



                    if (!string.IsNullOrEmpty(tm.VendorName))
                    {

                        if (tm.VendorName.Length > midNameLength)
                        {
                            errorCount++;
                            //  totalErrorCount++;
                            validationErrorMessage.Add(string.Format("Merchant Id Lenght must not be more than {0}", midNameLength));

                        }

                    }

                    if (!decimal.TryParse(tm.TaxAmount.ToString(), out amountrec))
                    {
                        errorCount++;
                        //  totalErrorCount++;
                        validationErrorMessage.Add(string.Format("Employee Annual Basic Salary must be a number"));
                    }

                    if (!decimal.TryParse(tm.TaxRate.ToString(), out amountrec))
                    {
                        errorCount++;
                        //  totalErrorCount++;
                        validationErrorMessage.Add(string.Format("Employee Annual Housing Salary must be a number"));
                    }


                    if (errorCount == 0)
                    {
                        tm.VALIDATIONERRORSTATUS = false;
                        tm.Status = "P";
                    }
                    else
                    {
                        totalErrorCount++;
                        tm.VALIDATIONERRORSTATUS = true;
                    }

                    unitOfWork.WhtUpload.Add(tm);
                }
                
                unitOfWork.Complete();
            }

            return totalErrorCount;

        }
        [HttpPost]
        public ActionResult VerifyRecord()
        {
            try
            {
                string html = string.Empty;
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    WHTUPLOADHISTORY salup = null;
                    var rec = unitOfWork.WhtUpload.GetWhtUpload(Session["EnrollID"].ToString()).ToList();
                    foreach (var item in rec)
                    {
                        salup = new WHTUPLOADHISTORY();


                        salup.TaxRate = item.TaxRate;
                        salup.TaxAmount = item.TaxAmount;
                        salup.VendorName = item.VendorName;
                        salup.VendorTINNO = item.VendorTINNO;
                        salup.AuthUserID = item.AuthUserID;
                        salup.CreateDate = item.CreateDate;
                        salup.CreatedBy = item.CreatedBy;
                        salup.Last_Modified_Authid = item.Last_Modified_Authid;
                        salup.Last_Modified_Date = item.Last_Modified_Date;
                        salup.Last_Modified_Uid = item.Last_Modified_Uid;
                        salup.Status = "A";
                        salup.VALIDATIONERRORSTATUS = item.VALIDATIONERRORSTATUS;
                        salup.EnrollmentID = item.EnrollmentID;

                        item.Status = "A";
                        unitOfWork.WhtUpload.Add(item);
                        unitOfWork.WhtUpload.Update(item);
                        //repoWHT.Update(item);

                        unitOfWork.WhtHistory.Add(salup);
                        //repowhthist.Insert(salup);
                    }
                    unitOfWork.Complete();

                    var dr = unitOfWork.WhtUpload.GetWhtUpload(Session["EnrollID"].ToString()).ToList();
                    html = PartialView("_verifywht", dr).RenderToString();
                }
                return Json(new { data_html = html, RespCode = 0, RespMessage = "Record " }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { RespCode = 1, RespMessage = "Problem Processing Validation. " });
            }
        }
        [HttpPost]
        public ActionResult Validate(string EnrollID)
        {
            try
            {
                EnrollID = Session["EnrollID"].ToString();
                var rec = new List<WHTUPLOAD>();
                int sucCount = 0;
                int errCount = 0;
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    errCount = ValidateUpload(EnrollID);
                    rec = unitOfWork.WhtUpload.GetWhtUpload(EnrollID).ToList();
                    sucCount = rec.Count - errCount;
                }
                var html = PartialView("_wthUpld", rec).RenderToString();
                return Json(new { data_html = html, RespCode = 0, RespMessage = "Record ", SucCount = sucCount, FailCount = errCount });
            }
            catch (Exception ex)
            {
                return Json(new { RespCode = 1, RespMessage = "Problem Processing Validation. " });
            }
        }

        public decimal? CalculateWithTax()
        {
          var EnrollID = Session["EnrollID"].ToString();
            decimal? rec = 0;

            using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
            {
                rec = unitOfWork.WhtUpload.calculateWitholdTaxPayment(EnrollID);
            }
            return rec;
        }
    }
}