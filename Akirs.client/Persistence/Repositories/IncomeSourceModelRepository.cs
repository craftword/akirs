using Akirs.client.DL;
using Akirs.client.Implementation;
using Akirs.client.Models;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence.Repositories
{
    public class IncomeSourceModelRepository : Repository<IncomeSourceModel>, IIncomeSourceModelRepository
    {
        public IncomeSourceModelRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }
        public IncomeSourceModelRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }

        //public interface IIcomeSourceModelRepository: IRepository<IncomeSource> { }

        public IncomeSourceModel GetIncomeSourceTypeDetailsById(int Itbid)
        {
            var familydetails = (from i in PlutoContext.IncomeSources.Where(p => p.ItbID == Itbid).ToList()
                                 join j in PlutoContext.IncomeSourceTypes.ToList()
                                 on i.SourceOfIncomeID equals j.ItbID
                                 select new IncomeSourceModel
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
                                     Status = i.Status == "P" ? "Pending" : "Closed",
                                     IsNew = i.IsNew == "Y" ? "Yes" : "No"
                                 }).FirstOrDefault();

            return familydetails;


        }
        public IEnumerable<IncomeSourceModel> GetIncomeSourceTypeDetails(string EnrollId)
        {
            var familydetails = from i in PlutoContext.IncomeSources.Where(p => p.EnrollmentID == EnrollId).ToList()
                                join j in PlutoContext.IncomeSourceTypes.ToList()
                                on i.SourceOfIncomeID equals j.ItbID
                                select new IncomeSourceModel
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
                                    Status = i.Status == "P"? "Pending":"Closed",
                                    IsNew = i.IsNew == "Y"? "Yes":"No"
                                };


            return familydetails; //PlutoContext.FamilyDetails.Where(p=>p.EnrollmentID == EnrollId).ToList();
        }
        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}