using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebSystem.Helper;
using WebSystem.CustomTagHelper;
using Microsoft.CodeAnalysis.Text;
using Microsoft.AspNetCore.Routing;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebSystem.Models
{
    public partial class TblTUser
    {
        [Key]

        public int userID { get; set; } = 0;
        public string Name { get; set; } = "";
        public string Company { get; set; } = "";
        public string UserName { get; set; } = "";

        public int Period { get; set; } = 0;
        public string Email { get; set; } = "";

        public decimal? HouseCalculation { get; set; } = 0;
        public decimal? VehicleCalculation { get; set; } = 0;
        public decimal? CarbonAbsCalculation { get; set; } = 0;
        public decimal? TotalEmision { get; set; } = 0;
        public string? Regional { get; set; } = "";
        public string? Zona { get; set; } = "";
        public string? Field { get; set; } = "";
        public int? IsAdmin { get; set; } = 0;
        public int? IsFirstLogin { get; set; } = 0;
        public string Password { get; set; } = "";

        //public string encLink { get; set; } = "<a s-text=\"User 1\" s-actionName=\"Master_User_Edit\" s-controllerName=\"Admin\" s-rK1=\"testID\" s-rV1=\"1\" s-rK2=\"userID\" s-rV2=\"1\"></a>";
        //public string encLink { get; set; } = "<a s-text=\"User 1\" s-actionName=\"Master_User_Edit\" s-controllerName=\"Admin\" s-rK1=\"testID\" s-rV1=\"1\" href=\"" + "http://www.google.com\"" + ">Test</a>";

        //public string testID { get; set; } = 

        public string encLink { get; set; }
        public string encLink_Edit { get; set; }
        //= CustomQueryStringHelper.EncryptString("", "Master_User_Edit", "Admin", );

        public string EncStr()
        {
            SecureLinkTagHelper testlink = new SecureLinkTagHelper();
            testlink.text = "Edit";
            testlink.actionName = "Master_User_Edit";
            testlink.controllerName = "Admin";
            testlink.routekey1 = "userID";
            testlink.routeValues1 = Convert.ToString(userID);
            testlink.routekey2 = "Email";
            testlink.routeValues2 = "jabier@pertamina.com";
            string enc = CustomQueryStringHelper.EncryptString("", "Master_User_Edit", "Admin", testlink);
            return enc;


        }

    }
}
