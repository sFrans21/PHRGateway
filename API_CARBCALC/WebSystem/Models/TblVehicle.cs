using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebSystem.Models
{
    public partial class TblVehicle
    {

        [Key]
        public int VehicleId { get; set; }
        public int VehicleTypeId { get; set; }
        [Required]
        [StringLength(100)]
        public string VehicleName { get; set; }
        [Column("img")]
        [StringLength(1000)]
        public string Img { get; set; }
        public IFormFile VehicleImage { get; set; }

    }
}
