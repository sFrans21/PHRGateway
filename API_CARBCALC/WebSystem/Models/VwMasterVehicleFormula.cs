using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebSystem.Models
{
    public partial class VwMasterVehicleFormula
    {

        [Key]
        public int TransportGroupFormulaId { get; set; }
        public int TransportGroupid { get; set; }
        public int CapacityId { get; set; }
        public int TransportCapacity { get; set; }
        public decimal TransportKMperLiter { get; set; }
        public int VehicleId { get; set; }
        public string VehicleName { get; set; }
        public string TransportationName { get; set; }
        public string FuelName { get; set; }
        public int FuelId { get; set; }
        public string Action { get; set; }


        // [InverseProperty("Fuel")]
        //public virtual ICollection<TblVehicleCapacity> TblVehicleCapacity { get; set; }
    }
}
