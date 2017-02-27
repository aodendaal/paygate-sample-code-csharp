using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PaygateTest.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string PayRequestId { get; set; }
        public string Reference { get; set; }
        public int Amount { get; set; }
        public string EmailAddress { get; set; }
        public int Status { get; set; }
        
    }
}