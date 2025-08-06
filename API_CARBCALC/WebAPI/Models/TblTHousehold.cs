using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebAPI.Models
{
    [Table("TblT_Household")]
    public partial class TblTHousehold
    {
        public TblTHousehold()
        {
            TblHouseholdCalc = new HashSet<TblHouseholdCalc>();
        }

        [Key]
        public int HouseholdId { get; set; }
        public int AmountPeople { get; set; }
        public decimal Standmeter { get; set; }
        public decimal LpgConsumption { get; set; }
        public decimal CityGasConsumption { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? PeriodeId { get; set; }
        public int? UserId { get; set; }
        public decimal? ElectricityCons { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? GasCons { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ElectricityEmision { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? GasEmision { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? PeopleEmision { get; set; }

        [ForeignKey(nameof(PeriodeId))]
        [InverseProperty(nameof(TblPeriode.TblTHousehold))]
        public virtual TblPeriode Periode { get; set; }
        [InverseProperty("Household")]
        public virtual ICollection<TblHouseholdCalc> TblHouseholdCalc { get; set; }

        //public decimal KonsumsiListrik()
        //{
        //    //var standmeter = household.Standmeter;
        //    var konsumsiListrik = Standmeter + 1;
        //    return konsumsiListrik;
        //}

        //public decimal KonsumsiGas()
        //{
        //    //var cityGas = household.CityGasConsumption;
        //    var konsumsiGas = CityGasConsumption + 2;
        //    return konsumsiGas;
        //}

        //public decimal EmisiCo2Listrik()
        //{
        //    //var amountPeople = household.AmountPeople;
        //    var emisiCo2Listrik = AmountPeople + 3;
        //    return emisiCo2Listrik;
        //}

        //public decimal EmisiCo2Gas()
        //{
        //    //var lpgCons = household.LpgConsumption;
        //    var emisiCo2Gas = LpgConsumption + 4;
        //    return emisiCo2Gas;
        //}

        //public decimal EmisiCo2Person()
        //{
        //    //var amountPeople = household.AmountPeople;
        //    var emisiCo2Person = AmountPeople + 5;
        //    return emisiCo2Person;
        //}
    }
}
