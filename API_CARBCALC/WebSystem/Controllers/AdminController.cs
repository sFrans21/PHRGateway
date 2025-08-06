using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.IO;				
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers; 
using System.Threading.Tasks;
using System.Xml.Linq;
using WebAPI.Models;
using WebSystem.Helper;
using WebSystem.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Net.Mime.MediaTypeNames;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using WebSystem.CustomTagHelper;
using WebSystem.Filters;
using Microsoft.AspNetCore.Routing;
using System.Collections;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebSystem.Controllers
{
    public class AdminController : Controller
    {
        HelperAPI _api = new HelperAPI();
		private readonly IWebHostEnvironment webHostEnvironment;												
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
        }

        private HttpClient HttpClients;

        private readonly IConfiguration Configuration;
        public AdminController(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            SetViewBag();

            //uncomment this lines for production
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }

            //if (testID == 1)
            //    return View("Master_User_Edit");

            //


            return View();
        }


        
        public async Task<JsonResult> GetListUser()
        {
            SetViewBag();
            List<TblTUser> UserList = new List<TblTUser>();       

            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/admin/GetListUser/");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                

                UserList = JsonConvert.DeserializeObject<List<TblTUser>>(result);

                // Add encLink to each row of UserList
                for (var i = 0; i <= UserList.Count-1; i++)
                {
                    
                    UserList[i].encLink = CustomQueryStringHelper.EncryptString("", "Master_User_Edit", "Admin", new { userID = UserList[i].userID, applicationId = "CC-1" });
                    UserList[i].encLink_Edit = CustomQueryStringHelper.EncryptString("", "Master_User_Details", "Admin", new { userID = UserList[i].userID, applicationId = "CC-1" });

                }



            }
            return Json(UserList);
        }

        public async Task<JsonResult> GetUser(int userId)
        {
            SetViewBag();
            List<TblTUser> UserList = new List<TblTUser>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/user/GetListUser/");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                UserList = JsonConvert.DeserializeObject<List<TblTUser>>(result);
            }
            return Json(UserList);
        }

        [DecryptQueryStringParameter]
        public IActionResult Master_User_Details(int id, string userID)
        {
            SetViewBag();
            TblTUser userProperties2 = new TblTUser();

            // Uncomment this for production
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }

            List<TblTUser> userInfo = new List<TblTUser>();
            HttpClient client = _api.Initial();

            id = Convert.ToInt32(userID);
            HttpResponseMessage res = client.GetAsync("api/user/GetUser/" + id).Result;

            userInfo = res.Content.ReadAsAsync<List<TblTUser>>().Result;

            var userName = userInfo[0].UserName;
            var userCompany = userInfo[0].Company;
            var userRegional = userInfo[0].Regional;
            var userZona = userInfo[0].Zona;
            var userField = userInfo[0].Field;
            var userAdmin = userInfo[0].IsAdmin;

            //Set the Viewbag properties
            ViewBag.userId = id;
            ViewBag.userName = userName;
            ViewBag.userCompany = userCompany;
            ViewBag.userRegional = userRegional;
            ViewBag.userZona = userZona;
            ViewBag.userField = userField;
            ViewBag.userAdmin = userAdmin;

            //
            //_ctl.setDropDown(userInfo);
            //setDropDown(userProperties2);

            return View(userInfo[0]);
        }

        //Use Decrypt Attribute Filter
        //[DecryptQueryStringParameter]
        public IActionResult Master_User_Edit(int id, string userID, string Email)
        {
            SetViewBag();
            TblTUser userProperties2 = new TblTUser();
            //UserController _ctl = new UserController();

            // Uncomment this for production
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            
            //id = Convert.ToInt32(userID);
            List<TblTUser> userInfo = new List<TblTUser>();

            HttpClient client = _api.Initial();
            HttpResponseMessage res = client.GetAsync("api/user/GetUser/" + id).Result;

            userInfo = res.Content.ReadAsAsync<List<TblTUser>>().Result;
            
            var userName = userInfo[0].UserName;
            var userCompany = userInfo[0].Company;
            var userRegional = userInfo[0].Regional;
            var userZona = userInfo[0].Zona;
            var userField = userInfo[0].Field;
            var userAdmin = userInfo[0].IsAdmin;

            //Set the Viewbag properties
            ViewBag.userId = id;
            ViewBag.userName = userName;
            ViewBag.userCompany = userCompany;
            ViewBag.userRegional = userRegional;
            ViewBag.userZona = userZona;
            ViewBag.userField = userField;
            ViewBag.userAdmin = userAdmin;

            //
            //_ctl.setDropDown(userInfo);
            setDropDown(userProperties2);

            return View(userInfo[0]);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update_User(TblTUser userInfo)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            HttpClient client = _api.Initial();
            try
            {

                HttpResponseMessage res = client.PutAsJsonAsync("api/user/" + userInfo.userID, userInfo).Result;
                TempData["SuccessMessage"] = "Data telah diperbaharui";
                return RedirectToAction(nameof(Master_User));
            }
            catch
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Delete_User(int id)
        {
            if (HttpContext.Session.GetString(_api.userName) == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            //SetViewBag();
            HttpClient client = _api.Initial();

            try
            {
                var userInfo = new TblTUser();
                HttpResponseMessage res = await client.DeleteAsync($"api/user/{id}");
                TempData["SuccessMessage"] = "Data telah dihapus";
                return RedirectToAction("Index");
            }

            catch
            {
                return BadRequest();
            }

        }

       

        public IActionResult Master_User()
        {
            SetViewBag();

            //uncomment this lines for production
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }

            return View();

        }
        

        public IActionResult Master_User_Create()
        {
            SetViewBag();

            //uncomment this lines for production
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            TblTUser userProperties2 = new TblTUser();
            setDropDown(userProperties2);
            return View("Master_User_Create");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create_User(TblTUser userInfo)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            HttpClient client = _api.Initial();

            try
            {

                HttpResponseMessage res = client.PostAsJsonAsync("api/user/", userInfo).Result;
                TempData["SuccessMessage"] = "Data telah disimpan";
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return BadRequest();
            }
        }
        #region Formula
        public IActionResult Master_VehicleFormula()
        {
            SetViewBag();

            //uncomment this lines for production
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            return View("Master_VehicleFormula");

        }
        public async Task<JsonResult> GetVehicleFormulaList()
        {
            SetViewBag();
            List<VwMasterVehicleFormula> listVehicleFormula = new List<VwMasterVehicleFormula>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/Vehicle/GetMasterVehicleFormula");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                listVehicleFormula = JsonConvert.DeserializeObject<List<VwMasterVehicleFormula>>(result);
            }
            return Json(listVehicleFormula);
        }
        public IActionResult Create_VehicleFormula()
        {
            SetViewBag();

            //uncomment this lines for production
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }

            VwMasterVehicleFormula vwMasterVehicleFormula = new VwMasterVehicleFormula();
            vwMasterVehicleFormula.Action = "CREATE";

            setDropDown_Vehicle(vwMasterVehicleFormula);


            return View("Master_VehicleFormula_Create");
        }

        public IActionResult Save_VehicleFormula(VwMasterVehicleFormula VehicleFormula)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            HttpClient client = _api.Initial();

            try
            {

                //HttpResponseMessage res = client.PostAsJsonAsync("api/vehicle/PostVehicleFormula/", VehicleFormula).Result;
                //TempData["SuccessMessage"] = "Data berhasil disimpan";
                

                List<Execute_SP_Output> test = new List<Execute_SP_Output>();

                HttpResponseMessage res = client.PostAsJsonAsync("api/vehicle/PostVehicleFormula/", VehicleFormula).Result;


                if (res.IsSuccessStatusCode)
                {
                    List<Execute_SP_Output> ExOutput = res.Content.ReadAsAsync<List<Execute_SP_Output>>().Result;
                    if (ExOutput[0].IsSukses == 1)
                    {
                        TempData["SuccessMessage"] = ExOutput[0].Info;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = ExOutput[0].Info;
                    }

                }
                else
                {
                    TempData["ErrorMessage"] = "Error!";
                }

                return RedirectToAction("Master_VehicleFormula", "admin");

            }
            catch
            {
                return BadRequest();
            }
        }


        public IActionResult Master_VehicleFormula_Details(int id)
        {
            SetViewBag();

            // Uncomment this for production
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }

            VwMasterVehicleFormula VehicleFormulaDetail = new VwMasterVehicleFormula();

            HttpClient client = _api.Initial();
            HttpResponseMessage res = client.GetAsync("api/vehicle/GetMasterVehicleFormula_Detail/" + id).Result;

            VehicleFormulaDetail = res.Content.ReadAsAsync<VwMasterVehicleFormula>().Result;
            VehicleFormulaDetail.Action = "VIEW";

            setDropDown_Vehicle(VehicleFormulaDetail);
            return View(VehicleFormulaDetail);
        }
        public IActionResult Master_VehicleFormula_Edit(int id)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }

            VwMasterVehicleFormula VehicleFormulaInfo = new VwMasterVehicleFormula();

            HttpClient client = _api.Initial();
            HttpResponseMessage res = client.GetAsync("api/vehicle/GetMasterVehicleFormula_Detail/" + id).Result;

            VehicleFormulaInfo = res.Content.ReadAsAsync<VwMasterVehicleFormula>().Result;

            setDropDown_Vehicle(VehicleFormulaInfo);
            return View(VehicleFormulaInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update_VehicleFormula(VwMasterVehicleFormula VehicleFormulaInfo)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            HttpClient client = _api.Initial();
            try
            {
                //HttpResponseMessage res = client.PutAsJsonAsync("api/vehicle/PutMasterVehicleFormula/", VehicleFormulaInfo).Result;
                //TempData["SuccessMessage"] = "Data telah diperbaharui";
                //return RedirectToAction("Master_VehicleFormula", "admin");
                List<Execute_SP_Output> test = new List<Execute_SP_Output>();

                HttpResponseMessage res = client.PostAsJsonAsync("api/vehicle/PostVehicleFormula/", VehicleFormulaInfo).Result;


                if (res.IsSuccessStatusCode)
                {
                    List<Execute_SP_Output> ExOutput = res.Content.ReadAsAsync<List<Execute_SP_Output>>().Result;
                    if (ExOutput[0].IsSukses == 1)
                    {
                        TempData["SuccessMessage"] = ExOutput[0].Info;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = ExOutput[0].Info;
                    }
                    
                }
                else
                {
                    TempData["ErrorMessage"] = "Error!";
                }
                    
                return RedirectToAction("Master_VehicleFormula", "admin");
            }
            catch
            {
                return BadRequest();
            }
        }
        #endregion

        #region Fuel
        public IActionResult Master_VehicleFuel()
        {
            SetViewBag();

            //uncomment this lines for production
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            return View("Master_VehicleFuel");

        }
        public async Task<JsonResult> GetVehicleFuelList()
        {
            SetViewBag();
            List<TblFuel> listVehicleFuel = new List<TblFuel>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/Vehicle/GetMasterVehicleFuel");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                listVehicleFuel = JsonConvert.DeserializeObject<List<TblFuel>>(result);
            }
            return Json(listVehicleFuel);
        }
        public IActionResult Create_VehicleFuel()
        {
            SetViewBag();

            //uncomment this lines for production
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            return View("Master_VehicleFuel_Create");
        }

        public IActionResult Save_VehicleFuel(TblFuel VehicleFuelInfo)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            HttpClient client = _api.Initial();

            try
            {

                HttpResponseMessage res = client.PostAsJsonAsync("api/vehicle/postMasterVehicleFuel/", VehicleFuelInfo).Result;
                TempData["SuccessMessage"] = "Data telah disimpan";
                return RedirectToAction("Master_VehicleFuel", "admin");

            }
            catch
            {
                return BadRequest();
            }
        }

        public IActionResult Master_VehicleFuel_Details(int id)
        {
            SetViewBag();

            // Uncomment this for production
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }

            TblFuel VehicleFuelDetail = new TblFuel();

            HttpClient client = _api.Initial();
            HttpResponseMessage res = client.GetAsync("api/vehicle/GetMasterVehicleFuel_Detail/" + id).Result;

            VehicleFuelDetail = res.Content.ReadAsAsync<TblFuel>().Result;


            return View(VehicleFuelDetail);
        }
        public IActionResult Master_VehicleFuel_Edit(int id)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }

            TblFuel VehicleFuelInfo = new TblFuel();

            HttpClient client = _api.Initial();
            HttpResponseMessage res = client.GetAsync("api/vehicle/GetMasterVehicleFuel_Detail/" + id).Result;

            VehicleFuelInfo = res.Content.ReadAsAsync<TblFuel>().Result;

            return View(VehicleFuelInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update_VehicleFuel(TblFuel VehicleFuelInfo)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            HttpClient client = _api.Initial();
            try
            {
                HttpResponseMessage res = client.PutAsJsonAsync("api/vehicle/PutMasterVehicleFuel/", VehicleFuelInfo).Result;
                TempData["SuccessMessage"] = "Data telah diperbaharui";
                return RedirectToAction("Master_VehicleFuel", "admin");
            }
            catch
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Delete_VehicleFuel(int id)
        {
            if (HttpContext.Session.GetString(_api.userName) == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            //SetViewBag();
            HttpClient client = _api.Initial();

            try
            {
                var VehicleFuelInfo = new TblFuel();
                HttpResponseMessage res = await client.DeleteAsync($"api/Vehicle/DeleteMasterVehicleFuel/" + id);
                TempData["SuccessMessage"] = "Data telah dihapus";
                return RedirectToAction("Master_VehicleFuel", "admin");
            }

            catch
            {
                return BadRequest();
            }

        }

        #endregion

        #region Vehicle
        public IActionResult Master_Vehicle()
        {
            SetViewBag();

            //uncomment this lines for production
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            return View("Master_Vehicle");

        }
        public async Task<JsonResult> GetVehicleList()
        {
            SetViewBag();
            List<VwVehicleList> listVehicle = new List<VwVehicleList>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/Vehicle/GetMasterVehicle");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                listVehicle = JsonConvert.DeserializeObject<List<VwVehicleList>>(result);
            }
            return Json(listVehicle);
        }
        public IActionResult Create_Vehicle()
        {
            SetViewBag();

            //uncomment this lines for production
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            return View("Master_Vehicle_Create");
        }
       
        public IActionResult Save_Vehicle(TblVehicle VehicleInfo)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            HttpClient client = _api.Initial();

            try
            {
                string uniqueFileName = UploadedFile(VehicleInfo);

                //Tree modelSave = new Tree
                //{
                //    Img = uniqueFileName,
                //};
                VehicleInfo.Img = uniqueFileName;

                HttpResponseMessage res = client.PostAsJsonAsync("api/vehicle/postMasterVehicle/", VehicleInfo).Result;
                TempData["SuccessMessage"] = "Data telah disimpan";
                return RedirectToAction("Master_Vehicle", "admin");

            }
            catch
            {
                return BadRequest();
            }
        }

        public IActionResult Master_Vehicle_Details(int id)
        {
            SetViewBag();

            // Uncomment this for production
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }

            TblVehicle VehicleDetail = new TblVehicle();

            HttpClient client = _api.Initial();
            HttpResponseMessage res = client.GetAsync("api/vehicle/GetMasterVehicle_Detail/" + id).Result;

            VehicleDetail = res.Content.ReadAsAsync<TblVehicle>().Result;


            return View(VehicleDetail);
        }
        public IActionResult Master_Vehicle_Edit(int id)
        {
            SetViewBag();
            //if (_api.userName == null || ViewBag.UserId == 0)
            //{
            //    return RedirectToAction("UserLogin", "User");
            //}

            TblVehicle VehicleInfo = new TblVehicle();

            HttpClient client = _api.Initial();
            HttpResponseMessage res = client.GetAsync("api/vehicle/GetMasterVehicle_Detail/" + id).Result;

            VehicleInfo = res.Content.ReadAsAsync<TblVehicle>().Result;



            if (string.IsNullOrEmpty(VehicleInfo.Img))
            {
                VehicleInfo.Img = "";
            }
            else
            {
                //string filePath = Path.Combine(webHostEnvironment.WebRootPath, "assets\\Tree\\" + treeInfo[0].Img);
                //byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                //IFormFile formFile = new FileBytesToIFormFile(fileBytes, treeInfo[0].Img);
                //treeInfo[0].TreeImage = formFile;
            }


            string[] words = VehicleInfo.Img.Split("_");

            int loop = 1;
            int countSplit = words.Count();
            string RealName = "";
            while (countSplit > 1)
            {
                countSplit--;
                RealName = RealName + words[loop].ToString();
                loop++;

                if (countSplit > 1)
                    RealName = RealName + "_";

            }
            if (RealName == "")
                RealName = VehicleInfo.Img;

            ViewBag.img = RealName;///*"/assets/Tree/" +*/ treeInfo[0].Img;

            //treeInfo[0].TreeImage.ContentType = "image/jpeg";

            return View(VehicleInfo);
        }
        

        private string UploadedFile(TblVehicle model)
        {
            string uniqueFileName = null;

            if (model.VehicleImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "assets\\Vehicle");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.VehicleImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.VehicleImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update_Vehicle(TblVehicle VehicleInfo)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            HttpClient client = _api.Initial();
            try
            {
                string uniqueFileName = UploadedFile(VehicleInfo);

                if(string.IsNullOrEmpty(uniqueFileName) &&  !string.IsNullOrEmpty(VehicleInfo.Img))
                {
                    VehicleInfo.Img = VehicleInfo.Img;
                }
                else
                {
                    VehicleInfo.Img = uniqueFileName;
                }
               

                HttpResponseMessage res = client.PutAsJsonAsync("api/vehicle/PutMasterVehicle/" , VehicleInfo).Result;
                TempData["SuccessMessage"] = "Data telah diperbaharui";
                return RedirectToAction("Master_Vehicle", "admin");
            }
            catch
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> Delete_Vehicle(int id)
        {
            if (HttpContext.Session.GetString(_api.userName) == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            //SetViewBag();
            HttpClient client = _api.Initial();

            try
            {
                //Cek apakah vehicle sudah ada formulanya?
                 List<VwMasterVehicleFormula> listVehicleFormula = new List<VwMasterVehicleFormula>();
                HttpResponseMessage resMasterFormula = await client.GetAsync($"api/Vehicle/GetMasterVehicleFormula");

                if (resMasterFormula.IsSuccessStatusCode)
                {
                    var result = resMasterFormula.Content.ReadAsStringAsync().Result;
                    listVehicleFormula = JsonConvert.DeserializeObject<List<VwMasterVehicleFormula>>(result).Where(a => a.VehicleId == id).ToList();
                }

                if(listVehicleFormula.Count() == 0)
                {
                    HttpResponseMessage res = await client.DeleteAsync($"api/Vehicle/{id}");
                    TempData["SuccessMessage"] = "Data telah dihapus!";
                    return RedirectToAction("Master_Vehicle", "admin");
                }
                else
                {
                    TempData["ErrorMessage"] = "Tidak bisa hapus data!";
                    return RedirectToAction("Master_Vehicle", "admin");
                }
                
            }

            catch
            {
                return BadRequest();
            }

        }

        //public  async Task Delete_Vehicle1(int id)
        //{
        //    //SetViewBag();
        //    using (var client = new HttpClient())
        //    {
        //        var VehicleInfo = new TblVehicle();
        //        HttpResponseMessage res = await  client.DeleteAsync($"http://localhost:51974/api/Vehicle/{id}");
        //        TempData["SuccessMessage"] = "Data telah dihapus";
        //        res.EnsureSuccessStatusCode();
        //    }
        //}

        #endregion

        #region Tree
        public IActionResult Master_Tree()
        {
            SetViewBag();

            //uncomment this lines for production
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }

            return View("Master_Tree");

        }

        public async Task<JsonResult> GetTreeList()
        {
            SetViewBag();
            List<Tree> listTree = new List<Tree>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/carbonabsorbtion/GetTreeList");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                listTree = JsonConvert.DeserializeObject<List<Tree>>(result);
            }
            return Json(listTree);
        }
        public IActionResult Create_Tree()
        {
            SetViewBag();

            //uncomment this lines for production
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            return View("Create_Tree");
        }

        private string UploadedFile(Tree model)
        {
            string uniqueFileName = null;

            if (model.TreeImage != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "assets\\Tree");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.TreeImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.TreeImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        public IActionResult Save_Tree(Tree treeInfo)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            HttpClient client = _api.Initial();

            try
            {
                string uniqueFileName = UploadedFile(treeInfo);

                //Tree modelSave = new Tree
                //{
                //    Img = uniqueFileName,
                //};
                treeInfo.Img = uniqueFileName;

                HttpResponseMessage res = client.PostAsJsonAsync("api/admin/", treeInfo).Result;
                TempData["SuccessMessage"] = "Data telah disimpan";
                return RedirectToAction("Master_Tree", "admin");

            }
            catch
            {
                return BadRequest();
            }
        }
        public IActionResult Master_Tree_Edit(int id)
        {
            SetViewBag();
            //if (_api.userName == null || ViewBag.UserId == 0)
            //{
            //    return RedirectToAction("UserLogin", "User");
            //}

            SecureLinkTagHelper testlink = new SecureLinkTagHelper();
            testlink.routekey1 = "userID";
            testlink.routeValues1 = "1";

            List<Tree> treeInfo = new List<Tree>();

            HttpClient client = _api.Initial();
            HttpResponseMessage res = client.GetAsync("api/carbonabsorbtion/GetTreeInfo/" + id).Result;

            treeInfo = res.Content.ReadAsAsync<List<Tree>>().Result;

            var treeId = treeInfo[0].TreeId;
            var treeName = treeInfo[0].TreeName;
            var treeCarb = treeInfo[0].CarbonAbs;

            if (string.IsNullOrEmpty(treeInfo[0].Img))
            {
                treeInfo[0].Img = "";
            }
            else
            {
                //string filePath = Path.Combine(webHostEnvironment.WebRootPath, "assets\\Tree\\" + treeInfo[0].Img);
                //byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

                //IFormFile formFile = new FileBytesToIFormFile(fileBytes, treeInfo[0].Img);
                //treeInfo[0].TreeImage = formFile;
            }

            ViewBag.treeId = treeId;
            ViewBag.treeName = treeName;
            ViewBag.treeCarb = treeCarb;

            string[] words = treeInfo[0].Img.Split("_");

            int loop = 1;
            int countSplit = words.Count();
            string RealName = "";
            while (countSplit > 1)
            {
                countSplit--;
                RealName = RealName + words[loop].ToString();
                loop++;

                if (countSplit > 1)
                    RealName = RealName + "_";

            }

            ViewBag.img = RealName;///*"/assets/Tree/" +*/ treeInfo[0].Img;

            //treeInfo[0].TreeImage.ContentType = "image/jpeg";

            return View(treeInfo[0]);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update_Tree(Tree treeInfo)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            HttpClient client = _api.Initial();
            try
            {
                string uniqueFileName = UploadedFile(treeInfo);

                treeInfo.Img = uniqueFileName;

                HttpResponseMessage res = client.PutAsJsonAsync("api/admin/" + treeInfo.TreeId, treeInfo).Result;
                TempData["SuccessMessage"] = "Data telah diperbaharui";
                return RedirectToAction("Master_Tree", "admin");
            }
            catch
            {
                return BadRequest();
            }
        }
       

        public async Task<IActionResult> Delete_Tree(int id)
        {
            if (HttpContext.Session.GetString(_api.userName) == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            //SetViewBag();
            HttpClient client = _api.Initial();

            try
            {
                var treeInfo = new Tree();
                HttpResponseMessage res = await client.DeleteAsync($"api/admin/{id}");
                TempData["SuccessMessage"] = "Data telah dihapus";
                return RedirectToAction("Master_Tree", "admin");
            }

            catch
            {
                return BadRequest();
            }

        }
        #endregion

        public void setDropDown_Vehicle(VwMasterVehicleFormula _VwMasterVehicleFormula)
        {
            HttpClient client = _api.Initial();

            //Transportation
            HttpResponseMessage resListTransport = client.GetAsync("api/vehicle/GetListTransportation").Result;
            List<TblTransportation> _ListTransport = new List<TblTransportation>();
            _ListTransport = resListTransport.Content.ReadAsAsync<List<TblTransportation>>().Result;

            IEnumerable<SelectListItem> ListTransport = from value in _ListTransport
                                                        select new SelectListItem
                                                        {
                                                            Text = value.TransportationName.ToString(),
                                                            Value = value.TransportationId.ToString()
                                                            //,Selected = vwVehicleList_User_Action.TransportationId.ToString()
                                                        };

            //Fuel
            HttpResponseMessage resListFuel = client.GetAsync("api/vehicle/GetMasterVehicleFuel").Result;
            List<TblFuel> _ListFuel = new List<TblFuel>();
            _ListFuel = resListFuel.Content.ReadAsAsync<List<TblFuel>>().Result;

            //31 jan 2023
            //if (_VwMasterVehicleFormula.FuelId == 1007)
            //{
            //    _ListFuel = _ListFuel.Where(a => a.FuelId != 6).ToList();
            //}
            //else if (_VwMasterVehicleFormula.FuelId == 6)
            //{
            //    _ListFuel = _ListFuel.Where(a => a.FuelId != 1007).ToList();
            //}
            //else
            //{
            //    _ListFuel = _ListFuel.Where(a => a.FuelId != 1007 && a.FuelId != 6).ToList();
            //}
            //else if(vwVehicleList_User_Action.FuelId != 1007)
            //{
            //    _ListFuel = _ListFuel.Where(a => a.FuelId != 1007).ToList();
            //}


            IEnumerable<SelectListItem> ListFuel = from value in _ListFuel
                                                   select new SelectListItem
                                                   {
                                                       Text = value.FuelName.ToString(),
                                                       Value = value.FuelId.ToString()
                                                       //,Selected = value == vwVehicleList_User_Action.FuelId.ToString()
                                                   };


            //Vehicle
            List<TblVehicle> _ListVehicle = new List<TblVehicle>();
            if (_VwMasterVehicleFormula.Action == "CREATE")
            {
                HttpResponseMessage resListVehicle = client.GetAsync("api/vehicle/GetVehicleList_AddFormula").Result;
                _ListVehicle = resListVehicle.Content.ReadAsAsync<List<TblVehicle>>().Result;
            }
            else 
            {
                HttpResponseMessage resListVehicle = client.GetAsync("api/vehicle/GetListCBOVehicle").Result;
                _ListVehicle = resListVehicle.Content.ReadAsAsync<List<TblVehicle>>().Result;
            }

            //List<SelectListItem> ListTransport = new List<SelectListItem>()
            //{
            //    new SelectListItem { Value = "1", Text = "Umum" },
            //    new SelectListItem { Value = "2", Text = "Pribadi" }
            //};

            // ViewBag.Transporttasi = ListTransport;
            //ViewBag.Fuel = ListFuel;
            //ViewBag.VType = ListVType;
            //ViewBag.VCapacity = ListVCapacity;

            if (_VwMasterVehicleFormula.TransportGroupid == null || _VwMasterVehicleFormula.TransportGroupid == 0)
                ViewBag.Transporttasi = new SelectList(_ListTransport, "TransportationId", "TransportationName");
            else
                ViewBag.Transporttasi = new SelectList(_ListTransport.ToArray(), "TransportationId", "TransportationName", _VwMasterVehicleFormula.TransportGroupid);

            if (_VwMasterVehicleFormula.FuelId == null || _VwMasterVehicleFormula.FuelId == 0)
                ViewBag.Fuel = new SelectList(_ListFuel, "FuelId", "FuelName");
            else
                ViewBag.Fuel = new SelectList(_ListFuel.ToArray(), "FuelId", "FuelName", _VwMasterVehicleFormula.FuelId);


            if (_VwMasterVehicleFormula.VehicleId == null || _VwMasterVehicleFormula.VehicleId == 0)
                ViewBag.Vehicle = new SelectList(_ListVehicle, "VehicleId", "VehicleName");
            else
                ViewBag.Vehicle = new SelectList(_ListVehicle.ToArray(), "VehicleId", "VehicleName", _VwMasterVehicleFormula.VehicleId);

        }
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

            //Transportation
            HttpResponseMessage resListTransport = client.GetAsync("api/vehicle/GetListTransportation").Result;
            List<TblTransportation> _ListTransport = new List<TblTransportation>();
            _ListTransport = resListTransport.Content.ReadAsAsync<List<TblTransportation>>().Result;

            IEnumerable<SelectListItem> ListTransport = from value in _ListTransport
                                                        select new SelectListItem
                                                        {
                                                            Text = value.TransportationName.ToString(),
                                                            Value = value.TransportationId.ToString()
                                                            //,Selected = vwVehicleList_User_Action.TransportationId.ToString()
                                                        };

            if (_TblTUser.Zona == null)
                ViewBag.Field = new SelectList(_ListTransport, "Field", "Field");
            else
                ViewBag.Field = new SelectList(_ListTransport.ToArray(), "Field", "Field", _TblTUser.Field);

            //Field
            HttpResponseMessage resListField = client.GetAsync("api/User/GetListField").Result;
            List<TblMasterField> _ListField = new List<TblMasterField>();
            _ListField = resListField.Content.ReadAsAsync<List<TblMasterField>>().Result;

            IEnumerable<SelectListItem> ListField = from value in _ListField
                                                    select new SelectListItem
                                                    {
                                                        Text = value.Field.ToString(),
                                                        Value = value.Field.ToString()
                                                        //,Selected = value == vwVehicleList_User_Action.FuelId.ToString()
                                                    };

            if (_TblTUser.Zona == null)
                ViewBag.Field = new SelectList(_ListField, "Field", "Field");
            else
                ViewBag.Field = new SelectList(_ListField.ToArray(), "Field", "Field", _TblTUser.Field);



     }

    }
}
