using Akirs.client.DL;
using Akirs.client.Enums;
using Akirs.client.repository;
using Akirs.client.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Akirs.client.Persistence.Repositories
{
    public class SalaryUploadSecondaryRepository : Repository<SALARYUPLOAD_HISTORY>, ISalaryUploadSecondaryRepository
    {
        public SalaryUploadSecondaryRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }

        public List<SALARYUPLOAD_HISTORY> GetSalaryUploadSecondary(string EnrollId, short month, int year)
        {
            var maxindex = PlutoContext.SALARYUPLOAD_HISTORY.Where(p => p.EnrollmentID == EnrollId && p.UploadYear == year).ToList();

            if (maxindex.Count > 0)
            {
                var max = maxindex.Max(p => p.IndexCount);
                var ret = PlutoContext.SALARYUPLOAD_HISTORY.Where(p => p.EnrollmentID == EnrollId && p.UploadMonthIndex == month && p.IndexCount == max).ToList();

                return ret;
            }
            return null;

        }
        public decimal GetSalaryUploadSecondaryCount(string EnrollId)
        {
            var ret = PlutoContext.SALARYUPLOAD_HISTORY.Where(p => p.EnrollmentID == EnrollId).ToList();
            if (ret == null || ret.Count == 0)
            {
                return 0;
            }
            else
            {
                return (decimal)ret.Max(p => p.Counter);
            }
        }
        public List<int?> GetYears(string EnrollId)
        {
            try
            {
                var ret = PlutoContext.SALARYUPLOAD_HISTORY.Where(p => p.EnrollmentID == EnrollId).ToList();
                if (ret.Count > 0)
                {
                    var monthsindex = ret.Select(p => p.UploadYear).DistinctBy(p => p.Value);
                    return monthsindex.ToList();
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public List<short?> GetMonths(string EnrollId)
        {
            try
            {

                var ret = PlutoContext.SALARYUPLOAD_HISTORY.Where(p => p.EnrollmentID == EnrollId && p.UploadYear == DateTime.Now.Year).ToList();
                //if(maxindex)

                if (ret.Count > 0)
                {

                    var monthsindex = ret.Select(p => p.UploadMonthIndex).DistinctBy(p => p.Value);
                    return monthsindex.ToList();
                }
                else
                {
                    return null;
                }


            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}