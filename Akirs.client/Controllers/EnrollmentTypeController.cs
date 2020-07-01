using Akirs.client.DL;
using Akirs.client.Enums;
using Akirs.client.Models;
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
using System.Web.Mvc;

namespace Akirs.client.Controllers
{
    public class EnrollmentTypeController : Controller
    {
        AKIRSTAXEntities db = new AKIRSTAXEntities();

        int counter;
        // GET: EnrollmentType
        public ActionResult Index()
        {
            return View();
        }

        //public JsonResult checkEmailAvailability(string AspNetUsers)
        //{
        //    System.Threading.Thread.Sleep(200);
        //        var SearchData = db.AspNetUsers.Where(x => x.Email == AspNetUsers).SingleOrDefault();
        //    if (SearchData != null)
        //    {
        //        return Json(1);
        //    }
        //    else
        //    {
        //        return Json(0);
        //    }
        //}


        public JsonResult checkEmailAvailability(string AspNetUsers)
        {
            System.Threading.Thread.Sleep(200);
            var SearchData = db.AspNetUsers.Where(x => x.Email == AspNetUsers).FirstOrDefault();
            if (SearchData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
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

                    //string eID = Session["EnrollID"].ToString();
                    salary = unitOfWork.SalaryHistory.GetSalaryUpload(EnrollID).ToList();

                    Assessmentdata  = unitOfWork.Assessment.GetAssessmentSingleWork(EnrollID, string.Format("{0:yyyy}", DateTime.Now)).ToList();
                    var result = CalculateWithTax(EnrollID);
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
        public ActionResult RentRangeList()
        {
            List<RentPrice> IncomeSource = new List<RentPrice>();
            try
            {
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    IncomeSource = unitOfWork.RentPrices.GetRentPrices().ToList();

                }
                return Json(new { data = IncomeSource, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var obj1 = new { data = new List<IncomeSourceType>(), RespCode = 2, RespMessage = ex.Message };
                return Json(obj1, JsonRequestBehavior.AllowGet);
            }

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
        public ActionResult RelationShipList()
        {
            List<RelationShip> RelationShip = new List<RelationShip>();
            try
            {
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    RelationShip = unitOfWork.RelationshipType.GetRelationshipType().ToList();

                }
                return Json(new { data = RelationShip, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var obj1 = new { data = new List<RelationShip>(), RespCode = 2, RespMessage = ex.Message };
                return Json(obj1, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult EnrollmentList()
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
        [HttpPost]
        public ActionResult UploadFilesWht()
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


                        List<WHTUPLOAD_temp> saveuplList = new List<WHTUPLOAD_temp>();
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                var saveupl = new WHTUPLOAD_temp();
                                //saveupl.EnrollmentID = Session["EnrollID"].ToString();
                                saveupl.VendorTINNO = workSheet.Cells[rowIterator, 1].Value.ToString();
                                saveupl.VendorName = workSheet.Cells[rowIterator, 2].Value.ToString();
                                saveupl.TaxRate = decimal.Parse(workSheet.Cells[rowIterator, 3].Value.ToString());
                                saveupl.TaxAmount = decimal.Parse(workSheet.Cells[rowIterator, 4].Value.ToString());

                                saveupl.VALIDATIONERRORSTATUS = true;
                                saveuplList.Add(saveupl);
                                //repoSalarytemp.Insert(saveupl);

                            }
                        }
                        //var dataList = ExcelReader.GetDataToList(path, addRecord); /// ExxcellReaderClosedXml.GetDataToList(path, addRecord);
                        //int cnt = 0;
                        Session["Witholding"] = saveuplList;

                        int cnt = saveuplList.Count();
                        if (cnt > 0)
                        {

                            try
                            {
                                //repoSalarytemp.Save();
                            }
                            catch (Exception ex)
                            {

                            }
                            var html = PartialView("_wthUpld", saveuplList).RenderToString();
                            return Json(new { data_html = html, RespCode = 0, RespMessage = "Please Upload Using .xlsx file" });
                        }
                        else
                        {
                            var html = PartialView("_wthUpld").RenderToString();
                            return Json(new { RespCode = 1, RespMessage = "Problem processing file upload." });
                        }
                    }

                    // If we got this far, something failed, redisplay form
                    //return Json(new { RespCode = 1, RespMessage = errorMsg });
                }
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
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult UploadFilesSalary()
        {
            IList<SalaryuploadObj> model = null;
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

                        List<Salaryupload_temp> saveuplList = new List<Salaryupload_temp>();
                        using (var package = new ExcelPackage(file.InputStream))
                        {
                            var currentSheet = package.Workbook.Worksheets;
                            var workSheet = currentSheet.First();
                            var noOfCol = workSheet.Dimension.End.Column;
                            var noOfRow = workSheet.Dimension.End.Row;

                            for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                            {
                                var saveupl = new Salaryupload_temp();
                                //saveupl.EnrollmentID = Session["EnrollID"].ToString();
                                saveupl.EmployeeID = workSheet.Cells[rowIterator, 1].Value.ToString();
                                saveupl.EmployeeName = workSheet.Cells[rowIterator, 2].Value.ToString();
                                saveupl.AnnualBasic = decimal.Parse(workSheet.Cells[rowIterator, 3].Value.ToString());
                                saveupl.AnnualHousing = decimal.Parse(workSheet.Cells[rowIterator, 4].Value.ToString());
                                saveupl.AnnualTransport = decimal.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());
                                saveupl.AnnualMeal = decimal.Parse(workSheet.Cells[rowIterator, 6].Value.ToString());
                                saveupl.AnnualOthers = decimal.Parse(workSheet.Cells[rowIterator, 7].Value.ToString());
                                saveupl.NHFContribution = decimal.Parse(workSheet.Cells[rowIterator, 8].Value.ToString());
                                saveupl.Pension = decimal.Parse(workSheet.Cells[rowIterator, 9].Value.ToString());
                                saveupl.Premium = decimal.Parse(workSheet.Cells[rowIterator, 10].Value.ToString());
                                saveupl.NHIS = decimal.Parse(workSheet.Cells[rowIterator, 11].Value.ToString());
                                saveupl.Others = decimal.Parse(workSheet.Cells[rowIterator, 12].Value.ToString());
                                saveupl.Status = "A";
                                saveupl.VALIDATIONERRORSTATUS = true;
                                saveuplList.Add(saveupl);
                                //repoSalarytemp.Insert(saveupl);

                            }
                        }
                        //var dataList = ExcelReader.GetDataToList(path, addRecord); /// ExxcellReaderClosedXml.GetDataToList(path, addRecord);
                        //int cnt = 0;
                        Session["Salary"] = saveuplList;

                        int cnt = saveuplList.Count();
                        if (cnt > 0)
                        {

                            try
                            {
                                //repoSalarytemp.Save();
                            }
                            catch (Exception ex)
                            {

                            }
                            var html = PartialView("_salaryUpld", saveuplList).RenderToString();
                            return Json(new { data_html = html, RespCode = 0, RespMessage = "Please Upload Using .xlsx file" });
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
                return Json(new { data = model, RespCode = 1, RespMessage = ex.Message });
            }
            catch (Exception ex)
            {
                return Json(new { data = model, RespCode = 1, RespMessage = ex.Message });
            }
            return Json(new { data = model, BatchId = "", RespCode = 0, RespMessage = "File Uploaded Successfully" });
        }
        public ActionResult MinistryList()
        {
            List<Ministry> RelationShip = new List<Ministry>();
            try
            {
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    RelationShip = unitOfWork.Ministries.GetMinistry().ToList();

                }
                return Json(new { data = RelationShip, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var obj1 = new { data = new List<RelationShip>(), RespCode = 2, RespMessage = ex.Message };
                return Json(obj1, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult RevenueList(string MinistryCode)
        {
            List<RevenueHead> RelationShip = new List<RevenueHead>();
            try
            {
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    RelationShip = unitOfWork.RevenueHead.GetRevenueHead(MinistryCode).ToList();

                }
                return Json(new { data = RelationShip, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var obj1 = new { data = new List<RelationShip>(), RespCode = 2, RespMessage = ex.Message };
                return Json(obj1, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult CreateTransactionLog(TransactionLogModel model)
        {
            bool result;
            try
            {
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    model.UserId = int.Parse(Session["EnrollID"].ToString());
                    result = unitOfWork.TransactionLog.CreateTansactionLog(model);
                    if(result == true)
                    {
                        return Json(new { data = result, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        return Json(new { data = false, RespCode = -2, RespMessage = "Failed" }, JsonRequestBehavior.AllowGet);

                    }
                }
            }
            catch(Exception ex)
            {
                return Json(new { data = false, RespCode = -2, RespMessage = "Failed" }, JsonRequestBehavior.AllowGet);

            }
        }

        public ActionResult StateList()
        {
            List<State> RelationShip = new List<State>();
            try
            {
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    RelationShip = unitOfWork.State.GetStates().ToList();

                }
                return Json(new { data = RelationShip, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var obj1 = new { data = new List<RelationShip>(), RespCode = 2, RespMessage = ex.Message };
                return Json(obj1, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult CityList(string StateCode )
        {
            List<City> RelationShip = new List<City>();
            try
            {
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    RelationShip = unitOfWork.City.GetCities(StateCode).ToList();

                }
                return Json(new { data = RelationShip, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var obj1 = new { data = new List<RelationShip>(), RespCode = 2, RespMessage = ex.Message };
                return Json(obj1, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult MaritalList()
        {
            List<MaritalStatu> RelationShip = new List<MaritalStatu>();
            try
            {
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    RelationShip = unitOfWork.MaritalStatus.GetMaritalStatus().ToList();

                }
                return Json(new { data = RelationShip, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var obj1 = new { data = new List<RelationShip>(), RespCode = 2, RespMessage = ex.Message };
                return Json(obj1, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult AddParty(string rt)
        {
            //BindCombo();
            if (rt == "1")
            {
                //var rec = repoEnroll.GetAll();
                var model = new EnrollmentViewModel();
                model.TaxtypeID = 1;
                model.DateOfBirth = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    ViewBag.EnrollType = unitOfWork.TaxType.Get(1).Taxtypename;

                }


                //var html = PartialView("_DirectAccess", model).RenderToString();
                //return Json(new { data_html = html, RespCode = 0 }, JsonRequestBehavior.AllowGet);
                ViewBag.ViewToShow = "_DirectAccess";
                return View(model);
            }
            else if (rt == "2")
            {
                var model = new EnrollmentViewModel();
                model.TaxtypeID = 2;
                model.DateOfBirth = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    ViewBag.EnrollType = unitOfWork.TaxType.Get(2).Taxtypename;

                }
                //var html = PartialView("_Payee", model).RenderToString();
                //return Json(new { data_html = html, RespCode = 0 }, JsonRequestBehavior.AllowGet);
                ViewBag.ViewToShow = "_Payee";
                return View(model);

            }
            else if (rt == "3")
            {
                var model = new EnrollmentViewModel();
                model.TaxtypeID = 3;
                model.DateOfBirth = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    ViewBag.EnrollType = unitOfWork.TaxType.Get(3).Taxtypename;

                }
                //var html = PartialView("_Levy", model).RenderToString();
                //return Json(new { data_html = html, RespCode = 0 }, JsonRequestBehavior.AllowGet);
                ViewBag.ViewToShow = "_Levy";
                return View(model);
            }
            else if (rt == "4")
            {
                var model = new EnrollmentViewModel();
                model.TaxtypeID = 4;
                model.DateOfBirth = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    ViewBag.EnrollType = unitOfWork.TaxType.Get(4).Taxtypename;

                }
                //var html = PartialView("_Levy", model).RenderToString();
                //return Json(new { data_html = html, RespCode = 0 }, JsonRequestBehavior.AllowGet);
                ViewBag.ViewToShow = "_witHoldTax";
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // Creating DirectAssessment and calling Stored Procedure
        [HttpPost]
        public ActionResult CreateDirectAssessment(EnrollmentModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.EnrollmentViewModel.TaxtypeID == 1)
                {
                    string comid = null;
                    string EnrollID = string.Empty;
                    string FullName = string.Empty;
                    CompanyData comp = new CompanyData();
                    proc_GetCompanyName_Result gf = new proc_GetCompanyName_Result();
                    proc_GetNextEnRoll_Result rec = new proc_GetNextEnRoll_Result();
                    using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                    {

                        if (model.EnrollmentViewModel.CountryID == null)
                        {
                            model.EnrollmentViewModel.CountryID = "NG";
                        }
                        rec = unitOfWork.NextEnroll.GetNextEnrollNumber();

                        FullName = model.EnrollmentViewModel.Lastname + " " + model.EnrollmentViewModel.Middlename + " " + model.EnrollmentViewModel.Firstname;

                        EnrollID = rec.NextEnrollmentID.ToString();
                        var enrolmode = new EnrollmentLog
                        {

                            EnrollmentID = EnrollID,
                            TaxtypeID = model.EnrollmentViewModel.TaxtypeID,
                            Firstname = model.EnrollmentViewModel.Firstname,
                            Middlename = model.EnrollmentViewModel.Middlename,
                            Lastname = model.EnrollmentViewModel.Lastname,
                            Address = model.EnrollmentViewModel.Address,
                            Phonenumber = model.EnrollmentViewModel.Phonenumber,
                            Email = model.EnrollmentViewModel.Email,
                            MaritalStatus = model.EnrollmentViewModel.MaritalStatus,
                            Gender = model.EnrollmentViewModel.Gender,
                            Position = model.EnrollmentViewModel.Position,
                            Numberofchildren = model.EnrollmentViewModel.Numberofchildren,
                            Status = "A",
                            CreatedBy = User.Identity.Name,
                            CreateDate = DateTime.Now,
                            CountryID = "NG",
                            StateCode = model.EnrollmentViewModel.StateCode,
                            CityCode = model.EnrollmentViewModel.CityCode,
                            LGACode = model.EnrollmentViewModel.LGACode,
                            RentRangeId = model.EnrollmentViewModel.RentRangeId,
                            


                        };
                        //repoEnroll.Insert(enrolmode);
                        unitOfWork.EnrollLog.Add(enrolmode);

                        var NewUID = SmartObj.GenerateRandNum();

                        Random rdn = new Random();
                        int password = rdn.Next(111111, 999999);
                        var user = new AspNetUser
                        {
                            UserName = EnrollID,
                            Id = Guid.NewGuid().ToString(),
                            Email = model.EnrollmentViewModel.Email,
                            LastName = model.EnrollmentViewModel.Lastname,
                            FirstName = model.EnrollmentViewModel.Firstname,
                            RoleId = 12,
                            UserId = User.Identity.Name,
                            CreateUserId = User.Identity.Name,
                            CreateDate = DateTime.Now,
                            EnforcePasswordChangeDays = 90,
                            FullName = model.EnrollmentViewModel.Firstname + " " + model.EnrollmentViewModel.Lastname,
                            ForcePassword = true,
                            IsApproved = true,
                            MobileNo = model.EnrollmentViewModel.Phonenumber,
                            Status = "A",
                            DeptCode = null,
                            EnrollmentID = EnrollID,
                            EnforcePasswordChange = "Y",
                            PasswordHash = SmartObj.Encrypt(password.ToString()) //SmartObj.Encrypt(model.EnrollmentViewModel.Firstname)
                        };

                        unitOfWork.Users.Add(user);

                        foreach (var familyitem in model.FamilyModel)
                        {
                            FamilyDetail familydetails = new FamilyDetail()
                            {
                                EnrollmentID = EnrollID,
                                Age = familyitem.Age,
                                FullName = familyitem.FullName,
                                RelationshipType = familyitem.RelationshipType,
                                Status = "A"
                            };

                            unitOfWork.FamilyDetails.Add(familydetails);
                        }


                        foreach (var item in model.IncomeSource)
                        {
                            IncomeSource incomesource = new IncomeSource()
                            {
                                Amount = item.Amount,
                                EnrollmentID = EnrollID,
                                SourceOfIncomeID = item.SourceOfIncomeID,
                                Status = "P",
                                IncomeYear = item.IncomeYear,
                                IsNew = item.IsNew
                            };
                            unitOfWork.IncomeDeclarartion.Add(incomesource);
                        }
                        try
                        {
                            unitOfWork.Complete();
                        }
                        catch (Exception ex)
                        {

                        }
                        // Send email and sms to customer
                        proc_Notification_Result rec2 = new proc_Notification_Result();
                        rec2 = unitOfWork.AddNotification.AddAlert("ENROLLMENT", model.EnrollmentViewModel.Email, model.EnrollmentViewModel.Phonenumber, EnrollID + "," + model.EnrollmentViewModel.Firstname + "," + FullName, model.EnrollmentViewModel.Firstname);

                        if (rec2 == null)
                        {

                            var obj = new { RespCode = 1, RespMessage = "Record Not Found" };
                            return Json(obj, JsonRequestBehavior.AllowGet);

                        }
                    }
                    return Json(new { RespCode = 0, RespMessage = "Success created your account. Kindly check for email", EnrollID = EnrollID }, JsonRequestBehavior.AllowGet);
                }
                else if (model.EnrollmentViewModel.TaxtypeID == 3)
                {
                    //Process Payment save in tranlog
                    var tranlog = new TransactionLog()
                    {


                    };
                }
                else if (model.EnrollmentViewModel.TaxtypeID == 2)
                {
                    string FullName = string.Empty;
                    try
                    {
                        proc_GetCompanyName_Result gf = new proc_GetCompanyName_Result();
                        proc_GetNextEnRoll_Result rec = new proc_GetNextEnRoll_Result();
                        string comid = null;
                        string EnrollID = string.Empty;
                        CompanyData comp = new CompanyData();
                        if (model.EnrollmentViewModel.Companyname != null || String.IsNullOrWhiteSpace(model.EnrollmentViewModel.Companyname))
                        {
                            using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                            {
                                gf = unitOfWork.CompanyName.GetCustomerName(model.EnrollmentViewModel.Companyname);
                                rec = unitOfWork.NextEnroll.GetNextEnrollNumber();
                                EnrollID = rec.NextEnrollmentID.ToString();
                                if (gf.noofcomp == 0)
                                {
                                    comp = new CompanyData
                                    {
                                        CompanyName = model.EnrollmentViewModel.Companyname,
                                        TaxtypeID = model.EnrollmentViewModel.TaxtypeID,
                                        Status = "A",
                                        CreatedBy = User.Identity.Name,
                                        CreateDate = DateTime.Now,
                                        CompanyEmail = model.EnrollmentViewModel.Email,
                                        CompanyPhonenumber = model.EnrollmentViewModel.Phonenumber
                                    };
                                    unitOfWork.CompanyData.Add(comp);
                                    unitOfWork.Complete();
                                }
                                comid = gf == null ? comp.ItbID.ToString() : gf.noofcomp.ToString();

                                FullName = model.EnrollmentViewModel.Companyname;
                                var enrolmode = new EnrollmentLog
                                {

                                    EnrollmentID = EnrollID,
                                    TaxtypeID = model.EnrollmentViewModel.TaxtypeID,
                                    Firstname = model.EnrollmentViewModel.Firstname,
                                    Middlename = model.EnrollmentViewModel.Middlename,
                                    CompanyID = int.Parse(comid),
                                    Lastname = model.EnrollmentViewModel.Lastname,
                                    Address = model.EnrollmentViewModel.Address,
                                    Phonenumber = model.EnrollmentViewModel.Phonenumber,
                                    Email = model.EnrollmentViewModel.Email,
                                    MaritalStatus = model.EnrollmentViewModel.MaritalStatus,
                                    Gender = model.EnrollmentViewModel.Gender,
                                    Position = model.EnrollmentViewModel.Position,
                                    Numberofchildren = model.EnrollmentViewModel.Numberofchildren,
                                    Status = "A",
                                    CreatedBy = User.Identity.Name,
                                    CreateDate = DateTime.Now,
                                    CountryID = "NG",
                                    StateCode = model.EnrollmentViewModel.StateCode,
                                    CityCode = model.EnrollmentViewModel.CityCode,
                                    LGACode = model.EnrollmentViewModel.LGACode
                                };
                                //repoEnroll.Insert(enrolmode);
                                unitOfWork.EnrollLog.Add(enrolmode);
                                unitOfWork.Complete();

                                var NewUID = SmartObj.GenerateRandNum();
                                var pass = string.Empty;
                                var passstr = model.EnrollmentViewModel.Companyname.ToString().Replace(" ", "").Trim();
                                if (passstr.Length >= 10)
                                {
                                    pass = passstr.Substring(0, 9);
                                }
                                else
                                {
                                    pass = passstr.Substring(0, (passstr.Length - 1));
                                }

                                // Create Role
                                var role = new AspNetRole
                                {
                                    Id = new Random().Next(0000, 99999).ToString(),
                                    Name = "Admin",
                                };
                                unitOfWork.UserRoleRepository.Add(role);
                                unitOfWork.Complete();

                                //Fetch role details
                                var entity = unitOfWork.UserRoleRepository.GetRoleById(role.Id);

                                Random rdn = new Random();
                                int password = rdn.Next(111111, 999999);
                                // Create User
                                var user = new AspNetUser
                                {
                                    UserName = EnrollID,
                                    Id = Guid.NewGuid().ToString(),
                                    Email = model.EnrollmentViewModel.Email,
                                    LastName = model.EnrollmentViewModel.Lastname,
                                    FirstName = model.EnrollmentViewModel.Firstname,
                                    RoleId = Int32.Parse(entity.Id),
                                    RoleName = entity.Name,
                                    UserId = User.Identity.Name,
                                    CreateUserId = User.Identity.Name,
                                    CreateDate = DateTime.Now,
                                    EnforcePasswordChangeDays = 90,
                                    FullName = model.EnrollmentViewModel.Firstname + " " + model.EnrollmentViewModel.Lastname,
                                    ForcePassword = true,
                                    IsApproved = true,
                                    MobileNo = model.EnrollmentViewModel.Phonenumber,
                                    Status = "P",
                                    DeptCode = null,
                                    EnrollmentID = EnrollID,
                                    PasswordHash = SmartObj.Encrypt(password.ToString()),  //SmartObj.Encrypt(pass),
                                    EnforcePasswordChange = "Y",
                                    IsDeleted = false
                                };

                                unitOfWork.Users.Add(user);
                                unitOfWork.Complete();

                                SALARYUPLOAD salup = null;
                                var salarytemp = (List<Salaryupload_temp>)Session["Salary"];
                                int counter = 1;
                                //var rec = unitOfWork.SalaryUpload.GetSalaryUpload(Session["EnrollID"].ToString()).ToList();
                                foreach (var item in salarytemp)
                                {
                                    salup = new SALARYUPLOAD();


                                    salup.AnnualBasic = item.AnnualBasic;
                                    salup.AnnualHousing = item.AnnualHousing;
                                    salup.AnnualMeal = item.AnnualMeal;
                                    salup.AnnualOthers = item.AnnualOthers;
                                    salup.AnnualTransport = item.AnnualTransport;
                                    salup.AuthUserID = item.AuthUserID;
                                    salup.CreateDate = DateTime.Now;
                                    salup.CreatedBy = item.CreatedBy;
                                    salup.EmployeeID = item.EmployeeID;
                                    salup.EmployeeName = item.EmployeeName;
                                    salup.GrossPay = item.GrossPay;
                                    salup.Last_Modified_Authid = item.Last_Modified_Authid;
                                    salup.Last_Modified_Date = item.Last_Modified_Date;
                                    salup.Last_Modified_Uid = item.Last_Modified_Uid;
                                    salup.NHFContribution = item.NHFContribution;
                                    salup.Pension = item.Pension;
                                    salup.Premium = item.Premium;
                                    salup.NHIS = item.NHIS;
                                    salup.Others = item.Others;
                                    salup.PayrollStatus = PayrollStatus.APPROVED.ToString();
                                    salup.VALIDATIONERRORSTATUS = item.VALIDATIONERRORSTATUS;
                                    salup.EnrollmentID = EnrollID;
                                    salup.Counter = counter++;
                                    salup.IsDeleted = false;
                                    salup.NextUploadDate = DateTime.Now.AddDays(30);

                                    //item.Status = "P";

                                    //unitOfWork.SalaryUpload.Add(item);
                                    //unitOfWork.SalaryUpload.Update(item);
                                    //repoSalarytemp.Update(item);
                                    unitOfWork.SalaryHistory.Add(salup);                        //repoSalary.Insert(salup);
                                    unitOfWork.Complete();
                                }


                                // Send email and sms to customer
                                proc_Notification_Result rec2 = new proc_Notification_Result();
                                rec2 = unitOfWork.AddNotification.AddAlert("ENROLLMENT", model.EnrollmentViewModel.Email, model.EnrollmentViewModel.Phonenumber, EnrollID + "," + pass + "," + FullName, pass);


                                if (rec2 == null)
                                {

                                    var obj = new { RespCode = 1, RespMessage = "Record Not Found" };
                                    return Json(obj, JsonRequestBehavior.AllowGet);

                                }

                            }
                        }
                        return Json(new { RespCode = 0, RespMessage = "Success created your account. Kindly check for email or mobile phone", EnrollID = EnrollID }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        var message = ex.InnerException.InnerException.Message;
                    }
                }

                else if (model.EnrollmentViewModel.TaxtypeID == 4)
                {
                    string FullName = string.Empty;
                    try
                    {
                        proc_GetCompanyName_Result gf = new proc_GetCompanyName_Result();
                        proc_GetNextEnRoll_Result rec = new proc_GetNextEnRoll_Result();
                        string comid = null;
                        string EnrollID = string.Empty;
                        CompanyData comp = new CompanyData();
                        if (model.EnrollmentViewModel.Companyname != null || String.IsNullOrWhiteSpace(model.EnrollmentViewModel.Companyname))
                        {
                            using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                            {
                                gf = unitOfWork.CompanyName.GetCustomerName(model.EnrollmentViewModel.Companyname);
                                rec = unitOfWork.NextEnroll.GetNextEnrollNumber();
                                EnrollID = rec.NextEnrollmentID.ToString();
                                if (gf.noofcomp == 0)
                                {
                                    comp = new CompanyData
                                    {
                                        CompanyName = model.EnrollmentViewModel.Companyname,
                                        TaxtypeID = model.EnrollmentViewModel.TaxtypeID,
                                        Status = "A",
                                        CreatedBy = User.Identity.Name,
                                        CreateDate = DateTime.Now,
                                        CompanyEmail = model.EnrollmentViewModel.Email,
                                        CompanyPhonenumber = model.EnrollmentViewModel.Phonenumber
                                    };
                                    unitOfWork.CompanyData.Add(comp);
                                    unitOfWork.Complete();
                                }
                                comid = gf == null ? comp.ItbID.ToString() : gf.noofcomp.ToString();

                                FullName = model.EnrollmentViewModel.Companyname;
                                var enrolmode = new EnrollmentLog
                                {

                                    EnrollmentID = EnrollID,
                                    TaxtypeID = model.EnrollmentViewModel.TaxtypeID,
                                    Firstname = model.EnrollmentViewModel.Firstname,
                                    Middlename = model.EnrollmentViewModel.Middlename,
                                    CompanyID = int.Parse(comid),
                                    Lastname = model.EnrollmentViewModel.Lastname,
                                    Address = model.EnrollmentViewModel.Address,
                                    Phonenumber = model.EnrollmentViewModel.Phonenumber,
                                    Email = model.EnrollmentViewModel.Email,
                                    MaritalStatus = model.EnrollmentViewModel.MaritalStatus,
                                    Gender = model.EnrollmentViewModel.Gender,
                                    Position = model.EnrollmentViewModel.Position,
                                    Numberofchildren = model.EnrollmentViewModel.Numberofchildren,
                                    Status = "A",
                                    CreatedBy = User.Identity.Name,
                                    CreateDate = DateTime.Now,
                                    CountryID = "NG",
                                    StateCode = model.EnrollmentViewModel.StateCode,
                                    CityCode = model.EnrollmentViewModel.CityCode,
                                    LGACode = model.EnrollmentViewModel.LGACode
                                };
                                //repoEnroll.Insert(enrolmode);
                                unitOfWork.EnrollLog.Add(enrolmode);

                                var NewUID = SmartObj.GenerateRandNum();
                                var pass = string.Empty;
                                var passstr = model.EnrollmentViewModel.Companyname.ToString().Replace(" ", "").Trim();
                                if (passstr.Length >= 10)
                                {
                                    pass = passstr.Substring(0, 9);
                                }
                                else
                                {
                                    pass = passstr.Substring(0, (passstr.Length - 1));
                                }

                                // Create Role
                                var role = new AspNetRole
                                {
                                    Id = new Random().Next(0000, 99999).ToString(),
                                    Name = "Admin",
                                };
                                unitOfWork.UserRoleRepository.Add(role);
                                unitOfWork.Complete();

                                //Fetch role details
                                var entity = db.AspNetRoles.Where(x => x.Id == role.Id).FirstOrDefault();

                                Random rdn = new Random();
                                int password = rdn.Next(111111, 999999);
                                var user = new AspNetUser
                                {
                                    UserName = EnrollID,
                                    Id = Guid.NewGuid().ToString(),
                                    Email = model.EnrollmentViewModel.Email,
                                    LastName = model.EnrollmentViewModel.Lastname,
                                    FirstName = model.EnrollmentViewModel.Firstname,
                                    RoleId = Int32.Parse(entity.Id),
                                    RoleName = entity.Name,
                                    UserId = User.Identity.Name,
                                    CreateUserId = User.Identity.Name,
                                    CreateDate = DateTime.Now,
                                    EnforcePasswordChangeDays = 90,
                                    FullName = model.EnrollmentViewModel.Firstname + " " + model.EnrollmentViewModel.Lastname,
                                    ForcePassword = true,
                                    IsApproved = true,
                                    MobileNo = model.EnrollmentViewModel.Phonenumber,
                                    Status = "P",
                                    DeptCode = null,
                                    EnrollmentID = EnrollID,
                                    PasswordHash = SmartObj.Encrypt(password.ToString()),  //SmartObj.Encrypt(pass),
                                    EnforcePasswordChange = "Y",
                                    IsDeleted = false
                                };

                                unitOfWork.Users.Add(user);
                                unitOfWork.Complete();

                                //var Witholding = (List<WHTUPLOAD_temp>)Session["Witholding"];

                                //WHTUPLOAD wht = null;
                                //foreach (var item in Witholding)
                                //{
                                //    wht = new WHTUPLOAD();


                                //    wht.TaxRate = item.TaxRate;
                                //    wht.TaxAmount = item.TaxAmount;
                                //    wht.VendorName = item.VendorName;
                                //    wht.VendorTINNO = item.VendorTINNO;
                                //    wht.AuthUserID = item.AuthUserID;
                                //    wht.CreateDate = item.CreateDate;
                                //    wht.CreatedBy = item.CreatedBy;
                                //    wht.Last_Modified_Authid = item.Last_Modified_Authid;
                                //    wht.Last_Modified_Date = item.Last_Modified_Date;
                                //    wht.Last_Modified_Uid = item.Last_Modified_Uid;
                                //    wht.Status = "P";
                                //    wht.VALIDATIONERRORSTATUS = item.VALIDATIONERRORSTATUS;
                                //    wht.EnrollmentID = EnrollID;


                                //    unitOfWork.WhtUpload.Add(wht);
                                //    unitOfWork.Complete();
                                //}

                                // Send email and sms to customer
                                proc_Notification_Result rec2 = new proc_Notification_Result();
                                rec2 = unitOfWork.AddNotification.AddAlert("ENROLLMENT", model.EnrollmentViewModel.Email, model.EnrollmentViewModel.Phonenumber, EnrollID + "," + pass + "," + FullName, pass);


                                if (rec2 == null)
                                {

                                    var obj = new { RespCode = 1, RespMessage = "Record Not Found" };
                                    return Json(obj, JsonRequestBehavior.AllowGet);

                                }

                            }
                        }
                        return Json(new { RespCode = 0, RespMessage = "Account Created Successfully. Kindly check your mail for your login details", EnrollID = EnrollID }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {
                        var message = ex.InnerException.InnerException.Message;
                    }
                }
            }

            return Json(new { RespCode = 1, RespMessage = "An error occurred" }, JsonRequestBehavior.AllowGet);
        }

        //public decimal? CalculateWithTax(string enrollID)
        public ActionResult CalculateWithTax(string enrollID)
        {
            try
            {
                decimal? taxAmount = 0;
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    taxAmount = unitOfWork.WhtUpload.calculateWitholdTaxPayment(enrollID);
                }
                //return taxAmount;

                return Json(new { data = taxAmount, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { RespCode = 1, RespMessage = "An error occurred" }, JsonRequestBehavior.AllowGet);

            }

        }

        //MakePay
        [HttpPost]
        public ActionResult MakePay(basicInfo model)
        {
            if (ModelState.IsValid)
            {
                StringBuilder sb = new StringBuilder();
                int  productID = 0;
               string sAmtToPay, SelectedServ, transID, RedirectURL, currency, unHash, MAC, Hash, paymentCode, StageserURL, custID, custName;
                sAmtToPay=SelectedServ = transID = RedirectURL = currency = unHash = MAC = Hash = paymentCode = custID= custName= StageserURL ="";
                double? QTkoboValue, amtTopay;
                //var rand = "9999" + Math.Floor((Math.random() * 100000000) + 1);
                var rand = "9999";
                MAC = "D3D1D05AFE42AD50818167EAC73C109168A0F108F32645C8B59E897FA930DA44F9230910DAC9E20641823799A107A02068F7BC0F4CC41D2952E249552255710F";
                QTkoboValue =amtTopay=0;
                transID = rand;
                amtTopay = model.Amount;
                productID = model.TaxtypeID;
                RedirectURL = "http://localhost:4301/WebPayQuery.aspx";
                Session["RedirectURL"] = RedirectURL;
                StageserURL = "https://stageserv.interswitchng.com/test_paydirect/pay";

                QTkoboValue = amtTopay * 100;
                unHash = transID + productID + paymentCode + QTkoboValue + RedirectURL + MAC;

                Hash = SmartObj.hashSHA512(unHash);
               
                sb.Append("<html>");
                sb.AppendFormat("<body onload='document.forms[\"form\"].submit()'>");
                sb.AppendFormat("<form name='form' action='{0}' method='post'>", StageserURL);
                sb.AppendFormat("<input type='hidden' name='product_id' value='{0}'>", productID);
                sb.AppendFormat("<input type='hidden' name='cust_id' value='{0}'>", custID);
                sb.AppendFormat("<input type='hidden' name='cust_name' value='{0}'>", custName);
                sb.AppendFormat("<input type='hidden' name='pay_item_id' value='{0}'>", paymentCode);
                sb.AppendFormat("<input type='hidden' name='txn_ref' value='{0}'>", transID);
                sb.AppendFormat("<input type='hidden' name='currency' value='{0}'>", "566");
                sb.AppendFormat("<input type='hidden' name='amount' value='{0}'>", QTkoboValue);
                sb.AppendFormat("<input type='hidden' name='site_redirect_url' value='{0}'>", RedirectURL);
                sb.AppendFormat("<input type='hidden' name='hash' value='{0}'>", Hash);
                //Other params go here
                sb.Append("</form>");
                sb.Append("</body>");
                sb.Append("</html>");

                return Json(new { data = sb.ToString(), RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { RespCode = 1, RespMessage = "An error occurred" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AssessmentList(string EnrollID)
        {
            List<proc_computeAssessment_Result> Assessmentdata = new List<proc_computeAssessment_Result>();
            try
            {

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    Assessmentdata = unitOfWork.Assessment.GetAssessment(EnrollID, string.Format("{0:yyyy}", DateTime.Now)).ToList();
                }
                Session["netamount"] = Assessmentdata.Select(f => f.NetTax).FirstOrDefault();
                Session["enrollmentID"] = Assessmentdata.Select(f => f.enrollmentID).FirstOrDefault();
                Session["taxtype"] = 1;
                return Json(new { data = Assessmentdata, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new proc_computeAssessment_Result(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult AddParty(EnrollmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.TaxtypeID == 1)
                {
                    ErrorManager.SaveLog($"Start");
                    string comid = null;
                    string EnrollID = string.Empty;
                    CompanyData comp = new CompanyData();
                    proc_GetCompanyName_Result gf = new proc_GetCompanyName_Result();
                    proc_GetNextEnRoll_Result rec = new proc_GetNextEnRoll_Result();
                    using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                    {


                      

                        if (model.CountryID == null)
                        {
                            model.CountryID = "NG";
                        }
                        rec = unitOfWork.NextEnroll.GetNextEnrollNumber();
                        EnrollID = rec.NextEnrollmentID.ToString();
                        ErrorManager.SaveLog($"Generated EnrollID {EnrollID}");
                        var enrolmode = new EnrollmentLog
                        {

                            EnrollmentID = EnrollID,
                            TaxtypeID = model.TaxtypeID,
                            Firstname = model.Firstname,
                            Middlename = model.Middlename,
                            Lastname = model.Lastname,
                            Address = model.Address,
                            Phonenumber = model.Phonenumber,
                            Email = model.Email,
                            MaritalStatus = model.MaritalStatus,
                            Gender = model.Gender,
                            Position = model.Position,
                            Numberofchildren = model.Numberofchildren,
                            Status = "A",
                            CreatedBy = User.Identity.Name,
                            CreateDate = DateTime.Now,
                            CountryID = "NG",
                            StateCode = model.StateCode,
                            CityCode = model.CityCode
                            
                        };
                        //repoEnroll.Insert(enrolmode);
                        unitOfWork.EnrollLog.Add(enrolmode);

                        var NewUID = SmartObj.GenerateRandNum();
                        ErrorManager.SaveLog($"Generated New Guid {NewUID}");

                        var user = new AspNetUser
                        {
                            UserName = EnrollID,
                            Id = Guid.NewGuid().ToString(),
                            Email = model.Email,
                            LastName = model.Lastname,
                            FirstName = model.Firstname,
                            RoleId = 12,
                            UserId = User.Identity.Name,
                            CreateUserId = User.Identity.Name,
                            CreateDate = DateTime.Now,
                            EnforcePasswordChangeDays = 90,
                            FullName = model.Firstname + " " + model.Lastname,
                            ForcePassword = true,
                            IsApproved = true,
                            MobileNo = model.Phonenumber,
                            Status = "A",
                            DeptCode = null,
                            EnrollmentID = EnrollID,
                            EnforcePasswordChange="Y",
                            PasswordHash = SmartObj.Encrypt(model.Firstname)
                        };
                        ErrorManager.SaveLog($"Saved User");
                        unitOfWork.Users.Add(user);

                        try
                        {
                            unitOfWork.Complete();
                            ErrorManager.SaveLog($"Save Completed");
                        }
                        catch (Exception ex)
                        {
                            ErrorManager.SaveLog($"Error occured {ex.Message}");

                            return Json(new { RespCode = -1, RespMessage = ex.Message == null ?ex.InnerException.Message:ex.Message}, JsonRequestBehavior.AllowGet);
                        }
                        // Send email and sms to customer
                        proc_Notification_Result rec2 = new proc_Notification_Result();
                        rec2 = unitOfWork.AddNotification.AddAlert("ENROLLMENT", model.Email, model.Phonenumber, EnrollID + "," + model.Firstname, model.Firstname);
                        if (rec2 == null)
                        {

                            var obj = new { RespCode = 1, RespMessage = "Record Not Found" };
                            return Json(obj, JsonRequestBehavior.AllowGet);

                        }
                    }
                    return Json(new { RespCode = 0, RespMessage = "Success created your account. Kindly check for email", EnrollID = EnrollID }, JsonRequestBehavior.AllowGet);
                }
                else if (model.TaxtypeID == 3)
                {
                    //Process Payment save in tranlog
                    var tranlog = new TransactionLog()
                    {
                       

                    };
                }
                else if (model.TaxtypeID == 2)
                {
                    try
                    {
                        proc_GetCompanyName_Result gf = new proc_GetCompanyName_Result();
                        proc_GetNextEnRoll_Result rec = new proc_GetNextEnRoll_Result();
                        string comid = null;
                        string EnrollID = string.Empty;
                        CompanyData comp = new CompanyData();
                        if (model.Companyname != null || String.IsNullOrWhiteSpace(model.Companyname))
                        {
                            using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                            {
                                //var ctE = unitOfWork.EnrollLog.GetEnrollDetailsCouunt(model.Firstname); //repoEnroll.AllEager(f => f.Email == model.Email).Count();
                                //if (ctE > 0)
                                //{
                                //    // return Json(new { RespCode = 1, RespMessage = "E-Mail Address already exist" });
                                //    return Json(new { RespCode = 1, RespMessage = "Email already exists" }, JsonRequestBehavior.AllowGet);

                                //}

                                //var use = unitOfWork.Users.GetCountByEmail(model.Email); //repoEnroll.AllEager(f => f.Email == model.Email).Count();
                                //if (use > 0)
                                //{
                                //    // return Json(new { RespCode = 1, RespMessage = "E-Mail Address already exist" });
                                //    return Json(new { RespCode = 1, RespMessage = "Email already exists" }, JsonRequestBehavior.AllowGet);

                                //}

                                gf = unitOfWork.CompanyName.GetCustomerName(model.Companyname);
                                rec = unitOfWork.NextEnroll.GetNextEnrollNumber();
                                EnrollID = rec.NextEnrollmentID.ToString();
                                if (gf.noofcomp  == 0)
                                {
                                    comp = new CompanyData
                                    {
                                        CompanyName = model.Companyname,
                                        TaxtypeID = model.TaxtypeID,
                                        Status = "A",
                                        CreatedBy = User.Identity.Name,
                                        CreateDate = DateTime.Now
                                    };
                                    unitOfWork.CompanyData.Add(comp);
                                    unitOfWork.Complete();
                                }
                                comid = gf == null ? comp.ItbID.ToString() : gf.noofcomp.ToString();


                                var enrolmode = new EnrollmentLog
                                {

                                    EnrollmentID = EnrollID,
                                    TaxtypeID = model.TaxtypeID,
                                    Firstname = model.Firstname,
                                    Middlename = model.Middlename,
                                    CompanyID = int.Parse (comid),
                                    Lastname = model.Lastname,
                                    Address = model.Address,
                                    Phonenumber = model.Phonenumber,
                                    Email = model.Email,
                                    MaritalStatus = model.MaritalStatus,
                                    Gender = model.Gender,
                                    Position = model.Position,
                                    Numberofchildren = model.Numberofchildren,
                                    Status = "A",
                                    CreatedBy = User.Identity.Name,
                                    CreateDate = DateTime.Now,
                                    CountryID = "NG",
                                    StateCode = model.StateCode,
                                    CityCode = model.CityCode
                                };
                                //repoEnroll.Insert(enrolmode);
                                unitOfWork.EnrollLog.Add(enrolmode);

                                var NewUID = SmartObj.GenerateRandNum();
                                var pass = string.Empty;
                                var passstr = model.Companyname.ToString().Replace(" ","").Trim();
                                if (passstr.Length>=10)
                                {
                                    pass =  passstr.Substring(0,9);
                                }
                                else
                                {
                                     pass =  passstr.Substring(0, (passstr.Length-1));
                                }
                               
                               var user = new AspNetUser
                                {
                                    UserName = EnrollID,
                                    Id = Guid.NewGuid().ToString(),
                                    Email = model.Email,
                                    LastName = model.Lastname,
                                    FirstName = model.Firstname,
                                    RoleId = 12,
                                    UserId = User.Identity.Name,
                                    CreateUserId = User.Identity.Name,
                                    CreateDate = DateTime.Now,
                                    EnforcePasswordChangeDays = 90,
                                    FullName = model.Firstname + " " + model.Lastname,
                                    ForcePassword = true,
                                    IsApproved = true,
                                    MobileNo = model.Phonenumber,
                                    Status = "P",
                                    DeptCode = null,
                                    EnrollmentID = EnrollID,
                                    PasswordHash = SmartObj.Encrypt(pass),
                                    EnforcePasswordChange ="Y"
                                };

                                unitOfWork.Users.Add(user);

                                try
                                {
                                    unitOfWork.Complete();
                                }
                                catch (Exception ex)
                                {

                                }

                                // Send email and sms to customer
                                proc_Notification_Result rec2 = new proc_Notification_Result();
                                rec2 = unitOfWork.AddNotification.AddAlert("ENROLLMENT", model.Email, model.Phonenumber, EnrollID + "," + pass, pass);
                                if (rec2 == null)
                                {

                                    var obj = new { RespCode = 1, RespMessage = "Record Not Found" };
                                    return Json(obj, JsonRequestBehavior.AllowGet);

                                }

                            }
                        }
                        return Json(new { RespCode = 0, RespMessage = "Success created your account. Kindly check for email or mobile phone", EnrollID = EnrollID }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return Json(new { RespCode = 1, RespMessage = "An error occurred"}, JsonRequestBehavior.AllowGet);

        }
        //Makepayment

        public ActionResult VerifyPay()
        {
            return View();
        }

       

        [HttpPost]
        public ActionResult Makepayment(int Taxtype)
        {
          int productID;
         String sAmtToPay, SelectedServ, transID, RedirectURL, currency, unHash, MAC, Hash, paymentCode, StageserURL;
         double QTkoboValue, amtTopay;
         string custID, custName;
            if (ModelState.IsValid)
            {
                
                paymentCode = "";
                MAC = "D3D1D05AFE42AD50818167EAC73C109168A0F108F32645C8B59E897FA930DA44F9230910DAC9E20641823799A107A02068F7BC0F4CC41D2952E249552255710F";

                SelectedServ = "Direct Assessment";

                amtTopay = double.Parse(Session["netamount"].ToString());
                QTkoboValue = amtTopay * 100;
                //Session["QTkoboValue"] = QTkoboValue;
                Session.Add("QTkoboValue", QTkoboValue);

                productID = Taxtype;

                ////if (productID == 2)
                ////{
                ////    paymentCode = "101";
                ////    productID = 6205;

                ////}
                ////else if (productID == 3)
                ////{
                ////    paymentCode = "101";
                ////    productID = 6205;

                ////}
                ////else if (productID == 1)
                ////{
                ////    paymentCode = "101";
                ////    productID = 6205;

                ////}
                //Session["productID"] = productID;
                Session.Add("productID", productID);
                custID = "10020030340003";
                custName = "Babatunde Aluko";
                currency = "566";
                Session["paymentCode"] = paymentCode;
                transID = SmartObj.TransactionGenerator();
                Session.Add("transID", transID);
                //Session["transID"] = transID;
                RedirectURL = "http://localhost:4301/WebPayQuery.aspx";
                Session["RedirectURL"] = RedirectURL;
                StageserURL = "https://stageserv.interswitchng.com/test_paydirect/pay";


                unHash = transID + productID + paymentCode + QTkoboValue + RedirectURL + MAC;

                Hash = SmartObj.hashSHA512(unHash);
                StringBuilder sb = new StringBuilder();
                sb.Append("<html>");
                sb.AppendFormat("<body onload='document.forms[\"form\"].submit()'>");
                sb.AppendFormat("<form name='form' action='{0}' method='post'>", StageserURL);
                sb.AppendFormat("<input type='hidden' name='product_id' value='{0}'>", productID);
                sb.AppendFormat("<input type='hidden' name='cust_id' value='{0}'>", custID);
                sb.AppendFormat("<input type='hidden' name='cust_name' value='{0}'>", custName);
                sb.AppendFormat("<input type='hidden' name='pay_item_id' value='{0}'>", paymentCode);
                sb.AppendFormat("<input type='hidden' name='txn_ref' value='{0}'>", transID);
                sb.AppendFormat("<input type='hidden' name='currency' value='{0}'>", "566");
                sb.AppendFormat("<input type='hidden' name='amount' value='{0}'>", QTkoboValue);
                sb.AppendFormat("<input type='hidden' name='site_redirect_url' value='{0}'>", RedirectURL);
                sb.AppendFormat("<input type='hidden' name='hash' value='{0}'>", Hash);
                //Other params go here
                sb.Append("</form>");
                sb.Append("</body>");
                sb.Append("</html>");

                Response.Write(sb.ToString());
                Response.End();
                //return Json(new { RespCode = 0, RespMessage = "Success created your account. Kindly check for email", EnrollID = EnrollID }, JsonRequestBehavior.AllowGet);

            }

            return Json(new { RespCode = 1, RespMessage = "An error occurred" }, JsonRequestBehavior.AllowGet);

        }
    }
    public class ErrorManager
    {
        private static object cvLockObject = new object();
        private static string cvsLogFile = System.Configuration.ConfigurationManager.AppSettings["LogFile"];
        private static string filePath = System.Configuration.ConfigurationManager.AppSettings["LogFilePath"];
        private static string LogSize = System.Configuration.ConfigurationManager.AppSettings["LogSize"];
        private static string GetUniqueFilePath(string filepath)
        {
            if (File.Exists(filepath))
            {
                string folder = Path.GetDirectoryName(filepath);
                string filename = Path.GetFileNameWithoutExtension(filepath);
                string extension = Path.GetExtension(filepath);
                int number = 1;

                Match regex = Regex.Match(filepath, @"(.+) \((\d+)\)\.\w+");

                if (regex.Success)
                {
                    filename = regex.Groups[1].Value;
                    number = int.Parse(regex.Groups[2].Value);
                }

                do
                {
                    number++;
                    filepath = Path.Combine(folder, string.Format("{0} ({1}){2}", filename, number, extension));
                }
                while (File.Exists(filepath));
            }

            return filepath;
        }
        public static void SaveLog(string psDetails)
        {

            FileInfo f = new FileInfo(Path.Combine(filePath, cvsLogFile));
            string new_file_name = string.Empty;
            if (File.Exists(Path.Combine(filePath, cvsLogFile)))
            {
                long s1 = f.Length;
                if (s1 > Convert.ToInt32(LogSize))
                {
                    new_file_name = GetUniqueFilePath(Path.Combine(filePath, cvsLogFile));

                    string filename = new_file_name;


                    File.Move(Path.Combine(filePath, cvsLogFile), filename);
                }
            }
            lock (cvLockObject)
            {
                File.AppendAllText(Path.Combine(filePath, cvsLogFile), DateTime.Now.ToString() + ": " + psDetails + Environment.NewLine);

            }
        }
    }
}