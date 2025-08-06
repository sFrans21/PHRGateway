using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebSystem.Models
{
    public class CarbonAbsorption
    {
        [Key]
        public int CarbonAbsorptionId { get; set; }
        public int TreeId { get; set; }
        public int PeriodeId { get; set; }

        public int UserId { get; set; }
        public int Age { get; set; }

        [Required(ErrorMessage = "Jumlah batang perlu diinput.")]
        //[Range(1, 9999, ErrorMessage = "Minimal jumlah batang = 1 dan maksimal jumlah batang = 9999")]
        public int Amount { get; set; }                

        public double CarbAbs { get; set; }                
        
        public DateTime CreateDate { get; set; }

        public string TreeName { get; set; }

        public double TreeEmision()
        {
            var treeEmision= TreeId * CarbAbs;
            return treeEmision;
        }        
      
    }
}
