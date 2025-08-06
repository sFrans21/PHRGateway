using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using WebSystem.Helper;
using WebSystem.Models;

namespace WebSystem.Controllers
{
    public class UserController : Controller
    {
        HelperAPI _api = new HelperAPI();

        //const string userId = "_UserId";
        //const string userName = "_UserName";
        //const string displayname = "_DisplayName";
        //const string email = "_Email";
        //const string company = "_Company";
        //const string regional = "_Regional";
        //const string zona = "_Zona";
        //const string field = "Field";
        public void SetViewBag()
        {
            _api.GetValueSession(HttpContext);

            ViewBag.UserId = _api.UserID;
            ViewBag.UserName = _api.UserName;
            ViewBag.Displayname = _api.Name;
            ViewBag.asd = _api.Company;
            ViewBag.periodedetail = _api.PeriodeStatic;
            ViewBag.periodId = _api.PeriodeID;
            ViewBag.YearValue = _api.YearValue;
            ViewBag.MonthValue = _api.MonthValue;
            ViewBag.IsAdmin = _api.IsAdmin;

            //ViewBag.UserId = Convert.ToInt32(HttpContext.Session.GetString(userId));
            //ViewBag.UserName = HttpContext.Session.GetString(userName);
            //ViewBag.Displayname = HttpContext.Session.GetString(displayname);
            //ViewBag.periodedetail = HttpContext.Session.GetString(periodsession);
            //ViewBag.periodId = Convert.ToInt32(HttpContext.Session.GetString(periodeIdsession));
            //ViewBag.YearValue = Convert.ToInt32(HttpContext.Session.GetString(yearValuesession));
            //ViewBag.MonthValue = Convert.ToInt32(HttpContext.Session.GetString(MonthValuesession));
        }

        private HttpClient HttpClients;

        private readonly IConfiguration Configuration;
        public UserController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IActionResult Index(string username, string password)
        {
            //TblTUser userLogin = new TblTUser();
            //LoginAsync("ario.damar");
            //GetLogin("mk.jody.ananda", "Pertamax@2022");
            //LoginAsync("ario.damar");
            //var isLogin = GetLogin("mk.jody.ananda", "Pertamax@2022");
            //GetEmployee("ario.damar");
            //UserLogin();
            return View("UserLogin");
        }
        
        
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult GetIMANLogin2(string UserName, string password)
        //{
        //    TblTUser userProperties2 = new TblTUser();
        //    TblTUser userProperties_ref = new TblTUser();
        //    var APIUrl = Configuration["APIUrl"];
        //    //private string strView;
        //    HttpClient client = _api.Initial();

        //    //Check if username contains pertamina domain
        //    //split the pertamina domain from username
        //    //if (UserName.Contains("\\"))
        //    //{
        //    //    UserName = UserName.Split('\\')[1];
        //    //}
        //    // if(_ap)

        //    DataTable DT_UBDCC =  _api.UBDCC();
        //    DataRow[] DR_UBDCC = DT_UBDCC.Select("UserName = '" + UserName + "' AND Pass = '" + password + "'");
        //    if(DR_UBDCC.Count() > 0)
        //    {
        //        //var result = res.Content.ReadAsStringAsync().Result;
        //        //JObject jObject = JObject.Parse(result);
        //        //JToken jUser = jObject["authenticated"];
        //        bool userAuthenticated = true;//Convert.ToBoolean(jUser);
        //        ViewBag.IsAuthenticated = userAuthenticated;

        //        if (userAuthenticated)
        //        {
        //            //set session variable
        //            HttpContext.Session.SetString(_api.userName, UserName);


        //            userProperties2.UserName = UserName;
        //            userProperties2.Password = password;
        //            GetIMANData(userProperties2, ref userProperties2);

        //            ViewBag.Displayname = userProperties2.Name;
        //            ViewBag.asd = userProperties2.Company;
        //            HttpContext.Session.SetString(_api.userId, (string)userProperties2.userID.ToString());
        //            HttpContext.Session.SetString(_api.userName, (string)userProperties2.UserName);
        //            HttpContext.Session.SetString(_api.displayname, (string)userProperties2.Name);
        //            HttpContext.Session.SetString(_api.email, (string)userProperties2.Email);
        //            HttpContext.Session.SetString(_api.company, (string)userProperties2.Company);
        //            HttpContext.Session.SetString(_api.regional, (string)userProperties2.Regional);
        //            HttpContext.Session.SetString(_api.zona, (string)userProperties2.Zona);
        //            HttpContext.Session.SetString(_api.field, (string)userProperties2.Field);
        //            HttpContext.Session.SetString(_api.IsAdminsession, (string)userProperties2.IsAdmin.ToString());


        //            setDropDown(userProperties2);

        //            return View("UserEdit", userProperties2);
        //            //
        //            //strView = "UserEdit";
        //            //RedirectToAction("/User/UserEdit");

        //        }
        //        else
        //        {
        //            //strView = "UserLogin";
        //            return View("UserLogin"); //redirect to Login Page
        //        }
        //    }
        //    else
        //    {
        //        //strView = "UserLogin";
        //        return View("UserLogin"); //redirect to Login Page
        //    }

