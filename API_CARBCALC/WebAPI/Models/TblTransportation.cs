using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI.Models
{
    [Table("Tbl_Transportation")]
    public partial class TblTransportation
    {
        public TblTransportation()
        {
            TblTVehicleEmision = new HashSet<TblTVehicleEmision>();
            //TblVehicleCapacity = new HashSet<TblVehicleCapacity>();
        }

        [Key]
        public int TransportationId { get; set; }
        [Required]
        [StringLength(50)]
        public string TransportationName { get; set; }

        [InverseProperty("Transportation")]
        public virtual ICollection<TblTVehicleEmision> TblTVehicleEmision { get; set; }
        //[InverseProperty("Transportation")]
        //public virtual ICollection<TblVehicleCapacity> TblVehicleCapacity { get; set; }
    }
}
