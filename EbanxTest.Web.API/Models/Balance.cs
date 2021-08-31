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
        [Key]
        private int BalanceID { get; set; }
        [Column (TypeName = "double")]
        private double BalanceValue { get; set; }
    }
}
