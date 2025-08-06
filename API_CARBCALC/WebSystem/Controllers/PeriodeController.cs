using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebSystem.Helper;
using WebSystem.Models;

namespace WebSystem.Controllers
{
    public class PeriodeController : Controller
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();
        HelperAPI _api = new HelperAPI();

        //const string userId = "_UserId";
        //const string userName = "_UserName";
        //const string displayname = "_DisplayName";
        //const string periodsession = "_Periode";
        //const string periodeIdsession = "_PeriodeId";

        // GET: PeriodeController
        public void SetViewBag()
        {
            //ViewBag.periodedetail = HelperStatic.PeriodeStatic;
            ////ViewBag.userName = HelperStatic.UserID;
            //ViewBag.UserName = HelperStatic.UserName;
            //ViewBag.Displayname = HelperStatic.Name;

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
            //ViewBag.periodId = HttpContext.Session.GetString(periodeIdsession);
        }
        public ActionResult Index()
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            else
            {
            }

            if(_api.PeriodeStatic != null)
            {

                //var test = HttpContext.Session.GetString(_api.periodsession);
                //var testid = Convert.ToInt32(HttpContext.Session.GetString(_api.periodeIdsession));
                //var testM = Convert.ToInt32(HttpContext.Session.GetString(_api.MonthValuesession));
                //var testY = Convert.ToInt32(HttpContext.Session.GetString(_api.yearValuesession));

                //Delete session periode
                HttpContext.Session.SetString(_api.periodeIdsession, 0.ToString());
                HttpContext.Session.SetString(_api.periodsession, "");
                HttpContext.Session.SetString(_api.MonthValuesession, 0.ToString());
                HttpContext.Session.SetString(_api.yearValuesession, 0.ToString());
                
                //test = HttpContext.Session.GetString(_api.periodsession);
                //testid = Convert.ToInt32(HttpContext.Session.GetString(_api.periodeIdsession));
                //testM = Convert.ToInt32(HttpContext.Session.GetString(_api.MonthValuesession));
                //testY = Convert.ToInt32(HttpContext.Session.GetString(_api.yearValuesession));

            }
            else
            {
                HttpContext.Session.SetString(_api.periodsession, "");
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult create(Periode periode)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            // ViewBag.UserName = HelperStatic.UserName;
            return View();
        }

        /// <summary>
        /// Mengambil data ringkasan periode untuk ditampilkan di halaman utama.
        /// </summary> 
        [HttpGet]
        [Route("GetListSummary")]
        public async Task<JsonResult> GetListSummary()
        {
            SetViewBag();

            List<VwPeriodeListSummary> listSummary = new List<VwPeriodeListSummary>();
            //List<VwEmission_Report> listSummary = new List<VwEmission_Report>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/Periode/GetListSummary/" + ViewBag.UserId);
            //HttpResponseMessage res = await client.GetAsync("api/Report/GetUsernameFilter/" + ViewBag.UserId);
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                 listSummary = JsonConvert.DeserializeObject<List<VwPeriodeListSummary>>(result);
                //listSummary = JsonConvert.DeserializeObject<List<VwEmission_Report>>(result);
            }
            return Json(listSummary);
        }

        /// <summary>
        /// Mengambil data ringkasan periode untuk ditampilkan dalam bentuk grafik.
        /// </summary> 
        [HttpGet]
        [Route("GetListSummary_Chart")]
        public async Task<JsonResult> GetListSummary_Chart()
        {
            SetViewBag();

            List<VwPeriodeListSummary> listSummary = new List<VwPeriodeListSummary>();
            //List<VwEmission_Report> listSummary = new List<VwEmission_Report>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/Periode/GetListSummary_Chart/" + ViewBag.UserId);
            //HttpResponseMessage res = await client.GetAsync("api/Report/GetUsernameFilter/" + ViewBag.UserId);
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                listSummary = JsonConvert.DeserializeObject<List<VwPeriodeListSummary>>(result);
                //listSummary = JsonConvert.DeserializeObject<List<VwEmission_Report>>(result);
            }
            return Json(listSummary);
        }

        // GET: PeriodeController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: PeriodeController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: PeriodeController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: PeriodeController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: PeriodeController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: PeriodeController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: PeriodeController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
