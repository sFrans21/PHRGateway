using APIWHD.Data;
using APIWHD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace APIWHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BroadcastController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly APIDBContext _context;
        private readonly SupperAppAPIDBContext _supappcontext;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;


        public BroadcastController(IConfiguration configuration, APIDBContext context, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory, SupperAppAPIDBContext supappcontext)
        {
            _configuration = configuration; //?? throw new ArgumentNullException(nameof(configuration));
            _context = context; //?? throw new ArgumentNullException(nameof(context));
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
            _supappcontext = supappcontext;
        }

        /// <summary>
        /// Mengambil daftar semua siaran yang tersedia.
        /// </summary> 
        [HttpGet("GetList")]
        public async Task<IActionResult> GetList()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _supappcontext.Broadcasts.ToListAsync();
                // Mengganti double backslash menjadi single backslash pada setiap nilai Filepath
                //result.ForEach(broadcast => broadcast.FilePath = broadcast.FilePath.Replace("\\\\", "\\"));
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        /// <summary>
        /// Mengambil detail siaran tertentu berdasarkan ID-nya.
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

                var result = await _supappcontext.Broadcasts.Where(a => a.sid == Id).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }

        }

        /// <summary>
        /// Mengunggah berkas siaran baru.
        /// </summary> 
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImageFile([FromForm] BroadcastUploadModel uploadModel)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                try
                {
                    if (uploadModel.files != null)
                    {
                        // Assuming that uploadModel.files is of type IFormFile
                        var formFile = uploadModel.files;

                        string extension = Path.GetExtension(formFile.FileName).ToLowerInvariant();

                        // Check if the file extension is one of the allowed types
                        if (extension == ".jpg" || extension == ".jpeg" || extension == ".png")
                        {
                            string uniqueFileName = $"{DateTime.Now:yyMMdd_hhmmss_fff}{extension}";
                            var userDirectory = Path.Combine(_configuration["AppSettings:ImgUploadFolderBroadcastPath"]);
                            var destinationPath = Path.Combine(userDirectory, uniqueFileName);
                            Directory.CreateDirectory(userDirectory);

                            var filePath = Path.Combine(_configuration["AppSettings:ImgBroadcastWeb"], uniqueFileName).Replace("\\", "/");

                            var broadcast = new Broadcast
                            {
                                FileName = uniqueFileName,
                                FilePath = filePath,
                                StartDate = DateTime.Now,
                                EndDate = DateTime.Now.AddDays(3)
                            };

                            using (var stream = formFile.OpenReadStream())
                            using (var fileStream = new FileStream(destinationPath, FileMode.Create))
                            {
                                await stream.CopyToAsync(fileStream);
                            }

                            _supappcontext.Add(broadcast);
                            await _supappcontext.SaveChangesAsync();

                            return Ok($"Upload successful!");
                        }
                        else
                        {
                            return BadRequest("Invalid file format. Only JPEG, JPG, and PNG files are allowed.");
                        }
                    }
                    else
                    {
                        return BadRequest("No files are accepted, make sure you already submit the image.");
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error processing the request: {ex.Message}");
                }
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }

        }

        /// <summary>
        /// Menghapus siaran berdasarkan ID-nya.
        /// </summary> 
        [HttpDelete("DeleteExpiredBroadcasts")]
        public async Task<IActionResult> DeleteExpiredBroadcasts([FromForm] string id)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                try
                {
                    if (!DateTime.TryParseExact(id, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime formattedDate))
                    {
                        return BadRequest("Invalid date format. Please provide the date in dd-MM-yyyy format.");
                    }


                    var expiredBroadcasts = await _supappcontext.Broadcasts.Where(b => b.EndDate <= formattedDate).ToListAsync();

                    foreach (var broadcast in expiredBroadcasts)
                    {
                        // Delete the file from storage
                        var filePath = Path.Combine(_configuration["AppSettings:ImgUploadFolderPath"], broadcast.FileName);
                        System.IO.File.Delete(filePath);

                        // Remove the broadcast from the database
                        _supappcontext.Broadcasts.Remove(broadcast);
                    }

                    _supappcontext.SaveChanges();

                    return Ok("Expired broadcasts deleted successfully.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error deleting expired broadcasts: {ex.Message}");
                }
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }


        }

        //[HttpDelete("DeleteFolderName")] //by form-data body dengan tambahan [FromForm]
        /// <summary>
        /// Menghapus siaran berdasarkan nama berkas.
        /// </summary> 
        [HttpDelete("DeleteFolderName")]
        //[HttpDelete("DeleteFolderName/{id}")] //by link
        //public async Task<IActionResult> DeleteFolderByDate([FromForm] string id)
        public async Task<IActionResult> DeleteFolderByDate(string id)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                try
                {
                    //if (!DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime formattedDate))
                    //{
                    //    return BadRequest("Invalid date format. Please provide the date in dd-MM-yyyy format.");
                    //}

                    if (id == null)
                    {
                        return BadRequest("Folder name Required!");
                    }

                    //var folderName = formattedDate.ToString("ddMMyyyy");
                    var folderName = id;
                    var folderPath = Path.Combine(_configuration["AppSettings:ImgUploadFolderBroadcastPath"], folderName);

                    var dbFileDelete = await _supappcontext.Broadcasts.Where(b => EF.Functions.Like(b.FilePath, $"%{folderName}%")).ToListAsync();
                    if (dbFileDelete.Count > 0)
                    {
                        foreach (var file in dbFileDelete)
                        {
                            //// Delete the corresponding folder and its contents
                            //var filePath = Path.Combine(_configuration["AppSettings:ImgUploadFolderBroadcastPath"], file.FilePath);
                            //if (Directory.Exists(filePath))
                            //{
                            //    Directory.Delete(filePath, true);
                            //}

                            // Remove the file record from the database
                            _supappcontext.Broadcasts.Remove(file);
                        }

                        _supappcontext.SaveChanges();
                    }

                    if (Directory.Exists(folderPath))
                    {
                        // Delete the folder and its contents
                        Directory.Delete(folderPath, true);

                        return Ok($"Folder '{folderName}' deleted successfully.");
                    }
                    else
                    {
                        return NotFound($"Folder '{folderName}' not found.");
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error deleting folder: {ex.Message}");
                }
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

    }
}
