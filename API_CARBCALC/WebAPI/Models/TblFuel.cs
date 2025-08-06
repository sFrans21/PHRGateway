using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI.Models
{
    [Table("Tbl_Fuel")]
    public partial class TblFuel
    {
        public TblFuel()
        {
            TblTVehicleEmision = new HashSet<TblTVehicleEmision>();
            //TblVehicleCapacity = new HashSet<TblVehicleCapacity>();
        }

        [Key]
        public int FuelId { get; set; }
        [Required]
        [StringLength(50)]
        public string FuelName { get; set; }
        public decimal Co2_KG { get; set; }

        [InverseProperty("Fuel")]
        public virtual ICollection<TblTVehicleEmision> TblTVehicleEmision { get; set; }
       // [InverseProperty("Fuel")]
        //public virtual ICollection<TblVehicleCapacity> TblVehicleCapacity { get; set; }
    }
}
