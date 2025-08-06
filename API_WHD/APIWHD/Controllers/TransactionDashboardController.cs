using APIWHD.Data;
using APIWHD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace APIWHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionDashboardController : ControllerBase
    {
        private readonly ITransactionDashboardService _dashboard;
        private readonly IConfiguration _configuration;
        private readonly APIDBContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public TransactionDashboardController
            (
                ITransactionDashboardService dashboardService
                , APIDBContext context
                , IConfiguration configuration
                , IMemoryCache memoryCache
                , IHttpClientFactory httpClientFactory
            )
        {
            _dashboard = dashboardService;
            _context = context;
            _configuration = configuration;
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        //api/TransactionDashboard/sp/{userName} -- using SP digunakan untuk beranda
        /// <summary>
        /// Mengambil data dashboard transaksi berdasarkan nama pengguna. SP digunakan untuk beranda.
        /// </summary> 
        [HttpGet("sp/{userName}")]
        public async Task<IActionResult> GetTransactionDashboard(string userName)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var dashboardItems = await _dashboard.GetTransactionDashboardAsync(userName);
                // Mendapatkan nilai baseuri dari appsettings.json
                //string baseUri = _configuration["AppSettings:baseuri"];
                //// Menggabungkan nilai baseuri dengan fileName
                //foreach (var item in dashboardItems)
                //{
                //    if (!string.IsNullOrEmpty(item.Filename))
                //    {
                //        string filePath = item.Filename[1..];
                //        item.Filename = Path.Combine(baseUri, filePath).Replace('\\', '/');
                //    }
                //    else
                //    {
                //        item.Filename = null;
                //    }
                //}
                return Ok(dashboardItems);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        // api/TransactionDashboard/GetAll -- digunakan untuk seluruh pelaporan
        /// <summary>
        /// Mengambil data dashboard transaksi berdasarkan nama pengguna.
        /// </summary> 
        [HttpGet]
        [Route("{userName}")]
        public async Task<IActionResult> GetbyUserName(string userName)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwWellReport.Where(a => a.Uploader == userName && a.FileName != null).ToListAsync();
                int count = await _context.VwWellReport.Where(a => a.Uploader == userName && a.FileName != null).CountAsync();

                // Mendapatkan nilai baseuri dari appsettings.json
                //string baseUri = _configuration["AppSettings:baseuri"];
                //// Menggabungkan nilai baseuri dengan fileName
                //foreach (var item in result)
                //{
                //    if (!string.IsNullOrEmpty(item.FileName))
                //    {
                //        string filePath = item.FileName[1..];
                //        item.FileName = Path.Combine(baseUri, filePath).Replace('\\', '/');
                //    }
                //    else
                //    {
                //        item.FileName = null;
                //    }
                //}
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        // api/TransactionDashboard/GetAll
        /// <summary>
        /// Mengambil semua data dashboard transaksi.
        /// </summary> 
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwWellReport.Where(a => a.FileName != null).ToListAsync();
                int count = await _context.VwWellReport.Where(a => a.FileName != null).CountAsync();

                // Mendapatkan nilai baseuri dari appsettings.json
                //string baseUri = _configuration["AppSettings:baseuri"];
                //// Menggabungkan nilai baseuri dengan fileName
                //foreach (var item in result)
                //{
                //    if (!string.IsNullOrEmpty(item.FileName))
                //    {
                //        string filePath = item.FileName[1..];
                //        item.FileName = Path.Combine(baseUri, filePath).Replace('\\', '/');
                //    }
                //    else
                //    {
                //        item.FileName = null;
                //    }
                //}
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        // api/TransactionDashboard/GetById/{id}
        /// <summary>
        /// Mengambil detail transaksi berdasarkan ID transaksi.
        /// </summary> 
        [HttpGet]
        [Route("GetById/{transactionId}")]
        public async Task<IActionResult> GetById(int transactionId)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                //var result = await _context.VwWellReport.Where(a => a.ID == transactionId).ToListAsync();

                // Execute the stored procedure
                var result = await _context.TransactionDetail
                    .FromSqlRaw("EXEC USP_GetTransactionDetail @DocId = {0}", transactionId)
                    .ToListAsync();

                // Mendapatkan nilai baseuri dari appsettings.json
                //string baseUri = _configuration["AppSettings:baseuri"];
                // Menggabungkan nilai baseuri dengan fileName
                //foreach (var item in result)
                //{
                //    if (!string.IsNullOrEmpty(item.FileName))
                //    {
                //        string filePath = item.FileName[1..];
                //        item.FileName = Path.Combine(baseUri, filePath).Replace('\\', '/');
                //    }
                //    else
                //    {
                //        item.FileName = null;
                //    }
                //}
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        // api/TransactionDashboard/GetById/{id}
        //[HttpGet]
        //[Route("GetBydocumentId/{id}")]
        //public async Task<IActionResult> GetBydocumentId(int id)
        //{
        //    var result = await _context.VwWellReport.Where(a => a.ID == id).ToListAsync();

        //    // Mendapatkan nilai baseuri dari appsettings.json
        //    string baseUri = _configuration["AppSettings:baseuri"];
        //    // Menggabungkan nilai baseuri dengan fileName
        //    foreach (var item in result)
        //    {
        //        item.FileName = Path.Combine(baseUri, item.FileName);
        //    }
        //    return Ok(result);
        //}

        //  api/TransactionDashboard/DasboardResult
        /// <summary>
        /// Mengambil hasil data untuk dashboard.
        /// </summary> 
        [HttpGet]
        [Route("DasboardResult")]
        public async Task<IActionResult> GetDasboardResult()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwDashMyWHD.ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        // Add Transaction
        /// <summary>
        /// Menambahkan data transaksi baru.
        /// </summary> 
        [HttpPost]
        public async Task<IActionResult> PostTransaction(Whd_Transaction transaction)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                if (ModelState.IsValid)
                {
                    transaction.CreatedAt = DateTime.Now;
                    transaction.UpdatedAt = DateTime.Now;
                    #region logic change name into number
                    //string activityDesc = transaction.Description.ToLower();
                    //if (activityDesc.Contains("drilling"))
                    //{
                    //    transaction.ActivityID = 1;
                    //}
                    //else if (activityDesc.Contains("work over"))
                    //{
                    //    transaction.ActivityID = 2;
                    //}
                    //else if (activityDesc.Contains("intervention"))
                    //{
                    //    transaction.ActivityID = 3;

                    //}
                    //else if (activityDesc.Contains("well service"))
                    //{
                    //    transaction.ActivityID = 4;
                    //}
                    //else
                    //{
                    //    transaction.ActivityID = 0;
                    //}
                    #endregion
                    _context.Add(transaction);
                    await _context.SaveChangesAsync();
                    //return Ok(transaction); // result semua kolom pada model
                    return Ok(new { transaction.TransactionID }); // result hanya "transactionID": value
                }
                return BadRequest(ModelState);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        // Edit Transaction
        /// <summary>
        /// Memeperbaharui data transaksi berdasarkan ID-nya.
        /// </summary> 
        [HttpPut]
        [Route("updatetransaction/{id}")]
        public async Task<IActionResult> PutTransaction(int id, Whd_Transaction transaction)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                if (id != transaction.TransactionID)
                {
                    return BadRequest("ID Transaksi tidak ditemukan atau tidak sesuai mohon dicek kembali");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var existingTransaction = await _context.Whd_Transaction.FindAsync(id);

                        if (existingTransaction == null)
                        {
                            return NotFound();
                        }

                        existingTransaction.WellID = transaction.WellID;
                        existingTransaction.CatgoryDetailID = transaction.CatgoryDetailID;
                        existingTransaction.ToolsID = transaction.ToolsID;
                        existingTransaction.RealtimePelaporan = transaction.RealtimePelaporan;
                        existingTransaction.ActivityID = transaction.ActivityID;
                        existingTransaction.Description = transaction.Description;
                        existingTransaction.UpdatedAt = DateTime.Now;
                        existingTransaction.UpdatedBy = transaction.UpdatedBy;

                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TransactionExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    //return NoContent();
                    return Ok(transaction);
                }
                return BadRequest(ModelState); ;
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        #region HTTPDELETE tanpa menghapus image dalam path
        //HTTPDELETE tanpa menghapus image dalam path
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTransaction(int id)
        //{
        //    var transaction = await _context.Whd_Transaction.FindAsync(id);
        //    if (transaction == null)
        //    {
        //        return NotFound();
        //    }

        //    // Hapus terlebih dahulu dokumen terkait dengan TransactionID yang sama.
        //    var relatedDocuments = _context.Whd_DocumentsTrans.Where(d => d.TransactionID == id);
        //    _context.Whd_DocumentsTrans.RemoveRange(relatedDocuments);

        //    // Kemudian hapus transaksi itu sendiri.
        //    _context.Whd_Transaction.Remove(transaction);

        //    await _context.SaveChangesAsync();

        //    // Mengembalikan pesan custom sebagai ContentResult.
        //    //var response = new ContentResult
        //    //{
        //    //    StatusCode = (int)HttpStatusCode.NoContent,
        //    //    Content = "Data berhasil dihapus"
        //    //};
        //    //return response;

        //    return Ok("Data Berhasil dihapus");
        //}
        #endregion

        // HTTP DELETE: Delete a document by DocumentTransID (IMAGE SATUAN)
        //[HttpDelete("deletedocument/{documentTransID}")]
        //public async Task<IActionResult> DeleteDocument(int documentTransID)
        //{
        //    try
        //    {
        //        var document = await _context.Whd_DocumentsTrans.FindAsync(documentTransID);
        //        //var filepath = await _

        //        if (document.FilePath == null)
        //        {
        //            _context.Whd_DocumentsTrans.Remove(document);
        //            await _context.SaveChangesAsync();
        //            return Ok("Transaction tidak memiliki gambar, data tetap dihapus di database");
        //        }

        //        // Remove the file from the file system
        //        if (System.IO.File.Exists(document.FilePath))
        //        {
        //            System.IO.File.Delete(document.FilePath);
        //        }

        //        _context.Whd_DocumentsTrans.Remove(document);
        //        await _context.SaveChangesAsync();

        //        return Ok("Document deleted successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error processing the request: {ex.Message}");
        //    }
        //}

        // HTTP DELETE: Delete a document by DocumentTransID (IMAGE SATUAN)
        /// <summary>
        /// Menghapus dokumen transaksi berdasarkan ID dokumen.
        /// </summary> 
        [HttpDelete("deletedocument/{documentTransID}")]
        public async Task<IActionResult> DeleteDocument(int documentTransID)
        {   
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                try
                {
                    var documentsToDelete = await _context.Whd_DocumentsTrans
                        .Where(d => d.DocumentsTransID == documentTransID)
                        .ToListAsync();

                    //var baseurl = _configuration["AppSettings:baseuri"];
                    var directoryfile = _configuration["AppSettings:ImgUploadFolderPath"];

                    if (documentsToDelete == null || documentsToDelete.Count == 0)
                    {
                        return NotFound("Document not found");
                    }

                    foreach (var document in documentsToDelete)
                    {
                        //var filePath = document.FilePath.Substring(1).Replace('\\', '/');
                        //var directorypath = Path.Combine(baseurl, filePath).Replace('\\', '/');
                        var directorypath = Path.Combine(directoryfile, document.CreatedBy, document.FileName).Replace('\\', '/');
                        if (directorypath != null)
                        {
                            // Remove the file from the file system
                            if (System.IO.File.Exists(directorypath))
                            {
                                System.IO.File.Delete(directorypath);
                            }
                        }

                        _context.Whd_DocumentsTrans.Remove(document);
                    }

                    await _context.SaveChangesAsync();

                    return Ok("Documents deleted successfully");
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


        // HTTP DELETE: Delete a document per transaction
        /// <summary>
        /// Menghapus data transaksi berdasarkan ID transaksi, termasuk dokumen terkait.
        /// </summary> 
        [HttpDelete("deletetransaction/{transactionid}")]
        public async Task<IActionResult> DeleteTransaction(int transactionid)
        {   
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                try
                {
                    var transaction = await _context.Whd_Transaction.FindAsync(transactionid);
                    //var baseurl = _configuration["AppSettings:baseuri"];
                    var directoryfile = _configuration["AppSettings:ImgUploadFolderPath"];

                    if (transaction == null)
                    {
                        return NotFound("Transaction not found");
                    }

                    var documentsToDelete = await _context.Whd_DocumentsTrans.Where(d => d.TransactionID == transactionid).ToListAsync();
                    foreach (var document in documentsToDelete)
                    {
                        //var filePath = document.FilePath[1..].Replace('\\', '/');
                        //var directorypath = Path.Combine(baseurl, filePath).Replace('\\', '/');
                        var directorypath = Path.Combine(directoryfile, document.CreatedBy, document.FileName).Replace('\\', '/');
                        // Remove the files from the file system
                        if (System.IO.File.Exists(directorypath))
                        {
                            System.IO.File.Delete(directorypath);
                        }

                        _context.Whd_DocumentsTrans.Remove(document);
                    }

                    _context.Whd_Transaction.Remove(transaction);
                    await _context.SaveChangesAsync();

                    return Ok("Transaction deleted successfully");
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

        // Header Key = Content-Type, value = multipart/form-data
        // Body -> form-data key = files (tergantung penamaan IFormFile), hover ke bagian kanan kolom key, pilih file
        #region upload default
        //[HttpPost("upload")]
        //public async Task<IActionResult> UploadZipFile(IFormFile files)
        //{
        //    // Check if any file is provided
        //    if (files == null || files.Length == 0)
        //    {
        //        return BadRequest("No file is selected");
        //    }

        //    try
        //    {
        //        // Open the uploaded ZIP file as a ZipArchive
        //        using (var archive = new ZipArchive(files.OpenReadStream(), ZipArchiveMode.Read))
        //        {
        //            // Iterate through each entry (file) in the zip archive
        //            foreach (var entry in archive.Entries)
        //            {
        //                if (entry.Length > 0)
        //                {
        //                    string extension = Path.GetExtension(entry.FullName);
        //                    string uniqueFileName = $"{DateTime.Now:yyMMdd_hhmmss_fff}{extension}";
        //                    //string uniqueFileName = $"{DateTime.Now:yyMMdd_hhmmss_fff}_{Path.GetFileName(entry.FullName)}";
        //                    // Determine the destination path for the extracted file
        //                    //var destinationPath = Path.Combine("C:\\titip\\Perjalanan Dinas\\Testimage", uniqueFileName);
        //                    var destinationPath = Path.Combine(_configuration["AppSettings:ImgUploadFolderPath"], uniqueFileName);

        //                    // Create the necessary directory structure if it doesn't exist
        //                    Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

        //                    // Extract the file from the archive to the specified destination
        //                    using (var stream = entry.Open())
        //                    using (var fileStream = new FileStream(destinationPath, FileMode.Create))
        //                    {
        //                        await stream.CopyToAsync(fileStream);
        //                    }
        //                }
        //            }
        //        }

        //        // Return a success response
        //        return Ok("Zip file extracted successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any errors that occurred during extraction
        //        return StatusCode(500, $"Error extracting zip file: {ex.Message}");
        //    }
        //}
        #endregion

        #region upload advance
        //[HttpPost("upload")]
        //public async Task<IActionResult> UploadZipFile(IFormFile files)
        //{
        //    try
        //    {
        //        if (files != null && files.Length > 0)
        //        {
        //            using (var archive = new ZipArchive(files.OpenReadStream(), ZipArchiveMode.Read))
        //            {
        //                foreach (var entry in archive.Entries)
        //                {
        //                    if (entry.Length > 0)
        //                    {
        //                        string extension = Path.GetExtension(entry.FullName);
        //                        string uniqueFileName = $"{DateTime.Now:yyMMdd_hhmmss_fff}{extension}";
        //                        var destinationPath = Path.Combine(_configuration["AppSettings:ImgUploadFolderPath"], uniqueFileName);
        //                        Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));

        //                        using var stream = entry.Open();
        //                        using var fileStream = new FileStream(destinationPath, FileMode.Create);
        //                        await stream.CopyToAsync(fileStream);
        //                    }
        //                }
        //            }

        //            return Ok("Zip file extracted successfully");
        //        }
        //        else
        //        {
        //            return BadRequest("No file is selected");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error extracting zip file: {ex.Message}");
        //    }
        //}
        #endregion

        #region upload with username
        /// <summary>
        /// Mengunggah berkas untuk transaksi.
        /// </summary> 
        [HttpPost("upload")]
        //public async Task<IActionResult> UploadZipFile(IFormFile files, string username) // by query params
        public async Task<IActionResult> UploadZipFile([FromForm] UploadModel uploadModel) // by form-data body
        {
            
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                try
                {
                    //if (!string.IsNullOrEmpty(username))
                    if (!string.IsNullOrEmpty(uploadModel.username))
                    {
                        //if (files != null && files.Length > 0)
                        if (uploadModel.files != null && uploadModel.files.Length > 0)
                        {
                            //var userDirectory = Path.Combine(_configuration["AppSettings:ImgUploadFolderPath"], username);
                            //var username = uploadModel.username;
                            var imgUploadFolderPath = _configuration["AppSettings:ImgUploadFolderPath"]; // directory store
                            var filepath = _configuration["AppSettings:ImgWeb"]; // link
                            var directorywithurl = Path.Combine(imgUploadFolderPath, uploadModel.username).Replace('\\', '/'); // directory store
                            var userDirectory = Path.Combine(filepath, uploadModel.username).Replace('\\', '/'); // link

                            if (!Directory.Exists(directorywithurl))
                            {
                                Directory.CreateDirectory(directorywithurl);
                            }

                            //using (var archive = new ZipArchive(files.OpenReadStream(), ZipArchiveMode.Read))
                            using (var archive = new ZipArchive(uploadModel.files.OpenReadStream(), ZipArchiveMode.Read))
                            {
                                foreach (var entry in archive.Entries)
                                {
                                    if (entry.Length > 0)
                                    {
                                        string extension = Path.GetExtension(entry.FullName);
                                        //string uniqueFileName = $"Test API{DateTime.Now:yyMMdd_hhmmss_fff}{extension}";
                                        string uniqueFileName = $"{DateTime.Now:yyMMdd_hhmmss_fff}{extension}";
                                        var destinationPath = Path.Combine(userDirectory, uniqueFileName).Replace('\\', '/'); // link
                                        var destiantionPathImage = Path.Combine(directorywithurl, uniqueFileName).Replace('\\', '/'); // directory store
                                        Directory.CreateDirectory(Path.GetDirectoryName(destiantionPathImage));

                                        var documentTrans = new Whd_DocumentsTrans
                                        {
                                            //TransactionID = 92,
                                            TransactionID = uploadModel.transactionID,
                                            FileName = uniqueFileName,
                                            //FilePath = '/' + destinationPath,
                                            FilePath = destinationPath,
                                            UpdatedAt = DateTime.Now,
                                            CreatedBy = uploadModel.username,
                                            UpdatedBy = uploadModel.username
                                        };

                                        using var stream = entry.Open();
                                        using var fileStream = new FileStream(destiantionPathImage, FileMode.Create);
                                        await stream.CopyToAsync(fileStream);
                                        _context.Add(documentTrans);
                                        await _context.SaveChangesAsync();
                                    }
                                }
                            }
                            return Ok($"Uploaded successfully");
                        }
                        else
                        {
                            // Handle scenario when no file is uploaded
                            var documentTrans = new Whd_DocumentsTrans
                            {
                                //TransactionID = 92,
                                TransactionID = uploadModel.transactionID,
                                FileName = null,
                                FilePath = null,
                                UpdatedAt = DateTime.Now,
                                //CreatedBy = username,
                                CreatedBy = uploadModel.username,
                                UpdatedBy = uploadModel.username
                            };

                            _context.Add(documentTrans);
                            await _context.SaveChangesAsync();

                            return Ok("No file uploaded, but data added to the database");
                        }
                    }
                    else
                    {
                        return BadRequest("Username is required");
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


        #endregion

        #region edit dokumen tanpa extrak zip
        // HTTP PUT: Update document information and replace associated file by DocumentTransID (hapus data lama, kemudian tambah list baru berdasarkan image baru)
        //[HttpPut]
        //[Route("updatedocument1/{documentTransID}")]
        //public async Task<IActionResult> UpdateDocument1(int documentTransID, [FromForm] UploadModel uploadModel)
        //{
        //    try
        //    {
        //        var document = await _context.Whd_DocumentsTrans.FindAsync(documentTransID);

        //        if (document == null)
        //        {
        //            return NotFound("Document not found");
        //        }

        //        // Remove the old file from the file system and database
        //        if (!string.IsNullOrEmpty(document.FilePath) && System.IO.File.Exists(document.FilePath))
        //        {
        //            System.IO.File.Delete(document.FilePath);
        //        }

        //        _context.Whd_DocumentsTrans.Remove(document);
        //        await _context.SaveChangesAsync();

        //        // Save the new zip file to the same directory
        //        var userDirectory = Path.GetDirectoryName(document.FilePath); // Use the same directory as the old file

        //        if (!Directory.Exists(userDirectory))
        //        {
        //            Directory.CreateDirectory(userDirectory);
        //        }

        //        string uniqueFileName = $"{DateTime.Now:yyMMdd_hhmmss_fff}.zip";
        //        var destinationPath = Path.Combine(userDirectory, uniqueFileName);

        //        using var stream = uploadModel.files.OpenReadStream();
        //        using var fileStream = new FileStream(destinationPath, FileMode.Create);
        //        await stream.CopyToAsync(fileStream);

        //        // Insert the new document information into the database
        //        var newDocumentTrans = new Whd_DocumentsTrans
        //        {
        //            TransactionID = document.TransactionID,
        //            FileName = uniqueFileName,
        //            FilePath = destinationPath,
        //            UpdatedAt = DateTime.Now,
        //            CreatedBy = uploadModel.username,
        //            UpdatedBy = uploadModel.username
        //        };

        //        _context.Add(newDocumentTrans);
        //        await _context.SaveChangesAsync();

        //        return Ok("Document updated successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error processing the request: {ex.Message}");
        //    }
        //}
        #endregion

        #region update file keroyokan masih error karena file yang terupload itu jadi berlipat ganda tapi db berhaasil update
        // HTTP PUT: Update document information and replace associated file by DocumentTransID
        //[HttpPut]
        //[Route("updatedocument/{TransID}")]
        //public async Task<IActionResult> UpdateDocument(int TransID, [FromForm] UploadModel uploadModel)
        //{
        //    //var document1 = await _context.Whd_DocumentsTrans.Where(d => d.TransactionID == TransID).ToListAsync();
        //    // Retrieve the access token from cache
        //    if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
        //    {
        //        // Use the access token in your API call
        //        var client = _httpClientFactory.CreateClient();
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //        try
        //        {
        //            //var document = await _context.Whd_DocumentsTrans.FindAsync(TransID);
        //            var documents = await _context.Whd_DocumentsTrans.Where(d => d.TransactionID == TransID).ToListAsync();
        //            var filepathuri = _configuration["AppSettings:ImgWeb"]; // link
        //            var userDirectory = Path.Combine(_configuration["AppSettings:ImgUploadFolderPath"], uploadModel.username); // directory store

        //            if (documents == null)
        //            {
        //                return NotFound("Document not found");
        //            }

        //            foreach (var document in documents) // looping melalui setiap dokumen
        //            {
        //                var documentFilePath = Path.Combine(userDirectory, document.FileName);

        //                if (documentFilePath == null)
        //                {
        //                    documentFilePath = null;
        //                }

        //                //var filepath = Path.Combine(baseurl, documentFilePath);
        //                string filepath;
        //                if (documentFilePath != null)
        //                {
        //                    //filepath = Path.Combine(baseurl, documentFilePath);
        //                    filepath = documentFilePath;
        //                }
        //                else
        //                {
        //                    filepath = null;
        //                }

        //                // Remove the old file from the file system
        //                if (!string.IsNullOrEmpty(filepath) && System.IO.File.Exists(filepath))
        //                {
        //                    System.IO.File.Delete(filepath);
        //                }

        //                // Extract the ZIP file to the same directory
        //                using var archive = new ZipArchive(uploadModel.files.OpenReadStream(), ZipArchiveMode.Read);
        //                foreach (var entry in archive.Entries)
        //                {
        //                    if (entry.Length > 0)
        //                    {
        //                        string entryExtension = Path.GetExtension(entry.FullName);
        //                        string entryUniqueFileName = $"{DateTime.Now:yyMMdd_hhmmss_fff}{entryExtension}";
        //                        var entryDestinationPath = Path.Combine(userDirectory, entryUniqueFileName).Replace('\\', '/'); // directory store
        //                                                                                                                        //var newfilepath = Path.Combine(baseurl, entryDestinationPath);
        //                        var newfilepath = Path.Combine(filepathuri, uploadModel.username, entryUniqueFileName).Replace('\\', '/');

        //                        using var entryStream = entry.Open();
        //                        using var entryFileStream = new FileStream(entryDestinationPath, FileMode.Create);
        //                        await entryStream.CopyToAsync(entryFileStream);

        //                        // Update the existing document information in the database
        //                        document.DocumentsTransID = document.DocumentsTransID;
        //                        document.FileName = entryUniqueFileName; // Use the original ZIP file name or customize as needed
        //                                                                 //document.FilePath = "/" + entryDestinationPath; // Use the directory where the files were extracted
        //                        document.FilePath = newfilepath; // link
        //                        document.UpdatedAt = DateTime.Now;
        //                        document.UpdatedBy = uploadModel.username;
        //                    }
        //                }
        //            }

        //            //_context.Update(documents);
        //            _context.UpdateRange(documents);
        //            await _context.SaveChangesAsync();

        //            return Ok("Document updated successfully");
        //        }
        //        catch (Exception ex)
        //        {
        //            return StatusCode(500, $"Error processing the request: {ex.Message}");
        //        }
        //    }
        //    else
        //    {
        //        // Handle the case where the access token is not available
        //        return Unauthorized("Access token required please login.");
        //    }
        //}
        #endregion

        #region update 2
        // HTTP PUT: Update document information and replace associated file by DocumentTransID.
        /// <summary>
        /// Memperbaharui dokumen transaksi.
        /// </summary> 
        [HttpPut]
        [Route("updatedocument/{documentTransID}")]
        public async Task<IActionResult> UpdateDocument(int documentTransID, [FromForm] UploadModel uploadModel)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                try
                {
                    var document = await _context.Whd_DocumentsTrans.FindAsync(documentTransID);
                    var filepathuri = _configuration["AppSettings:ImgWeb"]; // link
                    var userDirectory = Path.Combine(_configuration["AppSettings:ImgUploadFolderPath"], uploadModel.username); // directory store
                    //var directorypath = Path.Combine(baseurl, userDirectory);

                    if (document == null)
                    {
                        return NotFound("Document not found");
                    }

                    //var documentFilePath = document.FilePath;
                    //var documentFilePath = Path.Combine(filepathuri, uploadModel.username);

                    //if (documentFilePath != null)
                    //{
                    //    //documentFilePath = document.FilePath.Substring(1).Replace('\\', '/');
                    //    documentFilePath = document.FilePath.Substring(1).Replace('\\', '/');
                    //}
                    //else
                    //{
                    //    documentFilePath = null;
                    //}

                    var documentFilePath = Path.Combine(userDirectory, document.FileName);

                    if (documentFilePath == null)
                    {
                        documentFilePath = null;
                    }

                    //var filepath = Path.Combine(baseurl, documentFilePath);
                    string filepath;
                    if (documentFilePath != null)
                    {
                        //filepath = Path.Combine(baseurl, documentFilePath);
                        filepath = documentFilePath;
                    }
                    else
                    {
                        filepath = null;
                    }

                    // Remove the old file from the file system
                    if (!string.IsNullOrEmpty(filepath) && System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }

                    // Extract the ZIP file to the same directory
                    using var archive = new ZipArchive(uploadModel.files.OpenReadStream(), ZipArchiveMode.Read);
                    foreach (var entry in archive.Entries)
                    {
                        if (entry.Length > 0)
                        {
                            string entryExtension = Path.GetExtension(entry.FullName);
                            string entryUniqueFileName = $"{DateTime.Now:yyMMdd_hhmmss_fff}{entryExtension}";
                            var entryDestinationPath = Path.Combine(userDirectory, entryUniqueFileName).Replace('\\', '/'); // directory store
                            //var newfilepath = Path.Combine(baseurl, entryDestinationPath);
                            var newfilepath = Path.Combine(filepathuri, uploadModel.username, entryUniqueFileName).Replace('\\', '/');

                            using var entryStream = entry.Open();
                            using var entryFileStream = new FileStream(entryDestinationPath, FileMode.Create);
                            await entryStream.CopyToAsync(entryFileStream);

                            // Update the existing document information in the database
                            document.FileName = entryUniqueFileName; // Use the original ZIP file name or customize as needed
                            //document.FilePath = "/" + entryDestinationPath; // Use the directory where the files were extracted
                            document.FilePath = newfilepath; // link
                            document.UpdatedAt = DateTime.Now;
                            document.UpdatedBy = uploadModel.username;
                        }
                    }

                    _context.Update(document);
                    await _context.SaveChangesAsync();

                    return Ok("Document updated successfully");
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
        #endregion

        private bool TransactionExists(int id)
        {
            return _context.Whd_Transaction.Any(e => e.TransactionID == id);
        }
    }
}
