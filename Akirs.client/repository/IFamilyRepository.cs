using Akirs.client.DL;
using Akirs.client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akirs.client.repository
{
    public interface IFamilyRepository : IRepository<FamilyModel>
    {
        IEnumerable<FamilyModel> GetFamilyDetails(string EnrollId);
    }


}
