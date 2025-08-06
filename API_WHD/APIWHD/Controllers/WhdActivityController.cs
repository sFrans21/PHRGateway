using APIWHD.Data;
using APIWHD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace APIWHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhdActivityController : ControllerBase
    {
        private readonly APIDBContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public WhdActivityController(APIDBContext context, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        // api/WhdActivity
        /// <summary>
        /// Mengambil daftar aktivitas.
        /// </summary> 
        [HttpGet]
        public async Task<IActionResult> GetActivity()
        {

            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);


                var result = await _context.Whd_Activity.ToListAsync();
                if (result.Count == 0)
                {
                    return BadRequest($"No Activity");
                }
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
