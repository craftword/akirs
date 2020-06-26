using Akirs.client.DL;
using Akirs.client.Enums;
using Akirs.client.repository;
using Akirs.client.utility;
using Akirs.client.ViewModel;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;


namespace Akirs.client.Persistence.Repositories
{
    public class SalaryUploadRepository : Repository<SALARYUPLOAD>, ISalaryUploadRepository
    {
        public SalaryUploadRepository(AKIRSTAXEntities context)
            : base(context)
        {
            
        }
        public string ApprovePayrollByEmployeeId(string EnrollId)
        {
           
                var entity = PlutoContext.SALARYUPLOADs.Where(x => x.EnrollmentID == EnrollId
                                                                && x.PayrollStatus == PayrollStatus.PENDING.ToString()
                                                                && x.IsDeleted == false).ToList();
            if (entity.Count > 0)
            {
                using (var unitOfWork = new UnitOfWork(PlutoContext))
                {
                    foreach (var employee in entity)
                    {
                    employee.PayrollStatus = PayrollStatus.APPROVED.ToString();
                   
                        unitOfWork.SalaryUpload.Update(employee);
                        unitOfWork.Complete();
                    }
                }
                var message = "Approved Successfully";
                return message;
            }
            else
            {
                var message = " No Records Found For Approval";
                return message;
            }
            
        }

        public string SendPayeeForApproval(string EnrollId)
        {

            var entity = PlutoContext.SALARYUPLOADs.Where(x => x.EnrollmentID == EnrollId
                                                            && x.PayrollStatus == PayrollStatus.AWAITINGAPPROVAL.ToString()
                                                            && x.IsDeleted == false).ToList();
            if (entity.Count > 0)
            {
                using (var unitOfWork = new UnitOfWork(PlutoContext))
                {
                    foreach (var employee in entity)
                    {
                        employee.PayrollStatus = PayrollStatus.PENDING.ToString();

                        unitOfWork.SalaryUpload.Update(employee);
                        unitOfWork.Complete();
                    }
                }
                var message = "Sent For Approval Successfully";
                return message;
            }
            else
            {
                var message = " No Records Found For Approval";
                return message;
            }

        }

        public string RejectPayrollEmployee(string EnrollId)
        {

            var entity = PlutoContext.SALARYUPLOADs.Where(x => x.EnrollmentID == EnrollId
                                                            && x.PayrollStatus == PayrollStatus.PENDING.ToString()
                                                            && x.IsDeleted == false).ToList();
            if (entity.Count > 0)
            {
                using (var unitOfWork = new UnitOfWork(PlutoContext))
                {
                    foreach (var employee in entity)
                    {
                        employee.PayrollStatus = PayrollStatus.REJECTED.ToString();

                        unitOfWork.SalaryUpload.Update(employee);
                        unitOfWork.Complete();
                    }
                }
                var message = "Record Rejected. Sent Back for Review";
                return message;
            }
            else
            {
                var message = " No Records Found ";
                return message;
            }

        }

