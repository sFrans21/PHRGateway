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
    //api/WhdCategory/
    [Route("api/[controller]")]
    [ApiController]
    public class WhdCategoryController : ControllerBase
    {
        public readonly APIDBContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public WhdCategoryController(APIDBContext context, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        // Get Category All
        // api/WhdCategory/GetCategoryAll
        /// <summary>
        /// Mengambil semua data kategori.
        /// </summary> 
        [HttpGet]
        [Route("GetCategoryAll")]
        public async Task<IActionResult> GetCategoryAll()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.Whd_Category.ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        // Get Category by CategoryDesc clue
        /// <summary>
        /// Mengambil data kategori berdasarkan petunjuk deskripsi kategori.
        /// </summary> 
        [HttpGet]
        [Route("GetCategorybyCategoryClue/{categorydesc}")]
        public async Task<IActionResult> GetCategorybyCategoryClue(string categorydesc)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.Whd_Category.Where(a => EF.Functions.Like(a.CategoryDesc, $"%{categorydesc}%")).ToListAsync();
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