        //    //HttpResponseMessage res = client.GetAsync(APIUrl + "api/Account/GetToken?username=" + UserName + "&password=" + password).Result;
        //    //if (res.IsSuccessStatusCode)
        //    //{
        //    //    var result = res.Content.ReadAsStringAsync().Result;
        //    //    JObject jObject = JObject.Parse(result);
        //    //    JToken jUser = jObject["authenticated"];
        //    //    bool userAuthenticated = Convert.ToBoolean(jUser);
        //    //    ViewBag.IsAuthenticated = userAuthenticated;

        //    //    if (userAuthenticated)
        //    //    {
        //    //        //set session variable
        //    //        HttpContext.Session.SetString(_api.userName, UserName);


        //    //        userProperties2.UserName = UserName;
        //    //        userProperties2.Password = password;
        //    //        GetIMANData(userProperties2, ref userProperties2);

        //    //        ViewBag.Displayname = userProperties2.Name;
        //    //        ViewBag.asd = userProperties2.Company;
        //    //        HttpContext.Session.SetString(_api.userId, (string)userProperties2.userID.ToString());
        //    //        HttpContext.Session.SetString(_api.userName, (string)userProperties2.UserName);
        //    //        HttpContext.Session.SetString(_api.displayname, (string)userProperties2.Name);
        //    //        HttpContext.Session.SetString(_api.email, (string)userProperties2.Email);
        //    //        HttpContext.Session.SetString(_api.company, (string)userProperties2.Company);
        //    //        HttpContext.Session.SetString(_api.regional, (string)userProperties2.Regional);
        //    //        HttpContext.Session.SetString(_api.zona, (string)userProperties2.Zona);
        //    //        HttpContext.Session.SetString(_api.field, (string)userProperties2.Field);
        //    //        HttpContext.Session.SetString(_api.IsAdminsession, (string)userProperties2.IsAdmin.ToString());


        //    //        setDropDown(userProperties2);

        //    //        return View("UserEdit", userProperties2);
        //    //        //
        //    //        //strView = "UserEdit";
        //    //        //RedirectToAction("/User/UserEdit");

        //    //    }
        //    //    else
        //    //    {
        //    //        //strView = "UserLogin";
        //    //        return View("UserLogin"); //redirect to Login Page
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    return View(viewName: "UserLogin");
        //    //}

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetIMANLogin(string UserName, string password)
        {
            TblTUser userProperties2 = new TblTUser();
            TblTUser userProperties_ref = new TblTUser();
            var APIUrl = Configuration["APIUrl"];
            //private string strView;
            HttpClient client = _api.Initial();

            //Check if username contains pertamina domain
            //split the pertamina domain from username
            if (UserName.Contains("\\"))
            {
                UserName = UserName.Split('\\')[1];
            }

            if (UserName.Contains("@"))
            {
                UserName = UserName.Split('@')[0];
            }

            //HttpResponseMessage res = client.GetAsync(APIUrl + "api/Account/GetToken?username=" + UserName + "&password=" + password).Result;
            HttpResponseMessage res = client.GetAsync(APIUrl + "api/Account/GetToken?username=" + UserName + "&password=" + HttpUtility.UrlEncode(password)).Result;
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                JObject jObject = JObject.Parse(result);
                JToken jUser = jObject["authenticated"];
                bool userAuthenticated = Convert.ToBoolean(jUser);
                ViewBag.IsAuthenticated = userAuthenticated;

                if (userAuthenticated)
                {
                    //set session variable
                    HttpContext.Session.SetString(_api.userName, UserName);


                    userProperties2.UserName = UserName;
                    userProperties2.Password = password;
                    GetIMANData(userProperties2, ref userProperties2);

                    ViewBag.Displayname = userProperties2.Name;
                    ViewBag.asd = userProperties2.Company;
                    HttpContext.Session.SetString(_api.userId, (string)userProperties2.userID.ToString());
                    HttpContext.Session.SetString(_api.userName, (string)userProperties2.UserName);
                    HttpContext.Session.SetString(_api.displayname, (string)userProperties2.Name);
                    HttpContext.Session.SetString(_api.email, (string)userProperties2.Email);
                    HttpContext.Session.SetString(_api.company, (string)userProperties2.Company);
                    HttpContext.Session.SetString(_api.regional, (string)userProperties2.Regional);
                    HttpContext.Session.SetString(_api.zona, (string)userProperties2.Zona);
                    HttpContext.Session.SetString(_api.field, (string)userProperties2.Field);
                    HttpContext.Session.SetString(_api.IsAdminsession, (string)userProperties2.IsAdmin.ToString());
                    HttpContext.Session.SetString(_api.periodeIdsession, (string)userProperties2.Period.ToString());

                    setDropDown(userProperties2);

                    return View("UserEdit", userProperties2);
                    //
                    //strView = "UserEdit";
                    //RedirectToAction("/User/UserEdit");

                }
                else
                {
                    //strView = "UserLogin";
                    return View("UserLogin"); //redirect to Login Page
                }
            }
            else
            {
                return View(viewName: "UserLogin");
            }

        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult GetIMANLogin_BD(string UserName, string password)
        //{
        //    TblTUser userProperties2 = new TblTUser();
        //    TblTUser userProperties_ref = new TblTUser();
        //    var APIUrl = Configuration["APIUrl"]; 
        //    //private string strView;
        //    HttpClient client = _api.Initial();            

