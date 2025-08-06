using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI.Models
{
    [Table("Tbl_VehicleType")]
    public partial class TblVehicleType
    {
        public TblVehicleType()
        {
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string VehicleName { get; set; }

    }
}
