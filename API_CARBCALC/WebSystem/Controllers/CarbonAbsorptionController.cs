using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using WebSystem.Filters;
using WebSystem.Function;
using WebSystem.Helper;
using WebSystem.Models;



namespace WebSystem.Controllers
{
    public class CarbonAbsorptionController : Controller
    {
        HelperAPI _api = new HelperAPI();

        //const string userId = "_UserId";
        //const string userName = "_UserName";
        //const string displayname = "_DisplayName";
        //const string periodsession = "_Periode";
        //const string periodeIdsession = "_PeriodeId";

        void SetViewBag()
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
            //ViewBag.periodId = HttpContext.Session.GetString(periodeIdsession);
        }
        public IActionResult Index()
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }

            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetList()
        {
            SetViewBag();
            List<CarbonAbsorptionViewModel> carbonAbsorptions = new List<CarbonAbsorptionViewModel>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/carbonabsorbtion/GetCarbonAbsorption/" + ViewBag.UserId + "/" + ViewBag.periodId);
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                carbonAbsorptions = JsonConvert.DeserializeObject<List<CarbonAbsorptionViewModel>>(result);

                // Add encLink to each row of List
                for (var i = 0; i <= carbonAbsorptions.Count - 1; i++)
                {
                    carbonAbsorptions[i].encLink_Edit = CustomQueryStringHelper.EncryptString("", "Edit", "CarbonAbsorption", new { enc_id = carbonAbsorptions[i].CarbonAbsorptionId, applicationId = "CC-1" });
                    carbonAbsorptions[i].encLink_Details = CustomQueryStringHelper.EncryptString("", "Details", "CarbonAbsorption", new { enc_id = carbonAbsorptions[i].CarbonAbsorptionId, applicationId = "CC-1" });
                    carbonAbsorptions[i].encLink_Delete = CustomQueryStringHelper.EncryptString("", "Delete", "CarbonAbsorption", new { enc_id = carbonAbsorptions[i].CarbonAbsorptionId, applicationId = "CC-1" });

                }
            }
            return Json(carbonAbsorptions);
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

        public string treeName;
        public async Task<JsonResult> GetTreeInfo(int treeId)
        {
            List<Tree> treeInfo = new List<Tree>();
            HttpClient client = _api.Initial();
            HttpResponseMessage res = await client.GetAsync("api/carbonabsorbtion/GetTreeInfo/" + treeId);
            if (res.IsSuccessStatusCode)
            {
                var result = res.Content.ReadAsStringAsync().Result;
                treeInfo = JsonConvert.DeserializeObject<List<Tree>>(result);
                treeName = treeInfo[0].TreeName;
            }
            return Json(treeInfo);
        }

        //Use Decrypt Attribute Filter
        [DecryptQueryStringParameter]
        public IActionResult Details(CarbonAbsorption carbonAbsorption, int id, string enc_id)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            //
            HttpClient client = _api.Initial();
            List<Tree> treeInfo = new List<Tree>();
            List<CarbonAbsorption> carbonAbsorptionList = new List<CarbonAbsorption>();
            List<CarbonAbsorptionViewModel> ListMonth = new List<CarbonAbsorptionViewModel>();
            List<CarbonAbsorptionTotalYear> ListYear = new List<CarbonAbsorptionTotalYear>();

            id = Convert.ToInt32(enc_id);

            if (id == 0)
                return View(new CarbonAbsorption());
            {
                HttpResponseMessage res = client.GetAsync("api/carbonabsorbtion/GetCarbonAbsorption/" + id.ToString()).Result;
                carbonAbsorptionList = res.Content.ReadAsAsync<List<CarbonAbsorption>>().Result;

                //Get this month accumulation
                HttpResponseMessage resMonth = client.GetAsync("api/carbonabsorbtion/GetCarbonAbsorptionTotalMonth/" + Convert.ToString(ViewBag.UserId) +  "/" + Convert.ToString(ViewBag.periodId)).Result;
                ListMonth = resMonth.Content.ReadAsAsync<List<CarbonAbsorptionViewModel>>().Result;

                //Get this year accumulation
                HttpResponseMessage resYear = client.GetAsync("api/carbonabsorbtion/GetCarbonAbsorptionTotalYear/" + Convert.ToString(ViewBag.UserId)).Result;
                ListYear = resYear.Content.ReadAsAsync<List<CarbonAbsorptionTotalYear>>().Result;

                //ViewBag.CarbAbsMonth = treeInfo[0].CarbonAbs / 12; 
                var amount = carbonAbsorptionList[0].Amount;
                //var age = carbonAbsorptionList[0].Age;

                //var treeName = 

                //
                HttpResponseMessage res2 = client.GetAsync("api/carbonabsorbtion/GetTreeInfo/" + carbonAbsorptionList[0].TreeId).Result;
                treeInfo = res2.Content.ReadAsAsync<List<Tree>>().Result;
                //
                ViewBag.TreeName = treeInfo[0].TreeName.ToString();
                ViewBag.Amount = amount;
                ViewBag.Age = carbonAbsorptionList[0].Age;
                ViewBag.treeEmision = treeInfo[0].CarbonAbs;
                ViewBag.treeEmisionYear = Math.Round(ListYear[0].TotalYear,2); //  .amount * treeInfo[0].CarbonAbs;
                ViewBag.treeEmisionMonth = Math.Round(ListMonth[0].CarbonAbs,2);  //(amount * treeInfo[0].CarbonAbs) / 12;
                carbonAbsorption.CarbAbs = ViewBag.treeEmisionMonth;
                
                //return View(res.Content.ReadAsAsync<CarbonAbsorption>().Result);
                //return PartialView("_DetailPartialView", res);
            }
            return View(carbonAbsorption);
        }

        // GET: HouseholdController/Create
        public IActionResult Create()
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }


            //Get Tree List
            HttpClient client = _api.Initial();
            HttpResponseMessage resTree = client.GetAsync("api/carbonabsorbtion/GetTreeList/").Result;
            List<Tree> _ListTree = new List<Tree>();
            _ListTree = resTree.Content.ReadAsAsync<List<Tree>>().Result;

            IEnumerable<SelectListItem> ListTree = from value in _ListTree
                                                   select new SelectListItem
                                                   {
                                                       Text = value.TreeName.ToString(),
                                                       Value = value.TreeId.ToString()
                                                   };

            //Put List to ViewBag
            ViewBag.ListTree = new SelectList(_ListTree.ToArray(), "TreeId", "TreeName");

            //CarbonAbsorption input = new CarbonAbsorption();
            //input.Amount = 1;
            //return View(input);
            return View(new CarbonAbsorption());
        }

        //POST: Carbon Absorption Controller/Create
       //[EncryptedActionParameter] // testing mode

       [HttpPost]
       [ValidateAntiForgeryToken]
        public IActionResult Create(CarbonAbsorption carbonAbsorption)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            HttpClient client = _api.Initial();
            List<Tree> treeInfo = new List<Tree>();
            List<CarbonAbsorptionViewModel_Sum> ListMonth = new List<CarbonAbsorptionViewModel_Sum>();
            List<CarbonAbsorptionTotalYear> ListYear = new List<CarbonAbsorptionTotalYear>();

            //Get Tree List
            HttpResponseMessage resTree = client.GetAsync("api/carbonabsorbtion/GetTreeList/").Result;            
            List<Tree> _ListTree = new List<Tree>();
            _ListTree = resTree.Content.ReadAsAsync<List<Tree>>().Result;

            IEnumerable<SelectListItem> ListTree = from value in _ListTree
                                                   select new SelectListItem
                                                   {
                                                       Text = value.TreeName.ToString(),
                                                       Value = value.TreeId.ToString()
                                                       //,Selected = value == vwVehicleList_User_Action.FuelId.ToString()
                                                   };

            if (carbonAbsorption.TreeId == null)
                ViewBag.ListTree = new SelectList(_ListTree, "TreeId", "TreeName");
            else
            { 

                ListTree = new SelectList(_ListTree.ToArray(), "TreeId", "TreeName", carbonAbsorption.TreeId);
                ViewBag.ListTree = ListTree;

            }
            
            
            ///var treeInfo = ListTree.Select(l => new SelectListItem { Selected = (l.Value == Convert.ToString(carbonAbsorption.TreeId)), Text = l.Text, Value = Convert.ToString(carbo*/nAbsorption.TreeId) }.Selected);
            //string test = ListTree.Select(l => new SelectListItem { Selected = (l.Value == Convert.ToString(carbonAbsorption.TreeId)), Text = l.Text, Value = Convert.ToString(carbonAbsorption.TreeId)}));
            
            //HttpClient client = _api.Initial();
            HttpResponseMessage res = client.GetAsync("api/carbonabsorbtion/GetTreeInfo/" + carbonAbsorption.TreeId).Result;
            treeInfo = res.Content.ReadAsAsync<List<Tree>>().Result;
            //
            var treeEmision = carbonAbsorption.TreeEmision().ToString();
            var treeId = carbonAbsorption.TreeId;
            var periodId = carbonAbsorption.PeriodeId;
            var age = carbonAbsorption.Age;
            var amount = carbonAbsorption.Amount;

            //Get accumulation from this month
            HttpResponseMessage resMonth = client.GetAsync("api/carbonabsorbtion/GetCarbonAbsorptionTotalMonth/" + ViewBag.UserId + "/" + ViewBag.periodId).Result;
            ListMonth = resMonth.Content.ReadAsAsync<List<CarbonAbsorptionViewModel_Sum>>().Result;

            //Get accumulation this year
            HttpResponseMessage resYear = client.GetAsync("api/carbonabsorbtion/GetCarbonAbsorptionTotalYear/" + ViewBag.UserId).Result;
            ListYear = resYear.Content.ReadAsAsync<List<CarbonAbsorptionTotalYear>>().Result;

            double SumMonth, SumYear;
            if (ListMonth.Count == 0)
                SumMonth = 0;
            else
                SumMonth = ListMonth[0].CarbonAbs;

            if (ListYear.Count == 0)
                SumYear = 0;
            else
                SumYear = ListYear[0].TotalYear;

            //
            ViewBag.treeName = treeInfo[0].TreeName;
            //
            ViewBag.treeEmision = amount * treeInfo[0].CarbonAbs;
            ViewBag.treeEmisionMonth = (amount * treeInfo[0].CarbonAbs) / 12;
            ViewBag.treeEmisionMonthAcc = SumMonth+ Convert.ToDouble(ViewBag.treeEmisionMonth);
            ViewBag.treeEmisionMonthAcc = Math.Round(ViewBag.treeEmisionMonthAcc, 2); 
            
            ViewBag.treeEmisionYear = SumYear + Convert.ToDouble(ViewBag.treeEmisionMonth);
            ViewBag.treeEmisionYear = Math.Round(ViewBag.treeEmisionYear, 2);
            ViewBag.treeId = treeId;
            ViewBag.periodId = periodId;
            ViewBag.age = age;
            ViewBag.amount = amount;

            return View(carbonAbsorption);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(CarbonAbsorption carbonAbsorption)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            HttpClient client = _api.Initial();

            try
            {
                carbonAbsorption.PeriodeId = Convert.ToInt32(ViewBag.periodId);// ViewBag.periodId; //ByRendy
                carbonAbsorption.UserId = Convert.ToInt32(ViewBag.UserId); //ByRendy

                HttpResponseMessage res = client.PostAsJsonAsync("api/carbonabsorbtion/", carbonAbsorption).Result;
                TempData["SuccessMessage"] = "Data telah disimpan";
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return BadRequest();
            }
        }

        //GET: CarbonAbsorptionController/Edit/5
        [DecryptQueryStringParameter]
        //public IActionResult Edit(int id = 0, string enc_id)
        public IActionResult Edit(int id, string enc_id)
        {
            SetViewBag();
            id = Convert.ToInt16(enc_id);

            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            HttpClient client = _api.Initial();

            if (id == 0)
                return View(new CarbonAbsorption());
            {
                //Get Tree List            
                HttpResponseMessage resTree = client.GetAsync("api/carbonabsorbtion/GetTreeList/").Result;
                List<Tree> _ListTree = new List<Tree>();
                _ListTree = resTree.Content.ReadAsAsync<List<Tree>>().Result;

                IEnumerable<SelectListItem> ListTree = from value in _ListTree
                                                       select new SelectListItem
                                                       {
                                                           Text = value.TreeName.ToString(),
                                                           Value = value.TreeId.ToString()                                                          
                                                       };


                //Put List to ViewBag
                ViewBag.ListTree = new SelectList(_ListTree.ToArray(), "TreeId", "TreeName");
                HttpResponseMessage res = client.GetAsync("api/carbonabsorbtion/" + id.ToString()).Result;
                return View(res.Content.ReadAsAsync<CarbonAbsorption>().Result);
            }

        }

        //Use Decrypt Attribute Filter
        //[DecryptQueryStringParameter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CarbonAbsorption carbonAbsorption, int id)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }

            //id = Convert.ToInt16(enc_id);

            HttpClient client = _api.Initial();
            
            //
            List<Tree> treeInfo = new List<Tree>();
            List<CarbonAbsorptionViewModel_Sum> ListMonth = new List<CarbonAbsorptionViewModel_Sum>();
            List<CarbonAbsorptionTotalYear> ListYear = new List<CarbonAbsorptionTotalYear>();
            List<CarbonAbsorption> carbonAbsorptionList = new List<CarbonAbsorption>();           


            HttpResponseMessage res = client.GetAsync("api/carbonabsorbtion/GetTreeInfo/" + carbonAbsorption.TreeId).Result;

            //HttpResponseMessage res = client.GetAsync("api/carbonabsorbtion/" + id.ToString()).Result;
            treeInfo = res.Content.ReadAsAsync<List<Tree>>().Result;

            //Get accumulation from this month
            HttpResponseMessage resMonth = client.GetAsync("api/carbonabsorbtion/GetCarbonAbsorptionTotalMonth/" + ViewBag.UserId + "/" + ViewBag.periodId).Result;
            ListMonth = resMonth.Content.ReadAsAsync<List<CarbonAbsorptionViewModel_Sum>>().Result;

            //Get accumulation this year
            HttpResponseMessage resYear = client.GetAsync("api/carbonabsorbtion/GetCarbonAbsorptionTotalYear/" + ViewBag.UserId).Result;
            ListYear = resYear.Content.ReadAsAsync<List<CarbonAbsorptionTotalYear>>().Result;

            //Get temp Carbon list
            HttpResponseMessage resTemp = client.GetAsync("api/carbonabsorbtion/GetCarbonAbsorption/" + id).Result;
            carbonAbsorptionList = resTemp.Content.ReadAsAsync<List<CarbonAbsorption>>().Result;

            var treeEmision = carbonAbsorption.TreeEmision().ToString();
            var treeId = carbonAbsorption.TreeId;
            var amount = carbonAbsorption.Amount;
            var age = carbonAbsorption.Age;
            var periodId = carbonAbsorption.PeriodeId;
            var treeName = carbonAbsorption.TreeName;
            //carbonAbsorption.CarbAbs = testList[0].CarbonAbs/amount;

            ViewBag.treeId = treeId;
            ViewBag.treeName = treeInfo[0].TreeName.ToString();
            ViewBag.amount = amount;
            ViewBag.age = age;
            ViewBag.periodId = periodId;
            ViewBag.treeEmision = amount*treeInfo[0].CarbonAbs;

            //set tmp variables
            double tempMonth = ListMonth[0].CarbonAbs - carbonAbsorptionList[0].CarbAbs;
            double tempYear = ListYear[0].TotalYear - carbonAbsorptionList[0].CarbAbs;

            ViewBag.treeEmisionMonth = Math.Round((ListMonth[0].CarbonAbs-tempMonth) + Convert.ToDouble((amount* treeInfo[0].CarbonAbs) / 12),2);
            ViewBag.treeEmisionYear = Math.Round((ListYear[0].TotalYear-tempYear) + Convert.ToDouble((amount * treeInfo[0].CarbonAbs) / 12),2);
            carbonAbsorption.CarbAbs = ViewBag.treeEmisionMonth;

            //return View(res.Content.ReadAsAsync<CarbonAbsorption>().Result);
            return View (carbonAbsorption);
        }


        //[DecryptQueryStringParameter]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(CarbonAbsorption carbonAbsorption)
        {
            SetViewBag();
            if (_api.userName == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            HttpClient client = _api.Initial();
            try
            {
                carbonAbsorption.PeriodeId = Convert.ToInt32(ViewBag.periodId); //Modified by JA
                carbonAbsorption.UserId = Convert.ToInt32(ViewBag.UserId); // Modified by JA

                HttpResponseMessage res = client.PutAsJsonAsync("api/carbonabsorbtion/" + carbonAbsorption.CarbonAbsorptionId, carbonAbsorption).Result;
                TempData["SuccessMessage"] = "Data telah diperbaharui";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return BadRequest();
            }
        }

        //Use Decrypt Attribute Filter
        //[DecryptQueryStringParameter]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString(_api.userName)  == null || ViewBag.UserId == 0)
            {
                return RedirectToAction("UserLogin", "User");
            }
            //SetViewBag();
            HttpClient client = _api.Initial();

            try 
            { 
            var carbonAbsorption = new CarbonAbsorption();
            HttpResponseMessage res = await client.DeleteAsync($"api/carbonabsorbtion/{id}");
            TempData["SuccessMessage"] = "Data telah dihapus";
            return RedirectToAction("Index");
            }

            catch
            {
                return BadRequest();
            }

        }

    }

}
