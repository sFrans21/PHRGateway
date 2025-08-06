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
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        public readonly APIDBContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminController(APIDBContext context, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
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
        

        [Route("GetTreeInfo/{TreeId}")]
        public async Task<IActionResult> GetTreeInfo(int treeID)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblTree.Where(a => a.TreeId == treeID).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }

        }

        [Route("GetTreeList/")]
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

        }

        [Route("GetAllUsers/")]
        public async Task<IActionResult> GetAllUsers()
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

        [Route("GetListUser/")]
        public async Task<IActionResult> GetListUser()
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


        public async Task<IActionResult> Post([FromBody] TblTree treeInfo, [FromForm] FnTree fnTree)
        {
            //HttpClient client = _api.Initial();
            //List<TblTree> treeInfo = new List<TblTree>();
            //HttpResponseMessage res = client.GetAsync("api/carbonabsorbtion/GetTreeInfo/" + carbonAbsorption.TreeId).Result;
            //treeInfo = res.Content.ReadAsStringAsync<List<TblTree>>().Result;

            //treeInfo = GetTreeInfo(carbonAbsorption.TreeId);

            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                if (!ModelState.IsValid)
                    return BadRequest("Not a valid model");

                treeInfo.TreeName = fnTree.TreeName(treeInfo);
                treeInfo.CarbonAbs = fnTree.CarbonAbs(treeInfo);


                _context.TblTree.Add(treeInfo);
                //_context.SaveChanges();
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        

        // PUT api/<AdminnController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TblTree treeInfo, [FromForm] FnTree fnTree)
        {

            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                if (id != treeInfo.TreeId)
                {
                    return BadRequest();
                }

                //List<TblTree> treeInfo = new List<TblTree>();
                //HttpResponseMessage res = client.GetAsync("api/carbonabsorbtion/GetTreeInfo/" + carbonAbsorption.TreeId).Result;
                //treeInfo = res.Content.ReadAsStringAsync<List<TblTree>>().Result;

                //treeInfo = GetTreeInfo(treeInfo2.TreeId);

                treeInfo.TreeId = fnTree.TreeId(treeInfo);
                treeInfo.TreeName = fnTree.TreeName(treeInfo);
                treeInfo.CarbonAbs = fnTree.CarbonAbs(treeInfo);

                _context.Entry(treeInfo).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreeExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
            

        }


        //[HttpPut("{id}")]

        //public async Task<IActionResult> Put(int id, [FromBody] TblVehicle VehicleInfo)
        //{
        //    if (id != VehicleInfo.VehicleId)
        //    {
        //        return BadRequest();
        //    }

        //    //List<TblTree> treeInfo = new List<TblTree>();
        //    //HttpResponseMessage res = client.GetAsync("api/carbonabsorbtion/GetTreeInfo/" + carbonAbsorption.TreeId).Result;
        //    //treeInfo = res.Content.ReadAsStringAsync<List<TblTree>>().Result;

        //    //treeInfo = GetTreeInfo(treeInfo2.TreeId);


        //    _context.Entry(VehicleInfo).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TreeExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();

        //}

        // DELETE api/<AdminController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var treeInfo = await _context.TblTree.FindAsync(id);
                if (treeInfo == null)
                {
                    return NotFound();
                }

                _context.TblTree.Remove(treeInfo);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }            
        }

        private bool TreeExists(int id)
        {
            return _context.TblTCarbonAbsorption.Any(e => e.CarbonAbsorptionId == id);
        }
    }
}
