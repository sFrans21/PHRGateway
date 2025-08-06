using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI.Models
{
    [Table("Vw_Fuel")]
    public partial class VwFuel
    {

        [Key]
        public int FuelId { get; set; }
        public string FuelName { get; set; }
        [Required]
        [StringLength(50)]
        public string VehicleID { get; set; }

       // [InverseProperty("Fuel")]
        //public virtual ICollection<TblVehicleCapacity> TblVehicleCapacity { get; set; }
    }
}
