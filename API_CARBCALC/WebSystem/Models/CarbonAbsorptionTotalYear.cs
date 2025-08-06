using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebSystem.Models 
{
    public class CarbonAbsorptionTotalYear 
    {    
        [Key]

        public int UserId { get; set; }
        public double TotalYear { get; set; }

    }
}
