using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PaygateTest.Data
{
    public class TestContext : DbContext
    {
        public DbSet<Models.Transaction> Transactions { get; set; }
    }
}