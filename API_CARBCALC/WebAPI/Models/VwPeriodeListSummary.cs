using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    //[Table("Vw_PeriodeListSummary")]
    [Table("Vw_PeriodeListSummary")]
    public class VwPeriodeListSummary
    {
        [Key]
        public int PeriodeId { get; set; }
        //public int UserId { get; set; }
        //public string PeriodeDetail { get; set; }
        //public string Name { get; set; }
        //public string Company { get; set; }
        //public int ViewID { get { return Month + Year; }}
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
        public string PeriodeDetail { get; set; }

    }
}
