//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;
//using System;
//using APIWHD.Models;

using APIWHD.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using APIWHD.Services;
using System;
using Newtonsoft.Json;
using System.Linq;
using OtpNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net;

namespace APIWHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MFAController : ControllerBase
    {
        private readonly MFAServices _mfaServices;
        private readonly IConfiguration _configuration;

        public MFAController(MFAServices mfaServices, IConfiguration configuration)
        {
            _mfaServices = mfaServices;
            _configuration = configuration;
        }

        //[HttpPost]
        //[Route("GetOrGenerateMFASecret")]
        //public async Task<IActionResult> GetOrGenerateMFASecret([FromBody] MFASecretRequestModel request)
        //{
        //    // Process the incoming request
        //    Console.WriteLine($"Received request: {request.MFAAccount}, {request.MFAIssuer}");

        //    // Call the GetMFASecret method from the service with dynamic values
        //    var getResult = await _mfaServices.GetMFASecret(request);

        //    // Deserialize the JSON response
        //    var mfaSecretResponse = JsonConvert.DeserializeObject<MFASecretResponse>(getResult);

        //    // Check if the MFASecret is not null
        //    if (mfaSecretResponse.Object != null && mfaSecretResponse.Object.Count > 0)
        //    {
        //        var firstObject = mfaSecretResponse.Object.First();
        //        if (firstObject.MFASecret != null && firstObject.MFASecretTOTP != null)
        //        {
        //            // Store the MFA secret in a temporary storage (e.g., session)
        //            HttpContext.Session.SetString("MFASecret", firstObject.MFASecret);
        //            HttpContext.Session.SetString("MFAAccount", request.MFAAccount);
        //            HttpContext.Session.SetString("MFAIssuer", request.MFAIssuer);
        //            HttpContext.Session.SetString("IsFromGet", "true");

        //            return Ok(new { success = true, message = "MFA secret retrieved successfully."});
        //        }
        //    }

        //    // If MFASecret is null, call the GenerateMFASecret method
        //    var generateResult = await _mfaServices.GenerateMFASecret(request);
        //    var generateMfaSecretResponse = JsonConvert.DeserializeObject<MFASecretResponse>(generateResult);

        //    if (generateMfaSecretResponse.Object != null && generateMfaSecretResponse.Object.Count > 0)
        //    {
        //        var firstGeneratedObject = generateMfaSecretResponse.Object.First();

        //        // Store the generated MFA secret in a temporary storage (e.g., session)
        //        HttpContext.Session.SetString("MFASecret", firstGeneratedObject.MFASecret);
        //        HttpContext.Session.SetString("MFAAccount", request.MFAAccount);
        //        HttpContext.Session.SetString("MFAIssuer", request.MFAIssuer);
        //        HttpContext.Session.SetString("IsFromGet", "false");

        //        return Ok(new { success = true, message = "MFA secret generated successfully.", MFASecret = firstGeneratedObject.MFASecret, MFASecretTOTP = firstGeneratedObject.MFASecretTOTP });
        //    }
        //    else
        //    {
        //        return NotFound(new { success = false, message = "No MFA secret found."});
        //    }
        //}

        /// <summary>
        /// Mendapatkan kode rahasia MFA atau menghasilkan kode baru jika tidak ada yang tersedia.
        /// </summary> 
        [HttpPost]
        [Route("GetOrGenerateMFASecret")]
        public async Task<IActionResult> GetOrGenerateMFASecret([FromBody] MFASecretRequestModel request, [FromServices] SessionService sessionService)
        {


            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            //var ipAddress = "10.236.241.25";

            if (ipAddress == null)
            {
                return BadRequest("Unable to retrieve IP address.");
            }

            IPAddress clientIp = IPAddress.Parse(ipAddress);

            // Fetch all subnets from the database
            var subnets = listPrivateSubnet().ToList();

            var matchingSubnet = subnets.FirstOrDefault(subnet =>
            {
                IPAddress subnetIp = IPAddress.Parse(subnet.IPAddress);
                IPAddress subnetMask = IPAddress.Parse(subnet.SubnetMask);

                IPAddress clientNetworkAddress = SubnetHelper.GetNetworkAddress(clientIp, subnetMask);
                IPAddress subnetNetworkAddress = SubnetHelper.GetNetworkAddress(subnetIp, subnetMask);

                return clientNetworkAddress.Equals(subnetNetworkAddress);
            });

            if (matchingSubnet != null)
            {
                return Ok(new { success = true, message = "P6C", ipAddress });
            }
            else
            {
                // Generate a session ID
                var sessionId = Guid.NewGuid().ToString();

                var isFromGet = false;
                var MFASecret = "";
                var MFASecretTOTP = "";

                // Call the GetMFASecret method from the service with dynamic values
                request.MFAIssuer = _configuration["MFAIssuer"];
                var getResult = await _mfaServices.GetMFASecret(request);

                // Deserialize the JSON response
                var mfaSecretResponse = JsonConvert.DeserializeObject<MFASecretResponse>(getResult);

                // Check if the MFASecret is not null
                if (mfaSecretResponse.Object != null && mfaSecretResponse.Object.Count > 0)
                {
                    var firstObject = mfaSecretResponse.Object.First();
                    if (firstObject.MFASecret != null && firstObject.MFASecretTOTP != null)
                    {
                        // Store the MFA secret in the server-side storage
                        await sessionService.StoreSessionDataAsync(sessionId, new MFASecretData
                        {
                            MFASecret = firstObject.MFASecret,
                            MFASecretTOTP = firstObject.MFASecretTOTP,
                            MFAAccount = request.MFAAccount,
                            MFAIssuer = _configuration["MFAIssuer"],
                            IsFromGet = true
                        });
                        isFromGet = true;
                        MFASecret = firstObject.MFASecret;
                        MFASecretTOTP = firstObject.MFASecretTOTP;
                        return Ok(new { success = true, message = "MFA secret retrieved successfully.", sessionId, isFromGet, MFASecret, MFASecretTOTP, ipAddress });
                    }
                }

                // If MFASecret is null, call the GenerateMFASecret method
                var generateResult = await _mfaServices.GenerateMFASecret(request);
                var generateMfaSecretResponse = JsonConvert.DeserializeObject<MFASecretResponse>(generateResult);

                if (generateMfaSecretResponse.Object != null && generateMfaSecretResponse.Object.Count > 0)
                {
                    var firstGeneratedObject = generateMfaSecretResponse.Object.First();

                    // Store the generated MFA secret in the server-side storage
                    await sessionService.StoreSessionDataAsync(sessionId, new MFASecretData
                    {
                        MFASecret = firstGeneratedObject.MFASecret,
                        MFASecretTOTP = firstGeneratedObject.MFASecretTOTP,
                        MFAAccount = request.MFAAccount,
                        MFAIssuer = _configuration["MFAIssuer"],
                        IsFromGet = false
                    });
                    isFromGet = false;
                    MFASecret = firstGeneratedObject.MFASecret;
                    MFASecretTOTP = firstGeneratedObject.MFASecretTOTP;
                    return Ok(new { success = true, message = "MFA secret generated successfully.", sessionId, isFromGet, MFASecret, MFASecretTOTP, ipAddress });
                }
                else
                {
                    return NotFound(new { success = false, message = "No MFA secret found.", sessionId });
                }
            }


        }

        //[HttpPost]
        //[Route("ValidateAndPushMFASecret")]
        //public async Task<IActionResult> ValidateAndPushMFASecret([FromBody] MFAValidationRequestModel request)
        //{
        //    // Retrieve the stored MFA secret from the session
        //    var secretKey = HttpContext.Session.GetString("MFASecret");
        //    var mfaAccount = HttpContext.Session.GetString("MFAAccount");
        //    var mfaIssuer = HttpContext.Session.GetString("MFAIssuer");
        //    var isFromGet = HttpContext.Session.GetString("IsFromGet");

        //    if (string.IsNullOrEmpty(secretKey))
        //    {
        //        return BadRequest(new { message = "SecretKey cannot be null or empty." });
        //    }

        //    var otpVerifier = new Totp(Base32Encoding.ToBytes(secretKey));
        //    bool isValid = otpVerifier.VerifyTotp(DateTime.UtcNow, request.Otp, out _, VerificationWindow.RfcSpecifiedNetworkDelay);

        //    if (!isValid)
        //    {
        //        return Ok(new { isValid });
        //    }

        //    // If the secret is from GenerateMFASecret, push it
        //    if (isFromGet == "false")
        //    {
        //        var pushResult = await _mfaServices.PushMFASecret(new PushMFASecretModel
        //        {
        //            MFAAccount = mfaAccount,
        //            MFAIssuer = mfaIssuer,
        //            MFASecret = secretKey,
        //            CreatedBy = mfaIssuer
        //        });

        //        var pushMfaSecretResponse = JsonConvert.DeserializeObject<MFASecretResponse>(pushResult);

        //        if (pushMfaSecretResponse.Object == null || pushMfaSecretResponse.Object.Count == 0)
        //        {
        //            return Ok(new { message = pushMfaSecretResponse.Message });
        //        }
        //    }

        //    // Clear the session data after processing
        //    HttpContext.Session.Remove("MFASecret");
        //    HttpContext.Session.Remove("MFAAccount");
        //    HttpContext.Session.Remove("MFAIssuer");
        //    HttpContext.Session.Remove("IsFromGet");

        //    return Ok(new { isValid });
        //}

        /// <summary>
        /// Memvalidasi dan mendorong kode One-Time Password (OTP) dan ID sesi yang dikirimkan, untuk menyelesaikan proses autentikasi MFA.
        /// </summary> 
        [HttpPost]
        [Route("ValidateAndPushMFASecret")]
        public async Task<IActionResult> ValidateAndPushMFASecret([FromBody] MFAValidationRequestModel request, [FromServices] SessionService sessionService)
        {
            // Retrieve the stored MFA secret from the server-side storage
            var sessionData = await sessionService.RetrieveSessionDataAsync(request.SessionId);

            if (sessionData == null || string.IsNullOrEmpty(sessionData.MFASecret))
            {
                return BadRequest(new { message = "Session data not found or SecretKey cannot be null or empty." });
            }

            var otpVerifier = new Totp(Base32Encoding.ToBytes(sessionData.MFASecret));
            bool isValid = otpVerifier.VerifyTotp(DateTime.UtcNow, request.Otp, out _, VerificationWindow.RfcSpecifiedNetworkDelay);

            if (!isValid)
            {
                return Ok(new { isValid });
            }

            // If the secret is from GenerateMFASecret, push it
            if (!sessionData.IsFromGet)
            {
                var pushResult = await _mfaServices.PushMFASecret(new PushMFASecretModel
                {
                    MFAAccount = sessionData.MFAAccount,
                    MFAIssuer = _configuration["MFAIssuer"],
                    MFASecret = sessionData.MFASecret,
                    CreatedBy = sessionData.MFAAccount
                });

                var pushMfaSecretResponse = JsonConvert.DeserializeObject<MFASecretResponse>(pushResult);

                if (pushMfaSecretResponse.Object == null || pushMfaSecretResponse.Object.Count == 0)
                {
                    return Ok(new { message = pushMfaSecretResponse.Message });
                }
            }

            // Clear the session data after processing
            await sessionService.ClearSessionDataAsync(request.SessionId);

            return Ok(new { isValid });
        }
        

    //    private List<Subnet> listPrivateSubnet()
    //    {
    //        return new List<Subnet>() {
    //    //new Subnet() { SubnetName="Private 10", IPAddress= "10.0.0.0", SubnetMask="10.255.255.255", Description="Private 10" },
    //    new Subnet() { SubnetName="Private 10", IPAddress= "10.236.227.0", SubnetMask="255.255.255.0", Description="Private 10" },
    //    new Subnet() { SubnetName="Private 10", IPAddress= "10.236.252.0", SubnetMask="255.255.254.0", Description="Private 10" },
    //    //new Subnet() { SubnetName="Private 10 VPN", IPAddress= "10.252.212.0", SubnetMask="255.255.252.0", Description="Private 10 VPN" },
    //    new Subnet() { SubnetName="Local", IPAddress= "127.0.0.0", SubnetMask="127.255.255.254", Description="Local 127" },
    //    new Subnet() { SubnetName="Private 172", IPAddress= "172.16.0.0", SubnetMask="172.31.255.255", Description="Private 172" },
    //    new Subnet() { SubnetName="Private 192", IPAddress= "192.168.0.0", SubnetMask="192.168.255.255", Description="Private 192" }

    //};
    //    }

        private List<Subnet> listPrivateSubnet()
        {
            return new List<Subnet>() {
        //new Subnet() { SubnetName="Private 10", IPAddress= "10.0.0.0", SubnetMask="10.255.255.255", Description="Private 10" },
        new Subnet() { SubnetName="Private 10 VPN", IPAddress= "10.252.212.0", SubnetMask="255.255.252.0", Description="Private 10 VPN"},
        //new Subnet() { SubnetName="Local", IPAddress= "127.0.0.0", SubnetMask="127.255.255.254", Description="Local 127" },
        //new Subnet() { SubnetName="Private 172", IPAddress= "172.16.0.0", SubnetMask="172.31.255.255", Description="Private 172" },
        //new Subnet() { SubnetName="Private 192", IPAddress= "192.168.0.0", SubnetMask="192.168.255.255", Description="Private 192" },

        //segmentasi rdtx
        new Subnet() { SubnetName="LAN Lt. 11", IPAddress= "10.236.226.0", SubnetMask="255.255.255.0", Description="LAN Lt. 11" },
        new Subnet() { SubnetName="LAN Lt. 12", IPAddress= "10.236.227.0", SubnetMask="255.255.255.0", Description="LAN Lt. 12" },
        new Subnet() { SubnetName="LAN Lt. 15", IPAddress= "10.236.228.0", SubnetMask="255.255.255.0", Description="LAN Lt. 15" },
        new Subnet() { SubnetName="LAN Lt. 16", IPAddress= "10.236.229.0", SubnetMask="255.255.255.0", Description="LAN Lt. 16" },
        new Subnet() { SubnetName="LAN Lt. 17", IPAddress= "10.236.230.0", SubnetMask="255.255.255.0", Description="LAN Lt. 17" },
        new Subnet() { SubnetName="LAN & Wifi Lt. 10", IPAddress= "10.236.246.0", SubnetMask="255.255.254.0", Description="LAN & Wifi Lt. 10" },
        new Subnet() { SubnetName="LAN & Wifi Lt. 46", IPAddress= "10.236.245.0", SubnetMask="255.255.255.0", Description="LAN & Wifi Lt. 46" },
        new Subnet() { SubnetName="Wifi Lt. 11", IPAddress= "10.236.238.0", SubnetMask="255.255.255.0", Description="Wifi Lt. 11" },
        new Subnet() { SubnetName="Wifi Lt. 12", IPAddress= "10.236.252.0", SubnetMask="255.255.254.0", Description="Wifi Lt. 12" },
        new Subnet() { SubnetName="Wifi Lt. 15", IPAddress= "10.236.240.0", SubnetMask="255.255.255.0", Description="Wifi Lt. 15" },
        new Subnet() { SubnetName="Wifi Lt. 16", IPAddress= "10.236.241.0", SubnetMask="255.255.255.0", Description="Wifi Lt. 16" },
        new Subnet() { SubnetName="Wifi Lt. 17", IPAddress= "10.236.242.0", SubnetMask="255.255.255.0", Description="Wifi Lt. 17" },
        //segmentasi zona
        new Subnet() { SubnetName="Regional 1 Zona 1 Pangkalan Susu - Wampu", IPAddress= "10.201.32.0", SubnetMask="255.255.255.0", Description="Regional 1 Zona 1 Pangkalan Susu - Wampu" },
        new Subnet() { SubnetName="Regional 4 Zona 13 Donggi", IPAddress= "10.205.32.0", SubnetMask="255.255.252.0", Description="Regional 4 Zona 13 Donggi" },
        new Subnet() { SubnetName="Regional 4 Zona 13 Matindok", IPAddress= "10.205.48.0", SubnetMask="255.255.252.0", Description="Regional 4 Zona 13 Matindok" },
        new Subnet() { SubnetName="Regional 4 Zona 14 Papua - Salawati", IPAddress= "10.207.16.0", SubnetMask="255.255.255.0", Description="Regional 4 Zona 14 Papua - Salawati" },
        new Subnet() { SubnetName="Regional 4 Zona 14 Papua - Sele Linda", IPAddress= "10.207.17.0", SubnetMask="255.255.255.0", Description="Regional 4 Zona 14 Papua - Sele Linda" },
        new Subnet() { SubnetName="Regional 1 Zona 1 Siak", IPAddress= "10.246.112.0", SubnetMask="255.255.255.0", Description="Regional 1 Zona 1 Siak" },
        new Subnet() { SubnetName="Regional 1 Zona 1 Siak", IPAddress= "10.249.66.0", SubnetMask="255.255.255.0", Description="Regional 1 Zona 1 Siak" },
        new Subnet() { SubnetName="Regional 1 Zona 1 Siak", IPAddress= "10.249.68.0", SubnetMask="255.255.255.0", Description="Regional 1 Zona 1 Siak" },
        new Subnet() { SubnetName="Regional 1 Zona 1 Siak", IPAddress= "10.252.167.0", SubnetMask="255.255.255.0", Description="Regional 1 Zona 1 Siak" },
        new Subnet() { SubnetName="Regional 1 Zona 1 Siak", IPAddress= "10.249.62.0", SubnetMask="255.255.255.0", Description="Regional 1 Zona 1 Siak" },
        new Subnet() { SubnetName="Regional 1 Zona 1 Kampar", IPAddress= "10.249.71.0", SubnetMask="255.255.255.0", Description="Regional 1 Zona 1 Kampar" },
        new Subnet() { SubnetName="Regional 1 Zona 1 NSO", IPAddress= "10.252.168.0", SubnetMask="255.255.255.0", Description="Regional 1 Zona 1 NSO" },
        new Subnet() { SubnetName="Regional 1 Zona 1 Jambi Merang", IPAddress= "10.252.144.0", SubnetMask="255.255.255.0", Description="Regional 1 Zona 1 Jambi Merang" },
        new Subnet() { SubnetName="Regional 2 Pertamina EP Head Office", IPAddress= "10.12.0.0", SubnetMask="255.255.0.0", Description="Regional 2 Pertamina EP Head Office" },
        new Subnet() { SubnetName="Regional 1 Zona 1 Jambi - Kasang", IPAddress= "10.202.44.0", SubnetMask="255.255.254.0", Description="Regional 1 Zona 1 Jambi - Kasang" },
        new Subnet() { SubnetName="Regional 1 Zona 1 Jambi - Kenali Asam", IPAddress= "10.202.96.0", SubnetMask="255.255.224.0", Description="Regional 1 Zona 1 Jambi - Kenali Asam" },
        new Subnet() { SubnetName="Regional 1 Zona 1 Pangkalan Susu", IPAddress= "10.201.16.0", SubnetMask="255.255.240.0", Description="Regional 1 Zona 1 Pangkalan Susu" },
        new Subnet() { SubnetName="Regional 1 Zona 1 Rantau", IPAddress= "10.201.0.0", SubnetMask="255.255.248.0", Description="Regional 1 Zona 1 Rantau" },
        new Subnet() { SubnetName="Regional 1 Zona 1 Lirik", IPAddress= "10.202.0.0", SubnetMask="255.255.240.0", Description="Regional 1 Zona 1 Lirik" },
        new Subnet() { SubnetName="Regional 1 Zona 1 Lirik - Buatan", IPAddress= "10.202.23.0", SubnetMask="255.255.255.0", Description="Regional 1 Zona 1 Lirik - Buatan" },
        new Subnet() { SubnetName="Regional 1 Zona 1 Lirik - Ukui", IPAddress= "10.202.26.0", SubnetMask="255.255.255.0", Description="Regional 1 Zona 1 Lirik - Ukui" },
        new Subnet() { SubnetName="Regional 1 & WK Rokan Head Office", IPAddress= "10.236.224.0", SubnetMask="255.255.224.0", Description="Regional 1 & WK Rokan Head Office" },
        new Subnet() { SubnetName="Regional 1 Zona 2 & 3", IPAddress= "10.236.0.0", SubnetMask="255.255.128.0", Description="Regional 1 Zona 2 & 3" },
        new Subnet() { SubnetName="Regional 1 Zona 2 & 3", IPAddress= "10.236.128.0", SubnetMask="255.255.192.0", Description="Regional 1 Zona 2 & 3" },
        new Subnet() { SubnetName="Regional 1 Zona 2 & 3", IPAddress= "10.236.192.0", SubnetMask="255.255.224.0", Description="Regional 1 Zona 2 & 3" },
        new Subnet() { SubnetName="Regional 1 Zona 4 Ramba", IPAddress= "10.202.64.0", SubnetMask="255.255.240.0", Description="Regional 1 Zona 4 Ramba" },
        new Subnet() { SubnetName="Regional 1 Zona 4 Ramba - Mangunjaya", IPAddress= "10.202.72.0", SubnetMask="255.255.255.0", Description="Regional 1 Zona 4 Ramba - Mangunjaya" },
        new Subnet() { SubnetName="Regional 1 Zona 4 Ramba - Bentayan", IPAddress= "10.202.24.0", SubnetMask="255.255.255.0", Description="Regional 1 Zona 4 Ramba - Bentayan" },
        new Subnet() { SubnetName="Regional 1 Zona 4 Prabumulih Office", IPAddress= "10.203.0.0", SubnetMask="255.255.192.0", Description="Regional 1 Zona 4 Prabumulih Office" },
        new Subnet() { SubnetName="Regional 1 Zona 4 Adera", IPAddress= "10.203.96.0", SubnetMask="255.255.240.0", Description="Regional 1 Zona 4 Adera" },
        new Subnet() { SubnetName="Regional 1 Zona 4 Pendopo", IPAddress= "10.203.64.0", SubnetMask="255.255.224.0", Description="Regional 1 Zona 4 Pendopo" },
        new Subnet() { SubnetName="Regional 1 Zona 4 Ogan Komering", IPAddress= "10.252.140.0", SubnetMask="255.255.255.0", Description="Regional 1 Zona 4 Ogan Komering" },
        new Subnet() { SubnetName="Regional 1 Zona 4 Ogan Komering", IPAddress= "10.252.170.192", SubnetMask="255.255.255.192", Description="Regional 1 Zona 4 Ogan Komering" },
        new Subnet() { SubnetName="Regional 1 Zona 4 Raja Tempirai", IPAddress= "10.252.156.0", SubnetMask="255.255.255.0", Description="Regional 1 Zona 4 Raja Tempirai" },
        new Subnet() { SubnetName="Regional 2 Zona 7 Klayan Office", IPAddress= "10.204.0.0", SubnetMask="255.255.192.0", Description="Regional 2 Zona 7 Klayan Office" },
        new Subnet() { SubnetName="Regional 2 Zona 7 Subang", IPAddress= "10.204.128.0", SubnetMask="255.255.192.0", Description="Regional 2 Zona 7 Subang" },
        new Subnet() { SubnetName="Regional 2 Zona 7 Jatibarang", IPAddress= "10.204.64.0", SubnetMask="255.255.192.0", Description="Regional 2 Zona 7 Jatibarang" },
        new Subnet() { SubnetName="Regional 2 Zona 7 Tambun", IPAddress= "10.204.192.0", SubnetMask="255.255.192.0", Description="Regional 2 Zona 7 Tambun" },
        new Subnet() { SubnetName="Regional 3 Zona 9 Balikpapan Office", IPAddress= "10.206.96.0", SubnetMask="255.255.240.0", Description="Regional 3 Zona 9 Balikpapan Office" },
        new Subnet() { SubnetName="Regional 3 Zona 9 Sangasanga", IPAddress= "10.206.128.0", SubnetMask="255.255.240.0", Description="Regional 3 Zona 9 Sangasanga" },
        new Subnet() { SubnetName="Regional 3 Zona 9 Sangasanga", IPAddress= "10.206.16.0", SubnetMask="255.255.254.0", Description="Regional 3 Zona 9 Sangasanga" },
        new Subnet() { SubnetName="Regional 3 Zona 9 Sangasanga - Samboja", IPAddress= "10.206.2.0", SubnetMask="255.255.254.0", Description="Regional 3 Zona 9 Sangasanga - Samboja" },
        new Subnet() { SubnetName="Regional 3 Zona 9 Sangasanga - Samboja", IPAddress= "10.206.19.0", SubnetMask="255.255.255.192", Description="Regional 3 Zona 9 Sangasanga - Samboja" },
        new Subnet() { SubnetName="Regional 3 Zona 9 Sangasanga - Anggana", IPAddress= "10.206.19.64", SubnetMask="255.255.255.192", Description="Regional 3 Zona 9 Sangasanga - Anggana" },
        new Subnet() { SubnetName="Regional 3 Zona 9 Sangasanga - Anggana", IPAddress= "10.206.0.0", SubnetMask="255.255.254.0", Description="Regional 3 Zona 9 Sangasanga - Anggana" },
        new Subnet() { SubnetName="Regional 3 Zona 9 Sangatta", IPAddress= "10.206.12.0", SubnetMask="255.255.254.0", Description="Regional 3 Zona 9 Sangatta" },
        new Subnet() { SubnetName="Regional 3 Zona 9 Sangatta", IPAddress= "10.206.14.0", SubnetMask="255.255.255.0", Description="Regional 3 Zona 9 Sangatta" },
        new Subnet() { SubnetName="Regional 3 Zona 9 Sangatta", IPAddress= "10.206.28.0", SubnetMask="255.255.254.0", Description="Regional 3 Zona 9 Sangatta" },
        new Subnet() { SubnetName="Regional 3 Zona 9 Sangatta - Semberah", IPAddress= "10.206.15.0", SubnetMask="255.255.255.0", Description="Regional 3 Zona 9 Sangatta - Semberah" },
        new Subnet() { SubnetName="Regional 3 Zona 9 Tanjung", IPAddress= "10.80.129.0", SubnetMask="255.255.255.0", Description="Regional 3 Zona 9 Tanjung" },
        new Subnet() { SubnetName="Regional 3 Zona 9 Tanjung", IPAddress= "10.206.32.0", SubnetMask="255.255.240.0", Description="Regional 3 Zona 9 Tanjung" },
        new Subnet() { SubnetName="Regional 3 Zona 9 Tanjung - Batubutok", IPAddress= "10.206.22.0", SubnetMask="255.255.255.0", Description="Regional 3 Zona 9 Tanjung - Batubutok" },
        new Subnet() { SubnetName="Regional 3 Zona 9 Tanjung - Longikis", IPAddress= "10.206.23.0", SubnetMask="255.255.255.0", Description="Regional 3 Zona 9 Tanjung - Longikis" },
        new Subnet() { SubnetName="Regional 3 Zona 10 Tarakan", IPAddress= "10.206.64.0", SubnetMask="255.255.248.0", Description="Regional 3 Zona 10 Tarakan" },
        new Subnet() { SubnetName="Regional 3 Zona 10 Tarakan - Sembakung", IPAddress= "10.206.20.0", SubnetMask="255.255.255.0", Description="Regional 3 Zona 10 Tarakan - Sembakung" },
        new Subnet() { SubnetName="Regional 3 Zona 10 Tarakan - Sembakung", IPAddress= "10.206.160.0", SubnetMask="255.255.248.0", Description="Regional 3 Zona 10 Tarakan - Sembakung" },
        new Subnet() { SubnetName="Regional 3 Zona 10 Bunyu", IPAddress= "10.206.48.0", SubnetMask="255.255.248.0", Description="Regional 3 Zona 10 Bunyu" },
        new Subnet() { SubnetName="Regional 4 Zona 11 Surabaya Office", IPAddress= "10.205.16.0", SubnetMask="255.255.248.0", Description="Regional 4 Zona 11 Surabaya Office" },
        new Subnet() { SubnetName="Regional 4 Zona 11 Cepu", IPAddress= "10.205.0.0", SubnetMask="255.255.240.0", Description="Regional 4 Zona 11 Cepu" },
        new Subnet() { SubnetName="Regional 4 Zona 11 Poleng", IPAddress= "10.205.64.0", SubnetMask="255.255.248.0", Description="Regional 4 Zona 11 Poleng" },
        new Subnet() { SubnetName="Regional 4 Zona 11 Sukowati", IPAddress= "10.205.56.0", SubnetMask="255.255.248.0", Description="Regional 4 Zona 11 Sukowati" },
        new Subnet() { SubnetName="Regional 4 Zona 14 Papua", IPAddress= "10.207.1.0", SubnetMask="255.255.248.0", Description="Regional 4 Zona 14 Papua" },
        new Subnet() { SubnetName="Regional 4 Zona 14 Papua - Klamono", IPAddress= "10.207.0.0", SubnetMask="255.255.255.0", Description="Regional 4 Zona 14 Papua - Klamono" }


    };
        }
    }
}
