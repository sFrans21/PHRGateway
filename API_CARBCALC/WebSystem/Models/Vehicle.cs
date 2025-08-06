using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebSystem.Models
{
    public class Vehicle
    {
        [Key]
        public int VehicleEmisionId { get; set; }
        public int VehicleId { get; set; }
        public int FuelId { get; set; }
        public int TransportationId { get; set; }
        public int CapacityId { get; set; }
        public int PeriodeId { get; set; }
        public int UserId { get; set; }
        public int TravelFrequency { get; set; }
        [Column(TypeName = "date")]
        public DateTime Per { get; set; }
        public int AmountPeople { get; set; }
        public int Mileage { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
    }
}
