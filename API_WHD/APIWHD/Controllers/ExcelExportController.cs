using APIWHD.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace APIWHD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelExportController : ControllerBase
    {
        private readonly APIDBContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public ExcelExportController(APIDBContext dbContext, IConfiguration configuration, IMemoryCache memoryCache
                , IHttpClientFactory httpClientFactory)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        // export to local PC
        #region basic export to excel
        /// <summary>
        /// Mengekspor data dasar ke format Excel.
        /// </summary> 
        [HttpGet("basic")]
        public async Task<IActionResult> ExportToExcelbasic()
        {
            var data = await _dbContext.VwWellReport.ToListAsync();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Data");

                // Set header row
                worksheet.Cells["B1"].Value = "FileName";
                worksheet.Cells["C1"].Value = "NamaSumur";
                worksheet.Cells["D1"].Value = "Lokasi";
                worksheet.Cells["E1"].Value = "Tanggal";
                worksheet.Cells["F1"].Value = "Waktu";
                worksheet.Cells["G1"].Value = "Kategori";
                worksheet.Cells["H1"].Value = "Uploader";
                worksheet.Cells["I1"].Value = "NamaPeralatan";
                worksheet.Cells["J1"].Value = "Keterangan";
                worksheet.Cells["K1"].Value = "TerakhirDiUbah";
                worksheet.Cells["L1"].Value = "DiubahOleh";

                // Populate data rows
                for (int i = 0; i < data.Count; i++)
                {
                    var item = data[i];
                    int row = i + 3;

                    worksheet.Cells[row, 2].Value = item.FileName;
                    worksheet.Cells[row, 3].Value = item.NamaSumur;
                    worksheet.Cells[row, 4].Value = item.Lokasi;
                    worksheet.Cells[row, 5].Value = item.Tanggal;
                    worksheet.Cells[row, 6].Value = item.Waktu;
                    worksheet.Cells[row, 7].Value = item.Kategori;
                    worksheet.Cells[row, 8].Value = item.Uploader;
                    worksheet.Cells[row, 9].Value = item.NamaPeralatan;
                    worksheet.Cells[row, 10].Value = item.Keterangan;
                    worksheet.Cells[row, 11].Value = item.TerakhirDiUbah;
                    worksheet.Cells[row, 12].Value = item.DiubahOleh;
                }

                // Auto-fit columns
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Generate the Excel file
                var stream = new MemoryStream(package.GetAsByteArray());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "WellReports.xlsx");
                //return Ok("Excel saved Succesfully");
            }
        }
        #endregion
        #region export to lokal PC directory advanced
        /// <summary>
        /// Mengekspor semua data lokal ke Excel.
        /// </summary> 
        [HttpGet("GetAlllocal")]
        public async Task<IActionResult> ExportToExcel()
        {
            //var data = await _dbContext.VwWellReport.ToListAsync();
            var data = await _dbContext.VwWellReport.Where(x => x.FileName != null).OrderByDescending(x => x.TransactionID).ToListAsync();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Data");

                // Merge header row
                var headerRange = worksheet.Cells["A1:K1"];
                headerRange.Merge = true;
                headerRange.Value = "SEMUA LAPORAN";
                headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                headerRange.Style.Font.Bold = true;

                // Set header row
                //worksheet.Cells["A2"].Value = "Gambar";
                worksheet.Cells["A2"].Value = "File URL";
                worksheet.Cells["B2"].Value = "Nama Sumur";
                worksheet.Cells["C2"].Value = "Lokasi";
                worksheet.Cells["D2"].Value = "Tanggal";
                worksheet.Cells["E2"].Value = "Waktu";
                worksheet.Cells["F2"].Value = "Kategori";
                worksheet.Cells["G2"].Value = "Uploader";
                worksheet.Cells["H2"].Value = "Nama Peralatan";
                worksheet.Cells["I2"].Value = "Keterangan";
                worksheet.Cells["J2"].Value = "Terakhir DiUbah";
                worksheet.Cells["K2"].Value = "Diubah Oleh";

                // Set header row style
                using (var range = worksheet.Cells["A2:K2"])
                {
                    range.Style.Font.Bold = true;
                }

                // Populate data rows (rows mulai dari row 3)
                for (int i = 0; i < data.Count; i++)
                {
                    var item = data[i];
                    int row = i + 3;
                    var fileNameCell = worksheet.Cells[row, 1];

                    worksheet.Cells[row, 1].Value = item.FileName;
                    worksheet.Cells[row, 2].Value = item.NamaSumur;
                    worksheet.Cells[row, 3].Value = item.Lokasi;
                    worksheet.Cells[row, 4].Value = item.Tanggal;
                    worksheet.Cells[row, 5].Value = item.Waktu;
                    worksheet.Cells[row, 6].Value = item.Kategori;
                    worksheet.Cells[row, 7].Value = item.Uploader;
                    worksheet.Cells[row, 8].Value = item.NamaPeralatan;
                    worksheet.Cells[row, 9].Value = item.Keterangan;
                    worksheet.Cells[row, 10].Value = item.TerakhirDiUbah;
                    worksheet.Cells[row, 11].Value = item.DiubahOleh;

                    // Set hyperlink style for FileName column if it is a valid URL
                    
                    if (IsValidUrl(item.FileName))
                    {
                        var fileNameLink = new Uri(item.FileName);
                        fileNameCell.Hyperlink = fileNameLink;
                        fileNameCell.Style.Font.UnderLine = true;
                        fileNameCell.Style.Font.Color.SetColor(Color.Blue);
                    }

                }

                // Apply border to data range
                var dataRange = worksheet.Cells["A2:K" + (data.Count + 2)];
                dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;

                // Auto-fit columns
                //worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Autofit kolom-kolom kecuali kolom I
                for (int i = 1; i <= worksheet.Dimension.Columns; i++)
                {
                    if (i != 9) // Kolom I
                    {
                        worksheet.Column(i).AutoFit();
                    }
                }

                // Generate the Excel file
                var stream = new MemoryStream(package.GetAsByteArray());

                // Save the file to the specified directory
                //string directoryPath = @"C:\titip\API List";
                string directoryPath = _configuration["AppSettings:exportexcelLocal"];
                string uniqueFileName = $"{DateTime.Now:yyMMdd_hhmmss_fff}";
                string fileName = $"WellReports_{uniqueFileName}.xlsx";
                string filePath = Path.Combine(directoryPath, fileName);
                Directory.CreateDirectory(directoryPath);
                await using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await stream.CopyToAsync(fileStream);
                }
                return Ok($"Excel downloaded and saved to {filePath}");
            }
        }
        #endregion

        // export to local phone (handling on Flutter)
        #region export to lokal mobile getall
        /// <summary>
        /// Mengekspor semua data ke excel.
        /// </summary> 
        [HttpGet("GetAll")]
        public async Task<IActionResult> ExportToExcelGetAll()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var data = await _dbContext.VwWellReport.Where(x => x.FileName != null).OrderByDescending(x => x.TransactionID).ToListAsync();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Data");

                    // Merge header row
                    var headerRange = worksheet.Cells["A1:K1"];
                    headerRange.Merge = true;
                    headerRange.Value = "SEMUA LAPORAN";
                    headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    headerRange.Style.Font.Bold = true;

                    // Set header row
                    //worksheet.Cells["A2"].Value = "Gambar";
                    worksheet.Cells["A2"].Value = "File URL";
                    worksheet.Cells["B2"].Value = "Nama Sumur";
                    worksheet.Cells["C2"].Value = "Lokasi";
                    worksheet.Cells["D2"].Value = "Tanggal";
                    worksheet.Cells["E2"].Value = "Waktu";
                    worksheet.Cells["F2"].Value = "Kategori";
                    worksheet.Cells["G2"].Value = "Uploader";
                    worksheet.Cells["H2"].Value = "Nama Peralatan";
                    worksheet.Cells["I2"].Value = "Keterangan";
                    worksheet.Cells["J2"].Value = "Terakhir DiUbah";
                    worksheet.Cells["K2"].Value = "Diubah Oleh";

                    // Set header row style
                    using (var range = worksheet.Cells["A2:K2"])
                    {
                        range.Style.Font.Bold = true;
                    }

                    // Populate data rows (rows mulai dari row 3)
                    for (int i = 0; i < data.Count; i++)
                    {
                        var item = data[i];
                        int row = i + 3;
                        var fileNameCell = worksheet.Cells[row, 1];

                        worksheet.Cells[row, 1].Value = item.FileName;
                        worksheet.Cells[row, 2].Value = item.NamaSumur;
                        worksheet.Cells[row, 3].Value = item.Lokasi;
                        worksheet.Cells[row, 4].Value = item.Tanggal;
                        worksheet.Cells[row, 5].Value = item.Waktu;
                        worksheet.Cells[row, 6].Value = item.Kategori;
                        worksheet.Cells[row, 7].Value = item.Uploader;
                        worksheet.Cells[row, 8].Value = item.NamaPeralatan;
                        worksheet.Cells[row, 9].Value = item.Keterangan;
                        worksheet.Cells[row, 10].Value = item.TerakhirDiUbah;
                        worksheet.Cells[row, 11].Value = item.DiubahOleh;

                        // Set hyperlink style for FileName column if it is a valid URL
                        if (IsValidUrl(item.FileName))
                        {
                            var fileNameLink = new Uri(item.FileName);
                            fileNameCell.Hyperlink = fileNameLink;
                            fileNameCell.Style.Font.UnderLine = true;
                            fileNameCell.Style.Font.Color.SetColor(Color.Blue);
                        }

                    }

                    // Apply border to data range
                    var dataRange = worksheet.Cells["A2:K" + (data.Count + 2)];
                    dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;

                    // Auto-fit columns
                    //worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // Autofit kolom-kolom kecuali kolom I
                    for (int i = 1; i <= worksheet.Dimension.Columns; i++)
                    {
                        if (i != 9) // Kolom I
                        {
                            worksheet.Column(i).AutoFit();
                        }
                    }

                    string uniqueFileName = $"{DateTime.Now:yyMMdd_hhmmss_fff}";
                    string fileName = $"WellReports_{uniqueFileName}.xlsx";

                    // Generate the Excel file
                    var stream = new MemoryStream(package.GetAsByteArray());
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"WellReports_{uniqueFileName}.xlsx");
                    //return Ok("Excel saved Succesfully");
                }
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
            
        }
        #endregion
        #region export to lokal mobile by username
        /// <summary>
        /// Mengekspor data ke Excel yang terkait dengan nama pengguna tertentu.
        /// </summary> 
        [HttpGet("{username}")]
        public async Task<IActionResult> ExportToExcelByUsername(string username)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var data = await _dbContext.VwWellReport.Where(x => x.FileName != null && username == x.Uploader).OrderByDescending(x => x.TransactionID).ToListAsync();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Data");

                    // Merge header row
                    var headerRange = worksheet.Cells["A1:K1"];
                    headerRange.Merge = true;
                    headerRange.Value = "LAPORAN SAYA";
                    headerRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    headerRange.Style.Font.Bold = true;

                    // Set header row
                    //worksheet.Cells["A2"].Value = "Gambar";
                    worksheet.Cells["A2"].Value = "File URL";
                    worksheet.Cells["B2"].Value = "Nama Sumur";
                    worksheet.Cells["C2"].Value = "Lokasi";
                    worksheet.Cells["D2"].Value = "Tanggal";
                    worksheet.Cells["E2"].Value = "Waktu";
                    worksheet.Cells["F2"].Value = "Kategori";
                    worksheet.Cells["G2"].Value = "Uploader";
                    worksheet.Cells["H2"].Value = "Nama Peralatan";
                    worksheet.Cells["I2"].Value = "Keterangan";
                    worksheet.Cells["J2"].Value = "Terakhir DiUbah";
                    worksheet.Cells["K2"].Value = "Diubah Oleh";

                    // Set header row style
                    using (var range = worksheet.Cells["A2:K2"])
                    {
                        range.Style.Font.Bold = true;
                    }

                    // Populate data rows (rows mulai dari row 3)
                    for (int i = 0; i < data.Count; i++)
                    {
                        var item = data[i];
                        int row = i + 3;
                        var fileNameCell = worksheet.Cells[row, 1];

                        worksheet.Cells[row, 1].Value = item.FileName;
                        worksheet.Cells[row, 2].Value = item.NamaSumur;
                        worksheet.Cells[row, 3].Value = item.Lokasi;
                        worksheet.Cells[row, 4].Value = item.Tanggal;
                        worksheet.Cells[row, 5].Value = item.Waktu;
                        worksheet.Cells[row, 6].Value = item.Kategori;
                        worksheet.Cells[row, 7].Value = item.Uploader;
                        worksheet.Cells[row, 8].Value = item.NamaPeralatan;
                        worksheet.Cells[row, 9].Value = item.Keterangan;
                        worksheet.Cells[row, 10].Value = item.TerakhirDiUbah;
                        worksheet.Cells[row, 11].Value = item.DiubahOleh;

                        // Set hyperlink style for FileName column if it is a valid URL
                        if (IsValidUrl(item.FileName))
                        {
                            var fileNameLink = new Uri(item.FileName);
                            fileNameCell.Hyperlink = fileNameLink;
                            fileNameCell.Style.Font.UnderLine = true;
                            fileNameCell.Style.Font.Color.SetColor(Color.Blue);
                        }

                    }

                    // Apply border to data range
                    var dataRange = worksheet.Cells["A2:K" + (data.Count + 2)];
                    dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;

                    // Auto-fit columns
                    //worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    // Autofit kolom-kolom kecuali kolom I
                    for (int i = 1; i <= worksheet.Dimension.Columns; i++)
                    {
                        if (i != 9) // Kolom I
                        {
                            worksheet.Column(i).AutoFit();
                        }
                    }

                    string uniqueFileName = $"{DateTime.Now:yyMMdd_hhmmss_fff}";
                    string fileName = $"WellReports_{uniqueFileName}.xlsx";

                    // Generate the Excel file
                    var stream = new MemoryStream(package.GetAsByteArray());
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"WellReports_{uniqueFileName}.xlsx");
                    //return Ok("Excel saved Succesfully");
                }
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }            
        }
        #endregion

        // Helper method to check if a string is a valid URL
        private bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
