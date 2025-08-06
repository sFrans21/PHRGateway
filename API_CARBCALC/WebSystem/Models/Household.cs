using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebSystem.Models
{
    public class Household
    {
        [Key]
        public int HouseholdId { get; set; }
        [Required(ErrorMessage = "Jumlah orang perlu diinput.")]
        [Range(1, 99, ErrorMessage = "Minimal jumlah orang dalam rumah = 1 dan maksimal 99")]
        public int AmountPeople { get; set; }        
        [Required(ErrorMessage = "Konsumsi listrik perlu diinput.")]
        [Range(0, 999, ErrorMessage = "Maksimal konsumsi listrik bulan ini adalah < 999(KWH)")]
        public decimal Standmeter { get; set; }        
        [Required(ErrorMessage = "Konsumsi LPG perlu diinput.")]
        [Range(0, 9999, ErrorMessage = "Maksimal konsumsi listrik bulan ini adalah < 9999(KG)")]
        public decimal LpgConsumption { get; set; }        
        [Required(ErrorMessage = "Konsumsi gas kota perlu diinput.")]
        [Range(0, 9999, ErrorMessage = "Maksimal konsumsi listrik bulan ini adalah < 9999(M3)")]
        public decimal CityGasConsumption { get; set; }       
        public DateTime CreatedDate { get; set; }
        public string CreatedDateString { get; set; }
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

        public decimal KonsumsiListrik()
        {
            var konsumsiListrik = Standmeter / AmountPeople;
            return konsumsiListrik;
        }

        public decimal KonsumsiLPG()
        {
            var konsumsipg = LpgConsumption / AmountPeople;
            return konsumsipg;
        }

        public decimal KonsumsiGasKota()
        {
            var konsumsiGas = CityGasConsumption / AmountPeople;
            return konsumsiGas;
        }

        public decimal EmisiCo2Listrik()
        {
            var emisi = (decimal)0.000792;
            var emisiCo2Listrik = Standmeter / AmountPeople * emisi;
            return emisiCo2Listrik;
        }

        public decimal EmisiCo2LPG()
        {
            //var fegaskota = 63100; //Faktor Emisi (FE) Gas kota 
            //var kalor = (decimal)0.00052; //Nilai Kalor 63100 * 0.00052 = 3.28
            var constanta = (decimal)3.28;
            var emisiCo2GLpg = LpgConsumption / AmountPeople * constanta;
            return emisiCo2GLpg;
        }

        public decimal EmisiCo2Kota()
        {
            //var fegaskota = 56100; //Faktor Emisi (FE) Gas kota 
            //var kalor = (decimal)0.0000374; //Nilai Kalor 56100 * 0.0000374 = 2.1
            var constanta = (decimal)2.1;
            var emisiCo2City = CityGasConsumption / AmountPeople * constanta;
            return emisiCo2City;
        }

        public decimal EmisiCo2Gas()
        {
            var emisiCo2Gas = EmisiCo2LPG() + EmisiCo2Kota();
            return emisiCo2Gas;
        }

        public decimal EmisiCo2Person()
        {
            //var fegaskota = 56100; //Faktor Emisi (FE) Gas kota 
            //var kalor = (decimal)0.0000374; //Nilai Kalor
            var emisiCo2Person = EmisiCo2Gas() + EmisiCo2Listrik();
            return emisiCo2Person;
        }

        public string PeriodeDetail { get; set; }

        public decimal MonthElectrikCons { get; set; }
        public decimal MonthLPGCons { get; set; }
        public decimal MonthCityGasCons { get; set; }
        public decimal MonthElectrikEmision { get; set; }
        public decimal MonthGasEmision { get; set; }
        public decimal MonthPeopleEmision { get; set; }
        public decimal YearElectrikCons { get; set; }
        public decimal YearLPGCons { get; set; }
        public decimal YearCityGasCons { get; set; }
        public decimal YearElectric { get; set; }
        public decimal YearGas { get; set; }
        public decimal YearPerson { get; set; }
    }
}
