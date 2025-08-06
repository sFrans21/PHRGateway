using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Function
{
    public class FnTree
    {
        public int TreeId(TblTree treeinfo)
        {
            var treeId = treeinfo.TreeId;           
            return treeId;
        }

        
        public string TreeName (TblTree treeinfo)
        {
            var treeName = treeinfo.TreeName;
            return treeName;
        }

        public double CarbonAbs (TblTree treeinfo)
        {
            var carbonAbs = treeinfo.CarbonAbs;
            return carbonAbs;
        }        
       
    }
}
