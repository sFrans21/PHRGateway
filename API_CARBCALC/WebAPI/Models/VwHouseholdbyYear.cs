using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    [Table("Vw_HouseholdbyYear")]
    public class VwHouseholdbyYear
    {
        [Key]
        public int userID { get; set; }
        public int Tahun { get; set; }
        public decimal KonsumsiListrik { get; set; }
        public decimal KonsumsiLPG { get; set; }
        public decimal KonsumsiGasKota { get; set; }
        public decimal KonsumsiGas { get; set; }
        public decimal EmisiListrik { get; set; }
        public decimal EmisiGas { get; set; }
        public decimal EmisiperOrang { get; set; }

        [ForeignKey(nameof(userID))]
        [InverseProperty(nameof(TblPeriode.VwHouseholdbyYear))]

        public virtual TblPeriode Periode { get; set; }

    }
}
