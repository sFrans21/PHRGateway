using APIWHD.Models;
using APIWHD.Models.RequestModels;
using APIWHD.Models.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace APIWHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;
        private readonly IMemoryCache _memoryCache;

        public UserController(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<UserController> logger, IMemoryCache memory)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
            _memoryCache = memory;
        }

        // api/User/GetToken
        // api/User/GetToken?userName={username}&Password={pass}
        /// <summary>
        /// Mendapatkan token akses untuk pengguna berdasarkan username dan password.
        /// </summary> 
        [HttpGet]
        [Route("GetToken")]
        public async Task<IActionResult> GetTokenAsync([FromQuery] UserData userProperties)
        {
            try
            {
                string apiUrl = _configuration["APIUrl"];
                string xAuth = _configuration["XAuth"];

                using HttpClient client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", xAuth);

                HttpResponseMessage response = await client.GetAsync($"{apiUrl}api/Account/GetToken?username={userProperties.UserName}&password={HttpUtility.UrlEncode(userProperties.Password)}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    // Process the response content and return the user data
                    return Ok(content);
                }
                else
                {
                    return BadRequest("Unable to retrieve user data");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // POST AIMAN GET TOKEN
        // Header (key = Authorization, value = Bearer YOUR_ACCESS_TOKEN)
        // body raw
        /// <summary>
        /// Mendapatkan token akses untuk pengguna berdasarkan username dan password.
        /// </summary> 
        [HttpPost]
        [Route("GetToken")]
        public async Task<IActionResult> GetTokenAsyncPost([FromBody] UserData userProperties)
        {
            try
            {
                string apiUrl = _configuration["APIUrl"];
                string xAuth = _configuration["XAuth"];

                using HttpClient client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", xAuth);

                var requestData = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("username", userProperties.UserName),
            new KeyValuePair<string, string>("password", userProperties.Password)
        };

                var requestContent = new FormUrlEncodedContent(requestData);

                //HttpResponseMessage response = await client.PostAsync($"{apiUrl}api/Account/GetToken", requestContent);
                HttpResponseMessage response = await client.GetAsync($"{apiUrl}api/Account/GetToken?username={userProperties.UserName}&password={HttpUtility.UrlEncode(userProperties.Password)}");

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    // Process the response content and return the user data
                    return Ok(content);
                }
                else
                {
                    return BadRequest("Unable to retrieve user data");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //api/User/GetUserData?UserName=username_here
        /// <summary>
        /// Mengambil properti atau data profil pengguna berdasarkan nama pengguna.
        /// </summary> 
        [HttpGet]
        [Route("GetUserprop/{username}")]
        //public async Task<IActionResult> GetUserDataAsync([FromQuery] UsernameModel UserProperties) // pakai model
        public async Task<IActionResult> GetUserPropAsync(string username)
        {
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Mengambil username yang disimpan dalam _memoryCache
                string storedUsername = _memoryCache.Get<string>("Username");

                // Membandingkan username dengan storedUsername
                if (username != storedUsername)
                {
                    return BadRequest("Username properties yang dicari tidak sesuai dengan username login");
                }

                try
                {
                    string apiurl = _configuration["APIUrl"];
                    string xAuth = _configuration["XAuth"];
                    string xProp = _configuration["XProp"];

                    using HttpClient client = _httpClientFactory.CreateClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    client.DefaultRequestHeaders.Add("X-User-Prop", xProp);
                    client.DefaultRequestHeaders.Add("X-User-Auth", xAuth);

                    //HttpResponseMessage response = await client.GetAsync($"{apiurl}api/Employee/GetAllMasterEmployee?whereCondition=EmpAccount%3D'{UserProperties.Username}'");
                    HttpResponseMessage response = await client.GetAsync($"{apiurl}api/Employee/GetAllMasterEmployee?whereCondition=EmpAccount%3D'{username}'");
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var employeeData = JsonConvert.DeserializeObject<AimanEmployeeData>(content);

                        // Periksa jika objek kosong
                        if (employeeData.Object != null && employeeData.Object.Count > 0)
                        {
                            // Process the response content and return the user data
                            //return Ok(content);
                            return Ok(employeeData);
                        }
                        else
                        {
                            return BadRequest("Data tidak ada");
                        }

                    }
                    else
                    {
                        return BadRequest("Unable to retrieve user data");
                    }
                }
                catch (Exception ex)
                {
                    //return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                    // Log the exception details for debugging
                    _logger.LogError(ex, "An error occurred while processing the request.");

                    // Return a generic error message to the client
                    return StatusCode(StatusCodes.Status500InternalServerError, "Sorry, an error occurred. Please try again later.");
                }
            }
            else
            {
                //Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }
    }
}