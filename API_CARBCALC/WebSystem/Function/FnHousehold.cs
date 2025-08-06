using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSystem.Models;

namespace WebSystem.Function
{
    public class FnHousehold
    {
        public decimal KonsumsiListrik(Household household)
        {
            var standmeter = household.Standmeter;
            var konsumsiListrik = standmeter + 1;
            return konsumsiListrik;
        }

        public decimal KonsumsiGas(Household household)
        {
            var cityGas = household.CityGasConsumption;
            var konsumsiGas = cityGas + 2;
            return konsumsiGas;
        }

        public decimal EmisiCo2Listrik(Household household)
        {
            var amountPeople = household.AmountPeople;
            var emisiCo2Listrik = amountPeople + 3;
            return emisiCo2Listrik;
        }

        public decimal EmisiCo2Gas(Household household)
        {
            var lpgCons = household.LpgConsumption;
            var emisiCo2Gas = lpgCons + 4;
            return emisiCo2Gas;
        }

        public decimal EmisiCo2Person(Household household)
        {
            var amountPeople = household.AmountPeople;
            var emisiCo2Person = amountPeople + 5;
            return emisiCo2Person;
        }
    }
}
