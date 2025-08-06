using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSystem.Models
{
    public partial class VwVehicleList_UserSum
    {
        [Key]
        public string VehicleEmisionId { get; set; }
        public int userID { get; set; }
        public int VehicleId{ get; set; }
        public int FuelId { get; set; }

        public string TransportationId { get; set; }
        public int CapacityId { get; set; }
       
        public string Name { get; set; }
        public string Company { get; set; }
        public string VehicleName { get; set; }
        public string FuelName { get; set; }
        public string TransportationName { get; set; }
        public int CubicalCentimeter { get; set; }
        public int? PeriodeId { get; set; }
        public int TravelFrequency { get; set; }
        public string AmountPeople { get; set; }
        
        public decimal Mileage { get; set; }
        public decimal KmPerLiter { get; set; }
        public decimal BBMBulanLiter { get; set; }
        public decimal BBMOrangTahunLiter { get; set; }
        public decimal EmisionBulan { get; set; }
        public string CreatedDate { get; set; }


    }
}
