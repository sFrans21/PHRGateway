using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Function;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

using Microsoft.Data.SqlClient;
//using EFSample.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Data;
using System.Net.Http.Headers;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class VehicleController : ControllerBase
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();
        public readonly APIDBContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public VehicleController(APIDBContext context, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        [Route("GetListUser/{userid}/{PeriodeID}")]
        public async Task<IActionResult> Get(int userid, int PeriodeID)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwVehicleList_UserSum.Where(a => a.userID == userid && a.PeriodeId == PeriodeID).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpGet]
        [Route("GetListUserNotSUM/{userid}")]
        public async Task<IActionResult> GetListUserNotSUM(int userid)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwVehicleList_User.Where(a => a.userID == userid).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpGet]
        [Route("GetDetailVehicle/{vehicleId}")]
        public async Task<IActionResult> GetDetailVehicle(int vehicleId)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwVehicleList.Where(a => a.VehicleId == vehicleId).SingleOrDefaultAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpGet]
        [Route("GetVehicleEmisionSumYear/{Userid}/{Year}")]
        public async Task<IActionResult> GetVehicleEmisionSumYear(int Userid, int year)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwVehicleEmision_SUM_Year.Where(a => a.UserID == Userid && a.Tahun == year).SingleOrDefaultAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpGet]
        [Route("GetVehicleEmisionSumMonth/{Userid}/{Month}/{Year}")]
        public async Task<IActionResult> GetVehicleEmisionSumMonth(int Userid, int month, int year)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwVehicleEmision_SUM_Month.Where(a => a.UserID == Userid && a.Tahun == year && a.Bulan == month).SingleOrDefaultAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }



        [HttpGet]
        [Route("GetListVehicle")]
        public async Task<IActionResult> GetListVehicle()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwVehicleList.OrderBy(o => o.VehicleType).OrderBy(o => o.VehicleName).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }



        [HttpGet]
        [Route("GetListTransportation")]
        public async Task<IActionResult> GetListTransportation()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblTransportation.ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpGet]
        [Route("GetListFuel/{vehicleId}")]
        public async Task<IActionResult> GetListFuel(int vehicleId)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string VehicleID = vehicleId.ToString();
                var result = await _context.VwFuel.Where(a => (a.VehicleID == "" ? VehicleID : a.VehicleID) == VehicleID).OrderBy(o => o.FuelName).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }

        }

        [HttpGet]
        [Route("GetListVType")]
        public async Task<IActionResult> GetListVType()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblVehicleType.ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpGet]
        [Route("GetListVCapacity")]
        public async Task<IActionResult> GetListVCapacity()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblVehicleCapacity.OrderBy(o => o.CubicalCentimeter).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }
        [HttpGet]
        [Route("GetListCBOVehicle")]
        public async Task<IActionResult> GetListCBOVehicle()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblVehicle.ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        // GET api/<VehicleEmisionController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {

            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwVehicleList_User.Where(a => a.VehicleEmisionId == id).SingleOrDefaultAsync();
                //var vehicleemision = _context.VwVehicleList_User.Where(a => a.userID == userid).ToList();
                //var vehicleemision2 = vehicleemision.Where(a => a.VehicleEmisionId == id).SingleOrDefault();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpGet]
        [Route("Get_VehicleEmision_Calc/{VehicleID}/{JenisTransportasi}/{FueldID}/{CC}/{JarakTempuh}/{FrekuensiPerjalanan}/{AmountPeople}")]
        public async Task<IActionResult> Get_VehicleEmision_Calc(int VehicleID, int JenisTransportasi, int FueldID, int cc,
                                                                decimal JarakTempuh, int FrekuensiPerjalanan, int AmountPeople)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string sql = @"SELECT *
		                 FROM [FN_GetVehicleEmision_Calc](@VehicleID,@JenisTransportasi,@FueldID,@CC,@JarakTempuh,@FrekuensiPerjalanan,@AmountPeople)
                        ";


                var parameters = new[] {
                new SqlParameter("@VehicleID", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = VehicleID },
                new SqlParameter("@JenisTransportasi", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = JenisTransportasi },
                new SqlParameter("@FueldID", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = FueldID },
                new SqlParameter("@CC", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = cc },
                new SqlParameter("@JarakTempuh", SqlDbType.Decimal) { Direction = ParameterDirection.Input, Value = JarakTempuh },
                new SqlParameter("@FrekuensiPerjalanan", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = FrekuensiPerjalanan },
                new SqlParameter("@AmountPeople", SqlDbType.Int) { Direction = ParameterDirection.Input, Value = AmountPeople }
                };
                var result = await _context.FNGetVehicleEmision_Calc.FromSqlRaw(sql, parameters).SingleOrDefaultAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] VwVehicleList_User_Action VehicleEmision)
        {
            

            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                //  List<VwVehicleList_User_Action> list;
                int rowsAffected;
                string sql = "EXEC [SP_Vehicle_CRUD_V2] @VehicleEmisionId, @VehicleId, @AmountPeople, @Mileage, @TravelFrequency, @PeriodeId,@UserID,@Action,@TransportationId, @CapacityId, @FuelId ";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    // Create parameter(s)    
                    new SqlParameter { ParameterName = "@VehicleEmisionId", Value = VehicleEmision.VehicleEmisionId },
                    new SqlParameter { ParameterName = "@VehicleId", Value = VehicleEmision.VehicleId },
                    new SqlParameter { ParameterName = "@AmountPeople", Value = VehicleEmision.AmountPeople },
                    new SqlParameter { ParameterName = "@Mileage", Value = VehicleEmision.Mileage },
                    new SqlParameter { ParameterName = "@TravelFrequency", Value = VehicleEmision.TravelFrequency },
                    new SqlParameter { ParameterName = "@PeriodeId", Value = VehicleEmision.PeriodeId },
                    new SqlParameter { ParameterName = "@UserID", Value = VehicleEmision.userID},
                    new SqlParameter { ParameterName = "@Action", Value = VehicleEmision.action },
                    new SqlParameter { ParameterName = "@TransportationId", Value = VehicleEmision.TransportationId },
                    new SqlParameter { ParameterName = "@CapacityId", Value = VehicleEmision.CapacityId },
                    new SqlParameter { ParameterName = "@FuelId", Value = VehicleEmision.FuelId },
                };

                //list = _context.VwVehicleList_User_Action.FromSqlRaw<VwVehicleList_User_Action>(sql, parms.ToArray()).ToList();
                rowsAffected = _context.Database.ExecuteSqlRaw(sql, parms.ToArray());

                //Debugger.Break();
                return Ok();
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] VwVehicleList_User_Action VehicleEmision)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                //  List<VwVehicleList_User_Action> list;
                int rowsAffected;
                string sql = "EXEC SP_Vehicle_CRUD_V2 @VehicleEmisionId, @VehicleId, @AmountPeople, @Mileage, @TravelFrequency, @PeriodeId,@UserID,@Action,@TransportationId, @CapacityId, @FuelId ";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    // Create parameter(s)    
                    new SqlParameter { ParameterName = "@VehicleEmisionId", Value = VehicleEmision.VehicleEmisionId },
                    new SqlParameter { ParameterName = "@VehicleId", Value = VehicleEmision.VehicleId },
                    new SqlParameter { ParameterName = "@AmountPeople", Value = VehicleEmision.AmountPeople },
                    new SqlParameter { ParameterName = "@Mileage", Value = VehicleEmision.Mileage },
                    new SqlParameter { ParameterName = "@TravelFrequency", Value = VehicleEmision.TravelFrequency },
                    new SqlParameter { ParameterName = "@PeriodeId", Value = VehicleEmision.PeriodeId },
                    new SqlParameter { ParameterName = "@UserID", Value = VehicleEmision.userID},
                    new SqlParameter { ParameterName = "@Action", Value = VehicleEmision.action },
                    new SqlParameter { ParameterName = "@TransportationId", Value = VehicleEmision.TransportationId },
                    new SqlParameter { ParameterName = "@CapacityId", Value = VehicleEmision.CapacityId },
                    new SqlParameter { ParameterName = "@FuelId", Value = VehicleEmision.FuelId },
                };

                //list = _context.VwVehicleList_User_Action.FromSqlRaw<VwVehicleList_User_Action>(sql, parms.ToArray()).ToList();
                rowsAffected = _context.Database.ExecuteSqlRaw(sql, parms.ToArray());
                //Debugger.Break();
                if (rowsAffected > 0)
                    return Ok();
                else
                    return BadRequest();
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }


        #region MasterVeicleFormula

        [HttpPost]
        [Route("PostVehicleFormula")]
        //public IActionResult PostVehicleFormula([FromBody] VwMasterVehicleFormula VehicleFormula)
        public async Task<IActionResult> PostVehicleFormula([FromBody] VwMasterVehicleFormula VehicleFormula)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                int rowsAffected;
                string sql = "EXEC [SP_CMD_MasterVehicleFormula] @VehicleID, @TransportationID, @FuelID, @TransportCapacity, @TransportKMperLiter, @Action";

                List<SqlParameter> parms = new List<SqlParameter>
            {
                // Create parameter(s)    
                new SqlParameter { ParameterName = "@VehicleId", Value = VehicleFormula.VehicleId },
                new SqlParameter { ParameterName = "@TransportationId", Value = VehicleFormula.TransportGroupid },
                new SqlParameter { ParameterName = "@FuelId", Value = VehicleFormula.FuelId },
                new SqlParameter { ParameterName = "@TransportCapacity", Value = VehicleFormula.TransportCapacity },
                new SqlParameter { ParameterName = "@TransportKMperLiter", Value = VehicleFormula.TransportKMperLiter },
                new SqlParameter { ParameterName = "@Action", Value ="CREATE"},
            };

                //list = _context.VwVehicleList_User_Action.FromSqlRaw<VwVehicleList_User_Action>(sql, parms.ToArray()).ToList();
                //rowsAffected = _context.Database.ExecuteSqlRaw(sql, parms.ToArray());
                var result = _context.Execute_SP_Output.FromSqlRaw(sql, parms.ToArray()).ToListAsync();//_context.Database.ExecuteSqlRaw(sql, parms.ToArray());

                //Debugger.Break();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
            
        }

        [HttpGet]
        [Route("GetVehicleList_AddFormula")]
        public async Task<IActionResult> GetVehicleList_AddFormula()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwVehicleList_AddFormula.OrderBy(o => o.VehicleName).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }


        [HttpGet]
        [Route("GetMasterVehicleFormula")]
        public async Task<IActionResult> GetMasterVehicleFormula()
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwMasterVehicleFormula.ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        //[HttpGet]
        //[Route("GetMasterVehicleFormula_Detail/{TransportGroupFormulaId}")]
        //public VwMasterVehicleFormula GetMasterVehicleFormula_Detail(int TransportGroupFormulaId)
        //{
        //    return _context.VwMasterVehicleFormula.SingleOrDefault();
        //}

        [HttpGet]
        [Route("GetMasterVehicleFormula_Detail/{TransportGroupFormulaId}")]
        public async Task<IActionResult> GetMasterVehicleFormula_Detail(int TransportGroupFormulaId)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwMasterVehicleFormula.Where(o => o.TransportGroupFormulaId == TransportGroupFormulaId).SingleOrDefaultAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }

        }

        #endregion


        #region MasterVehicleFuel
        [HttpGet]
        [Route("GetMasterVehicleFuel")]
        public List<TblFuel> GetMasterVehicleFuel()
        {
            return _context.TblFuel.OrderBy(o => o.FuelName).ToList();
        }

        [HttpGet]
        [Route("GetMasterVehicleFuel_Detail/{Fuelid}")]
        public TblFuel GetMasterVehicleFuel_Detail(int Fuelid)
        {
            return _context.TblFuel.Where(o => o.FuelId == Fuelid).SingleOrDefault();
        }

        [HttpPost]
        [Route("postMasterVehicleFuel")]
        public IActionResult postMasterVehicleFuel([FromBody] TblFuel VehicleFuelInfo)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            _context.TblFuel.Add(VehicleFuelInfo);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("PutMasterVehicleFuel")]
        public async Task<IActionResult> PutMasterFuelVehicle([FromBody] TblFuel _MasterVehicleFuel/*, [FromForm] TblVehicle _vehicleList*/)
        {
            _context.Entry(_MasterVehicleFuel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleFuelExists(_MasterVehicleFuel.FuelId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        private bool VehicleFuelExists(int id)
        {
            return _context.TblFuel.Any(e => e.FuelId == id);
        }

        //[HttpDelete("{id}")]
        [HttpDelete]
        [Route("DeleteMasterVehicleFuel/{id}")]
        public async Task<IActionResult> DeleteMasterVehicleFuel(int id)
        {

            var MasterVFuel = await _context.TblFuel.FindAsync(id);
            if (MasterVFuel == null)
            {
                return NotFound();
            }

            _context.TblFuel.Remove(MasterVFuel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        #endregion

        #region MasterVehicle
        [HttpGet]
        [Route("GetMasterVehicle")]
        public async Task<IActionResult> GetMasterVehicle()
        {            
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblVehicle.OrderBy(o => o.VehicleName).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpGet]
        [Route("GetMasterVehicle_Detail/{vehicleId}")]
        public async Task<IActionResult> GetMasterVehicle_Detail(int Vehicleid)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblVehicle.Where(o => o.VehicleId == Vehicleid).SingleOrDefaultAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpPost]
        [Route("postMasterVehicle")]
        public async Task<IActionResult> postMasterVehicle([FromBody] TblVehicle VehicleInfo)
        {            
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                //HttpClient client = _api.Initial();
                //List<TblTree> treeInfo = new List<TblTree>();
                //HttpResponseMessage res = client.GetAsync("api/carbonabsorbtion/GetTreeInfo/" + carbonAbsorption.TreeId).Result;
                //treeInfo = res.Content.ReadAsStringAsync<List<TblTree>>().Result;

                //treeInfo = GetTreeInfo(carbonAbsorption.TreeId);

                if (!ModelState.IsValid)
                    return BadRequest("Not a valid model");

                VehicleInfo.VehicleTypeId = 1;
                _context.TblVehicle.Add(VehicleInfo);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpPut]
        [Route("PutMasterVehicle")]
        public async Task<IActionResult> PutMasterVehicle([FromBody] TblVehicle _MasterVehicle/*, [FromForm] TblVehicle _vehicleList*/)
        {
            _context.Entry(_MasterVehicle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehicleExists(_MasterVehicle.VehicleId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        private bool VehicleExists(int id)
        {
            return _context.TblVehicle.Any(e => e.VehicleId == id);
        }

        // DELETE api/<VehicleEmisionController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var MasterVe = await _context.TblVehicle.FindAsync(id);
            if (MasterVe == null)
            {
                return NotFound();
            }

            _context.TblVehicle.Remove(MasterVe);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion



    }
}
