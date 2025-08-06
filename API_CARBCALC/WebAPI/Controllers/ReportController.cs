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
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();
        public readonly APIDBContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public ReportController(APIDBContext context, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        #region Vw_Emission_Report
        //[HttpGet]
        //public List<VwEmission_Report> Get()
        //{
        //    return _context.VwEmission_Report.ToList();
        //}
        //[HttpGet]
        //[Route("GetCompanyFilter/{Company}")]
        //public List<VwEmission_Report> GetCompanyFilter(string Company)
        //{
        //    var vwReport = _context.VwEmission_Report.Where(a => a.Company == Company).ToList();

        //    return vwReport;
        //}

        //[HttpGet]
        //[Route("GetDateFilter/{month}/{year}")]
        //public List<VwEmission_Report> GetDateFilter(int month, int year)
        //{
        //    var vwReport = _context.VwEmission_Report.Where(a => a.Month == month && a.Year == year).ToList();

        //    return vwReport;
        //}

        //[HttpGet]
        //[Route("GetUsernameFilter/{username}")]
        //public List<VwEmission_Report> GetUsernameFilter(string username)
        //{
        //    var vwReport = _context.VwEmission_Report.Where(a => a.Name == username).ToList();

        //    return vwReport;
        //}
        #endregion


        #region Vw_Emission_Report_Field
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwEmission_Report_Field.ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }
        [HttpGet]
        [Route("GetCompanyFilter/{Company}")]
        public async Task<IActionResult> GetCompanyFilter(string Company)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwEmission_Report_Field.Where(a => a.Company == Company).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpGet]
        [Route("GetDateFilter/{month}/{year}")]
        public async Task<IActionResult> GetDateFilter(int month, int year)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwEmission_Report_Field.Where(a => a.Month == month && a.Year == year).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpGet]
        [Route("GetUsernameFilter/{username}")]
        public async Task<IActionResult> GetUsernameFilter(string username)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwEmission_Report_Field.Where(a => a.Name == username).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpPost]
        [Route("GetEmissionReport")]
        public async Task<IActionResult> GetEmissionReport(TblTUser _tblTUser)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);



                string sql = @"select *
		                 from [FN_GetReportAdmin](@userid)
                        ";


                var parameters = new[]
                {
                    new SqlParameter("@userid", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = _tblTUser.userID }
                };
                var result = await _context.VwEmission_Report_Field.FromSqlRaw(sql, parameters).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }
        #endregion


    }
}
