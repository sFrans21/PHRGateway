using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSystem.Models
{
    public partial class FnGetSummary
    {
        [Key]
        public int userID { get; set; }
        public int PeriodeId { get; set; }
        public string PeriodeDetail { get; set; }
        public decimal ListrikBulanBerjalanKWH { get; set; }
        public decimal GasBulanBerjalanM3 { get; set; }
        public decimal ListrikBulanBerjalanKWH_PerOrang { get; set; }
        public decimal GasBulanBerjalanM3_PerOrang { get; set; }

        public decimal TonCO2Listrik { get; set; }
        public decimal TonCO2BBM { get; set; }
        public decimal TONCO2Gas { get; set; }
        public decimal BBMBulan { get; set; }
        public decimal BBMTahun { get; set; }
        public decimal CarbonAbsPerMonth { get; set; }
        public decimal TreeAmount { get; set; }
        public decimal TotalEmisiTonCO2 { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }




    }
}
