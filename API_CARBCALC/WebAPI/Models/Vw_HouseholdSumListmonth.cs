using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    [Table("Vw_HouseholdSumListmonth")]
    public class Vw_HouseholdSumListmonth
    {
        [Key]
        public int HouseholdId { get; set; }
        public int UserId { get; set; }
        public int PeriodeId { get; set; }
        //public int Month { get; set; }
        public decimal KonsumsiListrik { get; set; }
        public decimal KonsumsiLPG { get; set; }
        public decimal KonsumsiGasKota { get; set; }
        public decimal KonsumsiGas { get; set; }
        public decimal EmisiListrik { get; set; }
        public decimal EmisiGas { get; set; }
        public decimal EmisiperOrang { get; set; }

    }
}
