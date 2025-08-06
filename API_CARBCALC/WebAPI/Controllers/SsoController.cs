using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebAPI.Models.RequestModels;
using WebAPI.Models.ResponseModels;

namespace WebAPI.Controllers
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
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> GetToken([FromForm] TokenRequestModel model)
        //public async Task<IActionResult> GetToken(TokenRequestModel model)
        {
            //return Ok("Test");
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
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponseModel>(result); // You need to use Newtonsoft.Json or System.Text.Json for deserialization


                // Store the access_token in the cache
                _memoryCache.Set("AccessToken", tokenResponse.access_token, TimeSpan.FromSeconds((double)tokenResponse.expires_in));
                _memoryCache.Set("Username", model.Username);
                return Ok(tokenResponse);
            }
            return BadRequest("Error while requesting token.");
        }


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
