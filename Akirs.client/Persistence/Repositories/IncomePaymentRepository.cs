using Akirs.client.DL;
using Akirs.client.Models;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence.Repositories
{
    public class IncomePaymentModelRepository : Repository<IncomePaymentModel>, IIncomePaymentModelRepository
    {
        public IncomePaymentModelRepository(AKIRSTAXEntities context) 
            : base(context)
        {
        }

             public IEnumerable<IncomePayment> GetIncomePayment(string EnrollId)
        {
            var Ipayment = PlutoContext.IncomePayments.Where(p => p.EnrollmentID == EnrollId).ToList();


            return Ipayment;
        }

        public IncomePayment GetIncomePaymentById(int Itbid)
        {
            var ret = PlutoContext.IncomePayments.Where(p => p.ItbID == Itbid).FirstOrDefault();
            return ret;
        }

        public IncomePaymentModel GetIncomeSourceTypeDetailsById(int Itbid)
        {
            var familydetails = (from i in PlutoContext.IncomeSources.Where(p => p.ItbID == Itbid).ToList()
                                 join j in PlutoContext.IncomeSourceTypes.ToList()
                                 on i.SourceOfIncomeID equals j.ItbID
                                 select new IncomePaymentModel
                                 {
                                     ItbID = i.ItbID,
                                     Amount = i.Amount,
                                     CreateDate = i.CreateDate,
                                     CreatedBy = i.CreatedBy,
                                     EnrollmentID = i.EnrollmentID,
                                     Last_Modified_Authid = i.Last_Modified_Authid,
                                     Last_Modified_Date = i.Last_Modified_Date,
                                     Last_Modified_Uid = i.Last_Modified_Uid,
                                     NHFFlag = i.NHFFlag,
                                     PensionFlag = i.PensionFlag,
                                     IncomeYear = i.IncomeYear,
                                     SourceOfIncome = j.SourceOfIncome,
                                     Status = i.Status == "P" ? "Pending" : "Closed"
                                 }).FirstOrDefault();

            return familydetails;


        }
        public IEnumerable<IncomePaymentModel> GetIncomePaymentDetails(string EnrollId,string yearValue, int count)
        {
            IncomePayment PaymentMsg = new IncomePayment();
            PaymentMsg = PlutoContext.IncomePayments.Where(j => j.EnrollmentID == EnrollId && j.IncomeYear == yearValue).OrderByDescending(m => m.Count).FirstOrDefault();

            var familydetails = from i in PlutoContext.IncomePayments.Where(p => p.Count == count).ToList()
                                join j in PlutoContext.IncomeSourceTypes.ToList()
                                on i.SourceOfIncomeID equals j.ItbID
                                select new IncomePaymentModel
                                {
                                    ItbID = i.ItbID,
                                    Amount = i.Amount,
                                    CreateDate = i.CreateDate,
                                    CreatedBy = i.CreatedBy,
                                    EnrollmentID = i.EnrollmentID,
                                    Last_Modified_Authid = i.Last_Modified_Authid,
                                    Last_Modified_Date = i.Last_Modified_Date,
                                    Last_Modified_Uid = i.Last_Modified_Uid,
                                    NHFFlag = i.NHFFlag,
                                    PensionFlag = i.PensionFlag,
                                    IncomeYear = i.IncomeYear,
                                    SourceOfIncome = j.SourceOfIncome,
                                    Status = i.Status == "P" ? "Pending" : "Paid"
                                };


            return familydetails; //PlutoContext.FamilyDetails.Where(p=>p.EnrollmentID == EnrollId).ToList();
        }

        public IEnumerable<IncomePaymentModel> GetIncomePaymentDetails2(string EnrollId, string yearValue)
        {
            var familydetails = from i in PlutoContext.IncomePayments.Where(p => p.IncomeYear == yearValue).ToList()
                                join j in PlutoContext.IncomeSourceTypes.ToList()
                                on i.SourceOfIncomeID equals j.ItbID
                                select new IncomePaymentModel
                                {
                                    ItbID = i.ItbID,
                                    Amount = i.Amount,
                                    CreateDate = i.CreateDate,
                                    CreatedBy = i.CreatedBy,
                                    EnrollmentID = i.EnrollmentID,
                                    Last_Modified_Authid = i.Last_Modified_Authid,
                                    Last_Modified_Date = i.Last_Modified_Date,
                                    Last_Modified_Uid = i.Last_Modified_Uid,
                                    NHFFlag = i.NHFFlag,
                                    PensionFlag = i.PensionFlag,
                                    IncomeYear = i.IncomeYear,
                                    SourceOfIncome = j.SourceOfIncome,
                                    Status = i.Status == "P" ? "Pending" : "Paid"
                                };


            return familydetails; //PlutoContext.FamilyDetails.Where(p=>p.EnrollmentID == EnrollId).ToList();
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }

   
}