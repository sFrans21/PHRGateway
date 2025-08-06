using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Function;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

using Microsoft.Data.SqlClient;
//using EFSample.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.Http.Headers;
using Microsoft.Extensions.Caching.Memory;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeriodeController : ControllerBase
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();
        public readonly APIDBContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public PeriodeController(APIDBContext context, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}


        [HttpGet]
        [Route("GetListSummary/{userid}")]
        public async Task<IActionResult> GetListSummary(int userid)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var vehicleemision = await _context.VwPeriodeListSummary.Where(a => a.UserId == userid && a.TotalEmission != 0).ToListAsync();
                //return _context.VwVehicleList_User.ToList();

                //vehicleemision = vehicleemision.OrderBy(a => a.Month).ThenBy(a => a.Year).ToList();
                vehicleemision = vehicleemision.OrderBy(a => a.Year).ThenBy(a => a.Month).ToList();
                return Ok(vehicleemision);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpGet]
        [Route("GetListSummary_Chart/{userid}")]
        public async Task<IActionResult> GetListSummary_Chart(int userid)        
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwPeriodeListSummary_Chart.Where(a => a.UserId == userid && a.TotalEmission != 0).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }
    }
}
