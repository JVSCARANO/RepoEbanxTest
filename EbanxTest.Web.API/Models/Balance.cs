using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EbanxTest.Web.API.Models
{
    public class Balance
    {
        public Balance (int balanceID, double balanceValue)
        {
            BalanceID = balanceID;
            BalanceValue = balanceValue;
        }
        
        [Key]
        public int BalanceID { get; set; }
        [Column (TypeName = "double")]
        public double BalanceValue { get; set; }
    }
}
