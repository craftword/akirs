using Akirs.client.DL;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence.Repositories
{
    public class AssessmentRecordRepository : Repository<AssessmentRecord>, IAssessmentRecordRepository
    {
        public AssessmentRecordRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }

        public IEnumerable<AssessmentRecord> GetAssessmentRecord(string EnrollId)
        {
            var ret = PlutoContext.AssessmentRecords.Where(p=>p.enrollmentID == EnrollId).ToList();
            return ret;
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}