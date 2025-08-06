using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Function
{
    public class FnCarbonAbsoprtion
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

        //public int Periode (TblTCarbonAbsorptionViewModel carbonAbsorption)
        //{
        //    var periode = carbonAbsorption.Periode;
        //    periode = 3;
        //    return periode;
        //}
       
    }
}
