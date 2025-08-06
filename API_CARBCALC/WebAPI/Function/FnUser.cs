using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Function
{
    public class FnUser
    {
        public int UserID(TblTUser userInfo)
        {
            var userID = userInfo.userID;           
            return userID;
        }

        
        public string UserName (TblTUser userInfo)
        {
            var userName = userInfo.UserName;
            return userName;
        }

        public string Name (TblTUser userInfo)
        {
            var name = userInfo.Name;
            return name;
        }
        
        public string Company (TblTUser userInfo)
        {
            var company = userInfo.Company;
            return company;
        }

        public string Regional (TblTUser userInfo)
        {
            var regional = userInfo.Regional;
            return regional;
        }

        public string Zona (TblTUser userInfo)
        {
            var zona = userInfo.Zona;
            return zona;
        }

        public string Field (TblTUser userInfo)
        {
            var field = userInfo.Field;
            return field;
        }

        public int IsAdmin(TblTUser userInfo)
        {
            var isadmin = userInfo.IsAdmin;
            return (int)isadmin;
        }
       
    }
}
