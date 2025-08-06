using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Function;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarbonAbsorbtionController : ControllerBase
    {
        public readonly APIDBContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public CarbonAbsorbtionController(APIDBContext context, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }
        // GET: api/<CarbonAbsorbtionController>
        [HttpGet]
        
        [Route("GetCarbonAbsorption/{userid}/{periodeID}")]
        public async Task<IActionResult> Get(int userid, int periodeID)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblTCarbonAbsorptionViewModel_Sum.Where(a => a.UserId == userid && a.PeriodeId == periodeID).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [Route("GetCarbonAbsorptionTotalMonth/{userid}/{periodeID}")]
        public async Task<IActionResult> GetTotalMonth(int userid, int periodeID)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblTCarbonAbsorptionViewModel_Sum.Where(a => a.UserId == userid && a.PeriodeId == periodeID && a.CarbonAbsorptionId == 0).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        //Get Total in Year 
        [Route("GetCarbonAbsorptionTotalYear/{userid}")]
        public async Task<IActionResult> GetTotalYear(int userid)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblTCarbonAbsorptionViewModel_SumYear.Where(a => a.UserId == userid).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [Route("GetCarbonAbsorption/{CarbonAbsorptionId}")]
        public async Task<IActionResult> Get(int CarbonAbsorptionId)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblTCarbonAbsorptionViewModel_Sum.Where(a => a.CarbonAbsorptionId == CarbonAbsorptionId).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }       

        //public List<TblTCarbonAbsorptionViewModel> Get(int CarbonAbsorptionId)
        //{
        //    return _context.TblTCarbonAbsorptionViewModel.Where(a => a.CarbonAbsorptionId == CarbonAbsorptionId).ToList();
        //}

        [Route("GetCarbonAbsorptionId/{CarbonAbsorptionId}")]
        public async Task<IActionResult> GetCarbonAbsorptionId(int CarbonAbsorptionId)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblTCarbonAbsorption.Where(a => a.CarbonAbsorptionId == CarbonAbsorptionId).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [Route("GetTreeInfo/{TreeId}")]
        public List<TblTree> GetTreeInfo(int treeID)
        {
            return _context.TblTree.Where(a => a.TreeId == treeID).ToList();
        }

        [Route("GetTreeList/")]
        //[Authorize]
        public async Task<IActionResult> GetTreeList()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblTree.OrderBy(o => o.TreeId).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
            //if (HttpContext.Session.GetString("_UserId") != null)
        }


        // GET api/<CarbonAbsorbtionController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblTCarbonAbsorptionViewModel_Sum.Where(a => a.CarbonAbsorptionId == id).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpPost]
        [Route("PostCarbonAbsorption")]
        public async Task<IActionResult> PostCarbonAbsorption([FromBody] TblTCarbonAbsorption carbonAbsorption)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                //HttpClient client = _api.Initial();
                List<TblTree> treeInfo = new List<TblTree>();
                //HttpResponseMessage res = client.GetAsync("api/carbonabsorbtion/GetTreeInfo/" + carbonAbsorption.TreeId).Result;
                //treeInfo = res.Content.ReadAsStringAsync<List<TblTree>>().Result;

                treeInfo = GetTreeInfo(carbonAbsorption.TreeId);

                if (!ModelState.IsValid)
                    return BadRequest("Not a valid model");


                //carbonAbsorption.TreeId = fnCarbonAbsoprtion.TreeId(carbonAbsorption);
                //carbonAbsorption.Amount = fnCarbonAbsoprtion.Amount(carbonAbsorption);
                //carbonAbsorption.Age = fnCarbonAbsoprtion.Age(carbonAbsorption);
                //carbonAbsorption.CarbAbs = Convert.ToDecimal(carbonAbsorption.Amount * treeInfo[0].CarbonAbs) / 12;
                ////carbonAbsorption.PeriodeId = fnCarbonAbsoprtion.Periode(carbonAbsorption);

                carbonAbsorption.CreateDate = DateTime.Now;
                //carbonAbsorption.UserId = 1221; //userid test

                _context.TblTCarbonAbsorption.Add(carbonAbsorption);
                //_context.SaveChanges();
                await _context.SaveChangesAsync();
                return Ok("Created!");
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TblTCarbonAbsorption carbonAbsorption, [FromForm] FnCarbonAbsoprtion fnCarbonAbsoprtion)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                //HttpClient client = _api.Initial();
                List<TblTree> treeInfo = new List<TblTree>();
                //HttpResponseMessage res = client.GetAsync("api/carbonabsorbtion/GetTreeInfo/" + carbonAbsorption.TreeId).Result;
                //treeInfo = res.Content.ReadAsStringAsync<List<TblTree>>().Result;

                treeInfo = GetTreeInfo(carbonAbsorption.TreeId);

                if (!ModelState.IsValid)
                    return BadRequest("Not a valid model");


                carbonAbsorption.TreeId = fnCarbonAbsoprtion.TreeId(carbonAbsorption);
                carbonAbsorption.Amount = fnCarbonAbsoprtion.Amount(carbonAbsorption);
                carbonAbsorption.Age = fnCarbonAbsoprtion.Age(carbonAbsorption);
                carbonAbsorption.CarbAbs = Convert.ToDecimal(carbonAbsorption.Amount * treeInfo[0].CarbonAbs) / 12;
                //carbonAbsorption.PeriodeId = fnCarbonAbsoprtion.Periode(carbonAbsorption);

                carbonAbsorption.CreateDate = DateTime.Now;
                //carbonAbsorption.UserId = 1221; //userid test

                _context.TblTCarbonAbsorption.Add(carbonAbsorption);
                await _context.SaveChangesAsync();
                return Ok("Created!");
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
            
        }

        // PUT api/<CarbonAbsorbtionController>/5
        [HttpPut("{id}")]       
        public async Task<IActionResult> Put(int id, [FromBody] TblTCarbonAbsorption carbonAbsorption, [FromForm] FnCarbonAbsoprtion fnCarbonAbsoprtion)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                if (id != carbonAbsorption.CarbonAbsorptionId)
                {
                    return BadRequest();
                }

                List<TblTree> treeInfo = new List<TblTree>();
                //HttpResponseMessage res = client.GetAsync("api/carbonabsorbtion/GetTreeInfo/" + carbonAbsorption.TreeId).Result;
                //treeInfo = res.Content.ReadAsStringAsync<List<TblTree>>().Result;

                treeInfo = GetTreeInfo(carbonAbsorption.TreeId);

                carbonAbsorption.TreeId = fnCarbonAbsoprtion.TreeId(carbonAbsorption);
                carbonAbsorption.Amount = fnCarbonAbsoprtion.Amount(carbonAbsorption);
                carbonAbsorption.Age = fnCarbonAbsoprtion.Age(carbonAbsorption);
                carbonAbsorption.CarbAbs = Convert.ToDecimal(carbonAbsorption.Amount * treeInfo[0].CarbonAbs) / 12;
                //carbonAbsorption.PeriodeId = fnCarbonAbsoprtion.Periode(carbonAbsorption);
                carbonAbsorption.CreateDate = DateTime.Now;
                //carbonAbsorption.UserId = 1221;
                _context.Entry(carbonAbsorption).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarbonAbsorbtionExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok($"Data with ID {id} updated!");
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
            

        }

        // DELETE api/<CarbonAbsorbtionController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var carbonAbsorption = await _context.TblTCarbonAbsorption.FindAsync(id);
                if (carbonAbsorption == null)
                {
                    return NotFound();
                }
                
                _context.TblTCarbonAbsorption.Remove(carbonAbsorption);
                await _context.SaveChangesAsync();

                return Ok($"Data with ID {id} deleted!");
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
            
        }

        private bool CarbonAbsorbtionExists(int id)
        {
            return _context.TblTCarbonAbsorption.Any(e => e.CarbonAbsorptionId == id);
        }
    }
}
