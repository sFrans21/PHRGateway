using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebSystem.Models
{
    public partial class TblVehicleCapacity
    {

        [Key]
        public int CapacityId { get; set; }
        //public int? VehicleId { get; set; }
        //public int? FuelId { get; set; }
        //public int? TransportationId { get; set; }
        public int CubicalCentimeter { get; set; }
        //[StringLength(500)]
        //public string Formula { get; set; }

        //[ForeignKey(nameof(FuelId))]
        //[InverseProperty(nameof(TblFuel.TblVehicleCapacity))]
        //public virtual TblFuel Fuel { get; set; }
        //[ForeignKey(nameof(TransportationId))]
        //[InverseProperty(nameof(TblTransportation.TblVehicleCapacity))]
        //public virtual TblTransportation Transportation { get; set; }
        //[ForeignKey(nameof(VehicleId))]
        //[InverseProperty(nameof(TblVehicle.TblVehicleCapacity))]
        //public virtual TblVehicle Vehicle { get; set; }
        //[InverseProperty("Capacity")]
    }
}
