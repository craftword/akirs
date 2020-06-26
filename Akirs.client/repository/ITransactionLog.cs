using Akirs.client.DL;
using Akirs.client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.repository
{
    public interface ITransactionLog : IRepository<TransactionLog>
    {
        bool CreateTansactionLog(TransactionLogModel trans); 

        List<TransactionLogModel> GetTransactionLog (string enr) ;
    }
}