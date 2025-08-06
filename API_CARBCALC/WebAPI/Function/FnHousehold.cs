using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Function
{
    public class FnHousehold
    {

        public decimal KonsumsiListrik(TblTHousehold household)
        {
            var standmeter = household.Standmeter;
            var amountpeople = household.AmountPeople;
            var konsumsiListrik = standmeter / amountpeople;
            return konsumsiListrik;
        }

        public decimal KonsumsiGas(TblTHousehold household)
        {
            var cityGas = household.CityGasConsumption;
            var lpgGas = household.LpgConsumption;
            var amountpeople = household.AmountPeople;
            var konsumsiGas = (cityGas + lpgGas) / amountpeople;
            return konsumsiGas;
        }

        //emisi co2 listrik / orang
        public decimal EmisiCo2Listrik(TblTHousehold household)
        {
            var emisi = (decimal)0.000792;
            var amountPeople = household.AmountPeople;
            var standmeter = household.Standmeter;
            var emisiCo2Listrik = standmeter / amountPeople * emisi;
            return emisiCo2Listrik;
        }

        //LPG / orang
        public decimal EmisiCo2LPG(TblTHousehold household)
        {
            //var fegaskota = 63100; //Faktor Emisi (FE) Gas kota 
            //var kalor = (decimal)0.00052; //Nilai Kalor 63100 * 0.00052 = 3.28
            var constanta = (decimal)3.28;
            var amountPeople = household.AmountPeople;
            var lpgCons = household.LpgConsumption;
            var emisiCo2Lpg = lpgCons / amountPeople * constanta;
            return emisiCo2Lpg;
        }

        //Gas kota / orang
        public decimal EmisiCo2Kota(TblTHousehold household)
        {
            //var fegaskota = 56100; //Faktor Emisi (FE) Gas kota 
            //var kalor = (decimal)0.0000374; //Nilai Kalor 56100 * 0.0000374 = 2.1
            var constanta = (decimal)2.1;
            var amountPeople = household.AmountPeople;
            var cityGas = household.CityGasConsumption;
            var emisiCo2City = cityGas / amountPeople * constanta;
            return emisiCo2City;
        }

        //emisi co2 dari gas / orang
        public decimal EmisiCo2Gas(TblTHousehold household)
        {
            var emisiCo2Gas = EmisiCo2LPG(household) + EmisiCo2Kota(household);
            return emisiCo2Gas;
        }

        //emisi co2 per orang
        public decimal EmisiCo2Person(TblTHousehold household)
        {
            var emisiCo2Person = EmisiCo2Gas(household) + EmisiCo2Listrik(household);
            return emisiCo2Person;
        }
    }
}
