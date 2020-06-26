using Akirs.client.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akirs.client.repository
{
    public interface IIncomePaymentRepository : IRepository<IncomePayment>
    {
        IEnumerable<IncomePayment> GetIncomePayment(string EnrollId);
        IncomePayment GetIncomePaymentById(int Itbid);

    }
}
