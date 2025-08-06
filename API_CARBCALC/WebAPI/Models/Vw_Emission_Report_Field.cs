using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    [Table("Vw_Emission_Report_Test")]
    public partial class Vw_Emission_Report_Field
    {
        [Key]
        public int PeriodeId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Regional { get; set; }
        public string Zona { get; set; }
        public string Field { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal ElectricityEmission { get; set; }
        public decimal GasEmission { get; set; }
        public decimal PeopleEmission { get; set; }
        public decimal VehicleEmission { get; set; }
        public decimal CarbonAbsorption { get; set; }
        public decimal TotalEmission { get; set; }
        public string MonthDetail { get; set; }
    }

}