        public SALARYUPLOAD GetDetailsForEdit( string EnrollId, SALARYUPLOAD model)
        {
            var entity = PlutoContext.SALARYUPLOADs.Where(x => x.EnrollmentID == EnrollId
                                                               && x.PayrollStatus == PayrollStatus.PENDING.ToString()
                                                               && x.IsDeleted == false).FirstOrDefault();
            if (entity != null)
            {
                using (var unitOfWork = new UnitOfWork(PlutoContext))
                {

                    entity.EnrollmentID = entity.EnrollmentID;
                    entity.EmployeeID = model.EmployeeID ?? entity.EmployeeID;
                    entity.EmployeeName = model.EmployeeName ?? entity.EmployeeName;
                    entity.AnnualBasic = model.AnnualBasic ?? entity.AnnualBasic;
                    entity.AnnualHousing = model.AnnualHousing ?? entity.AnnualHousing;
                    entity.AnnualMeal = model.AnnualMeal ?? entity.AnnualMeal;
                    entity.AnnualOthers = model.AnnualOthers ?? entity.AnnualOthers;
                    entity.AnnualTransport = model.AnnualTransport ?? entity.AnnualTransport;
                    entity.AuthUserID = model.AuthUserID ?? entity.AuthUserID;
                    entity.Counter = model.Counter ?? entity.Counter;
                    entity.CreateDate = entity.CreateDate;
                    entity.CreatedBy = model.CreatedBy ?? entity.CreatedBy;
                    entity.GrossPay = model.GrossPay ?? entity.GrossPay;
                    entity.IsDeleted = entity.IsDeleted;
                    entity.ItbID =  entity.ItbID;
                    entity.Last_Modified_Authid = model.Last_Modified_Authid ?? entity.Last_Modified_Authid;
                    entity.Last_Modified_Date = model.Last_Modified_Date ?? entity.Last_Modified_Date;
                    entity.Last_Modified_Uid = model.Last_Modified_Uid ?? entity.Last_Modified_Uid;
                    entity.NextUploadDate = entity.NextUploadDate;
                    entity.NHFContribution = model.NHFContribution ?? entity.NHFContribution;
                    entity.NHIS = model.NHIS ?? entity.NHIS;
                    entity.Others = model.Others ?? entity.Others;
                    entity.PayrollStatus = model.PayrollStatus ?? entity.PayrollStatus;
                    entity.Pension = model.Pension ?? entity.Pension;
                    entity.Premium = model.Premium ?? entity.Premium;
                    entity.VALIDATIONERRORSTATUS = model.VALIDATIONERRORSTATUS ?? entity.VALIDATIONERRORSTATUS;

                    unitOfWork.SalaryUpload.Update(entity);
                    unitOfWork.Complete();
                }
                return entity;
            }
            else
            {
                return null;
            }
        }
        public IEnumerable<SALARYUPLOAD> GetPendingSalaryUpload(string EnrollId)
        {
            var ret = PlutoContext.SALARYUPLOADs.Where(p => p.EnrollmentID == EnrollId  && p.PayrollStatus == PayrollStatus.PENDING.ToString() && p.IsDeleted == false
            ||  p.EnrollmentID == EnrollId && p.PayrollStatus == PayrollStatus.REJECTED.ToString() && p.IsDeleted == false).ToList();
            return ret;
        }
        public IEnumerable<SALARYUPLOAD> GetSalaryUpload(string EnrollId)
        {
            var ret = PlutoContext.SALARYUPLOADs.Where(p => p.EnrollmentID == EnrollId 
                                                        && p.PayrollStatus == PayrollStatus.PENDING.ToString()
                                                        && p.IsDeleted == false).ToList();
            return ret;
        }

        public string CreatePayeeUser(CreatePayeeUserViewModel model)
        {
            if (model == null)
            {
                throw new Exception("User Details is empty");
            }
                using (var unitOfWork = new UnitOfWork(PlutoContext))
            {
                // check if user already exist
                var checkUser = PlutoContext.AspNetUsers.Where(x => x.EnrollmentID == model.EnrollmentID && x.Email == model.Email).FirstOrDefault();

                if (checkUser == null)
                {
                    // Create the role
                    var role = new AspNetRole
                    {
                        Id = new Random().Next(0000, 99999).ToString(),
                        Name = model.RoleName.ToString(),
                    };
                    unitOfWork.UserRoleRepository.Add(role);
                    unitOfWork.Complete();

                    var entity = PlutoContext.AspNetRoles.Where(x => x.Id == role.Id).FirstOrDefault();

                    // Create User
                    var user = new AspNetUser
                    {
                        EnrollmentID = model.EnrollmentID,
                        Id = Guid.NewGuid().ToString(),
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        FullName = model.FirstName + " " + model.LastName,
                        RoleId = Int32.Parse(role.Id),
                        RoleName = model.RoleName.ToString(),
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        UserName = model.EnrollmentID,
                        PasswordHash = SmartObj.Encrypt(model.Password.ToString()),
                        CreateDate = DateTime.Now,
                        EnforcePasswordChangeDays = 90,
                        ForcePassword = true,
                        IsApproved = true,
                        Status = "A",
                        EnforcePasswordChange = "Y",
                        IsDeleted = false
                    };
                    unitOfWork.Users.Add(user);
                    unitOfWork.Complete();

                    // Send email and sms to customer
                    proc_Notification_Result rec2 = new proc_Notification_Result();
                    rec2 = unitOfWork.AddNotification.AddAlert("ENROLLMENT", model.Email, model.PhoneNumber, model.EnrollmentID + "," + model.FirstName + "," + model.FullName, model.FirstName);

                }

                else
                {
                    return "User Already Exist";
                }

            }
            return "User Created Successfully";
        }


