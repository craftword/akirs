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
    public class FamilyController : Controller
    {
        //string EnrollId = string.Empty;
        public FamilyController()
        {
            
            //EnrollId = Session["EnrollID"].ToString();
        }
        [HttpPost]
        // [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FamilyDetail model)
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
                            unitOfWork.FamilyDetails.Add(model);
                            unitOfWork.Complete();
                        }

                        return Json(new { data = model, RespCode = 0, RespMessage = "Record Created Successfully" });


                    }
                    else
                    {
                        using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                        {
                            unitOfWork.FamilyDetails.Add(model);
                            unitOfWork.FamilyDetails.Update(model);
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
            try
            {
                FamilyDetail rec = null;
                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    rec = unitOfWork.FamilyDetails.Get(id);
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
        public ActionResult Index()
        {
            return View();
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
        public ActionResult FamilyList()
        {
            List<FamilyModel> Familydata = new List<FamilyModel>();
            try
            {

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    Familydata = unitOfWork.Family.GetFamilyDetails(Session["EnrollID"].ToString()).ToList();
                }
                return Json(new { data = Familydata, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<FamilyDetail>(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Json(data);
        }

        public ActionResult UserProfile()
        {
            return View();
        }

        public ActionResult GetUserProfile()
        {
              
            var user = new EnrollmentLog();
            try
            {

                using (var unitOfWork = new UnitOfWork(new AKIRSTAXEntities()))
                {
                    user = unitOfWork.EnrollLog.GetEnrollDetails(Session["EnrollID"].ToString());
                    var userModel = new UserModel
                    {
                        EnrollmentId = user.EnrollmentID,
                        Email = user.Email,
                        FirstName = user.Firstname,
                        LastName = user.Lastname,
                        PhoneNo = user.Phonenumber,
                        Address = user.Address,
                        Gender = user.Gender,
                        NoOfChildren = user.Numberofchildren,
                        Status = user.Status,
                        MaritalStatus = user.MaritalStatus,
                        CreateDate = user.CreateDate,
                        City = user.CityCode,
                        CompanyName = user.Companyname
                    };
                    return Json(new { data = userModel, RespCode = 0, RespMessage = "Success" }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(new { data = new List<UserModel>(), RespCode = 2, RespMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return View();
        }
        public ActionResult CompanyProfile()
        {
            return View();
        }
    }
}