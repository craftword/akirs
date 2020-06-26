using Akirs.client.DL;
using Akirs.client.Models;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence.Repositories
{
    public class FamilyRepository : Repository<FamilyModel>, IFamilyRepository
    {
        public FamilyRepository(AKIRSTAXEntities context) 
            : base(context)
        {
        }

        public IEnumerable<FamilyModel> GetFamilyDetails(string EnrollId)
        {
            var familydetails = (from i in PlutoContext.FamilyDetails.Where(p => p.EnrollmentID == EnrollId).ToList()
                                join j in PlutoContext.RelationShips.ToList()
                                on i.RelationshipType equals j.ItbID
                                select new FamilyModel
                                {
                                    Age = i.Age,
                                    CreateDate = i.CreateDate,
                                    CreatedBy = i.CreatedBy,
                                    EnrollmentID = i.EnrollmentID,
                                    FullName = i.FullName,
                                    IsStudent = i.IsStudent,
                                    ItbID = i.ItbID,
                                    RelationshipType = j.RelationshipName,
                                    Status = i.Status == "A" ? "Active" : "Close"
                                });


            return familydetails; //PlutoContext.FamilyDetails.Where(p=>p.EnrollmentID == EnrollId).ToList();
        }
        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}