using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSystem.Models
{
    public class Summary
    {
        public decimal ElectricCons { get; set; }
        public decimal ElectricUsage { get; set; }
        public decimal GasCons { get; set; }
        public decimal GasUsage { get; set; }
        public int TreePlanted { get; set; }
        public decimal TotalElectricEmision { get; set; }
        public decimal TotalGasEmision { get; set; }
        public decimal TotalCoEmision { get; set; }
        public decimal TotalCarbonAbs { get; set; }
        public decimal FinalCarbonEmision { get; set; }


        public decimal Electric()
        {
            ElectricCons = 999;
            var electric = ElectricCons;
            return electric;
        }

        public decimal ElectricUs()
        {
            ElectricUsage = 91;
            var electricus = ElectricUsage;
            return electricus;
        }

        public decimal Gas()
        {
            GasUsage = 92;
            var gas = GasUsage;
            return gas;
        }

        public decimal GasUs()
        {
            GasUsage = 93;
            var gasus = GasUsage;
            return gasus;
        }

        public decimal Plant()
        {
            TreePlanted = 94;
            var plant = TreePlanted;
            return plant;
        }

        public decimal TotalElectric()
        {
            TotalElectricEmision = 100;
            var totaleletric = TotalElectricEmision;
            return totaleletric;
        }

        public decimal TotalGas()
        {
            TotalGasEmision = 101;
            var totalgas = TotalGasEmision;
            return totalgas;
        }

        public decimal TotalCo()
        {
            TotalCoEmision = 111;
            var totalco = TotalCoEmision;
            return totalco;
        }

        public decimal TotalAbs()
        {
            TotalCarbonAbs = 103;
            var totalabs = TotalCarbonAbs;
            return totalabs;
        }

        public decimal FinalCarbon()
        {
            FinalCarbonEmision = 8;
            var finalcarbon = FinalCarbonEmision;
            return finalcarbon;
        }
    }
}
