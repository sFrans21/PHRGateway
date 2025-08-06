using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Globalization;
using APIWHD.Data;
using APIWHD.Models;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace APIWHD.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoorporateNewsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly APIDBContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly SupperAppAPIDBContext _supappcontext;

        public CoorporateNewsController(IConfiguration configuration, APIDBContext context, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory, SupperAppAPIDBContext supappcontext)
        {
            _configuration = configuration; //?? throw new ArgumentNullException(nameof(configuration));
            _context = context; //?? throw new ArgumentNullException(nameof(context));
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
            _supappcontext = supappcontext;
        }

        /// <summary>
        /// Mengambil daftar semua berita didalam perusahaan.
        /// </summary> 
        [HttpGet("Getlist")]
        public async Task<IActionResult> GetList()
        {
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _supappcontext.CoorporateNews.ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
            
        }

        /// <summary>
        /// Mengambil detail berita perusahaan berdasarkan ID-nya.
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

                var result = await _supappcontext.CoorporateNews.Where(a => a.Id == Id).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }

        }

        //[HttpPost("upload")]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> AddEvents([FromForm] CalendarOfEvents eventData)
        //{
        //    try
        //    {
        //        if (eventData == null)
        //        {
        //            return BadRequest("Invalid data received");
        //        }

        //        // Additional validation if needed
        //        if (string.IsNullOrWhiteSpace(eventData.Title))
        //        {
        //            ModelState.AddModelError("Title", "Title is required");
        //        }

        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        var newCalendarEvent = new CalendarOfEvents
        //        {
        //            Title = eventData.Title,
        //            Location = eventData.Location,
        //            StartTime = eventData.StartTime ?? DateTime.UtcNow,
        //            EndTime = eventData.EndTime,
        //            Description = eventData.Description,
        //            Category = eventData.Category,
        //            AllDayEvent = eventData.AllDayEvent,
        //            Recurrence = eventData.Recurrence,
        //            Attachments = eventData.Attachments
        //        };

        //        _context.Add(newCalendarEvent);
        //        await _context.SaveChangesAsync();

        //        return Created($"api/calendar/{newCalendarEvent.Id}", newCalendarEvent);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error processing the request: {ex.Message}");
        //    }
        //}

    }
}
