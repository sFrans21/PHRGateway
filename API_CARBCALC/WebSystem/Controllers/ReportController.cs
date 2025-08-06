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

namespace WebSystem.Controllers
{
    public class ReportController : Controller
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();
        HelperAPI _api = new HelperAPI();

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
        }
        public IActionResult Index()
        {
            SetViewBag();
            if (ViewBag.UserName == null)
            {
                // User belum login, berikan notifikasi dan arahkan ke halaman login
                TempData["ErrorMessage"] = "Anda harus login terlebih dahulu.";
                return RedirectToAction("UserLogin", "User");
            }
            //else if (ViewBag.IsAdmin == 0 || ViewBag.IsAdmin == 5)
            else if (int.TryParse(ViewBag.IsAdmin, out int isAdminValue) && (isAdminValue == 0 || isAdminValue == 5)) //dari string to int
            {
                // User bukan admin, berikan notifikasi
                TempData["ErrorMessage"] = "Anda hanya bisa masuk ke report sebagai admin.";
                return RedirectToAction("Index","Home");
            }

            return View();
        }

        #region Vw_Emission_Report
        //[HttpGet]
        //[Route("GetListReport")]
        //public async Task<JsonResult> GetListReport()
        //{
        //    SetViewBag();
        //    List<VwEmission_Report> reports = new List<VwEmission_Report>();
        //    HttpClient client = _api.Initial();
        //    HttpResponseMessage res = await client.GetAsync("api/report");
        //    if (res.IsSuccessStatusCode)
        //    {
        //        var result = res.Content.ReadAsStringAsync().Result;
        //        reports = JsonConvert.DeserializeObject<List<VwEmission_Report>>(result);
        //    }
        //    return Json(reports);
        //}
        #endregion

        #region Vw_Emission_Report_Field
        /// <summary>
        /// mengambil daftar laporan emisi berdasarkan user ID.
        /// </summary> 
        [HttpGet]
        [Route("GetListReport")]
        public async Task<JsonResult> GetListReport()
        {
            SetViewBag();

            TblTUser _tblTUser = new TblTUser();
            _tblTUser.userID = ViewBag.UserID;

            List<Vw_Emission_Report_Field> reports = new List<Vw_Emission_Report_Field>();
            HttpClient client = _api.Initial();
            //HttpResponseMessage res = await client.GetAsync("api/report");
            HttpResponseMessage res = await client.PostAsJsonAsync("api/report/GetEmissionReport/", _tblTUser);
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                reports = JsonConvert.DeserializeObject<List<Vw_Emission_Report_Field>>(result);
            }
            return Json(reports);
        }
        #endregion
    }

}
