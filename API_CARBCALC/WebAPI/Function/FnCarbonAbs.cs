using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Function
{
    public class FnCarbonAbs
    {
        public int TreeId(TblTCarbonAbsorption carbonAbsorption)
        {
            var treeId = carbonAbsorption.TreeId;
            treeId = treeId + 0;
            return treeId;
        }


        //Put formula here
        public int Amount (TblTCarbonAbsorption carbonAbsorption)
        {
            var amount = carbonAbsorption.Amount;
            return amount;
        }

        public int Age (TblTCarbonAbsorption carbonAbsorption)
        {
            var age = carbonAbsorption.Age;
            return age;
        }

        public int Periode (TblTCarbonAbsorption carbonAbsorption)
        {
            var periode = carbonAbsorption.PeriodeId;
            return periode;
        }

        public decimal KonsumsiListrik(TblTHousehold household)
        {
            var standmeter = household.Standmeter;
            var konsumsiListrik = standmeter + 1;
            return konsumsiListrik;
        }

        public decimal KonsumsiGas(TblTHousehold household)
        {
            var cityGas = household.CityGasConsumption;
            var konsumsiGas = cityGas + 2;
            return konsumsiGas;
        }

        public decimal EmisiCo2Listrik(TblTHousehold household)
        {
            var amountPeople = household.AmountPeople;
            var emisiCo2Listrik = amountPeople + 3;
            return emisiCo2Listrik;
        }

        public decimal EmisiCo2Gas(TblTHousehold household)
        {
            var fegaskota = 56100; //Faktor Emisi (FE) Gas kota 
            var kalor = (decimal)0.0000374; //Nilai Kalor
            var lpgCons = household.LpgConsumption;
            var emisiCo2Gas = lpgCons * fegaskota * kalor;
            return (decimal)emisiCo2Gas;
        }

        //emisi Gas per orang
        public decimal EmisiCo2Person(TblTHousehold household)
        {
            var fegaskota = 56100; //Faktor Emisi (FE) Gas kota 
            var kalor = (decimal)0.0000374; //Nilai Kalor
            var amountPeople = household.AmountPeople;
            var lpgCons = household.LpgConsumption;
            var emisiCo2Person = lpgCons/amountPeople * fegaskota * kalor;
            return (decimal)emisiCo2Person;
        }
       
    }
}
