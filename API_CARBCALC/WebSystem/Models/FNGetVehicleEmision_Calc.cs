using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebSystem.Models
{
    public class FNGetVehicleEmision_Calc
    {
        [Key]
        public int VehicleId { get; set; }
        public int FuelId { get; set; }

        public int TransportationId { get; set; }
        public int CapacityId { get; set; }
        public string VehicleName { get; set; }
        public string FuelName { get; set; }
        public string TransportationName { get; set; }
        public int CubicalCentimeter { get; set; }
       
        public int TravelFrequency { get; set; }
        public int AmountPeople { get; set; }
        public int TransportCapacity { get; set; }

        public decimal Mileage { get; set; }
        public decimal KmPerLiter { get; set; }
        public decimal BBMBulanLiter { get; set; }
        public decimal BBMOrangTahunLiter { get; set; }
        public decimal EmisionBulan { get; set; }
    }
}
