using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI.Models
{
    [Table("Tbl_VehicleCalc")]
    public partial class TblVehicleCalc
    {
        [Key]
        public int VehicleCalcId { get; set; }
        public int? VehicleEmisionId { get; set; }
        public int? MonthlyFuel { get; set; }
        public int? YearlyFuel { get; set; }
    }
}
