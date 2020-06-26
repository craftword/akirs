using Akirs.client.DL;
using Akirs.client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akirs.client.repository
{
    public interface IIncomePaymentModelRepository : IRepository<IncomePaymentModel>
    {
        IEnumerable<IncomePaymentModel> GetIncomePaymentDetails(string EnrollId, string yearValue, int count);
        IEnumerable<IncomePaymentModel> GetIncomePaymentDetails2(string EnrollId, string yearValue);
        IncomePaymentModel GetIncomeSourceTypeDetailsById(int Itbid);
    }
}
