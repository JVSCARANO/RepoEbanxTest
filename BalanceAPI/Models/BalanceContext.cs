using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BalanceAPI.Models
{
    public class BalanceContext : DbContext
    {
        public BalanceContext(DbContextOptions<BalanceContext> options):base(options)
        {
                  
        }

        public DbSet<Balance> Balance { get; set; }
    }
}
