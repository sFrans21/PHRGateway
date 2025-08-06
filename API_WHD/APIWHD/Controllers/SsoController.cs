using APIWHD.Models;
using APIWHD.Models.RequestModels;
using APIWHD.Models.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIWHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SsoController : ControllerBase
    {
        private readonly IHttpClientFactory _httpclientfactory;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        public SsoController(IHttpClientFactory httpClientFactory, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _configuration = configuration;
            _httpclientfactory = httpClientFactory;
            _memoryCache = memoryCache;
        }

        //api/ssologin => (header with key Content-Type, and value application/json), ({"username": "username","password": "password"} on body and raw)
        //[HttpPost]
        //[Route("Login")]
        //public async Task<IActionResult> GetToken([FromForm] TokenRequestModel model)
        ////public async Task<IActionResult> GetToken(TokenRequestModel model)
        //{
        //    //return Ok("Test");
        //    var client = _httpclientfactory.CreateClient();
        //    var url = _configuration["LoginSSO:ssouri"];
        //    var parameter = new Dictionary<string, string>
        //    {
        //        {"client_id", _configuration["LoginSSO:client_id"] },
        //        {"client_secret", _configuration["LoginSSO:client_secret"] },
        //        {"grant_type", _configuration["LoginSSO:grant_type"] },
        //        {"scope", _configuration["LoginSSO:scope"] },
        //        {"username", model.Username },
        //        {"password", model.Password }
        //    };
        //    var response = await client.PostAsync(url, new FormUrlEncodedContent(parameter));
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = await response.Content.ReadAsStringAsync();
        //        var tokenResponse = JsonConvert.DeserializeObject<TokenResponseModel>(result); // You need to use Newtonsoft.Json or System.Text.Json for deserialization


        //        // Store the access_token in the cache
        //        _memoryCache.Set("AccessToken", tokenResponse.access_token, TimeSpan.FromSeconds((double)tokenResponse.expires_in));
        //        _memoryCache.Set("Username", model.Username);
        //        return Ok(tokenResponse);
        //    }
        //    return BadRequest("Error while requesting token.");
        //}

        //api/ssologin => (header with key Content-Type, and value application/json), ({"username": "username","password": "password"} on body and form-data)
        /// <summary>
        /// Untuk proses login SSO dengan mengirimkan username dan password.
        /// </summary> 
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> GetToken([FromForm] TokenRequestModel model)
        {
            try
            {
                var client = _httpclientfactory.CreateClient();
                var url = _configuration["LoginSSO:ssouri"];
                var parameter = new Dictionary<string, string>
                    {
                        {"client_id", _configuration["LoginSSO:client_id"] },
                        {"client_secret", _configuration["LoginSSO:client_secret"] },
                        {"grant_type", _configuration["LoginSSO:grant_type"] },
                        {"scope", _configuration["LoginSSO:scope"] },
                        {"username", model.Username },
                        {"password", model.Password }
                    };

                var response = await client.PostAsync(url, new FormUrlEncodedContent(parameter));

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponseModel>(result);

                    // Simpan access_token dalam cache
                    //_memoryCache.Set("AccessToken", tokenResponse.access_token, TimeSpan.FromSeconds((double)tokenResponse.expires_in)); // token expired in 300 seconds (based on SSO)
                    _memoryCache.Set("AccessToken", tokenResponse.access_token, TimeSpan.FromMinutes(15)); // token expired in 15 minutes
                    _memoryCache.Set("Username", model.Username);
                    return Ok(tokenResponse);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return BadRequest($"Error while requesting token: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Handle HTTP request specific errors
                return StatusCode(StatusCodes.Status503ServiceUnavailable, $"Error while requesting token: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                // Handle any other types of exceptions
                return StatusCode(StatusCodes.Status500InternalServerError, $"An unexpected error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Mengambil proses keluar (logout) dari SSO dengan menghapus token yang tersimpan di cache.
        /// </summary> 
        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()
        {
            // Remove the access token from cache
            _memoryCache.Remove("AccessToken");
            _memoryCache.Remove("Username");
            return Ok("Logged out successfully.");
        }
    }
}
