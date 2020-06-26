using Akirs.client.DL;
using Akirs.client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akirs.client.repository
{
    public interface IAssessmentRepository : IRepository<proc_computeAssessment_Result>
    {
        IEnumerable<proc_computeAssessment_Result> GetAssessment(string EnrollId, string yearValue);
        proc_computeAssessment_Result GetAssessmentSingle(string EnrollId, string yearValue);
        proc_computeAssessment_modification_Result GetAssessmentSingle2(string EnrollId, string yearValue);
        IEnumerable<proc_computeAssessment_Result> GetAssessmentSingleWork(string EnrollId, string yearValue);



        IEnumerable<AssessmentRecord> GetAssessmentSingleFirst(string EnrollId);
        AssessmentRecord GetDirectAssessmentSingleFirst(string v);

        //AccessmentRecord_Enroll GetDirectAssessmentSingleFirst(string EnrollId);


    }
}
