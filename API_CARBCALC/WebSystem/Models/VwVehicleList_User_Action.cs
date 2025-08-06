using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace WebSystem.Models
{
    public class VwVehicleList_User_Action
    {
        [Key]
        public int VehicleEmisionId { get; set; }
        public int userID { get; set; }
        public int VehicleId { get; set; }
        public int FuelId { get; set; }

        public int TransportationId { get; set; }
        public int CapacityId { get; set; }

        public string Name { get; set; }
        public string Company { get; set; }
        public string VehicleName { get; set; }
        public string FuelName { get; set; }
        public string TransportationName { get; set; }
        public int CubicalCentimeter { get; set; }
        public int? PeriodeId { get; set; }

        [Required(ErrorMessage = "Frekuensi perjalanan perlu diinput.")]
        [Range(1, 9999999, ErrorMessage = "Minimal frekuensi perjalanan = 1 dan Maksimal = 9999999")]
        public int TravelFrequency { get; set; }

        [Required(ErrorMessage = "Jumlah penumpang perlu diinput.")]
        [Range(1, 9999999, ErrorMessage = "Minimal frekuensi perjalanan = 1 dan Maksimal = 9999999")]
        public int AmountPeople { get; set; }
        [Required(ErrorMessage = "Jarak tempuh perlu diinput.")]
        [Range(1, 9999999, ErrorMessage = "Minimal frekuensi perjalanan = 1 dan Maksimal = 9999999")]
        public decimal Mileage { get; set; }
        public decimal BBMBulanLiter { get; set; }
        public decimal BBMOrangTahunLiter { get; set; }
        public decimal EmisionBulan { get; set; }

        //[ForeignKey(nameof(PeriodeId))]
        //[InverseProperty(nameof(TblPeriode.VwVehicleList_User))]

        //public virtual TblPeriode Periode { get; set; }
        // public List<VwVehicleList_User> vwVehicleUser = new List<VwVehicleList_User>();
        public string action { get; set; }
        public string Message { get; set; } = "";

       
    }
}
