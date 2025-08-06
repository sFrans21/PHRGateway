using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebSystem.Models
{
    public class CarbonAbsorptionViewModel
    {
        [Key]
        public int CarbonAbsorptionId { get; set; }
        public int TreeId { get; set; }

        [DisplayFormat(DataFormatString = "{0:Y}")]
        public DateTime? Periode { get; set; }
        public int UserId { get; set; }
        public int Age { get; set; }
        public int Amount { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDateString { get; set; }

        //additional field
        public string TreeName { get; set; }
        public string Img { get; set; }
        public float CarbonAbs { get; set; }
        public int PeriodeId { get; set; }

        public float TreeEmision2 { get; set; }

        public float TreeEmision()
        {
            var treeEmision= Amount * CarbonAbs;
            return treeEmision;
        }

        public string PeriodeDetail { get; set; }

        public string encLink_Edit { get; set; }
        public string encLink_Details { get; set; }
        public string encLink_Delete { get; set; }

    }
}
