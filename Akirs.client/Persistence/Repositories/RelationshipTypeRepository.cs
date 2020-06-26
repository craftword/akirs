using Akirs.client.DL;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Akirs.client.Persistence.Repositories
{
    public class RelationshipTypeRepository : Repository<RelationShip>, IRelationshipTypeRepository
    {
        public RelationshipTypeRepository(AKIRSTAXEntities context) 
            : base(context)
        {
        }

        public IEnumerable<RelationShip> GetRelationshipType()
        {
            var familydetails = PlutoContext.RelationShips.Where(p => p.Status == "A").ToList();


            return familydetails; //PlutoContext.FamilyDetails.Where(p=>p.EnrollmentID == EnrollId).ToList();
        }
        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}