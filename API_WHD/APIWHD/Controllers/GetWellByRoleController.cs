using APIWHD.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APIWHD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using System.Net.Http.Headers;

namespace APIWHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetWellByRoleController : ControllerBase
    {       
        //public readonly LoginViewModel _loginViewModel;
        public readonly APIDBContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public GetWellByRoleController(APIDBContext context, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        // api/GetWellByRole
        /// <summary>
        /// Mengambil data sumur berdasarkan peran pengguna.
        /// </summary> 
        [HttpGet]
        //[Route("Get")]
        //public async Task<List<VwGetWellByRole>> Get()
        //public async Task<ActionResult<List<VwGetWellByRole>>> Get()
        public async Task<IActionResult> Get()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwGetWellByRole.ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        // api/GetWellByRole/Get/username
        /// <summary>
        /// Mengambil data sumnur berdasarkan ID pengguna. 
        /// </summary> 
        [HttpGet]
        //[Route("Get")]
        [Route("{userid}")]
        //public async Task<List<VwGetWellByRole>> Getwell(string userid)
        public async Task<IActionResult> Getwell(string userid)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                int count = await _context.VwGetWellByRole.Where(a => a.User_Name == userid).CountAsync(); // count total list
                var result = await _context.VwGetWellByRole.Where(a => a.User_Name == userid).ToListAsync();
                if (result.Count == 0)
                {
                    return NotFound($"Tidak ada data");
                }
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        // api/GetWellByRole/count
        //[HttpGet]
        //[Route("count/{userid}")]
        //public async Task<int> CountData(string userid)
        //{
        //    //string username = AddFilter();
        //    //userid = username;

        //    int count = await _context.VwGetWellByRole
        //        .Where(a => a.User_Name == userid)
        //        .CountAsync();

        //    return count;
        //}
    }
}
