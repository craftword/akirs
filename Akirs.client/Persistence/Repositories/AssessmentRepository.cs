using Akirs.client.DL;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence.Repositories
{
    public class AssessmentRepository : Repository<proc_computeAssessment_Result>, IAssessmentRepository
    {
        public AssessmentRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }

        //may not need this as the enrollID is shared by uploaded employees
        //public IEnumerable<proc_computeAssessment_Result> GetAssessment(string EnrollId, string counter)
        //{
        //    var ret = PlutoContext.proc_computeAssessment(EnrollId).ToList();
        //    return ret;
        //}
        public IEnumerable<proc_computeAssessment_Result> GetAssessment(string EnrollId, string yearValue)
        {
            var ret = PlutoContext.proc_computeAssessment(EnrollId, yearValue).ToList();
            return ret;
        }

        public proc_computeAssessment_Result GetAssessmentSingle(string EnrollId, string yearValue)
        {
            var ret = PlutoContext.proc_computeAssessment(EnrollId, yearValue).FirstOrDefault();
            return ret;
        }

        public proc_computeAssessment_modification_Result GetAssessmentSingle2(string EnrollId, string yearValue)
        {
            var ret = PlutoContext.proc_computeAssessment_modification(EnrollId, yearValue).FirstOrDefault();
            return ret;
        }
        public IEnumerable<AssessmentRecord> GetAssessmentSingleFirst(string EnrollId)
        {
            var ret = PlutoContext.AssessmentRecords.Where(a => a.enrollmentID == EnrollId).ToList();
            return ret;
        }

        public IEnumerable<proc_computeAssessment_Result> GetAssessmentSingleWork(string EnrollId, string yearValue)
        {
            var ret = PlutoContext.proc_computeAssessment(EnrollId, yearValue).ToList();
            return ret;
        }

        public AssessmentRecord GetDirectAssessmentSingleFirst(string EnrollId)
        {
            var ret = PlutoContext.AssessmentRecords.FirstOrDefault(p => p.enrollmentID == EnrollId);
            return ret;
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}