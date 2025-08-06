using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebSystem.Function;
using WebSystem.Helper;
using WebSystem.Models;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebSystem.Controllers
{
    public class VehicleController : Controller
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();
        HelperAPI _api = new HelperAPI();
        //const string userId = "_UserId";
        //const string userName = "_UserName";
        //const string displayname = "_DisplayName";
        //const string periodsession = "_Periode";
        //const string periodeIdsession = "_PeriodeId";
        //const string yearValuesession = "_YearValue";
        //const string MonthValuesession = "_MonthValue";
        // GET: VehicleController
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


        public IActionResult Index()
        {
            SetViewBag();
            //_api.UserName = null;

            if (_api.UserName == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            return View();
        }

        /// <summary>
        /// Mengambil daftar data kendaraan yang ada di sistem.
        /// </summary> 
        [HttpGet]
        [Route("GetListVehicle")]
        public async Task<JsonResult> GetListVehicle()
        {
            SetViewBag();
            List<VwVehicleList> VehicleEmision = new List<VwVehicleList>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/vehicle/GetListVehicle/");
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                VehicleEmision = JsonConvert.DeserializeObject<List<VwVehicleList>>(result);
            }
            return Json(VehicleEmision);
        }

        /// <summary>
        /// Mengambil daftar pengguna yang memiliki kendaraan di sistem.
        /// </summary> 
        [HttpGet]
        [Route("GetListUser")]
        public async Task<JsonResult> GetListUser()
        {
            SetViewBag();
            //int userid = 1221;

            List<VwVehicleList_UserSum> VehicleEmision = new List<VwVehicleList_UserSum>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/vehicle/GetListUser/" + ViewBag.UserId + "/" + ViewBag.periodId);
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                VehicleEmision = JsonConvert.DeserializeObject<List<VwVehicleList_UserSum>>(result);
            }
            Decimal SumBulan = VehicleEmision.Sum(x => Convert.ToInt32(x.BBMBulanLiter));
            Decimal SumTahun = VehicleEmision.Sum(x => Convert.ToInt32(x.BBMOrangTahunLiter));

            ViewBag.SubTotalBulan = SumBulan;
            ViewBag.SubTotalTahun = SumTahun;
            //ViewBag.periodedetail = "TEST";

            return Json(VehicleEmision);
        }

        /// <summary>
        /// Mengambil data galeri kendaraan.
        /// </summary> 
        [HttpGet]
        public IActionResult ListVehicle()
        {
            SetViewBag();
            if (_api.UserName == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            return View();
            //HttpClient client = _api.Initial();

            //HttpResponseMessage res = client.GetAsync("api/vehicle/GetListVehicle").Result;
            //return View(res.Content.ReadAsAsync<VwVehicleList>().Result);
            

        }

        /// <summary>
        /// Mengambil data galeri kendaraan.
        /// </summary> 
        [HttpGet]
        [Route("GalleryVehicle")]
        public IActionResult GalleryVehicle()
        {
            SetViewBag();
            if (_api.UserName == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            HttpClient client = _api.Initial();

            HttpResponseMessage res = client.GetAsync("api/vehicle/GetListVehicle/").Result;
            return View(res.Content.ReadAsAsync<List<VwVehicleList>>().Result);
            
            //return View();
            //HttpClient client = _api.Initial();

            //HttpResponseMessage res = client.GetAsync("api/vehicle/GetListVehicle").Result;
            //return View(res.Content.ReadAsAsync<VwVehicleList>().Result);


        }
        [HttpGet]
        public IActionResult Create(int id = 0)
        {
            //HttpClient client = _api.Initial();
            //VwVehicleList_User_Action VehicleEmision = new VwVehicleList_User_Action();
            //var res = this.CreateProcess(id).Result;
            SetViewBag();
            if (_api.UserName == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            HttpClient client = _api.Initial();

            if (id == 0)
                return View(new VwVehicleList_User_Action());
            {

                HttpResponseMessage res = client.GetAsync("api/vehicle/GetDetailVehicle/" + id).Result;
                var SelectedData = res.Content.ReadAsAsync<VwVehicleList_User_Action>().Result;

                setDropDown(SelectedData);
                //if(SelectedData.VehicleId == 62)
                //{
                //    SelectedData.AmountPeople = 1000;
                //}
                //else if(SelectedData.VehicleId == 1059)
                //{
                //    SelectedData.AmountPeople = 1500;
                //}
                //else
                //{
                //    SelectedData.AmountPeople = 0;
                //}

                //SelectedData.Mileage = 1;
                //SelectedData.TravelFrequency = 1;
                return View(SelectedData);
            }

            //return View();
        }

        //[HttpGet]
        //[Route("CreateError")]
        //public IActionResult Create(FNGetVehicleEmision_Calc _fNGetVehicleEmision_Calc)
        //{
        //    //HttpClient client = _api.Initial();
        //    //VwVehicleList_User_Action VehicleEmision = new VwVehicleList_User_Action();
        //    //var res = this.CreateProcess(id).Result;
        //    SetViewBag();
        //    if (_api.UserName == null)
        //    {
        //        return RedirectToAction("UserLogin", "User");
        //    }

        //    HttpClient client = _api.Initial();

        //    if (_fNGetVehicleEmision_Calc.VehicleId == 0)
        //        return View(new VwVehicleList_User_Action());
        //    {

        //        HttpResponseMessage res = client.GetAsync("api/vehicle/GetDetailVehicle/" + _fNGetVehicleEmision_Calc.VehicleId).Result;
        //        var SelectedData = res.Content.ReadAsAsync<VwVehicleList_User_Action>().Result;


        //        SelectedData.AmountPeople = 1;
        //        SelectedData.Mileage = _fNGetVehicleEmision_Calc.Mileage;
        //        SelectedData.TravelFrequency = _fNGetVehicleEmision_Calc.TravelFrequency;

        //        setDropDown(SelectedData);
        //        return View(SelectedData);
        //    }

        //    //return View();
        //}

        public void setDropDown(VwVehicleList_User_Action vwVehicleList_User_Action)
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
            HttpResponseMessage resListFuel = client.GetAsync("api/vehicle/GetListFuel/"+ vwVehicleList_User_Action.VehicleId).Result;
            List<TblFuel> _ListFuel = new List<TblFuel>();
            _ListFuel = resListFuel.Content.ReadAsAsync<List<TblFuel>>().Result;

            //31 jan 2023
            if(vwVehicleList_User_Action.FuelId == 1007)
            {
                _ListFuel = _ListFuel.Where(a => a.FuelId != 6).ToList();
            }
            else if (vwVehicleList_User_Action.FuelId == 6)
            {
                _ListFuel = _ListFuel.Where(a => a.FuelId != 1007).ToList();
            }
            else
            {
                _ListFuel = _ListFuel.Where(a => a.FuelId != 1007 && a.FuelId != 6).ToList();
            }    
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
            //VehicleType
            HttpResponseMessage resListVType = client.GetAsync("api/vehicle/GetListVType").Result;
            List<TblVehicleType> _ListVType = new List<TblVehicleType>();
            _ListVType = resListVType.Content.ReadAsAsync<List<TblVehicleType>>().Result;

            IEnumerable<SelectListItem> ListVType = from value in _ListVType
                                                    select new SelectListItem
                                                    {
                                                        Text = value.VehicleName.ToString(),
                                                        Value = value.Id.ToString()
                                                    };


            //cc
            HttpResponseMessage resListVCapacity = client.GetAsync("api/vehicle/GetListVCapacity").Result;
            List<TblVehicleCapacity> _ListVCapacity = new List<TblVehicleCapacity>();
            _ListVCapacity = resListVCapacity.Content.ReadAsAsync<List<TblVehicleCapacity>>().Result;

            IEnumerable<SelectListItem> ListVCapacity = from value in _ListVCapacity.Distinct()
                                                        select new SelectListItem
                                                        {
                                                            Text = value.CubicalCentimeter.ToString(),
                                                            Value = value.CapacityId.ToString()
                                                        };

            //Vehicle
            HttpResponseMessage resListVehicle = client.GetAsync("api/vehicle/GetListCBOVehicle").Result;
            List<TblVehicle> _ListVehicle = new List<TblVehicle>();
            _ListVehicle = resListVehicle.Content.ReadAsAsync<List<TblVehicle>>().Result;

            //List<SelectListItem> ListTransport = new List<SelectListItem>()
            //{
            //    new SelectListItem { Value = "1", Text = "Umum" },
            //    new SelectListItem { Value = "2", Text = "Pribadi" }
            //};

           // ViewBag.Transporttasi = ListTransport;
            //ViewBag.Fuel = ListFuel;
            //ViewBag.VType = ListVType;
            //ViewBag.VCapacity = ListVCapacity;

            if (vwVehicleList_User_Action.TransportationId == null)
                ViewBag.Transporttasi = new SelectList(_ListTransport, "TransportationId", "TransportationName");
            else
                ViewBag.Transporttasi = new SelectList(_ListTransport.ToArray(), "TransportationId", "TransportationName", vwVehicleList_User_Action.TransportationId);
            
            if (vwVehicleList_User_Action.FuelId == null)
                ViewBag.Fuel = new SelectList(_ListFuel, "FuelId", "FuelName");
            else
                ViewBag.Fuel = new SelectList(_ListFuel.ToArray(), "FuelId", "FuelName", vwVehicleList_User_Action.FuelId);
            
            if (vwVehicleList_User_Action.CapacityId == null)
                ViewBag.VCapacity = new SelectList(_ListVCapacity, "CapacityId", "CubicalCentimeter");
            else
                ViewBag.VCapacity = new SelectList(_ListVCapacity.ToArray(), "CapacityId", "CubicalCentimeter", vwVehicleList_User_Action.CapacityId);


            if (vwVehicleList_User_Action.VehicleId == null)
                ViewBag.Vehicle = new SelectList(_ListVehicle, "VehicleId", "VehicleName");
            else
                ViewBag.Vehicle = new SelectList(_ListVehicle.ToArray(), "VehicleId", "VehicleName", vwVehicleList_User_Action.VehicleId);

        }
        public async Task<JsonResult> CreateProcess(int id = 0)
        {
            VwVehicleList Vehicle = new VwVehicleList();
            VwVehicleList_User_Action VehicleEmision = new VwVehicleList_User_Action();

            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/vehicle/GetDetailVehicle/" + id);
            if (res.IsSuccessStatusCode)
            {

                var result = res.Content.ReadAsStringAsync().Result;
                Vehicle = JsonConvert.DeserializeObject<VwVehicleList>(result);

                VehicleEmision.VehicleId = Vehicle.VehicleId;
                VehicleEmision.VehicleName = Vehicle.VehicleName;
            }
            return Json(VehicleEmision);
            //HttpClient client = _api.Initial();
            //return View(new VwVehicleList_User_Action());
        }

        //POST: HouseholdController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult create(VwVehicleList_User_Action vehicleEmision)
        {
            SetViewBag();
            if (_api.UserName == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            HttpClient client = _api.Initial();
            VwVehicleList_User_Action data = new VwVehicleList_User_Action();
            VwVehicleEmision_SUM_Year dataSumYear = new VwVehicleEmision_SUM_Year();
            VwVehicleEmision_SUM_Month dataSumMonth = new VwVehicleEmision_SUM_Month();

            FNGetVehicleEmision_Calc _fnGetVehicleEmision_Calc = new FNGetVehicleEmision_Calc();
            HttpResponseMessage res = client.GetAsync("api/vehicle/Get_VehicleEmision_Calc/" + vehicleEmision.VehicleId + "/" + vehicleEmision.TransportationId
                                                        + "/" + vehicleEmision.FuelId + "/" + vehicleEmision.CapacityId
                                                        + "/" + vehicleEmision.Mileage + "/" + vehicleEmision.TravelFrequency
                                                        + "/" + vehicleEmision.AmountPeople).Result;
            _fnGetVehicleEmision_Calc = res.Content.ReadAsAsync<FNGetVehicleEmision_Calc>().Result;

            // 31 jan 2023
            if(_fnGetVehicleEmision_Calc.AmountPeople > _fnGetVehicleEmision_Calc.TransportCapacity)
            {
                TempData["ErrorMessage"] = "Jumlah penumpang " + _fnGetVehicleEmision_Calc.VehicleName + " lebih dari maksimal penumpang. Info : Maksimal " + _fnGetVehicleEmision_Calc.VehicleName + " adalah " + _fnGetVehicleEmision_Calc.TransportCapacity ;
                return RedirectToAction("Create", "Vehicle", _fnGetVehicleEmision_Calc.VehicleId);
            }
            
            if(_fnGetVehicleEmision_Calc.VehicleName.ToLower().Contains("pesawat"))
            {
                if(_fnGetVehicleEmision_Calc.Mileage < 400)
                {
                    TempData["ErrorMessage"] = "Jarak " + _fnGetVehicleEmision_Calc.VehicleName + " kurang dari minimal KM. Info : Jarak minimal " + _fnGetVehicleEmision_Calc.VehicleName + " adalah 400KM";
                    return RedirectToAction("Create", "Vehicle", _fnGetVehicleEmision_Calc.VehicleId);
                }
                else if(_fnGetVehicleEmision_Calc.Mileage > 1300)
                {
                    TempData["ErrorMessage"] = "Jarak " + _fnGetVehicleEmision_Calc.VehicleName + " lebih dari maksimal KM. Info : Jarak maksimal " + _fnGetVehicleEmision_Calc.VehicleName + " adalah 1300KM";
                    return RedirectToAction("Create", "Vehicle", _fnGetVehicleEmision_Calc.VehicleId);
                }
            }


            try
            {
                vehicleEmision.VehicleName = _fnGetVehicleEmision_Calc.VehicleName;
                vehicleEmision.TransportationName = _fnGetVehicleEmision_Calc.TransportationName;
                vehicleEmision.FuelName = _fnGetVehicleEmision_Calc.FuelName;
                vehicleEmision.CubicalCentimeter = _fnGetVehicleEmision_Calc.CubicalCentimeter;

                HttpResponseMessage resSumMonth = client.GetAsync("api/vehicle/GetVehicleEmisionSumMonth/" + ViewBag.UserId + "/" + ViewBag.MonthValue + "/" + ViewBag.YearValue).Result;
                dataSumMonth = resSumMonth.Content.ReadAsAsync<VwVehicleEmision_SUM_Month>().Result;
                
                HttpResponseMessage resSumYear = client.GetAsync("api/vehicle/GetVehicleEmisionSumYear/" + ViewBag.UserId + "/" + ViewBag.YearValue).Result;
                dataSumYear = resSumYear.Content.ReadAsAsync<VwVehicleEmision_SUM_Year>().Result;

                decimal Emisibulan, Emisitahun;
                Emisibulan = (dataSumMonth == null ? 0 : dataSumMonth.EmisiBulan);
                Emisitahun = (dataSumYear == null ? 0 : dataSumYear.EmisiTahun);
                //if (dataSumMonth == null)
                //{
                //    Emisibulan = 0;
                //}
                //else
                //{
                //    Emisibulan = dataSumMonth.EmisiBulan;
                //}

                //if (dataSumYear == null)
                //{
                //    Emisitahun = 0;
                //}
                //else
                //{
                //    Emisitahun = dataSumYear.EmisiTahun;
                //}

                //vehicleEmision.EmisionBulan = _fnGetVehicleEmision_Calc.EmisionBulan + Emisibulan;
                //ViewBag.VehicleEmisionYear = dataSumYear.EmisiTahun;

                vehicleEmision.EmisionBulan = _fnGetVehicleEmision_Calc.EmisionBulan;
                ViewBag.VehicleEmisionMonth = _fnGetVehicleEmision_Calc.EmisionBulan + Emisibulan; 
                ViewBag.VehicleEmisionYear =  _fnGetVehicleEmision_Calc.EmisionBulan + Emisitahun;

                ViewBag.JarakTempuh = vehicleEmision.Mileage;
                ViewBag.FrekuensiPerjalanan = vehicleEmision.TravelFrequency;


                return View(vehicleEmision);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(VwVehicleList_User_Action vehicleEmision)
        {
            SetViewBag();
            if (_api.UserName == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            HttpClient client = _api.Initial();
            try
            {
                vehicleEmision.userID = ViewBag.UserID;
                vehicleEmision.PeriodeId = ViewBag.periodId;

                HttpResponseMessage res = client.PostAsJsonAsync("api/vehicle/", vehicleEmision).Result;
                TempData["SuccessMessage"] = "Data telah disimpan";
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return BadRequest();
            }
        }

        //POST: HouseholdController/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public IActionResult Edit(VwVehicleList_User_Action vehicleEmision)
        {
            SetViewBag();
            if (_api.UserName == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            decimal SumEmisibulan = 0;
            decimal SumEmisiTahun = 0;

            HttpClient client = _api.Initial();
            VwVehicleList_User_Action data = new VwVehicleList_User_Action();
            VwVehicleEmision_SUM_Year dataSumYear = new VwVehicleEmision_SUM_Year();
            VwVehicleEmision_SUM_Month dataSumMonth = new VwVehicleEmision_SUM_Month();
            List<VwVehicleList_User> dataVehicleUser = new List<VwVehicleList_User>();

            FNGetVehicleEmision_Calc _fnGetVehicleEmision_Calc = new FNGetVehicleEmision_Calc();
            HttpResponseMessage res = client.GetAsync("api/vehicle/Get_VehicleEmision_Calc/" + vehicleEmision.VehicleId + "/" + vehicleEmision.TransportationId
                                                        + "/" + vehicleEmision.FuelId + "/" + vehicleEmision.CapacityId
                                                        + "/" + vehicleEmision.Mileage + "/" + vehicleEmision.TravelFrequency
                                                        + "/" + vehicleEmision.AmountPeople).Result;
            _fnGetVehicleEmision_Calc = res.Content.ReadAsAsync<FNGetVehicleEmision_Calc>().Result;

            //1 feb 2023
            if (_fnGetVehicleEmision_Calc.AmountPeople > _fnGetVehicleEmision_Calc.TransportCapacity)
            {
                TempData["ErrorMessage"] = "Jumlah penumpang " + _fnGetVehicleEmision_Calc.VehicleName + " lebih dari maksimal penumpang. Info : Maksimal " + _fnGetVehicleEmision_Calc.VehicleName + " adalah " + _fnGetVehicleEmision_Calc.TransportCapacity;
                return RedirectToAction("Edit", "Vehicle", vehicleEmision.VehicleEmisionId);
            }
            
            if (_fnGetVehicleEmision_Calc.VehicleName.ToLower().Contains("pesawat"))
            {
                if (_fnGetVehicleEmision_Calc.Mileage < 400)
                {
                    TempData["ErrorMessage"] = "Jarak " + _fnGetVehicleEmision_Calc.VehicleName + " kurang dari minimal KM. Info : Jarak minimal " + _fnGetVehicleEmision_Calc.VehicleName + " adalah 400KM";
                    return RedirectToAction("Edit", "Vehicle", vehicleEmision.VehicleEmisionId);
                }
                else if (_fnGetVehicleEmision_Calc.Mileage > 1300)
                {
                    TempData["ErrorMessage"] = "Jarak " + _fnGetVehicleEmision_Calc.VehicleName + " lebih dari maksimal KM. Info : Jarak maksimal " + _fnGetVehicleEmision_Calc.VehicleName + " adalah 1300KM";
                    return RedirectToAction("Edit", "Vehicle", vehicleEmision.VehicleEmisionId);
                }
            }

            try
            {

                //var tes = households.Where(a => a.HouseholdId != household.HouseholdId).ToList();
                //konsumsilistrik = tes.Sum(x => Convert.ToDecimal(x.KonsumsiListrik())) + household.KonsumsiListrik();

                vehicleEmision.VehicleName = _fnGetVehicleEmision_Calc.VehicleName;
                vehicleEmision.TransportationName = _fnGetVehicleEmision_Calc.TransportationName;
                vehicleEmision.FuelName = _fnGetVehicleEmision_Calc.FuelName;
                vehicleEmision.CubicalCentimeter = _fnGetVehicleEmision_Calc.CubicalCentimeter;

                //HttpResponseMessage resSumMonth = client.GetAsync("api/vehicle/GetVehicleEmisionSumMonth/" + HelperStatic.UserID + "/" + HelperStatic.MonthValue + "/" + HelperStatic.YearValue).Result;
                //dataSumMonth = resSumMonth.Content.ReadAsAsync<VwVehicleEmision_SUM_Month>().Result;

                //HttpResponseMessage resSumYear = client.GetAsync("api/vehicle/GetVehicleEmisionSumYear/" + HelperStatic.UserID + "/" + HelperStatic.YearValue).Result;
                //dataSumYear = resSumYear.Content.ReadAsAsync<VwVehicleEmision_SUM_Year>().Result;

                //vehicleEmision.EmisionBulan = _fnGetVehicleEmision_Calc.EmisionBulan + dataSumMonth.EmisiBulan; 
                ////ViewBag.VehicleEmisionYear = dataSumYear.EmisiTahun;

                ////ViewBag.EmisiPerBulan = _fnGetVehicleEmision_Calc.EmisionBulan + dataSumMonth.EmisiBulan;
                //ViewBag.VehicleEmisionYear = dataSumYear.EmisiTahun + _fnGetVehicleEmision_Calc.EmisionBulan;

                HttpResponseMessage resSum =  client.GetAsync("api/vehicle/GetListUserNotSUM/" + ViewBag.UserID).Result;
                dataVehicleUser = JsonConvert.DeserializeObject<List<VwVehicleList_User>>(resSum.Content.ReadAsStringAsync().Result);

                //Get FilterDataEditMonth
                var FilterDataEditMonth = dataVehicleUser.Where(a => a.PeriodeId == ViewBag.periodId && a.VehicleEmisionId != vehicleEmision.VehicleEmisionId).ToList();
                SumEmisibulan = FilterDataEditMonth.Sum(x => x.EmisionBulan);

                //Get FilterDataEditYear
                var FilterDataEditYear = dataVehicleUser.Where(a => a.TahunPeriode == ViewBag.YearValue && a.VehicleEmisionId != vehicleEmision.VehicleEmisionId).ToList();
                SumEmisiTahun = FilterDataEditYear.Sum(x => x.EmisionBulan);

                vehicleEmision.EmisionBulan = _fnGetVehicleEmision_Calc.EmisionBulan;
                ViewBag.VehicleEmisionMonth = _fnGetVehicleEmision_Calc.EmisionBulan + SumEmisibulan;
                ViewBag.VehicleEmisionYear = _fnGetVehicleEmision_Calc.EmisionBulan + SumEmisiTahun;

                ViewBag.JarakTempuh = vehicleEmision.Mileage;
                ViewBag.FrekuensiPerjalanan = vehicleEmision.TravelFrequency;


                return View(vehicleEmision);
            }
            catch
            {
                return BadRequest();
            }
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public IActionResult UpdateSave(VwVehicleList_User_Action vehicleEmision)
        {
            SetViewBag();
            if (_api.UserName == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            HttpClient client = _api.Initial();
            try
            {
                vehicleEmision.action = "UPDATE";
                vehicleEmision.userID = ViewBag.UserID;
                vehicleEmision.PeriodeId = ViewBag.periodId;

                HttpResponseMessage res = client.PutAsJsonAsync("api/vehicle/", vehicleEmision).Result;
                TempData["SuccessMessage"] = "Data telah diperbaharui";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return BadRequest();
            }
        }

        public IActionResult Edit(int id = 0)
        {
            SetViewBag();
            if (_api.UserName == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            HttpClient client = _api.Initial();
            VwVehicleList_User_Action data = new VwVehicleList_User_Action();

            if (id == 0)
                return View(new VwVehicleList_User_Action());
            {
                HttpResponseMessage res = client.GetAsync("api/vehicle/" + id.ToString()).Result;

                data = res.Content.ReadAsAsync<VwVehicleList_User_Action>().Result;

                setDropDown(data);
                return View(data);
            }

        }

        public IActionResult Details(int id = 0)
        {
            SetViewBag();
            if (_api.UserName == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            HttpClient client = _api.Initial();
            VwVehicleList_User_Action data = new VwVehicleList_User_Action();
            VwVehicleEmision_SUM_Year dataSumYear = new VwVehicleEmision_SUM_Year();
            VwVehicleEmision_SUM_Month dataSumMonth = new VwVehicleEmision_SUM_Month();

            if (id == 0)
                return View(new VwVehicleList_User_Action());
            {
                HttpResponseMessage res = client.GetAsync("api/vehicle/" + id.ToString()).Result;
                data = res.Content.ReadAsAsync<VwVehicleList_User_Action>().Result;

                HttpResponseMessage resSumMonth = client.GetAsync("api/vehicle/GetVehicleEmisionSumMonth/" + ViewBag.UserID + "/" + ViewBag.MonthValue + "/" + ViewBag.YearValue).Result;
                dataSumMonth = resSumMonth.Content.ReadAsAsync<VwVehicleEmision_SUM_Month>().Result;

                HttpResponseMessage resSumYear = client.GetAsync("api/vehicle/GetVehicleEmisionSumYear/" + ViewBag.UserID + "/" + ViewBag.YearValue).Result;
                dataSumYear = resSumYear.Content.ReadAsAsync<VwVehicleEmision_SUM_Year>().Result;

                ViewBag.VehicleEmisionMonth = dataSumMonth.EmisiBulan;
                ViewBag.VehicleEmisionYear = dataSumYear.EmisiTahun;

                return View(data);
            }

        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        ////[Authorize]
        //public IActionResult Edit(Vehicle vehicle, int id)
        //{
        //    SetViewBag();
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res1 = client.GetAsync("api/vehicle/1221" + id.ToString()).Result;
        //    vehicle.VehicleId = id;
        //    try
        //    {
        //        HttpResponseMessage res = client.PutAsJsonAsync("api/vehicle/" + vehicle.VehicleId, vehicle).Result;
        //        TempData["SuccessMessage"] = "Data telah diperbaharui";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }

        //}

        // GET: VehicleController/Delete/5


        //[Route("PutMasterVehicle")]
        public async Task<IActionResult> Delete(int id)
        {
            SetViewBag();
            if (_api.UserName == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            //HttpClient client = _api.Initial();
            //var vehicle = new Vehicle();
            //HttpResponseMessage res = await client.DeleteAsync($"api/vehicle/{id}");
            //TempData["SuccessMessage"] = "Data telah dihapus";
            //return RedirectToAction("Index");
            HttpClient client = _api.Initial();
            VwVehicleList_User_Action vehicleEmision = new VwVehicleList_User_Action();

            vehicleEmision.action = "DELETE";
            vehicleEmision.VehicleEmisionId = id;
            vehicleEmision.VehicleEmisionId = id;
            vehicleEmision.userID = ViewBag.UserID;
            vehicleEmision.PeriodeId = ViewBag.periodId;
            try
            {
                HttpResponseMessage res = client.PostAsJsonAsync("api/vehicle/", vehicleEmision).Result;
                //HttpResponseMessage res = await client.DeleteAsync($"api/vehicle/{id}");
                TempData["SuccessMessage"] = "Data telah dihapus";
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return BadRequest();
            }
        }

    }

}
