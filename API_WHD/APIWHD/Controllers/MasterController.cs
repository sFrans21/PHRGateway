using APIWHD.Data;
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
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly APIDBContext _context;
        private readonly ITransactionDashboardService _services;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public MasterController(APIDBContext context, ITransactionDashboardService services, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _services = services;
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Mengambil data master untuk peran pengguna.
        /// </summary> 
        [HttpGet]
        [Route("Role")]
        public async Task<IActionResult> GetRole()
        {
            var result = await _context.Whd_Role.ToListAsync();
            return Ok(result);
        }

        //Get Role using TOKEN from SSOLOGIN
        /// <summary>
        /// Mengambil data otorisasi peran pengguna berdasarkan token yang disimpan di cache.
        /// </summary> 
        [HttpGet]
        [Route("RoleAuth")]
        public async Task<IActionResult> GetRoleAuth()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.Whd_Role.ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        /// <summary>
        /// Mengambil data peran pengguna berdasarkan nama pengguna.
        /// </summary> 
        [HttpGet]
        [Route("Role/{username}")]
        public async Task<IActionResult> GetRolebyUser(string username)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _services.GetUserRole(username);
                if (result.Count == 0)
                {
                    return NotFound($"Tidak ada Role untuk username {username}");
                }
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }


        /// <summary>
        /// Mengambil data master untuk field.
        /// </summary> 
        [HttpGet("Field")]
        public async Task<IActionResult> GetField()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.Whd_Field.ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        /// <summary>
        /// Mengambil data master untuk tipe.
        /// </summary> 
        [HttpGet("Type")]
        public async Task<IActionResult> Gettype()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.Whd_Type.ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        /// <summary>
        /// Mengambil data master untuk peran pengguna.
        /// </summary> 
        [HttpGet("UserRole")]
        public async Task<IActionResult> GetUser()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.Whd_Users.ToListAsync();
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
