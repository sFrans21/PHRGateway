using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSystem.Models
{
    public partial class VwEmission_Report
    {
        [Key]
        public int PeriodeId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
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
