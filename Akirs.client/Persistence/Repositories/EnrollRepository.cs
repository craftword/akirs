using Akirs.client.DL;
using Akirs.client.Models;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akirs.client.Persistence.Repositories
{
    public class EnrollRepository : Repository<EnrollmentLog>, IEnrollRepository
    {
        public EnrollRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }
        public EnrollmentLog GetEnrollDetails(string EnrollId)
        {

            var ret = PlutoContext.EnrollmentLogs.Where(p => p.EnrollmentID == EnrollId).FirstOrDefault();
            return ret;
        }
        public int GetEnrollDetailsCouunt(string Email)
        {
            var ret = PlutoContext.EnrollmentLogs.Where(p => p.Email == Email).Count();
            return ret;
        }
        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}