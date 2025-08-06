using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebSystem.Models;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using Microsoft.AspNetCore.Authentication;
using System.Reflection.PortableExecutable;
using WebSystem.Helper;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
//using ActiveAD_Login;


namespace WebSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        HelperAPI _api = new HelperAPI();
        //ActiveAD_Login.Controllers.UserInfo test = new ActiveAD_Login.Controllers.UserInfo();

        // Global variables
        //public static string _username; // = "";
        //public static string _email; //= "";
        //public static string _displayname; //= "";
        //public static string strCharUser;
        //public static string strDisplayName;

        //const string userId = "_UserId";
        //const string userName = "_UserName";
        //const string displayname = "_DisplayName";
        //const string email = "_Email";
        //const string company = "_Company";
        //const string regional = "_Regional";
        //const string zona = "_Zona";
        //const string field = "Field";

        //public string test = new ActiveAD_Login.Controllers.HomeController();
        //string test = new ActiveAD_Login.Controllers

        public void SetViewbag()
        {
            _api.GetValueSession(HttpContext);
            ViewBag.UserId = _api.UserID;
            ViewBag.UserName = _api.UserName;
            ViewBag.Displayname = _api.Name;
            ViewBag.asd = _api.Company;
            ViewBag.IsAdmin = _api.IsAdmin;
            ViewBag.periodId = _api.PeriodeID;

            //ViewBag.UserId = Convert.ToInt32(HttpContext.Session.GetString(userId));
            //ViewBag.UserName = HttpContext.Session.GetString(userName);
            //ViewBag.Displayname = HttpContext.Session.GetString(displayname);
            //ViewBag.asd = HttpContext.Session.GetString(displayname);

        }
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Index()
        {
            //// set session variables


            // //var username = "pertamina\\Mas Jabier"; //Get Windows AD username
            // var username = User.Identity.Name; //Get Windows AD username

            // TblTUser userInfo = new TblTUser();
            // //#endif
            // if ((username != "") && (username != null))
            // {
            //     ViewBag.UserName = HttpContext.Session.GetString(userName);//username.Split('\\')[1];

            //     #region Konek AD
            //     //ContextType authenticationType = ContextType.Domain;
            //     //PrincipalContext principalContext = new PrincipalContext(authenticationType, "pertamina.com");
            //     //UserPrincipal userPrincipal = null;
            //     //userPrincipal = UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, ViewBag.UserName);

            //     //ViewBag.Displayname = userPrincipal.DisplayName; //username.Split('\\')[1].ToUpper();
            //     //ViewBag.Email = userPrincipal.EmailAddress; //"carbon@pertamina.com"; //hardcode first

            //     //userInfo.UserName = username.Split('\\')[1];
            //     //userInfo.Name = userPrincipal.DisplayName; //username.Split('\\')[1].ToUpper();
            //     //userInfo.Email = userPrincipal.EmailAddress; //"carbon@pertamina.com"; //hardcode first
            //     #endregion

            //     #region Non AD
            //     //ViewBag.Email = "carbon@pertamina.com";
            //     //ViewBag.Displayname = ViewBag.UserName;
            //     userInfo.UserName = ViewBag.UserName;
            //     userInfo.Name = HttpContext.Session.GetString(displayname);
            //     userInfo.Email = HttpContext.Session.GetString(email);
            //     userInfo.Company = HttpContext.Session.GetString(company);
            //     userInfo.Regional = HttpContext.Session.GetString(regional);
            //     userInfo.Zona = HttpContext.Session.GetString(zona);
            //     userInfo.Field = HttpContext.Session.GetString(field);
            //     #endregion

            //     HttpClient client = _api.Initial();

            //     //HttpResponseMessage res = client.GetAsync("api/home/UserCheck/" + ViewBag.UserName + "/" + ViewBag.Displayname + "/PHR/" + ViewBag.Email).Result;
            //     HttpResponseMessage res = client.PostAsJsonAsync("api/home/UserCheckV2/", userInfo).Result; // HelperStatic.UserID + "/" + HelperStatic.PeriodeID);

            //     //List<VwPeriode> vmtest = res.Content.ReadAsAsync<List<VwPeriode>>().Result;

            //     List<TblTUser> vmtest = res.Content.ReadAsAsync<List<TblTUser>>().Result;
            //     //Set Variabel
            //     //HelperStatic.UserID = Convert.ToInt32(vmtest[0].UserId);                
            //     //HelperStatic.UserName = ViewBag.UserName;
            //     //HelperStatic.Name = ViewBag.Displayname; 

            //     //If user exists               
            SetViewbag();


            if (ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }

            else
            {
                return View();
            }

            //     //Set session variable  
            //     HttpContext.Session.SetString(userId, vmtest[0].userID.ToString());
            //     HttpContext.Session.SetString(userName, (string)ViewBag.UserName);
            //     HttpContext.Session.SetString(displayname, (string)ViewBag.Displayname);

            //     ViewBag.UserName = username;
            // }


           
            
        }

        //[HttpGet]
        //public async Task<JsonResult> GetUser()
        //{
        //    //List<VwPeriode> userInfo = new List<VwPeriode>();
        //    ////TblTUser userInfo = new TblTUser();
        //    //HttpClient client = _api.Initial();

        //    ////Get ADLogon
        //    ////Get I-Man User info 
        //    ////

        //    //HttpResponseMessage res = await client.GetAsync("api/home/UserCheck/" + _username + _displayname + "PHR" + _email); // HelperStatic.UserID + "/" + HelperStatic.PeriodeID);
        //    ////HttpResponseMessage res = await client.PostAsync("api/home/UserCheckV2/", userInfo); // HelperStatic.UserID + "/" + HelperStatic.PeriodeID);

        //    //if (res.IsSuccessStatusCode)
        //    //{
        //    //    var result = res.Content.ReadAsStringAsync().Result;
        //    //    userInfo = JsonConvert.DeserializeObject<List<VwPeriode>>(result);
        //    //}
        //    //return Json(userInfo);
        //}


        public IActionResult BackgroundInfo()
        {
            //set session variables
            //const string userId = "_UserId";
            //const string userName = "_UserName";
            //const string displayname = "_DisplayName";

            ////var username = "pertamina\\Mas Jabier"; //Get Windows AD username
            //var username = User.Identity.Name; //Get Windows AD username

            ////#endif
            //if ((username != "") && (username != null))
            //{
            //    ViewBag.UserName = username.Split('\\')[1];

            //    #region Konek AD
            //    //ContextType authenticationType = ContextType.Domain;
            //    //PrincipalContext principalContext = new PrincipalContext(authenticationType, "pertamina.com");
            //    //UserPrincipal userPrincipal = null;
            //    //userPrincipal = UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, ViewBag.UserName);

            //    //ViewBag.Displayname = userPrincipal.DisplayName; //username.Split('\\')[1].ToUpper();
            //    //ViewBag.Email = userPrincipal.EmailAddress; //"carbon@pertamina.com"; //hardcode first
            //    #endregion

            //    #region Non AD
            //    //ViewBag.Email = "carbon@pertamina.com";
            //    //ViewBag.Displayname = ViewBag.UserName;
            //    #endregion

            //    HttpContext.Session.SetString(userName, (string)ViewBag.UserName);
            //    HttpContext.Session.SetString(displayname, (string)ViewBag.Displayname);

            //    ViewBag.UserName = username;
            //}
            SetViewbag();


            if (ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }

            else
            {
                return View();
            }
        }

        public IActionResult Help()
        {
            ////set session variables
            ////const string userId = "_UserId";
            //const string userName = "_UserName";
            //const string displayname = "_DisplayName";

            ////var username = "pertamina\\Mas Jabier"; //Get Windows AD username
            //var username = User.Identity.Name; //Get Windows AD username

            ////#endif
            //if ((username != "") && (username != null))
            //{
            //    ViewBag.UserName = username.Split('\\')[1];

            //    #region Konek AD
            //    ContextType authenticationType = ContextType.Domain;
            //    PrincipalContext principalContext = new PrincipalContext(authenticationType, "pertamina.com");
            //    UserPrincipal userPrincipal = null;
            //    userPrincipal = UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, ViewBag.UserName);

            //    ViewBag.Displayname = userPrincipal.DisplayName; //username.Split('\\')[1].ToUpper();
            //    ViewBag.Email = userPrincipal.EmailAddress; //"carbon@pertamina.com"; //hardcode first
            //    #endregion

            //    #region Non AD
            //    //ViewBag.Email = "carbon@pertamina.com";
            //    //ViewBag.Displayname = ViewBag.UserName;
            //    #endregion

            //    HttpContext.Session.SetString(userName, (string)ViewBag.UserName);
            //    HttpContext.Session.SetString(displayname, (string)ViewBag.Displayname);

            //    ViewBag.UserName = username;
            //}
            
            SetViewbag();


            if (ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }

            else
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
