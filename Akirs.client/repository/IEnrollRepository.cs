using Akirs.client.DL;
using Akirs.client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akirs.client.repository
{
    public interface IEnrollRepository : IRepository<EnrollmentLog>
    {
        EnrollmentLog GetEnrollDetails(string EnrollId);
        int GetEnrollDetailsCouunt(string Email);
    }
}
