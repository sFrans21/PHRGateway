using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project,  then un-comment the following line:
// #nullable disable 

namespace WebAPI.Models
{
    //[Table("TblT_CarbonAbsorption")]
    public partial class TblTCarbonAbsorptionViewModel_Sum
    {
        //public TblTCarbonAbsorptionViewModel()
        //{
        //    TblAbsorptionCalc = new HashSet<TblAbsorptionCalc>();
        //}

        [Key]
        public int CarbonAbsorptionId { get; set; }
        public int TreeId { get; set; }


        //[DisplayFormat(DataFormatString = "{0:d}")]
        //public DateTime? Periode { get; set; }
        //public int Periode { get; set; }
        public int? UserId { get; set; }
        public int Age { get; set; }
        public int Amount { get; set; }
        //[Column(TypeName = "datetime")]
        //public DateTime? CreateDate { get; set; }

        //additional field
        public string TreeName { get; set; }
        public string img { get; set; }

        //[Column(TypeName = "double(18, 2)")]
        //[DisplayFormat(DataFormatString = "{0:n2}", ApplyFormatInEditMode = true)]
        public double CarbonAbs { get; set; }
        public int PeriodeId { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDateString { get; set; }

        //[ForeignKey(nameof(PeriodeId))]
        //[InverseProperty(nameof(TblPeriode.TblTCarbonAbsorption))]
        ////public virtual TblPeriode Periode { get; set; }
        //[ForeignKey(nameof(TreeId))]
        //[InverseProperty(nameof(TblTree.TblTCarbonAbsorption))]
        //public virtual TblTree Tree { get; set; }
        //[InverseProperty("CarbonAbsorption")]
        //public virtual ICollection<TblAbsorptionCalc> TblAbsorptionCalc { get; set; }
    }
}
