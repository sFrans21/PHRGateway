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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace WebSystem.Controllers

{
    public class SummaryController : Controller
    {
        HelperAPI _api = new HelperAPI();



        private readonly ILogger<HomeController> _logger;
        //HelperAPI _api = new HelperAPI();
        //const string userId = "_UserId";
        //const string userName = "_UserName";
        //const string displayname = "_DisplayName";
        //const string periodsession = "_Periode";
        //const string periodeIdsession = "_PeriodeId";
        //public const string yearValuesession = "_YearValue";
        //public const string MonthValuesession = "_MonthValue";

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
            var Pdfurl = Configuration["PdfDownloadLink"];
            ViewBag.url = Pdfurl;

            //ViewBag.UserId = Convert.ToInt32(HttpContext.Session.GetString(userId));
            //ViewBag.UserName = HttpContext.Session.GetString(userName);
            //ViewBag.Displayname = HttpContext.Session.GetString(displayname);
            //ViewBag.periodedetail = HttpContext.Session.GetString(periodsession);
            //ViewBag.periodId = Convert.ToInt32(HttpContext.Session.GetString(periodeIdsession));
            //ViewBag.YearValue = Convert.ToInt32(HttpContext.Session.GetString(yearValuesession));
            //ViewBag.MonthValue = Convert.ToInt32(HttpContext.Session.GetString(MonthValuesession));

            //ViewBag.periodedetail = HelperStatic.PeriodeStatic;
            //ViewBag.UserName = HelperStatic.UserName;
            //ViewBag.Displayname = HelperStatic.Name;
        }

        private readonly IConfiguration Configuration;
        public SummaryController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
                

        public IActionResult Index(FnGetSummary _FnGetSummary)
        {
            SetViewBag();
            if (_api.userName == null)
            {
                return RedirectToAction("UserLogin", "User");
            }


            string FromDate, Todate;
            FromDate = DateTime.Parse(_FnGetSummary.FromDate).ToString("yyyyMMdd");
            Todate = DateTime.Parse(_FnGetSummary.ToDate).ToString("yyyyMMdd");

            //FromDate = DateTime.Parse(_FnGetSummary.FromDate).ToString("yyyy-MM-dd");
            //Todate = DateTime.Parse(_FnGetSummary.ToDate).ToString("yyyy-MM-dd");

            HttpClient client = _api.Initial();
            HttpResponseMessage res = client.GetAsync("api/Summary/GetSum/" + _FnGetSummary.userID + "/" + _FnGetSummary.PeriodeId + "/" + FromDate + "/" + Todate).Result;
            //HttpResponseMessage res = client.GetAsync("api/Summary/GetSum/" + HelperStatic.UserID.ToString() + "/" + _FnGetSummary.CarbonAbsPerMonth.ToString()).Result;

            List<FnGetSummary> vmtest = res.Content.ReadAsAsync<List<FnGetSummary>>().Result;

            if (vmtest == null)
            {

            }
            else
            {
                TempData["SelectedPeriode"] = vmtest[0].CreatedDate.ToString("MM/dd/yyyy");
                ViewBag.SelectedPeriode = DateTime.Parse(vmtest[0].CreatedDate.ToString("MM/dd/yyyy"));

                ViewBag.Year = DateTime.Parse(vmtest[0].CreatedDate.ToString("MM/dd/yyyy")).Year;
                ViewBag.Month = DateTime.Parse(vmtest[0].CreatedDate.ToString("MM/dd/yyyy")).Month;
                if (ViewBag.Month <= 9)
                {
                    ViewBag.Month = ViewBag.Month - 1;
                    ViewBag.Month = "0" + ViewBag.Month;
                }

                ViewBag.FromDate = DateTime.Parse(vmtest[0].FromDate).ToString("dd-MM-yyyy", new System.Globalization.CultureInfo("id-ID"));
                ViewBag.ToDate = DateTime.Parse(vmtest[0].ToDate).ToString("dd-MM-yyyy", new System.Globalization.CultureInfo("id-ID"));

                //Add Month to variables
                //string MonthName = ViewBag.FromDate.ToString("MMMM");
                //string Year = ViewBag.ToDate.ToString("yyyy");


                //ViewBag.electriccons = vmtest[0].ListrikBulanBerjalanKWH.ToString();
                //ViewBag.electriccons = vmtest[0].ListrikBulanBerjalanKWH.ToString();
                //ViewBag.electricuse = vmtest[0].ListrikBulanBerjalanKWH_PerOrang.ToString();
                //ViewBag.gascons = vmtest[0].GasBulanBerjalanM3.ToString();
                //ViewBag.gasuse = vmtest[0].GasBulanBerjalanM3_PerOrang.ToString();
                //ViewBag.plant = vmtest[0].TreeAmount.ToString();
                //ViewBag.listrik = vmtest[0].TonCO2Listrik.ToString();
                //ViewBag.gas = vmtest[0].TONCO2Gas.ToString();
                //ViewBag.emisibbm = vmtest[0].TonCO2BBM.ToString();
                //ViewBag.bbmbulan = vmtest[0].BBMBulan.ToString();
                //ViewBag.bbmtahun = vmtest[0].BBMTahun.ToString();

                //ignore 0 decimal on last digit
                ViewBag.electriccons = float.Parse(vmtest[0].ListrikBulanBerjalanKWH.ToString());
                ViewBag.electriccons = float.Parse(vmtest[0].ListrikBulanBerjalanKWH.ToString());
                ViewBag.electricuse = float.Parse(vmtest[0].ListrikBulanBerjalanKWH_PerOrang.ToString());
                ViewBag.gascons = float.Parse(vmtest[0].GasBulanBerjalanM3.ToString());
                ViewBag.gasuse = float.Parse(vmtest[0].GasBulanBerjalanM3_PerOrang.ToString());
                ViewBag.plant = vmtest[0].TreeAmount.ToString();
                ViewBag.listrik = float.Parse(vmtest[0].TonCO2Listrik.ToString());
                ViewBag.gas = float.Parse(vmtest[0].TONCO2Gas.ToString());
                ViewBag.emisibbm = float.Parse(vmtest[0].TonCO2BBM.ToString());
                ViewBag.bbmbulan = float.Parse(vmtest[0].BBMBulan.ToString());
                ViewBag.bbmtahun = float.Parse(vmtest[0].BBMTahun.ToString());
                //
                ViewBag.co2 = float.Parse(vmtest[0].TotalEmisiTonCO2.ToString());
                ViewBag.abs = float.Parse(vmtest[0].CarbonAbsPerMonth.ToString());
                //float tes = float.Parse(vmtest[0].TotalEmisiTonCO2.ToString());
                //ViewBag.final = vmtest[0].CarbonAbsPerMonth - vmtest[0].TonCO2BBM;
                //ViewBag.final = vmtest[0].TonCO2BBM - vmtest[0].CarbonAbsPerMonth;
                ViewBag.final = float.Parse(vmtest[0].TotalEmisiTonCO2.ToString()) - float.Parse(vmtest[0].CarbonAbsPerMonth.ToString());
                ViewBag.username = ViewBag.UserName.ToString();
                //ViewBag.DisplayName = HelperStatic.Name.ToString();
                ViewBag.Periode = " " + vmtest[0].PeriodeDetail;
            }

            //User.Identity.Name.ToString();

            return View();
        }

        [HttpGet]
        public IActionResult Index(int id = 0)
        {
            SetViewBag();
            if (_api.userName == null)
            {
                return RedirectToAction("UserLogin", "User");
            }
            

            string FromDate, Todate;
            //DateTime FromDate, Todate;
            //FromDate = DateTime.Parse("11/11/1111");
            //Todate = DateTime.Parse("11/11/1111").ToString("dd MM yyyy");
            FromDate = "11111111";
            Todate = "11111111";

            HttpClient client = _api.Initial();
            //var testgetdate = DateTime.Now;
            //var testdatePeriodeb = HelperStatic.MonthValue.ToString() + "/" + 23.ToString() + "/" + HelperStatic.YearValue;//23.ToString() + "/" + HelperStatic.MonthValue.ToString() + "/" + HelperStatic.YearValue;
            //DateTime DatePeriode = Convert.ToDateTime(testdatePeriodeb);//DateTime.Parse(testdatePeriodeb.ToString("MM/dd/yyyy"));  //Convert.ToDateTime(testdatePeriodeb);
            //HttpResponseMessage res = client.GetAsync("api/Summary/GetSum/" + HelperStatic.UserID.ToString() + "/" + id.ToString() ).Result;
            HttpResponseMessage res = client.GetAsync("api/Summary/GetSum/" + ViewBag.UserID.ToString() + "/" + id.ToString() + "/" + FromDate + "/" + Todate).Result;

            FnGetSummary _FnGetSummary = new FnGetSummary();
            


            List<FnGetSummary> vmtest = res.Content.ReadAsAsync<List<FnGetSummary>>().Result;

            if(vmtest == null)
            {

            }
            else
            {
                _FnGetSummary.userID = ViewBag.UserID;
                _FnGetSummary.PeriodeId = id;
                _FnGetSummary.FromDate = DateTime.Parse(vmtest[0].FromDate).ToString("dd-MMMM-yyyy");
                _FnGetSummary.ToDate = DateTime.Parse(vmtest[0].ToDate).ToString("dd-MMMM-yyyy");

                ViewBag.FromDate = DateTime.Parse(vmtest[0].FromDate).ToString("dd-MM-yyyy", new System.Globalization.CultureInfo("id-ID"));
                ViewBag.ToDate = DateTime.Parse(vmtest[0].ToDate).ToString("dd-MM-yyyy", new System.Globalization.CultureInfo("id-ID"));


                TempData["SelectedPeriode"] =  vmtest[0].CreatedDate.ToString("dd-MM-yyyy");
                ViewBag.SelectedPeriode = DateTime.Parse(vmtest[0].CreatedDate.ToString("dd-MMMM-yyyy")) ;

                ViewBag.Year =  DateTime.Parse(vmtest[0].CreatedDate.ToString("dd-MMMM-yyyy")).Year;
                ViewBag.Month = DateTime.Parse(vmtest[0].CreatedDate.ToString("dd-MMMM-yyyy")).Month;
                if (ViewBag.Month <= 9)
                {
                    ViewBag.Month = ViewBag.Month - 1;
                    ViewBag.Month = "0" + ViewBag.Month;
                }
                    

                //ViewBag.electriccons = vmtest[0].ListrikBulanBerjalanKWH.ToString();
                //ViewBag.electriccons = vmtest[0].ListrikBulanBerjalanKWH.ToString();
                //ViewBag.electricuse = vmtest[0].ListrikBulanBerjalanKWH_PerOrang.ToString();
                //ViewBag.gascons = vmtest[0].GasBulanBerjalanM3.ToString();
                //ViewBag.gasuse = vmtest[0].GasBulanBerjalanM3_PerOrang.ToString();
                //ViewBag.plant = vmtest[0].TreeAmount.ToString();
                //ViewBag.listrik = vmtest[0].TonCO2Listrik.ToString();
                //ViewBag.gas = vmtest[0].TONCO2Gas.ToString();
                //ViewBag.emisibbm = vmtest[0].TonCO2BBM.ToString();
                //ViewBag.bbmbulan = vmtest[0].BBMBulan.ToString();
                //ViewBag.bbmtahun = vmtest[0].BBMTahun.ToString();

                //ignore 0 decimal on last digit
                ViewBag.electriccons = float.Parse(vmtest[0].ListrikBulanBerjalanKWH.ToString());
                ViewBag.electriccons = float.Parse(vmtest[0].ListrikBulanBerjalanKWH.ToString());
                ViewBag.electricuse = float.Parse(vmtest[0].ListrikBulanBerjalanKWH_PerOrang.ToString());
                ViewBag.gascons = float.Parse(vmtest[0].GasBulanBerjalanM3.ToString());
                ViewBag.gasuse = float.Parse(vmtest[0].GasBulanBerjalanM3_PerOrang.ToString());
                ViewBag.plant = vmtest[0].TreeAmount.ToString();
                ViewBag.listrik = float.Parse(vmtest[0].TonCO2Listrik.ToString());
                ViewBag.gas = float.Parse(vmtest[0].TONCO2Gas.ToString());
                ViewBag.emisibbm = float.Parse(vmtest[0].TonCO2BBM.ToString());
                ViewBag.bbmbulan = float.Parse(vmtest[0].BBMBulan.ToString());
                ViewBag.bbmtahun = float.Parse(vmtest[0].BBMTahun.ToString());
                //
                ViewBag.co2 = float.Parse(vmtest[0].TotalEmisiTonCO2.ToString());
                ViewBag.abs = float.Parse(vmtest[0].CarbonAbsPerMonth.ToString());
                //float tes = float.Parse(vmtest[0].TotalEmisiTonCO2.ToString());
                //ViewBag.final = vmtest[0].CarbonAbsPerMonth - vmtest[0].TonCO2BBM;
                //ViewBag.final = vmtest[0].TonCO2BBM - vmtest[0].CarbonAbsPerMonth;
                ViewBag.final = float.Parse(vmtest[0].TotalEmisiTonCO2.ToString()) - float.Parse(vmtest[0].CarbonAbsPerMonth.ToString());
                ViewBag.username = ViewBag.UserName.ToString();
                //ViewBag.DisplayName = HelperStatic.Name.ToString();
                ViewBag.Periode = " " + vmtest[0].PeriodeDetail;
            }
            
                //User.Identity.Name.ToString();
            string MonthName = DateTime.Parse(vmtest[0].FromDate).ToString("MMMM", new System.Globalization.CultureInfo("id-ID")); 
            string Year = DateTime.Parse(vmtest[0].FromDate).ToString("yyyy", new System.Globalization.CultureInfo("id-ID"));

            ViewBag.MonthName = MonthName;
            ViewBag.Year = Year;
            
            return View(_FnGetSummary);
        }        

    }    

}