        //    //Check if username contains pertamina domain
        //    //split the pertamina domain from username
        //    if (UserName.Contains("\\"))
        //    {
        //        UserName = UserName.Split('\\')[1];
        //    }         

            
        //    if((UserName=="mk.jody.ananda") && (password=="123456#"))
        //    { 
        //            HttpContext.Session.SetString(_api.userName, UserName);


        //            userProperties2.UserName = UserName;
        //            userProperties2.Password = password;
        //            //GetIMANData(userProperties2, ref userProperties2);

        //            ViewBag.Displayname = userProperties2.Name;
        //            ViewBag.asd = userProperties2.Company;
        //            HttpContext.Session.SetString(_api.userId, (string)userProperties2.userID.ToString());
        //            HttpContext.Session.SetString(_api.userName, (string)userProperties2.UserName);
        //            HttpContext.Session.SetString(_api.displayname, (string)userProperties2.Name);
        //            HttpContext.Session.SetString(_api.email, (string)userProperties2.Email);
        //            HttpContext.Session.SetString(_api.company, (string)userProperties2.Company);
        //            HttpContext.Session.SetString(_api.regional, (string)userProperties2.Regional);
        //            HttpContext.Session.SetString(_api.zona, (string)userProperties2.Zona);
        //            HttpContext.Session.SetString(_api.field, (string)userProperties2.Field);
        //            HttpContext.Session.SetString(_api.IsAdminsession, (string)userProperties2.IsAdmin.ToString());                
                    

        //            setDropDown(userProperties2);
        //        return RedirectToAction("Index", "Home");
        //    }

        //    return RedirectToAction("Index", "Home");

        //}

