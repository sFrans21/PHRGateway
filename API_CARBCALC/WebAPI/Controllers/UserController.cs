using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Function;
using WebAPI.Models;
//using Hel

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        //HelperAPI _api = new HelperAPI();

        public readonly APIDBContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(APIDBContext context, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51974/");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public IActionResult Index()
        {
            return View();
        }                

        [HttpGet]        
        [Route("GetUserId/{UserName}")]
        public async Task<IActionResult> GetUserId(string userName)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblTUser.Where(a => a.UserName == userName).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpGet]
        [Route("GetUser/{UserId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblTUser.Where(a => a.userID == userId).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpGet]
        [Route("GetAimanfromView/{UserName}")]
        public async Task<IActionResult> GetAimanfromView(string UserName)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwAIMAN_Username.Where(a => a.EmpAccount == UserName).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        //[HttpGet]
        //[ValidateAntiForgeryToken]
        //[Route("GetUserLogin/{username}")]
        //public async Task<JsonResult> GetLogin(string username, string password)
        //{
        //    //userName = UserName.

        //    List<TblTUser> userProperties = new List<TblTUser>();
        //    HttpClient client = Initial();
        //    HttpResponseMessage res = client.GetAsync("https://webappdev01.phe.pertamina.com/PHEMadamAPI/api/Account/GetToken?username=" + username + "&password=" + password).Result;
        //    if (res.IsSuccessStatusCode)
        //    {
        //        var result = res.Content.ReadAsStringAsync().Result;
        //        //userProperties = JsonConvert.DeserializeObject<List<TblTUser>>(result);
        //        //List<authenticated> auth  = JsonConvert.DeserializeObject<authenticated>(result);
        //        JObject jObject = JObject.Parse(result);
        //        JToken jUser = jObject["authenticated"];
        //        bool userAuthenticated = Convert.ToBoolean(jUser);
        //        //userName = (string)jUser["authenticated"];

        //        if (userAuthenticated)
        //        {
        //            //Get Employee information from i-Man/Madam
        //            //GetEmployee(userName);


        //            //Put userid, username, displayname to session
        //            //HttpContext.Session.SetString(userId, vmtest[0].UserId.ToString());
        //            HttpContext.Session.SetString(username, username);
        //            //HttpContext.Session.SetString(displayname, (string)ViewBag.Displayname);


        //            //Redirect to Profile Page
        //            //RedirectToAction("/User/UserEdit");


        //            string xProp = "ddrKka+bgB52QZ5Q/+7tz9xXBrBHcxmRmLrY3x9ZoU+Cqtz4gbhVPsZQP4+pKW8Dz0Qbp96wND7jqq5Lk7Sw2A/LbZEvWJxYzDHB1HzWdiuxY7jVjVWimzRYx080Ryq0+Bnsc1IkITS+ZVQ3tykAFQWuRuAZkjzezwxPLorbw8PXNxfy7+F54UL449xpXy1EJb7b1k09TB+VywN3Do7WzLgW5JEe1MQKmerOOtB4R69I0GJdG6/6D7Yp7EVrei9a0eLtMKQRtknjLcg5FhjLuTz+IX5qgYljdi40Jf1XK++nRd5k0hklPPRpkIjwiC5ooSRWWJbMDxIBynSDPaCcoKR1TH+IezKM1n0ED/cBgrlmlpCFZ5X/pOgXUqLz1diKmbi0jIbaEeakn7cajXICkE7dH+6NZFg7BCAZ0ro8TxLziEW018hUV5zXYZ3Bk3NqR+WVlvPpmMODJ5R2h/pT686ak6G3scJkEHeaqH80czWA20hrsSyz95mQbw14Iezg8Z3NDDpdTHHkYU2YR4HWF0oJaLBkvEJfCr71h8FxLCgUgMf50/HaQYXLIaTV06918u3B5XXjBQh19K0/JNT32gNKy5QK0TfmH1Kb29//Ss8nySATc2DOGZIUjqlgA0siqfzXCQY/oEWsBFbfkSf1iXTcvvKZU6TjyAtK5nrXXTznmPNzav16pde6XiTO6TGieRphtMJX4YqZpYOpMx0Y2tM0BNuHOkmn5trvYqqi4GOhB33TpV2ag45vZruC8vUW4VvXHkT/myr+6u6mChaszH+WtMwv9/iemhywOKDsTYc=";
        //            string xHeaders = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImN0eSI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFwaS5jYXJib24uY2FsY3VsYXRvciIsIm5iZiI6MTY3MzIyNjA2MywiZXhwIjoxNzA0NzYyMDYzLCJpYXQiOjE2NzMyMjYwNjN9.muxr4b-u9mLKBjn-c8y9-wEnicGSEVIuupHVpLXCSAM";
        //            string usernameTxt = "ario.damar";

        //            //HttpClient = new HttpClient();

        //            //HttpClient.DefaultRequestHeaders.Add("X-User-Prop", xProp);
        //            //HttpClient.DefaultRequestHeaders.Add("X-User-Auth", xHeaders);

        //            //HttpClient.Get
        //            //HttpResponseMessage res2 = await HttpClients.GetAsync("https://webappdev01.phe.pertamina.com/PHEMadamAPI/api/Employee/GetAllMasterEmployee?WhereCondition=EmpAccount%3D'" + usernameTxt + "'");


        //            //if (res2.IsSuccessStatusCode)
        //            //{
        //            //    var result2 = res2.Content.ReadAsStringAsync().Result;
        //            //    var jObject2 = JObject.Parse(result2);
        //            //    var cname = jObject2["Object"][0]["CompanyName"].ToObject<String>();
        //            //    var cEmpName = jObject2["Object"][0]["EmpName"].ToObject<String>();
        //            //    var cEmpEmail = jObject2["Object"][0]["EmpEmail"].ToObject<String>();
        //            //    var cRegional = jObject2["Object"][0]["DirectorateDesc"].ToObject<String>(); //Regional
        //            //    var cZona = jObject2["Object"][0]["DivisionDesc"].ToObject<String>(); //Zona    
        //            //    var cField = jObject2["Object"][0]["PersSubAreaText"].ToObject<String>(); //Field

        //            //    //Check if cZona contains Zona string, else put Zona = - (value=0)

        //            //    //checking field anomalies



        //            //    RedirectToAction("/User/UserEdit");
        //            //}

        //        }
        //        else
        //        {
        //            Response.Redirect("https://localhost:44399/User"); //redirect to Edit
        //        }

        //    }

        //    //userProperties = JsonConvert.DeserializeObject<List<TblTUser>>(result);

        //    //return Json(userProperties);
        //    RedirectToAction("/User/UserEdit");
        //    return Json(userProperties);
        //}

        [HttpGet]
        [Route("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblTUser.OrderBy(o => o.userID).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpGet]
        [Route("GetListCompany")]
        public async Task<IActionResult> GetListCompany()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblMasterCompany.OrderBy(o => o.Company).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpGet]
        [Route("GetListRegional")]
        public async Task<IActionResult> GetListRegional()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblMasterRegional.OrderBy(o => o.Regional).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }


        [HttpGet]
        [Route("GetListZona")]
        public async Task<IActionResult> GetListZona()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblMasterZona.OrderBy(o => o.ID).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpGet]
        [Route("GetListField")]
        public async Task<IActionResult> GetListField()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblMasterField.OrderBy(o => o.Field).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TblTUser userInfo, [FromForm] FnUser fnUser)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                if (!ModelState.IsValid)
                    return BadRequest("Not a valid model");

                userInfo.UserName = fnUser.UserName(userInfo);
                userInfo.Name = fnUser.Name(userInfo);
                userInfo.Company = fnUser.Company(userInfo);
                userInfo.Regional = fnUser.Regional(userInfo);
                userInfo.Zona = fnUser.Zona(userInfo);
                userInfo.Field = fnUser.Field(userInfo);
                userInfo.IsAdmin = fnUser.IsAdmin(userInfo);

                _context.TblTUser.Add(userInfo);
                await _context.SaveChangesAsync();
                return Ok("Data Added");
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TblTUser TblUser)
        {            
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                if (id != TblUser.userID)
                {
                    return BadRequest();
                }

                _context.Entry(TblUser).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok($"Data with id = {id} Updated!");
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var userInfo = await _context.TblTUser.FindAsync(id);
                if (userInfo == null)
                {
                    return NotFound();
                }

                _context.TblTUser.Remove(userInfo);
                await _context.SaveChangesAsync();

                return Ok("Data Deleted!");
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }
        private bool UserExists(int id)
        {
            return _context.TblTUser.Any(e => e.userID == id);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult GetIMANLogin(string UserName, string password)
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

        //    HttpResponseMessage res = client.GetAsync(APIUrl + "api/Account/GetToken?username=" + UserName + "&password=" + password).Result;
        //    if (res.IsSuccessStatusCode)
        //    {
        //        var result = res.Content.ReadAsStringAsync().Result;
        //        JObject jObject = JObject.Parse(result);
        //        JToken jUser = jObject["authenticated"];
        //        bool userAuthenticated = Convert.ToBoolean(jUser);
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
        //        return View(viewName: "UserLogin");
        //    }

        //}
    }
}
