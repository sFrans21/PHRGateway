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
    public class SummaryController : ControllerBase
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();
        public readonly APIDBContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public SummaryController(APIDBContext context, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
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
        [Route("GetSum/{userid}/{periodeid}/{fromdate}/{todate}")]
        public async Task<IActionResult> GetSum(string userid, string periodeid, string fromdate, string todate)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string sql = @"select UserID, PeriodeId,PeriodeDetail,
		                ListrikBulanBerjalanKWH ,
		                 GasBulanBerjalanM3 ,
		                 ListrikBulanBerjalanKWH_PerOrang ,
		                 GasBulanBerjalanM3_PerOrang ,
		                 TonCO2Listrik ,
		                 TonCO2BBM ,
                         TONCO2Gas ,
                         BBMBulan ,
                         BBMTahun ,
		                 CarbonAbsPerMonth ,
		                 TreeAmount ,
		                 TotalEmisiTonCO2,
                        CreatedDate,
                        FromDate,
                        ToDate
		                 from [FN_GetSummary](@userid,@periodeid,@From,@To)
                        ";


                var parameters = new[]
                {
                new SqlParameter("@userid", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = userid },
                new SqlParameter("@periodeid", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = periodeid },
                new SqlParameter("@From", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = fromdate },
                new SqlParameter("@To", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = todate }
            };
                var result = await _context.FnGetSummary.FromSqlRaw(sql, parameters).ToListAsync();
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
