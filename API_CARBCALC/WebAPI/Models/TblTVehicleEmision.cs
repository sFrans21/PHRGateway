using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI.Models
{
    [Table("TblT_VehicleEmision")]
    public partial class TblTVehicleEmision
    {
        [Key]
        public int VehicleEmisionId { get; set; }
        public int? VehicleId { get; set; }
        public int? FuelId { get; set; }
        public int? TransportationId { get; set; }
        public int? CapacityId { get; set; }
        public int? PeriodeId { get; set; }
        public int? UserId { get; set; }
        public int TravelFrequency { get; set; }
        [Column(TypeName = "date")]
        public DateTime Per { get; set; }
        public int AmountPeople { get; set; }
        public int Mileage { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        //[ForeignKey(nameof(CapacityId))]
        //[InverseProperty(nameof(TblVehicleCapacity.TblTVehicleEmision))]
        public virtual TblVehicleCapacity Capacity { get; set; }
        [ForeignKey(nameof(FuelId))]
        [InverseProperty(nameof(TblFuel.TblTVehicleEmision))]
        public virtual TblFuel Fuel { get; set; }
        [ForeignKey(nameof(PeriodeId))]
        [InverseProperty(nameof(TblPeriode.TblTVehicleEmision))]
        public virtual TblPeriode Periode { get; set; }
        [ForeignKey(nameof(TransportationId))]
        [InverseProperty(nameof(TblTransportation.TblTVehicleEmision))]
        public virtual TblTransportation Transportation { get; set; }
        
    }
}
