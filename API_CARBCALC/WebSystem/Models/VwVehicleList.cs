using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSystem.Models
{
    public partial class VwVehicleList
    {
        [Key]
        public int VehicleId { get; set; }
        public int TransportationId { get; set; }
        public int TypeId { get; set; }
        public int CapacityId { get; set; }
        public int FuelId { get; set; }
        public string TransportationName { get; set; }
        public string VehicleType { get; set; }
        public string VehicleName { get; set; }
        public int CubicalCentimeter { get; set; }
        public string FuelName { get; set; }
        public string Formula { get; set; }
        public string img { get; set; }

    }
}