        public SALARYUPLOAD GetSalaryUploadById(int Itbid)
        {
            var ret = PlutoContext.SALARYUPLOADs.Where(p => p.ItbID == Itbid && p.IsDeleted == false).FirstOrDefault();
            return ret;
        }

        public List<SALARYUPLOAD> UploadPayroll(HttpPostedFileBase file, string SessionID)
        {
            var querySalaryupload = PlutoContext.SALARYUPLOADs.Where(x => x.EnrollmentID == SessionID && x.IsDeleted == false).ToList();

            using (var unitOfWork = new UnitOfWork(PlutoContext))
            {
                List<SALARYUPLOAD> saveuplList = new List<SALARYUPLOAD>();
                if (querySalaryupload.Count > 0 )
                {
                    if (querySalaryupload.FirstOrDefault().NextUploadDate != null || querySalaryupload.FirstOrDefault().NextUploadDate >= DateTime.Now)
                    {
                        throw new Exception("Upload is done once a month, Please wait for next month");
                        
                    }
                    else
                    {
                        foreach (var entity in querySalaryupload)
                        {
                            entity.IsDeleted = true;

                            unitOfWork.SalaryUpload.Update(entity);
                            unitOfWork.Complete();
                        }
                    }
                }
                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var noOfCol = workSheet.Dimension.End.Column;
                        var noOfRow = workSheet.Dimension.End.Row;
                        var count = 0;

                        for (int rowIterator = 2; rowIterator <= noOfRow; rowIterator++)
                        {
                            count++;
                            var saveupl = new SALARYUPLOAD();

                            saveupl.EnrollmentID = SessionID;
                            saveupl.EmployeeID = workSheet.Cells[rowIterator, 1].Value.ToString();
                            saveupl.EmployeeName = workSheet.Cells[rowIterator, 2].Value.ToString();
                            saveupl.AnnualBasic = decimal.Parse(workSheet.Cells[rowIterator, 3].Value.ToString());
                            saveupl.AnnualHousing = decimal.Parse(workSheet.Cells[rowIterator, 4].Value.ToString());
                            saveupl.AnnualTransport = decimal.Parse(workSheet.Cells[rowIterator, 5].Value.ToString());
                            saveupl.AnnualMeal = decimal.Parse(workSheet.Cells[rowIterator, 6].Value.ToString());
                            saveupl.AnnualOthers = decimal.Parse(workSheet.Cells[rowIterator, 7].Value.ToString());
                            saveupl.NHFContribution = decimal.Parse(workSheet.Cells[rowIterator, 8].Value.ToString());
                            saveupl.PayrollStatus = PayrollStatus.AWAITINGAPPROVAL.ToString();
                            saveupl.IsDeleted = false;
                            saveupl.CreateDate = DateTime.Now;
                            saveupl.NextUploadDate = DateTime.Now.AddDays(30);
                            saveupl.Counter = count;
                            saveupl.VALIDATIONERRORSTATUS = true;

                            saveuplList.Add(saveupl);
                            unitOfWork.SalaryUpload.Add(saveupl);
                            unitOfWork.Complete();
                        }
                    }
                return saveuplList;

            }
        }

        public IEnumerable<AspNetUser> GetPayeeUserForDelete(string EnrollId)
        {
            var payeeUsers = PlutoContext.AspNetUsers.Where(x => x.EnrollmentID == EnrollId && x.IsDeleted == false && x.RoleName != "Admin").ToList();
            return payeeUsers;
        }

        public string DeletePayeeUser(string emailAddress)
        {
            using (var unitOfWork = new UnitOfWork(PlutoContext))
            {
                var payeeUser = PlutoContext.AspNetUsers.Where(x => x.Email == emailAddress && x.IsDeleted == false && x.RoleName != "Admin").ToList();

                if (payeeUser.Count > 0)
                {
                    foreach (var payee in payeeUser)
                    {
                        payee.IsDeleted = true;

                        PlutoContext.AspNetUsers.AddOrUpdate(payee);
                        unitOfWork.Complete();
                    }
                    var message = "User Deleted Successfully";
                    return message;
                }
                else
                {
                    var message = "User Does not Exist";
                    return message;
                }

            }
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}