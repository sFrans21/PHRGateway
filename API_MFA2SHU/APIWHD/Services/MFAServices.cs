using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using APIWHD.Models;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net;
using System.Linq;
using OtpNet;
using Microsoft.Extensions.Configuration;

namespace APIWHD.Services
{

    public class MFAServices
    {

        private readonly IConfiguration _configuration;

        public MFAServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> GetMFASecret(MFASecretRequestModel data)
        {
            using (var handler = new HttpClientHandler() { CookieContainer = new CookieContainer() })
            {
                using (var client = new HttpClient(handler) { BaseAddress = new Uri(_configuration["APIUrlMFA"]) })
                {
                    // Construct the URL with query parameters
                    var url = $"/SHU_RESTAPI/api/Utilities/GetMFASecret?MFAAccount={data.MFAAccount}&MFAIssuer={data.MFAIssuer}";

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add("X-User-Prop", _configuration["XPropGetnGenerateMFA"]);
                    client.DefaultRequestHeaders.Add("X-User-Auth", _configuration["XAuth"]);

                    client.Timeout = TimeSpan.FromMilliseconds(10000);

                    // Log request details
                    Console.WriteLine("Request URL: " + url);

                    var res = await client.SendAsync(request);

                    // Log response details
                    Console.WriteLine("Response Status Code: " + res.StatusCode);
                    Console.WriteLine("Response Headers: " + res.Headers.ToString());

                    var responseContent = await res.Content.ReadAsStringAsync();
                    Console.WriteLine("Response Content: " + responseContent);

                    return responseContent;
                }
            }
        }

        public async Task<string> GenerateMFASecret(MFASecretRequestModel data)
        {
            using (var handler = new HttpClientHandler() { CookieContainer = new CookieContainer() })
            {
                using (var client = new HttpClient(handler) { BaseAddress = new Uri("https://svc.phe.pertamina.com") })
                {
                    // Construct the URL with query parameters
                    var url = $"/SHU_RESTAPI/api/Utilities/GenerateMFASecret?MFAAccount={data.MFAAccount}&MFAIssuer={data.MFAIssuer}";

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add("X-User-Prop", _configuration["XPropGetnGenerateMFA"]);
                    client.DefaultRequestHeaders.Add("X-User-Auth", _configuration["XAuth"]);

                    client.Timeout = TimeSpan.FromMilliseconds(10000);

                    // Log request details
                    Console.WriteLine("Request URL: " + url);

                    var res = await client.SendAsync(request);

                    // Log response details
                    Console.WriteLine("Response Status Code: " + res.StatusCode);
                    Console.WriteLine("Response Headers: " + res.Headers.ToString());

                    var responseContent = await res.Content.ReadAsStringAsync();
                    Console.WriteLine("Response Content: " + responseContent);

                    return responseContent;
                }
            }
        }

        public async Task<string> PushMFASecret(PushMFASecretModel data)
        {
            using (var handler = new HttpClientHandler() { CookieContainer = new CookieContainer() })
            {
                using (var client = new HttpClient(handler) { BaseAddress = new Uri("https://svc.phe.pertamina.com") })
                {
                    // Construct the URL with query parameters
                    var url = $"/SHU_RESTAPI/api/Utilities/PushMFASecret?MFAAccount={data.MFAAccount}&MFAIssuer={data.MFAIssuer}&MFASecret={data.MFASecret}&CreatedBy={data.MFAAccount}";

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    client.DefaultRequestHeaders.Add("X-User-Auth", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCIsImN0eSI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFwaS5yZWcxLmJ1cyIsIm5iZiI6MTczODY1MDg2OSwiZXhwIjoxNzcwMTg2ODY5LCJpYXQiOjE3Mzg2NTA4Njl9.ViuKycL9Tw4-mPL_kidYjSynxZf4iu9Dln_hCH7ZP0o");

                    client.Timeout = TimeSpan.FromMilliseconds(10000);

                    // Log request details
                    Console.WriteLine("Request URL: " + url);

                    var res = await client.SendAsync(request);

                    // Log response details
                    Console.WriteLine("Response Status Code: " + res.StatusCode);
                    Console.WriteLine("Response Headers: " + res.Headers.ToString());

                    var responseContent = await res.Content.ReadAsStringAsync();
                    Console.WriteLine("Response Content: " + responseContent);

                    return responseContent;
                }
            }
        }

        public bool OTPValid (string Otp, string secretKey)
        {
            if(string.IsNullOrEmpty(secretKey))
            {
                return false;
            }

            var otpVerifier = new Totp(Base32Encoding.ToBytes(secretKey));
            return otpVerifier.VerifyTotp(Otp, out _, VerificationWindow.RfcSpecifiedNetworkDelay);
        }
    }

}

    

