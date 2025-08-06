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


namespace WebSystem.Controllers
{
    public class HouseholdController : Controller
    {
        HelperAPI _api = new HelperAPI();

        //const string userId = "_UserId";
        //const string userName = "_UserName";
        //const string displayname = "_DisplayName";
        //const string periodsession = "_Periode";
        //const string periodeIdsession = "_PeriodeId";
        //public const string yearValuesession = "_YearValue";
        //public const string MonthValuesession = "_MonthValue";

        //public HelperStatic _staticValue;
        // GET: HouseholdController

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

        public IActionResult Index(Periode periodes)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }

            try
            {
                //string userid = HelperStatic.UserID.ToString();
                VwPeriode vwPeriode = new VwPeriode();
                HttpClient client = _api.Initial();
                //HttpResponseMessage res = client.GetAsync("api/household");

                HttpResponseMessage res = null;
                //Cek apakah sudah memilih periode?
                if (string.IsNullOrEmpty(periodes.PeriodeDetail) && string.IsNullOrEmpty(_api.PeriodeStatic))
                {
                    TempData["ErrorMessage"] = " Anda belum memilih periode !";
                    return RedirectToAction("Index", "Periode");
                }
                else
                {
                     res = client.GetAsync("api/household/GetPeriod/" + ViewBag.UserId + "/" + periodes.PeriodeDetail).Result;
                }

                if (res.IsSuccessStatusCode)
                {
                    //var result = res.Content.ReadAsStringAsync().Result;
                    //_staticValue.SetPeriodeDetail(periodes.PeriodeDetail);
                    vwPeriode = res.Content.ReadAsAsync<VwPeriode>().Result;
                    if (vwPeriode == null)
                    {
                        TempData["ErrorMessage"] = " Anda tidak bisa input di periode yang dimana anda belum terdaftar !";
                        return RedirectToAction("Index", "Periode");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(periodes.PeriodeDetail))
                        {
                            
                            HttpContext.Session.SetString(_api.periodeIdsession, vwPeriode.PeriodeId.ToString());
                            HttpContext.Session.SetString(_api.periodsession, periodes.PeriodeDetail.ToString());
                            HttpContext.Session.SetString(_api.MonthValuesession, vwPeriode.MonthValue.ToString());
                            HttpContext.Session.SetString(_api.yearValuesession, vwPeriode.YearValue.ToString());
                            //SetViewBag();
                        }
                        else if (string.IsNullOrEmpty(_api.PeriodeID.ToString()))
                        {
                            //HelperStatic.SetPeriodeDetail(periodes.PeriodeDetail, vwPeriode);

                            HttpContext.Session.SetString(_api.periodeIdsession, vwPeriode.PeriodeId.ToString());
                            HttpContext.Session.SetString(_api.periodsession, periodes.PeriodeDetail.ToString());
                            HttpContext.Session.SetString(_api.MonthValuesession, vwPeriode.MonthValue.ToString());
                            HttpContext.Session.SetString(_api.yearValuesession, vwPeriode.YearValue.ToString());
                            // SetViewBag();
                        }
                        else if (periodes.PeriodeDetail != _api.PeriodeID.ToString())
                        {
                            //HelperStatic.SetPeriodeDetail(periodes.PeriodeDetail, vwPeriode);

                            HttpContext.Session.SetString(_api.periodeIdsession, vwPeriode.PeriodeId.ToString());
                            HttpContext.Session.SetString(_api.periodsession, periodes.PeriodeDetail.ToString());
                            HttpContext.Session.SetString(_api.MonthValuesession, vwPeriode.MonthValue.ToString());
                            HttpContext.Session.SetString(_api.yearValuesession, vwPeriode.YearValue.ToString());

                            // SetViewBag();
                        }
                        else
                        {

                            HttpContext.Session.SetString(_api.periodeIdsession, vwPeriode.PeriodeId.ToString());
                            HttpContext.Session.SetString(_api.periodsession, periodes.PeriodeDetail.ToString());
                            HttpContext.Session.SetString(_api.MonthValuesession, vwPeriode.MonthValue.ToString());
                            HttpContext.Session.SetString(_api.yearValuesession, vwPeriode.YearValue.ToString());
                            //SetViewBag();
                        }
                        SetViewBag();
                    }
                    //HttpContext.Session.SetString(userId, vmtest[0].UserId.ToString());
                    //HttpContext.Session.SetString(userName, (string)ViewBag.UserName);
                    //HttpContext.Session.SetString(displayname, (string)ViewBag.Displayname);
                    return View();
                }
                else
                {
                    SetViewBag();
                    return View();

                }
            }
            catch (Exception ex)
            {
                return View();
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetList()
        {
            SetViewBag();
            List<Household> households = new List<Household>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/household/GetHouseHold/" + ViewBag.UserId + "/" + ViewBag.periodId);
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                households = JsonConvert.DeserializeObject<List<Household>>(result);

                int LoopRow = 0;
                foreach (var _Household in households)
                {
                    households[LoopRow].CreatedDateString = _Household.CreatedDate.ToString("dd MMMM yyyy");
                    LoopRow++;
                }

            }


            
            return Json(households);
        }

        // GET: HouseholdController/Details/5
        public IActionResult Details(int id)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            HttpClient client = _api.Initial();
            List<Household> GetSum = new List<Household>();
            List<VwHouseholdbyYear> GetYear = new List<VwHouseholdbyYear>();
            Household household = new Household();
            VwHouseholdbyYear householdbyYear = new VwHouseholdbyYear();
            if (id == 0)
                return View(new Household());
            {
                HttpResponseMessage resGetSum = client.GetAsync("api/household/GetHouseHold/" + ViewBag.UserId + "/" + ViewBag.periodId).Result;
                HttpResponseMessage resGetYear = client.GetAsync("api/household/GetbyUser/" + ViewBag.UserId + "/" + ViewBag.YearValue).Result;
                //var _resGetSum = resGetSum.Content.ReadAsStringAsync().Result;
                //GetSum = JsonConvert.DeserializeObject<List<Household>>(_resGetSum);
                GetSum = resGetSum.Content.ReadAsAsync<List<Household>>().Result;
                GetYear = resGetYear.Content.ReadAsAsync<List<VwHouseholdbyYear>>().Result;


                HttpResponseMessage res = client.GetAsync("api/household/" + id.ToString()).Result;
                household = res.Content.ReadAsAsync<Household>().Result;

                //monthly
                household.MonthElectrikCons = GetSum.Sum(x => Convert.ToDecimal(x.KonsumsiListrik()));
                household.MonthLPGCons = GetSum.Sum(x => Convert.ToDecimal(x.KonsumsiLPG()));
                household.MonthCityGasCons = GetSum.Sum(x => Convert.ToDecimal(x.KonsumsiGasKota()));
                household.MonthElectrikEmision = GetSum.Sum(x => Convert.ToDecimal(x.EmisiCo2Listrik()));
                household.MonthGasEmision = GetSum.Sum(x => Convert.ToDecimal(x.EmisiCo2Gas()));
                household.MonthPeopleEmision = household.MonthElectrikEmision + household.MonthGasEmision;

                //Yearly                
                household.YearElectrikCons = GetYear.Sum(x => x.KonsumsiListrik);
                household.YearLPGCons = GetYear.Sum(x => x.KonsumsiLPG);
                household.YearCityGasCons = GetYear.Sum(x => x.KonsumsiGasKota);
                household.YearElectric = GetYear.Sum(x => x.EmisiListrik);
                household.YearGas = GetYear.Sum(x => x.EmisiGas);
                household.YearPerson = GetYear.Sum(x => x.EmisiperOrang);

                return View(household);
                //return PartialView("_DetailPartialView", res);
            }
        }

        // GET: HouseholdController/Create
        public IActionResult Create()
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            //HttpClient client = _api.Initial();
            //return View(); 

            //Household input = new Household();
            //input.AmountPeople = 1;
            //return View(input);
            return View(new Household());
        }

        //POST: HouseholdController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Household household)
        {
            SetViewBag();

            if (_api.userName == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            HttpClient client = _api.Initial();
            List<Household> households = new List<Household>();
            List<VwHouseholdbyYear> householdyear = new List<VwHouseholdbyYear>();
            HttpResponseMessage res = client.GetAsync("api/household/GetHouseHold/" + ViewBag.UserId + "/" + ViewBag.periodId).Result;
            HttpResponseMessage resyear = client.GetAsync("api/household/GetbyUser/" + ViewBag.UserId + "/" + ViewBag.YearValue).Result;
            households = res.Content.ReadAsAsync<List<Household>>().Result;
            householdyear = resyear.Content.ReadAsAsync<List<VwHouseholdbyYear>>().Result;

            #region monthly
            //monthly
            decimal konsumsilistrik;
            if (households.Count == 0)
            {
                konsumsilistrik = household.KonsumsiListrik();
            }
            else
            {
                //konsumsilistrik = (households.Sum(x => Convert.ToDecimal(x.Standmeter) + household.Standmeter)) / (households.Sum(x => x.AmountPeople) + household.AmountPeople);
                konsumsilistrik = households.Sum(x => Convert.ToDecimal(x.KonsumsiListrik())) + household.KonsumsiListrik();
                //listrikyearcons = (households.Sum(x => Convert.ToDecimal(x.Standmeter)) / households.Count(x => Convert.ToBoolean(x.Standmeter) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.Standmeter) / 12;
            }

            decimal konsumsilpg;
            if (households.Count == 0)
            {
                //konsumsilpg = household.LpgConsumption / household.AmountPeople;
                konsumsilpg = household.KonsumsiLPG();
            }
            else
            {
                //konsumsilpg = (households.Sum(x => Convert.ToDecimal(x.LpgConsumption) + household.LpgConsumption)) / (households.Sum(x => x.AmountPeople) + household.AmountPeople);
                konsumsilpg = households.Sum(x => Convert.ToDecimal(x.KonsumsiLPG())) + household.KonsumsiLPG();
            }

            decimal konsumsigaskota;
            if (households.Count == 0)
            {
                //konsumsigaskota = household.CityGasConsumption / household.AmountPeople;
                konsumsigaskota = household.KonsumsiGasKota();
            }
            else
            {
                //konsumsigaskota = (households.Sum(x => Convert.ToDecimal(x.CityGasConsumption) + household.CityGasConsumption)) / (households.Sum(x => x.AmountPeople) + household.AmountPeople);
                konsumsigaskota = households.Sum(x => Convert.ToDecimal(x.KonsumsiGasKota())) + household.KonsumsiGasKota();
            }

            decimal emisicolistrik;
            if (households.Count == 0)
            {
                emisicolistrik = household.EmisiCo2Listrik();
            }
            else
            {
                //emisicolistrik = (households.Sum(x => Convert.ToDecimal(x.Standmeter) + household.Standmeter)) / (households.Sum(x => x.AmountPeople) + household.AmountPeople) * (decimal)0.000792;
                //emisicolistrik = households.Sum(x => Convert.ToDecimal(x.ElectricityEmision)) + household.EmisiCo2Listrik();
                emisicolistrik = households.Sum(x => Convert.ToDecimal(x.ElectricityEmision)) + household.EmisiCo2Listrik();

            }

            decimal emisicogas;
            if (households.Count == 0)
            {
                emisicogas = household.EmisiCo2Gas();
            }
            else
            {
                //emisicogas = ((households.Sum(x => Convert.ToDecimal(x.LpgConsumption) + household.LpgConsumption)) / (households.Sum(x => x.AmountPeople) + household.AmountPeople) * (decimal)3.28) +
                //    ((households.Sum(x => Convert.ToDecimal(x.CityGasConsumption) + household.CityGasConsumption)) / (households.Sum(x => x.AmountPeople) + household.AmountPeople) * (decimal)2.1);
                emisicogas = households.Sum(x => Convert.ToDecimal(x.GasEmision)) + household.EmisiCo2Gas();

            }

            var emisicoperson = emisicolistrik + emisicogas;

            #endregion

            #region yearly
            //yearly
            decimal listrikyearcons;
            if (households.Count == 0)
            {
                listrikyearcons = household.KonsumsiListrik();
            }
            else
            {
                //listrikyearcons = (households.Sum(x => Convert.ToDecimal(x.Standmeter)) / households.Count(x => Convert.ToBoolean(x.Standmeter) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.Standmeter) / 12;
                listrikyearcons = householdyear.Sum(x => x.KonsumsiListrik) + household.KonsumsiListrik();
            }

            decimal lpgyearcons;
            if (households.Count == 0)
            {
                //lpgyearcons = household.LpgConsumption / household.AmountPeople;
                lpgyearcons = konsumsilpg;
            }
            else
            {
                //lpgyearcons = ((households.Sum(x => Convert.ToDecimal(x.LpgConsumption)) / households.Count(x => Convert.ToBoolean(x.LpgConsumption) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.Standmeter)) / ((households.Sum(x => x.AmountPeople)) / (households.Count(x => Convert.ToBoolean(x.AmountPeople) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.AmountPeople)) / 12;
                lpgyearcons = householdyear.Sum(x => x.KonsumsiLPG) + household.KonsumsiLPG();
            }

            decimal gascityyearcons;
            if (households.Count == 0)
            {
                //gascityyearcons = household.CityGasConsumption / household.AmountPeople;
                gascityyearcons = konsumsigaskota;
            }
            else
            {
                //gascityyearcons = ((households.Sum(x => Convert.ToDecimal(x.CityGasConsumption)) / households.Count(x => Convert.ToBoolean(x.CityGasConsumption) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.Standmeter)) / ((households.Sum(x => x.AmountPeople)) / (households.Count(x => Convert.ToBoolean(x.AmountPeople) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.AmountPeople)) / 12;
                gascityyearcons = householdyear.Sum(x => x.KonsumsiGasKota) + household.KonsumsiGasKota();
            }

            decimal emisilistrtikyear;
            if (households.Count == 0)
            {
                emisilistrtikyear = household.EmisiCo2Listrik();
            }
            else
            {
                //emisilistrtikyear = (households.Sum(x => Convert.ToDecimal(x.ElectricityEmision)) / households.Count(x => Convert.ToBoolean(x.ElectricityEmision) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.EmisiCo2Gas()) / 12;
                //emisilistrtikyear = (((households.Sum(x => Convert.ToDecimal(x.ElectricityEmision)) / households.Count(x => Convert.ToBoolean(x.ElectricityEmision) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.EmisiCo2Gas())) / ((households.Sum(x => x.AmountPeople)) / (households.Count(x => Convert.ToBoolean(x.AmountPeople) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.AmountPeople))) / 12;
                emisilistrtikyear = householdyear.Sum(x => x.EmisiListrik) + household.EmisiCo2Listrik();
            }

            decimal emisigasyear;
            if (households.Count == 0)
            {
                emisigasyear = household.EmisiCo2Gas();
            }
            else
            {
                //emisigasyear = (households.Sum(x => Convert.ToDecimal(x.GasEmision)) / households.Count(x => Convert.ToBoolean(x.GasEmision) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.EmisiCo2Gas()) / 12;
                //emisigasyear = ((((households.Sum(x => Convert.ToDecimal(x.GasEmision)) / households.Count(x => Convert.ToBoolean(x.GasEmision) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.EmisiCo2LPG())) / ((households.Sum(x => x.AmountPeople)) / (households.Count(x => Convert.ToBoolean(x.AmountPeople) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.AmountPeople))) + 
                //    (((households.Sum(x => Convert.ToDecimal(x.CityGasConsumption)) / households.Count(x => Convert.ToBoolean(x.CityGasConsumption) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.EmisiCo2Kota())) / ((households.Sum(x => x.AmountPeople)) / (households.Count(x => Convert.ToBoolean(x.AmountPeople) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.AmountPeople)))) / 12;
                emisigasyear = householdyear.Sum(x => x.EmisiGas) + household.EmisiCo2Gas();
            }

            decimal emisipersonyear;
            if (households.Count == 0)
            {
                emisipersonyear = household.EmisiCo2Person();
            }
            else
            {
                //emisipersonyear = (households.Sum(x => Convert.ToDecimal(x.PeopleEmision)) / households.Count(x => Convert.ToBoolean(x.PeopleEmision) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.EmisiCo2Person()) / 12;
                emisipersonyear = emisilistrtikyear + emisigasyear;
            }

            //monthly
            ViewBag.konsumsilistrik = konsumsilistrik.ToString("0.##");//0.## returns "0.5"
            ViewBag.konsumsilpg = konsumsilpg.ToString("0.##");
            ViewBag.konsumsigascity = konsumsigaskota.ToString("0.##");
            ViewBag.emisicolistrik = emisicolistrik.ToString("0.##");
            ViewBag.emisicogas = emisicogas.ToString("0.##");
            ViewBag.emisicoperson = emisicoperson.ToString("0.##");
            //yearly
            ViewBag.konsumsilistrikyear = listrikyearcons.ToString("0.##"); //#.## 2 desimal
            ViewBag.konsumsilpgyear = lpgyearcons.ToString("0.##");
            ViewBag.konsumsigascityyear = gascityyearcons.ToString("0.##");
            ViewBag.emisicolistrikyear = emisilistrtikyear.ToString("0.##");
            ViewBag.emisicogasyear = emisigasyear.ToString("0.##");
            ViewBag.emisicopersonyear = emisipersonyear.ToString("0.##");

            return View(household);
            #endregion
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Household household)
        {
            HttpClient client = _api.Initial();
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }

            try
            {
                household.PeriodeId = ViewBag.periodId; 
                household.UserId = ViewBag.UserId; 

                HttpResponseMessage res = client.PostAsJsonAsync("api/household/", household).Result;
                TempData["SuccessMessage"] = "Data telah disimpan";
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: HouseholdController/Edit/5
        public IActionResult Edit(int id = 0)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            HttpClient client = _api.Initial();

            if (id == 0)
                return View(new Household());
            {
                HttpResponseMessage res = client.GetAsync("api/household/" + id.ToString()).Result;
                return View(res.Content.ReadAsAsync<Household>().Result);
            }

        }

        // POST: HouseholdController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Household household, int id)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            HttpClient client = _api.Initial();
            HttpResponseMessage res = client.GetAsync("api/household/" + id.ToString()).Result;

            List<Household> households = new List<Household>();
            List<VwHouseholdbyYear> householdyear = new List<VwHouseholdbyYear>();
            HttpResponseMessage resGetSum = client.GetAsync("api/household/GetHouseHold/" + ViewBag.UserId + "/" + ViewBag.periodId).Result;
            HttpResponseMessage resyear = client.GetAsync("api/household/GetbyUser/" + ViewBag.UserId + "/" + ViewBag.YearValue).Result;
            households = resGetSum.Content.ReadAsAsync<List<Household>>().Result;
            householdyear = resyear.Content.ReadAsAsync<List<VwHouseholdbyYear>>().Result;
            var editedid = households.Where(a => a.HouseholdId != household.HouseholdId).ToList();
            var editedid1 = households.Where(a => a.HouseholdId == household.HouseholdId).ToList();
            //var konsumsilistrik = household.KonsumsiListrik().ToString("0.##");

            #region monthly
            decimal konsumsilistrik;
            if (households.Count == 1)
            {
                konsumsilistrik = household.KonsumsiListrik();
            }
            else
            {
                konsumsilistrik = editedid.Sum(x => Convert.ToDecimal(x.KonsumsiListrik())) + household.KonsumsiListrik();
                //Where(a => a.UserId == userid && a.PeriodeId == periodeID).ToList();
            }
            //var konsumsigas = household.KonsumsiGas().ToString("0.##");

            decimal konsumsilpg;
            if (households.Count == 1)
            {
                //konsumsilpg = household.LpgConsumption / household.AmountPeople;
                konsumsilpg = household.KonsumsiLPG();
            }
            else
            {
                //konsumsilpg = (households.Sum(x => Convert.ToDecimal(x.LpgConsumption) + household.LpgConsumption)) / (households.Sum(x => x.AmountPeople) + household.AmountPeople);
                konsumsilpg = editedid.Sum(x => Convert.ToDecimal(x.KonsumsiLPG())) + household.KonsumsiLPG();
            }

            decimal konsumsigaskota;
            if (households.Count == 1)
            {
                //konsumsigaskota = household.CityGasConsumption / household.AmountPeople;
                konsumsigaskota = household.KonsumsiGasKota();
            }
            else
            {
                //konsumsigaskota = (households.Sum(x => Convert.ToDecimal(x.CityGasConsumption) + household.CityGasConsumption)) / (households.Sum(x => x.AmountPeople) + household.AmountPeople);
                konsumsigaskota = editedid.Sum(x => Convert.ToDecimal(x.KonsumsiGasKota())) + household.KonsumsiGasKota();
            }

            //var emisicolistrik = household.EmisiCo2Listrik().ToString("0.##");
            //var emisicogas = household.EmisiCo2Gas().ToString("0.##");
            //var emisicoperson = household.EmisiCo2Person().ToString("0.##");

            decimal emisicolistrik;
            if (households.Count == 1)
            {
                emisicolistrik = household.EmisiCo2Listrik();
            }
            else
            {
                //emisicolistrik = (households.Sum(x => Convert.ToDecimal(x.Standmeter) + household.Standmeter)) / (households.Sum(x => x.AmountPeople) + household.AmountPeople) * (decimal)0.000792;
                //emisicolistrik = households.Sum(x => Convert.ToDecimal(x.ElectricityEmision)) + household.EmisiCo2Listrik();
                emisicolistrik = editedid.Sum(x => Convert.ToDecimal(x.ElectricityEmision)) + household.EmisiCo2Listrik();

            }

            decimal emisicogas;
            if (households.Count == 1)
            {
                emisicogas = household.EmisiCo2Gas();
            }
            else
            {
                //emisicogas = ((households.Sum(x => Convert.ToDecimal(x.LpgConsumption) + household.LpgConsumption)) / (households.Sum(x => x.AmountPeople) + household.AmountPeople) * (decimal)3.28) +
                //    ((households.Sum(x => Convert.ToDecimal(x.CityGasConsumption) + household.CityGasConsumption)) / (households.Sum(x => x.AmountPeople) + household.AmountPeople) * (decimal)2.1);
                emisicogas = editedid.Sum(x => Convert.ToDecimal(x.GasEmision)) + household.EmisiCo2Gas();
            }

            var emisicoperson = emisicolistrik + emisicogas;
            #endregion

            #region yearly
            decimal listrikyearcons;
            if (households.Count == 1)
            {
                listrikyearcons = household.KonsumsiListrik();
            }
            else
            {
                listrikyearcons = householdyear.Sum(x => x.KonsumsiListrik) + household.KonsumsiListrik() - editedid1.Sum(z => z.KonsumsiListrik());
            }

            decimal lpgyearcons;
            if (households.Count == 1)
            {
                //lpgyearcons = household.LpgConsumption / household.AmountPeople;
                lpgyearcons = konsumsilpg;
            }
            else
            {
                //lpgyearcons = ((households.Sum(x => Convert.ToDecimal(x.LpgConsumption)) / households.Count(x => Convert.ToBoolean(x.LpgConsumption) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.Standmeter)) / ((households.Sum(x => x.AmountPeople)) / (households.Count(x => Convert.ToBoolean(x.AmountPeople) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.AmountPeople)) / 12;
                lpgyearcons = householdyear.Sum(x => x.KonsumsiLPG) + household.KonsumsiLPG() - editedid1.Sum(z => z.KonsumsiLPG());
            }

            //decimal gasyearcons;
            //if (households.Count == 1)
            //{
            //    gasyearcons = household.CityGasConsumption;
            //}
            //else
            //{
            //    gasyearcons = (households.Sum(x => Convert.ToDecimal(x.CityGasConsumption)) / households.Count(x => Convert.ToBoolean(x.CityGasConsumption) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.CityGasConsumption) / 12;
            //}

            decimal gascityyearcons;
            if (households.Count == 1)
            {
                //gascityyearcons = household.CityGasConsumption / household.AmountPeople;
                gascityyearcons = konsumsigaskota;
            }
            else
            {
                //gascityyearcons = ((households.Sum(x => Convert.ToDecimal(x.CityGasConsumption)) / households.Count(x => Convert.ToBoolean(x.CityGasConsumption) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.Standmeter)) / ((households.Sum(x => x.AmountPeople)) / (households.Count(x => Convert.ToBoolean(x.AmountPeople) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.AmountPeople)) / 12;
                gascityyearcons = householdyear.Sum(x => x.KonsumsiGasKota) + household.KonsumsiGasKota() - editedid1.Sum(z => z.KonsumsiGasKota());
            }

            decimal emisilistrtikyear;
            if (households.Count == 1)
            {
                emisilistrtikyear = household.EmisiCo2Listrik();
            }
            else
            {
                //emisilistrtikyear = (households.Sum(x => Convert.ToDecimal(x.ElectricityEmision)) / households.Count(x => Convert.ToBoolean(x.ElectricityEmision) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.EmisiCo2Gas()) / 12;
                emisilistrtikyear = householdyear.Sum(x => x.EmisiListrik) + household.EmisiCo2Listrik() - editedid1.Sum(z => Convert.ToDecimal(z.ElectricityEmision));
            }

            decimal emisigasyear;
            if (households.Count == 1)
            {
                emisigasyear = household.EmisiCo2Gas();
            }
            else
            {
                //emisigasyear = (households.Sum(x => Convert.ToDecimal(x.GasEmision)) / households.Count(x => Convert.ToBoolean(x.GasEmision) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.EmisiCo2Gas()) / 12;
                emisigasyear = householdyear.Sum(x => x.EmisiGas) + household.EmisiCo2Gas() - editedid1.Sum(z => Convert.ToDecimal(z.GasEmision));
            }

            decimal emisipersonyear;
            if (households.Count == 1)
            {
                emisipersonyear = household.EmisiCo2Person();
            }
            else
            {
                //emisipersonyear = (households.Sum(x => Convert.ToDecimal(x.PeopleEmision)) / households.Count(x => Convert.ToBoolean(x.PeopleEmision) && x.UserId == HelperStatic.UserID && x.PeriodeId == ViewBag.periodId) + household.EmisiCo2Person()) / 12;
                emisipersonyear = emisigasyear + emisilistrtikyear;
            }
            #endregion

            //monthly
            ViewBag.konsumsilistrik = konsumsilistrik.ToString("0.##");//0.## returns "0.5"
            ViewBag.konsumsilpg = konsumsilpg.ToString("0.##");
            ViewBag.konsumsigascity = konsumsigaskota.ToString("0.##");
            ViewBag.emisicolistrik = emisicolistrik.ToString("0.##");
            ViewBag.emisicogas = emisicogas.ToString("0.##");
            ViewBag.emisicoperson = emisicoperson.ToString("0.##");
            //yearly
            ViewBag.konsumsilistrikyear = listrikyearcons.ToString("0.##"); //#.## 2 desimal
            ViewBag.konsumsilpgyear = lpgyearcons.ToString("0.##");
            ViewBag.konsumsigascityyear = gascityyearcons.ToString("0.##");
            ViewBag.emisicolistrikyear = emisilistrtikyear.ToString("0.##");
            ViewBag.emisicogasyear = emisigasyear.ToString("0.##");
            ViewBag.emisicopersonyear = emisipersonyear.ToString("0.##");

            return View(res.Content.ReadAsAsync<Household>().Result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Household household)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            HttpClient client = _api.Initial();
            try
            {
                household.PeriodeId = ViewBag.periodId; 
                household.UserId = ViewBag.UserID;
                household.CreatedDate = DateTime.Now;

                HttpResponseMessage res = client.PutAsJsonAsync("api/household/" + household.HouseholdId, household).Result;
                TempData["SuccessMessage"] = "Data telah diperbaharui";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: HouseholdController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString(_api.userName) == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }

            HttpClient client = _api.Initial();
            var household = new Household();
            HttpResponseMessage res = await client.DeleteAsync($"api/household/{id}");
            TempData["SuccessMessage"] = "Data telah dihapus";
            return RedirectToAction("Index");
        }


        //public IActionResult AddOrEdit(int id = 0)
        //{
        //    HttpClient client = _api.Initial();

        //    if (id == 0)
        //        return View(new Household());
        //    else
        //    {
        //        HttpResponseMessage res = client.GetAsync("api/household/" + id.ToString()).Result;
        //        return View(res.Content.ReadAsAsync<Household>().Result);
        //    }

        //}

        //[HttpPost]
        //public IActionResult AddOrEdit(Household household, int id)
        //{
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res1 = client.GetAsync("api/household/" + id.ToString()).Result;
        //    household.HouseholdId = id;
        //    if (household.HouseholdId == 0)
        //    {
        //        HttpResponseMessage res = client.PostAsJsonAsync("api/household", household).Result;
        //        TempData["SuccessMessage"] = "Data telah disimpan";
        //    }
        //    else
        //    {
        //        HttpResponseMessage res = client.PutAsJsonAsync("api/household/" + household.HouseholdId, household).Result;
        //        TempData["SuccessMessage"] = "Data telah diperbaharui";
        //    }
        //    return RedirectToAction("Index");
        //}
    }

}
