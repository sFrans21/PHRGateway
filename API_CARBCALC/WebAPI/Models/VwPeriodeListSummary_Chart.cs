using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebAPI.Models
{
    //[Table("Vw_PeriodeListSummary")]
    [Table("Vw_PeriodeListSummary_Chart")]
    public class VwPeriodeListSummary_Chart
    {
        [Key]
        
        public int UserId { get; set; }
        
        public string PeriodeDetail { get; set; }
        public decimal TotalEmission { get; set; }     



    }
}
