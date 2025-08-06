using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APIWHD.Data;
using APIWHD.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.Extensions.Caching.Memory;

namespace APIWHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsBroadcastController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly APIDBContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public NewsBroadcastController(IConfiguration configuration, APIDBContext context, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration; //?? throw new ArgumentNullException(nameof(configuration));
            _context = context; //?? throw new ArgumentNullException(nameof(context));
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;

        }

        /// <summary>
        /// Mengambil daftar siaran berita.
        /// </summary> 
        [HttpGet("Getlist")]
        public async Task<IActionResult> GetList()
        {
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.NewsBroadcasts.ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }


        }

        /// <summary>
        /// Mengambil detail siaran berita berdasarkan ID-nya.
        /// </summary> 
        [HttpGet]
        [Route("GetById/{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.NewsBroadcasts.Where(a => a.Id == Id).ToListAsync();
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
