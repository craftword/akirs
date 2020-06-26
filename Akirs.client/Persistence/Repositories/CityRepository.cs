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
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(AKIRSTAXEntities context)
            : base(context)
        {
        }
        public IEnumerable<City> GetCities(string StateCode)
        {

            var ret = PlutoContext.Cities.Where(p => p.StateCode == StateCode && p.Status =="A").ToList();
            return ret;
        }

        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}