using Akirs.client.DL;
using Akirs.client.Models;
using Akirs.client.repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Persistence.Repositories
{
    public class TransactionLogRepository : Repository<TransactionLog>, ITransactionLog
    {
        public TransactionLogRepository(AKIRSTAXEntities context)
            : base(context)
        {

        }

        public bool CreateTansactionLog(TransactionLogModel trans)


        {
            try
            {
                //adding transactionlog to db
                var newTranLog = new TransactionLog
                {
                    TransactionCode = trans.TransactionCode,
                    TransactionDate = DateTime.Now,
                    Amount = trans.Amount,
                    AccountName = trans.AccountName,
                    ResponseDescription = trans.ResponseDescription,
                    TransactionReference = trans.TransactionReference,
                    Status = trans.Status,
                    TransactionFee = trans.TransactionFee,
                    ReturnUserId = trans.ReturnUserId,
                    Createdate = DateTime.Now,
                    UserID = trans.UserId.ToString()

                };

                var savedLog = PlutoContext.TransactionLogs.Add(newTranLog);
                PlutoContext.SaveChanges();
                if (savedLog != null)
                {
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                return false;
            }
       
        }

        public List<TransactionLogModel> GetTransactionLog(string enrollId)
        {
            try
            {
                var tranLog = new List<TransactionLogModel>();

                var tranLogModel = new TransactionLogModel();

                var logs = PlutoContext.TransactionLogs.Where(t => t.UserID == enrollId).ToList();

                if (logs != null)
                {
                    foreach ( var item in logs)
                    {
                        tranLogModel.Amount = item.Amount;
                        tranLogModel.CreateDate = item.TransactionDate;
                        tranLogModel.AccountName = item.AccountName;
                        tranLogModel.Status = item.Status;


                        tranLog.Add(tranLogModel);

                    }
                    return tranLog;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

         //   return new List<TransactionLogModel>();
        }
        public AKIRSTAXEntities PlutoContext
        {
            get { return Context as AKIRSTAXEntities; }
        }
    }
}