using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebSystem.Models
{
    public class Periode
    {
        [Key]
        public int PeriodeId { get; set; }
        public int? UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Month { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime Year { get; set; }
        public string PeriodeDetail { get; set; }

        //[InverseProperty("Periode")]
        //public virtual ICollection<TblTCarbonAbsorption> TblTCarbonAbsorption { get; set; }
        //[InverseProperty("Periode")]
        //public virtual ICollection<TblTHousehold> TblTHousehold { get; set; }
        //[InverseProperty("Periode")]
        //public virtual ICollection<TblTVehicleEmision> TblTVehicleEmision { get; set; }
    }
}
