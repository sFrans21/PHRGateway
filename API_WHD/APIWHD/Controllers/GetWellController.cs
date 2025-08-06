using APIWHD.Data;
using APIWHD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using System.Net.Http.Headers;

namespace APIWHD.Controllers
{
    //api/GetWell/
    [Route("api/[controller]")]
    [ApiController]
    public class GetWellController : ControllerBase
    {
        public readonly APIDBContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public GetWellController(APIDBContext context, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        // api/GetWell/GetWellAll
        /// <summary>
        /// Mengambil semua data sumur.
        /// </summary> 
        [HttpGet]
        [Route("GetWellAll")]
        public async Task<IActionResult> GetWellAll()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwWellForDDL2.ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        // api/GetbyName/{search}
        /// <summary>
        /// Mengambil data sumur berdasarkan nama atau kata kunci pencarian.
        /// </summary> 
        [HttpGet]
        [Route("GetbyName/{search}")]
        public async Task<IActionResult> GetbyName(string search)
        {            
            //return await _context.VwWellForDDL2.ToListAsync();
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwWellForDDL2.Where(a => EF.Functions.Like(a.WellName, $"%{search}%") || EF.Functions.Like(a.WELL, $"%{search}%")).ToListAsync();

                if (result.Count == 0)
                {
                    return Ok($"Sumur {search} tidak ditemukan");
                }
                else
                {
                    return Ok(result);
                }
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

    }
}