        public void GetIMANData(TblTUser UserProperties, ref TblTUser _UserProperties)
        //public IActionResult Edit(CarbonAbsorption carbonAbsorption, int id)
        {
            //userName = UserName.
            var APIUrl = Configuration["APIUrl"];
            var xProp = Configuration["XProp"];
            var xHeaders = Configuration["XAuth"];            

            List<TblTUser> userPropertiesList = new List<TblTUser>();
            HttpClient client = _api.Initial();
            //HttpResponseMessage res = client.GetAsync("https://webappdev01.phe.pertamina.com/PHEMadamAPI/api/Account/GetToken?username=" + UserProperties.UserName + "&password=" + UserProperties.Password).Result;
            //HttpResponseMessage res = client.GetAsync(APIUrl + "api/Account/GetToken?username=" + UserProperties.UserName + "&password=" + UserProperties.Password).Result;
            HttpResponseMessage res = client.GetAsync(APIUrl + "api/Account/GetToken?username=" + UserProperties.UserName + "&password=" + HttpUtility.UrlEncode(UserProperties.Password)).Result;
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                //userProperties = JsonConvert.DeserializeObject<List<TblTUser>>(result);
                //List<authenticated> auth  = JsonConvert.DeserializeObject<authenticated>(result);
                JObject jObject = JObject.Parse(result);
                JToken jUser = jObject["authenticated"];
                bool userAuthenticated = Convert.ToBoolean(jUser);
                //userName = (string)jUser["authenticated"];

                if (userAuthenticated)  //make sure user is authenticated
                {



                    //xProp = "ddrKka+bgB52QZ5Q/+7tz9xXBrBHcxmRmLrY3x9ZoU+Cqtz4gbhVPsZQP4+pKW8Dz0Qbp96wND7jqq5Lk7Sw2A/LbZEvWJxYzDHB1HzWdiuxY7jVjVWimzRYx080Ryq0+Bnsc1IkITS+ZVQ3tykAFQWuRuAZkjzezwxPLorbw8PXNxfy7+F54UL449xpXy1EJb7b1k09TB+VywN3Do7WzLgW5JEe1MQKmerOOtB4R69I0GJdG6/6D7Yp7EVrei9a0eLtMKQRtknjLcg5FhjLuTz+IX5qgYljdi40Jf1XK++nRd5k0hklPPRpkIjwiC5ooSRWWJbMDxIBynSDPaCcoKR1TH+IezKM1n0ED/cBgrlmlpCFZ5X/pOgXUqLz1diKmbi0jIbaEeakn7cajXICkE7dH+6NZFg7BCAZ0ro8TxLziEW018hUV5zXYZ3Bk3NqR+WVlvPpmMODJ5R2h/pT686ak6G3scJkEHeaqH80czWA20hrsSyz95mQbw14Iezg8Z3NDDpdTHHkYU2YR4HWF0oJaLBkvEJfCr71h8FxLCgUgMf50/HaQYXLIaTV06918u3B5XXjBQh19K0/JNT32gNKy5QK0TfmH1Kb29//Ss8nySATc2DOGZIUjqlgA0siqfzXCQY/oEWsBFbfkSf1iXTcvvKZU6TjyAtK5nrXXTznmPNzav16pde6XiTO6TGieRphtMJX4YqZpYOpMx0Y2tM0BNuHOkmn5trvYqqi4GOhB33TpV2ag45vZruC8vUW4VvXHkT/myr+6u6mChaszH+WtMwv9/iemhywOKDsTYc=";
                    //xHeaders = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImN0eSI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFwaS5jYXJib24uY2FsY3VsYXRvciIsIm5iZiI6MTY3MzIyNjA2MywiZXhwIjoxNzA0NzYyMDYzLCJpYXQiOjE2NzMyMjYwNjN9.muxr4b-u9mLKBjn-c8y9-wEnicGSEVIuupHVpLXCSAM";
                    //string usernameTxt = "ario.damar";
                    //string usernameTxt = "umbang.kuncoro";

                    HttpClients = new HttpClient();

                    HttpClients.DefaultRequestHeaders.Add("X-User-Prop", xProp);
                    HttpClients.DefaultRequestHeaders.Add("X-User-Auth", xHeaders);

                    //UserProperties.UserName = "ario.damar";
                    //UserProperties.UserName = "umbang.kuncoro";
                    //UserProperties.UserName = "sapta.jaya";
                    //UserProperties.UserName = "Rahmat.hidayat20";
                    //serProperties.UserName = "joko.wibowo1";
                    //UserProperties.UserName = "Herry.agus";
                    //UserProperties.UserName = "totot.harianto";
                    //UserProperties.UserName = "muhammad.fuad";
                    //UserProperties.UserName = "mk.kevin1";

                    HttpResponseMessage resDB = client.PostAsJsonAsync("api/home/UserCheckV2/", _UserProperties).Result; // HelperStatic.UserID + "/" + HelperStatic.PeriodeID);
                    if (resDB.IsSuccessStatusCode)
                    {
                        List<TblTUser> DataDB = resDB.Content.ReadAsAsync<List<TblTUser>>().Result;

                        if (DataDB[0].Name == "")
                        {
                            //HttpResponseMessage res2 = HttpClients.GetAsync("https://webappdev01.phe.pertamina.com/PHEMadamAPI/api/Employee/GetAllMasterEmployee?WhereCondition=EmpAccount%3D'" + UserProperties.UserName + "'").Result;
                            HttpResponseMessage res2 = HttpClients.GetAsync(APIUrl + "api/Employee/GetAllMasterEmployee?WhereCondition=EmpAccount%3D'" + UserProperties.UserName + "'").Result;

                            if (res2.IsSuccessStatusCode)
                            {
                                var result2 = res2.Content.ReadAsStringAsync().Result;
                                var jObject2 = JObject.Parse(result2);
                                int test = jObject2["Object"].Count();

                                if (jObject2["Object"].Count() > 0 )
                                    {
                                    var cname = jObject2["Object"][0]["CompanyName"].ToObject<String>();
                                    var cEmpName = jObject2["Object"][0]["EmpName"].ToObject<String>();
                                    var cEmpEmail = jObject2["Object"][0]["EmpEmail"].ToObject<String>();
                                    var cRegional = jObject2["Object"][0]["DirectorateDesc"].ToObject<String>(); //Regional
                                    var cZona = jObject2["Object"][0]["DivisionDesc"].ToObject<String>(); //Zona    
                                    var cField = jObject2["Object"][0]["PersSubAreaText"].ToObject<String>(); //Field

                                    //Linq
                                    cname = cname != null ? cname : "-";

                                    //if (cRegional != null)
                                    //{
                                    //    if (cRegional.Contains("-"))
                                    //    {
                                    //        int countString = cRegional.IndexOf("- ");
                                    //        if (countString > 0)
                                    //        {
                                    //            cRegional = cRegional.Substring(0, countString - 1);
                                    //        }
                                    //        else
                                    //        {
                                    //            cRegional = "-";
                                    //        }
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    cRegional = "-";
                                    //}
                                    //LINQ dari logic if diatas
                                    //cRegional = cRegional != null && cRegional.Contains("-") ? cRegional.Substring(0, cRegional.IndexOf("- ") - 1) : "-";
                                    //cRegional = cRegional != null && cRegional.Contains("-") ? cRegional.Substring(0, cRegional.IndexOf("- ") >= 0 ? cRegional.IndexOf("- ") - 1 : cRegional.Length) : "-";
                                    //cRegional = cRegional != null && cRegional.Contains("-") ? cRegional.Substring(0, cRegional.IndexOf(" (") >= 0 ? cRegional.IndexOf(" (") : cRegional.Length) : "-";
                                    cRegional = cRegional != null && cRegional.Contains("-") ? (cRegional.Contains("PT Pertamina Hulu Rokan (SK-52)") ? "Regional 1": cRegional.Substring(0, cRegional.IndexOf("- ") - 1)) : "-";

                                    string cZonaTemp;
                                    //Check regional, if zona = 2 or zona 3, then set zona = WK Rokan with WK Rokan field as field
                                    if ((Convert.ToString(cZona) == "WK Rokan"))
                                    {
                                        cField = "WK Rokan field";
                                    }
                                    //Check if cZona contains Zona string, else put Zona = - (value=0)
                                    //if (cZona != null)
                                    //{
                                    //    if (Convert.ToString(cZona).Contains("Zona") || Convert.ToString(cZona).Contains("WK Rokan"))
                                    //    {
                                    //        cZonaTemp = cZona;
                                    //    }
                                    //    else
                                    //    {
                                    //        cZonaTemp = "-";
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    cZonaTemp = "-";
                                    //}
                                    //LINQ dari logic if diatas
                                    cZonaTemp = cZona != null && (Convert.ToString(cZona).Contains("Zona") || Convert.ToString(cZona).Contains("WK Rokan")) ? cZona : "-";

                                    //checking field anomalies
                                    //switch (cField)
                                    //{
                                    //    case "Air Itam":
                                    //        cField = "Raja Tempirai";
                                    //        break;

                                    //    case "Bajubang":
                                    //        cField = "Field Jambi";
                                    //        break;

                                    //    case "Balongan":
                                    //        cField = "Subang";
                                    //        break;

                                    //    case "Cilamaya":
                                    //        cField = "Subang";
                                    //        break;

                                    //    case "Sembakung":
                                    //        cField = "Tarakan";
                                    //        break;

                                    //    case "Semberah":
                                    //        cField = "Sangatta";
                                    //        break;

                                    //    case "Tempino":
                                    //        cField = "Field Jambi";
                                    //        break;

                                    //    default:
                                    //        cField = cField != null ? cField : "-";
                                    //        break;
                                    //}
                                    //LINQ dari logic if diatas
                                    cField = cField switch
                                    {
                                        "Air Itam" => "Raja Tempirai",
                                        "Bajubang" => "Field Jambi",
                                        "Balongan" => "Subang",
                                        "Cilamaya" => "Subang",
                                        "Sembakung" => "Tarakan",
                                        "Semberah" => "Sangatta",
                                        "Tempino" => "Field Jambi",
                                        _ => cField != null ? cField : "-",
                                    };

                                    //Set the model value
                                    _UserProperties.Company = cname;
                                    _UserProperties.Name = cEmpName;
                                    _UserProperties.Email = cEmpEmail;
                                    _UserProperties.Regional = cRegional;
                                    _UserProperties.Zona = cZonaTemp;
                                    _UserProperties.Field = cField;

                                    RedirectToAction("/User/UserEdit");
                                }
                                else if (jObject2["Object"].Count() <= 0)
                                //else
                                {
                                    ContextType authenticationType = ContextType.Domain;
                                    PrincipalContext principalContext = new PrincipalContext(authenticationType, "pertamina.com");
                                    UserPrincipal userPrincipal = null;
                                    userPrincipal = UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, UserProperties.UserName);

                                    ViewBag.Displayname = userPrincipal.DisplayName; //username.Split('\\')[1].ToUpper();
                                    ViewBag.Email = userPrincipal.EmailAddress; //"carbon@pertamina.com"; //hardcode first

                                    //var cname = "-";
                                    //var cEmpName = userPrincipal.DisplayName;
                                    //var cEmpEmail = userPrincipal.EmailAddress;
                                    //var cRegional = "-"; //Regional
                                    //var cZona = "-"; //Zona    
                                    //var cField = "-"; //Field
                                    _UserProperties.Company = "-";
                                    _UserProperties.Name = userPrincipal.DisplayName;
                                    _UserProperties.Email = userPrincipal.EmailAddress;
                                    _UserProperties.Regional = "-";
                                    _UserProperties.Zona = "-";
                                    _UserProperties.Field = "-";
                                }
                            }
                        }
                        else if(DataDB[0].Name != "")
                        {

                        }

                        HttpResponseMessage resGetDB = client.PostAsJsonAsync("api/home/UserCheckV2/", _UserProperties).Result; // HelperStatic.UserID + "/" + HelperStatic.PeriodeID);

                        List<TblTUser> GetDataDB = resGetDB.Content.ReadAsAsync<List<TblTUser>>().Result;
                        _UserProperties = GetDataDB[0];
                    }
                }

            }

        }

        [HttpGet]
        public IActionResult InformasiAkun()
        {
            SetViewBag();
            if (_api.UserName == null)
            {
                return RedirectToAction("UserLogin", "User");
            }
            TblTUser userInfo = new TblTUser();

            HttpClient client = _api.Initial();

            userInfo.UserName = _api.UserName;

            HttpResponseMessage resGetDB = client.PostAsJsonAsync("api/home/UserCheckV2/", userInfo).Result; // HelperStatic.UserID + "/" + HelperStatic.PeriodeID);

            List<TblTUser> GetDataDB = resGetDB.Content.ReadAsAsync<List<TblTUser>>().Result;
            userInfo = GetDataDB[0];

            setDropDown(userInfo);
            return View(userInfo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Konfirmasi(TblTUser UserKonfirmasi)
        {
            if (HttpContext.Session.GetString(_api.userName) == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            UserKonfirmasi.userID = Convert.ToInt32(HttpContext.Session.GetString(_api.userId));
            UserKonfirmasi.UserName = HttpContext.Session.GetString(_api.userName);
            UserKonfirmasi.Name = HttpContext.Session.GetString(_api.displayname);
            UserKonfirmasi.Email = HttpContext.Session.GetString(_api.email);
            UserKonfirmasi.IsAdmin = Convert.ToInt32(HttpContext.Session.GetString(_api.IsAdminsession));
            //UserKonfirmasi.Regional = HttpContext.Session.GetString(regional);
            //UserKonfirmasi.Zona = HttpContext.Session.GetString(zona);
            //UserKonfirmasi.Field = HttpContext.Session.GetString(field);


            HttpClient client = _api.Initial();
            try
            {

                HttpResponseMessage res = client.PutAsJsonAsync("api/User/" + UserKonfirmasi.userID, UserKonfirmasi).Result;
                TempData["SuccessMessage"] = "Data telah diperbaharui";

                //Jika berhasil Set session Company 
                HttpContext.Session.SetString(_api.company, (string)UserKonfirmasi.Company);
                HttpContext.Session.SetString(_api.regional, (string)UserKonfirmasi.Regional);
                HttpContext.Session.SetString(_api.zona, (string)UserKonfirmasi.Zona);
                HttpContext.Session.SetString(_api.field, (string)UserKonfirmasi.Field);

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return BadRequest();
            }

        }

        #region Disable
        //public async Task<JsonResult> GetEmployee(string username)
        //{
        //    HttpClient client = _api.Initial();
        //    List<TblTUser> userProperties = new List<TblTUser>();
        //    //Add authorization headers 
        //    string xProp = "ddrKka+bgB52QZ5Q/+7tz9xXBrBHcxmRmLrY3x9ZoU+Cqtz4gbhVPsZQP4+pKW8Dz0Qbp96wND7jqq5Lk7Sw2A/LbZEvWJxYzDHB1HzWdiuxY7jVjVWimzRYx080Ryq0+Bnsc1IkITS+ZVQ3tykAFQWuRuAZkjzezwxPLorbw8PXNxfy7+F54UL449xpXy1EJb7b1k09TB+VywN3Do7WzLgW5JEe1MQKmerOOtB4R69I0GJdG6/6D7Yp7EVrei9a0eLtMKQRtknjLcg5FhjLuTz+IX5qgYljdi40Jf1XK++nRd5k0hklPPRpkIjwiC5ooSRWWJbMDxIBynSDPaCcoKR1TH+IezKM1n0ED/cBgrlmlpCFZ5X/pOgXUqLz1diKmbi0jIbaEeakn7cajXICkE7dH+6NZFg7BCAZ0ro8TxLziEW018hUV5zXYZ3Bk3NqR+WVlvPpmMODJ5R2h/pT686ak6G3scJkEHeaqH80czWA20hrsSyz95mQbw14Iezg8Z3NDDpdTHHkYU2YR4HWF0oJaLBkvEJfCr71h8FxLCgUgMf50/HaQYXLIaTV06918u3B5XXjBQh19K0/JNT32gNKy5QK0TfmH1Kb29//Ss8nySATc2DOGZIUjqlgA0siqfzXCQY/oEWsBFbfkSf1iXTcvvKZU6TjyAtK5nrXXTznmPNzav16pde6XiTO6TGieRphtMJX4YqZpYOpMx0Y2tM0BNuHOkmn5trvYqqi4GOhB33TpV2ag45vZruC8vUW4VvXHkT/myr+6u6mChaszH+WtMwv9/iemhywOKDsTYc=";
        //    string xHeaders = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImN0eSI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFwaS5jYXJib24uY2FsY3VsYXRvciIsIm5iZiI6MTY3MzIyNjA2MywiZXhwIjoxNzA0NzYyMDYzLCJpYXQiOjE2NzMyMjYwNjN9.muxr4b-u9mLKBjn-c8y9-wEnicGSEVIuupHVpLXCSAM";
        //    string usernameTxt = "ario.damar";

        //    HttpClients = new HttpClient();

        //    HttpClients.DefaultRequestHeaders.Add("X-User-Prop", xProp);
        //    HttpClients.DefaultRequestHeaders.Add("X-User-Auth", xHeaders);
        //    //string userName = "mk.jody.ananda";
        //    //userProperties = txtUserName.Text();

        //    //
        //    //HttpResponseMessage res = await client.GetAsync("https://webappdev01.phe.pertamina.com/PHEMadamAPI/api/Account/GetToken?username=" + userName + "&password=" + "Pertamax@2022");
        //    //HttpResponseMessage res = await client.GetAsync("https://webappdev01.phe.pertamina.com/PHEMadamAPI/api/Employee/GetAllMasterEmployee?WhereCondition=EmpAccount%3D' + UserName + "'");
        //    var res = await HttpClients.GetAsync("https://webappdev01.phe.pertamina.com/PHEMadamAPI/api/Employee/GetAllMasterEmployee?WhereCondition=EmpAccount%3D'ario.damar'");
        //    if (res.IsSuccessStatusCode)
        //    {
        //        //Login OK
        //        //var result = res.Content.ReadAsStringAsync().Result;

        //        var resultEmp = res.Content.ReadAsStringAsync().Result;
        //        JObject jObjectEmployee = JObject.Parse(resultEmp);

        //        //Get token
        //        JToken jUserCompany = jObjectEmployee["CompanyName"];
        //        JToken jUserName = jObjectEmployee["EmpName"];
        //        JToken jUserEmpEmail = jObjectEmployee["EmpEmail"];
        //        JToken jUserDirectorate = jObjectEmployee["DirectorateDesc"];
        //        JToken jUserZona = jObjectEmployee["DivisionDesc"];
        //        JToken jUserPersArea = jObjectEmployee["PersAreaText"];
        //        JToken jUserPersSubAreaText = jObjectEmployee["PersSubAreaText"];

        //        //checking zona 2,3 anomalies, set to WK Rokan
        //        //if (jUserZona.ToString() = "Zona 2") || (Convert.ToString(jUserZona) = "Zona 3"))
        //        //{
        //        //    jUserZona = "Zona WK Rokan";
        //        //    jUserPersSubAreaText = "WK Rokan field";
        //        //}

        //        //checking field anomalies
        //        switch (Convert.ToString(jUserPersSubAreaText))
        //        {
        //            case "Air Itam":
        //                jUserPersSubAreaText = "Raja Tempirai";
        //                break;

        //            case "Bajubang":
        //                jUserPersSubAreaText = "Field Jambi";
        //                break;

        //            case "Balongan":
        //                jUserPersSubAreaText = "Subang";
        //                break;

        //            case "Cilamaya":
        //                jUserPersSubAreaText = "Subang";
        //                break;

        //            case "Sembakung":
        //                jUserPersSubAreaText = "Tarakan";
        //                break;

        //            case "Semberah":
        //                jUserPersSubAreaText = "Sangatta";
        //                break;

        //            case "Tempino":
        //                jUserPersSubAreaText = "Field Jambi";
        //                break;
        //        }
        //        //if Authenticated
        //        Response.Redirect("https://localhost:44399/");

        //        //
        //        //userProperties = res.Content.ReadAsAsync<List<userProperties>>().Result;
        //        //userProperties = JsonConvert.DeserializeObject<List<TblTUser>>(result);
        //    }

        //    return Json (res);
        //}



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> LoginAsync(string username)
        //{
        //    HttpClient client = _api.Initial();
        //    //string password = "Pertamax@2022";
        //    //string UserName = "ario.damar";
        //    //string Password = "Pertamax@2022";

        //    string xProp = "ddrKka+bgB52QZ5Q/+7tz9xXBrBHcxmRmLrY3x9ZoU+Cqtz4gbhVPsZQP4+pKW8Dz0Qbp96wND7jqq5Lk7Sw2A/LbZEvWJxYzDHB1HzWdiuxY7jVjVWimzRYx080Ryq0+Bnsc1IkITS+ZVQ3tykAFQWuRuAZkjzezwxPLorbw8PXNxfy7+F54UL449xpXy1EJb7b1k09TB+VywN3Do7WzLgW5JEe1MQKmerOOtB4R69I0GJdG6/6D7Yp7EVrei9a0eLtMKQRtknjLcg5FhjLuTz+IX5qgYljdi40Jf1XK++nRd5k0hklPPRpkIjwiC5ooSRWWJbMDxIBynSDPaCcoKR1TH+IezKM1n0ED/cBgrlmlpCFZ5X/pOgXUqLz1diKmbi0jIbaEeakn7cajXICkE7dH+6NZFg7BCAZ0ro8TxLziEW018hUV5zXYZ3Bk3NqR+WVlvPpmMODJ5R2h/pT686ak6G3scJkEHeaqH80czWA20hrsSyz95mQbw14Iezg8Z3NDDpdTHHkYU2YR4HWF0oJaLBkvEJfCr71h8FxLCgUgMf50/HaQYXLIaTV06918u3B5XXjBQh19K0/JNT32gNKy5QK0TfmH1Kb29//Ss8nySATc2DOGZIUjqlgA0siqfzXCQY/oEWsBFbfkSf1iXTcvvKZU6TjyAtK5nrXXTznmPNzav16pde6XiTO6TGieRphtMJX4YqZpYOpMx0Y2tM0BNuHOkmn5trvYqqi4GOhB33TpV2ag45vZruC8vUW4VvXHkT/myr+6u6mChaszH+WtMwv9/iemhywOKDsTYc=";
        //    string xHeaders = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImN0eSI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFwaS5jYXJib24uY2FsY3VsYXRvciIsIm5iZiI6MTY3MzIyNjA2MywiZXhwIjoxNzA0NzYyMDYzLCJpYXQiOjE2NzMyMjYwNjN9.muxr4b-u9mLKBjn-c8y9-wEnicGSEVIuupHVpLXCSAM";

        //    List<TblTUser> userProperties = new List<TblTUser>();

        //    //Add authorization headers 
        //    //Request.Headers.Add("X-User-Prop:", xProp);
        //    //Request.Headers.Add("X-User-Auth:", xHeaders);

        //    HttpClients = new HttpClient();

        //    HttpClients.DefaultRequestHeaders.Add("X-User-Prop", xProp);
        //    HttpClients.DefaultRequestHeaders.Add("X-User-Auth", xHeaders);

        //    //
        //    //HttpResponseMessage res = await client.GetAsync("https://webappdev01.phe.pertamina.com/PHEMadamAPI/api/Account/GetToken?username=" + UserName + "&password=" + Password);
        //    HttpResponseMessage res = await HttpClients.GetAsync("https://webappdev01.phe.pertamina.com/PHEMadamAPI/api/Employee/GetAllMasterEmployee?WhereCondition=EmpAccount%3D'" + username + "'");
        //    //client.GetAsync()
        //    //userProperties = res.Content.ReadAsAsync<List<TblTUser>>().Result;

        //    if (res.IsSuccessStatusCode)
        //    {
        //        //userProperties = res.Content.ReadAsAsync<List<userProperties>>().Result;
        //        //Login OK

        //        var result = res.Content.ReadAsStringAsync().Result;

        //        //
        //        //userProperties = res.Content.ReadAsAsync<List<userProperties>>().Result;
        //        userProperties = JsonConvert.DeserializeObject<List<TblTUser>>(result);
        //    }
        //    //try
        //    //{
        //    //    HttpResponseMessage res = client.PostAsJsonAsync("https://webappdev01.phe.pertamina.com/PHEMadamAPI/api/Account/GetToken?username?", userLogin).Result;
        //    //    //TempData["SuccessMessage"] = "Data telah disimpan";
        //    //    return RedirectToAction(nameof(Index));

        //    //}
        //    //catch
        //    //{
        //    //    return BadRequest();
        //    //}


        //    //List<TblTUser> userProperties = new List<TblTUser>();

        //    //userProperties = GetUser(userLogin.UserName, userLogin.Password);
        //    return Json(userProperties);
        //}
        #endregion
        public void setDropDown(TblTUser _TblTUser)
        {
            HttpClient client = _api.Initial();

            //Zona
            HttpResponseMessage resListZona = client.GetAsync("api/User/GetListZona").Result;
            List<TblMasterZona> _ListZona = new List<TblMasterZona>();
            _ListZona = resListZona.Content.ReadAsAsync<List<TblMasterZona>>().Result;

            IEnumerable<SelectListItem> ListZona = from value in _ListZona
                                                   select new SelectListItem
                                                   {
                                                       Text = value.Zona.ToString(),
                                                       Value = value.Zona.ToString()
                                                       //,Selected = value == vwVehicleList_User_Action.FuelId.ToString()
                                                   };

            if (_TblTUser.Zona == null)
                ViewBag.Zona = new SelectList(_ListZona, "Zona", "Zona");
            else
                ViewBag.Zona = new SelectList(_ListZona.ToArray(), "Zona", "Zona", _TblTUser.Zona);

            //Regional
            HttpResponseMessage resListRegional = client.GetAsync("api/User/GetListRegional").Result;
            List<TblMasterRegional> _ListRegional = new List<TblMasterRegional>();
            _ListRegional = resListRegional.Content.ReadAsAsync<List<TblMasterRegional>>().Result;

            IEnumerable<SelectListItem> ListRegional = from value in _ListRegional
                                                       select new SelectListItem
                                                   {
                                                       Text = value.Regional.ToString(),
                                                       Value = value.Regional.ToString()
                                                       //,Selected = value == vwVehicleList_User_Action.FuelId.ToString()
                                                   };

            if (_TblTUser.Zona == null)
                ViewBag.Regional = new SelectList(_ListRegional, "Regional", "Regional");
            else
                ViewBag.Regional = new SelectList(_ListRegional.ToArray(), "Regional", "Regional", _TblTUser.Regional);

            //Company
            HttpResponseMessage resListCompany = client.GetAsync("api/User/GetListCompany").Result;
            List<TblMasterCompany> _ListCompany = new List<TblMasterCompany>();
            _ListCompany = resListCompany.Content.ReadAsAsync<List<TblMasterCompany>>().Result;

            IEnumerable<SelectListItem> ListCompany = from value in _ListCompany
                                                       select new SelectListItem
                                                       {
                                                           Text = value.Company.ToString(),
                                                           Value = value.Company.ToString()
                                                           //,Selected = value == vwVehicleList_User_Action.FuelId.ToString()
                                                       };

            if (_TblTUser.Zona == null)
                ViewBag.Company = new SelectList(_ListCompany, "Company", "Company");
            else
                ViewBag.Company = new SelectList(_ListCompany.ToArray(), "Company", "Company", _TblTUser.Company);

            //Field
            HttpResponseMessage resListField= client.GetAsync("api/User/GetListField").Result;
            List<TblMasterField> _ListField= new List<TblMasterField>();
            _ListField= resListField.Content.ReadAsAsync<List<TblMasterField>>().Result;

            IEnumerable<SelectListItem> ListField= from value in _ListField
                                                      select new SelectListItem
                                                      {
                                                          Text = value.Field.ToString(),
                                                          Value = value.Field.ToString()
                                                          //,Selected = value == vwVehicleList_User_Action.FuelId.ToString()
                                                      };

            if (_TblTUser.Zona == null)
                ViewBag.Field= new SelectList(_ListField, "Field", "Field");
            else
                ViewBag.Field= new SelectList(_ListField.ToArray(), "Field", "Field", _TblTUser.Field);


        }


        public IActionResult UserEdit()
        {
            return View();

        }

        public IActionResult UserLogin()
        {
            return View();

        }
        public IActionResult UserLogin_BDCC()
        {
            return View();

        }

        public IActionResult UserLogin_BD()
        {
            return View();

        }


        public IActionResult Edit()
        {
            return View();

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("UserLogin");

        }

    }
}
