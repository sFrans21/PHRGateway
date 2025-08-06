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
    public class FilterController : ControllerBase
    {
        private readonly APIDBContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public FilterController(APIDBContext context, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Menyaring data laporan sumur berdasarkan berbagai kriteria seperti nama sumur, kategori, lokasi, tanggal, dan lainnya.
        /// </summary> 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VwWellReport>>> GetFilteredData
            (
                [FromQuery] string? wellName,
                [FromQuery] string? category,
                [FromQuery] string? tools,
                [FromQuery] string? location,
                [FromQuery] string? userName,
                [FromQuery] DateTime fromDate,
                [FromQuery] DateTime toDate
            )
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var query = _context.VwWellReport.AsQueryable();

                if (fromDate == DateTime.MinValue || toDate == DateTime.MinValue)
                {
                    return BadRequest("From date and To date required");
                }

                if (!string.IsNullOrEmpty(wellName))
                {
                    query = query.Where(w => w.NamaSumur == wellName);
                }

                if (!string.IsNullOrEmpty(category))
                {
                    query = query.Where(w => w.Kategori == category);
                }

                if (!string.IsNullOrEmpty(location))
                {
                    query = query.Where(w => w.Lokasi == location);
                }

                if (!string.IsNullOrEmpty(tools))
                {
                    query = query.Where(w => w.NamaPeralatan == tools);
                }

                if (!string.IsNullOrEmpty(userName))
                {
                    query = query.Where(w => w.DiubahOleh == userName);
                }

                query = query.Where(w => w.Tanggal2 >= fromDate && w.Tanggal2 <= toDate);

                var result = await query.ToListAsync();

                if (result.Count == 0)
                {
                    return NotFound("No data found");
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
