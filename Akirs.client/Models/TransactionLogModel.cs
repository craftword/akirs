using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akirs.client.Models
{
    public class TransactionLogModel
    {
        public string TransactionCode { get; set; }
        public DateTime TransactionDate { get; set; }

        public decimal Amount { get; set; }

        public decimal TransactionFee { get; set; }

        public string AccountName { get; set; }
        public string TransactionReference { get; set; }

        public string ResponseDescription { get; set; }
        public string Status { get; set; }

        public int UserId { get; set; }

        public DateTime CreateDate { get; set; }
        public string ReturnUserId { get; set; }
    }
}