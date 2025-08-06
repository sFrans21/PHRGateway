using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Function;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using System.Net.Http.Headers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HouseholdController : ControllerBase
    {
        public readonly APIDBContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpClientFactory _httpClientFactory;

        public HouseholdController(APIDBContext context, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _memoryCache = memoryCache;
            _httpClientFactory = httpClientFactory;
        }

        // GET: api/<HouseholdController>
        [HttpGet]
        [Route("GetHouseHold/{userid}/{periodeID}")]
        public async Task<IActionResult> Get(int userid, int periodeID)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblTHousehold.Where(a => a.UserId == userid && a.PeriodeId == periodeID).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        //akumulasi tahunan
        //GET: api/<HouseholdController>
        [HttpGet]
        [Route("GetbyUser/{userid}/{Year}")]
        public async Task<IActionResult> GetbyUser(int userid, int Year)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.VwHouseholdbyYear.Where(a => a.userID == userid && a.Tahun == Year).ToListAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        //sum list bulanan
        [HttpGet]
        [Route("GetTotalMonth/{userid}/{periodeid}")]
        public async Task<ActionResult<Dictionary<string, float>>> GetTotalMonth(int userId, int periodeId)
        {            
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var data = await _context.Vw_HouseholdSumListmonth
                                     .Where(x => x.UserId == userId && x.PeriodeId == periodeId)
                                     .ToListAsync();

                float konsumsilistrik = (float)data.Sum(x => x.KonsumsiListrik);
                float konsumsilpg = (float)data.Sum(x => x.KonsumsiLPG);
                float konsumsigaskota = (float)data.Sum(x => x.KonsumsiGasKota);
                float emisilistrik = (float)data.Sum(x => x.EmisiListrik);
                float emisigas = (float)data.Sum(x => x.EmisiGas);
                float emisiperorang = (float)data.Sum(x => x.EmisiperOrang);


                var result = new Dictionary<string, float>
            {
                { "KonsumsiListrik", konsumsilistrik },
                { "KonsumsiGas", konsumsilpg },
                { "KonsumsiGasKota", konsumsigaskota },
                { "EmisiListrik", emisilistrik },
                { "EmisiGas", emisigas },
                { "EmisiPerOrang", emisiperorang },
            };

                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        //sum list tahunan
        [HttpGet]
        [Route("GetTotalYear/{userid}/{year}")]
        public async Task<ActionResult<Dictionary<string, float>>> GetTotalYear(int userId, int year)
        {            
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var data = await _context.Vw_HouseholdSumListYear
                                     .Where(x => x.UserId == userId && x.Tahun == year)
                                     .ToListAsync();

                float konsumsilistrik = (float)data.Sum(x => x.KonsumsiListrik);
                float konsumsilpg = (float)data.Sum(x => x.KonsumsiLPG);
                float konsumsigaskota = (float)data.Sum(x => x.KonsumsiGasKota);
                float emisilistrik = (float)data.Sum(x => x.EmisiListrik);
                float emisigas = (float)data.Sum(x => x.EmisiGas);
                float emisiperorang = (float)data.Sum(x => x.EmisiperOrang);


                var result = new Dictionary<string, float>
            {
                { "KonsumsiListrik", konsumsilistrik },
                { "KonsumsiGas", konsumsilpg },
                { "KonsumsiGasKota", konsumsigaskota },
                { "EmisiListrik", emisilistrik },
                { "EmisiGas", emisigas },
                { "EmisiPerOrang", emisiperorang },
            };

                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        [HttpGet]
        [Route("GetPeriod/{userid}/{periodeDetail}")]
        public async Task<IActionResult> GetPeriod(string userid, string periodeDetail)
        {            
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                string sql = "EXEC [SP_CekPeriode] @UserID,@Periode";
                var parameters = new[] 
                {
                    new SqlParameter("@UserID", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = userid },
                    new SqlParameter("@Periode", SqlDbType.VarChar) { Direction = ParameterDirection.Input, Value = periodeDetail }
                };

                //var Result = _context.VwPeriode.FromSqlRaw(sql, parameters);//_context.Database.ExecuteSqlRaw(sql, parms.ToArray());

                //VwPeriode _VwPeriode = new VwPeriode();
                //_VwPeriode = Result.ToList()[0];
                //return (IActionResult)_VwPeriode;

                //DateTime FullDate = Convert.ToDateTime("1-" + periodeDetail);

                //var SelectedPeriode = _context.VwPeriode.Where(a => a.UserId == Convert.ToInt32(userid)
                //                                            && a.MonthValue == FullDate.Month
                //                                            && a.YearValue == FullDate.Year).SingleOrDefault();

                //return SelectedPeriode;

                var Result = await _context.VwPeriode.FromSqlRaw(sql, parameters).ToListAsync();

                if (Result.Count > 0)
                {
                    VwPeriode _VwPeriode = Result[0];
                    return Ok(_VwPeriode);
                }
                else
                {
                    return NotFound("Periode not found");
                }
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        // GET api/<HouseholdController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var result = await _context.TblTHousehold.Where(a => a.HouseholdId == id).SingleOrDefaultAsync();
                return Ok(result);
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        // POST api/<HouseholdController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TblTHousehold household, [FromForm] FnHousehold fnHousehold)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                if (!ModelState.IsValid)
                    return BadRequest("Not a valid model");


                household.CreatedDate = DateTime.Now;
                //household.Periode = 2;
                //household.UserId = 2;
                household.ElectricityCons = fnHousehold.KonsumsiListrik(household);
                household.GasCons = fnHousehold.KonsumsiGas(household);
                household.ElectricityEmision = fnHousehold.EmisiCo2Listrik(household);
                household.GasEmision = fnHousehold.EmisiCo2Gas(household);
                household.PeopleEmision = fnHousehold.EmisiCo2Person(household);


                _context.TblTHousehold.Add(household);
                await _context.SaveChangesAsync();
                return Ok("Created!");
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        // PUT api/<HouseholdController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TblTHousehold household, [FromForm] FnHousehold fnHousehold)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                if (id != household.HouseholdId)
                {
                    return BadRequest();
                }
                household.ElectricityCons = fnHousehold.KonsumsiListrik(household);
                household.GasCons = fnHousehold.KonsumsiGas(household);
                household.ElectricityEmision = fnHousehold.EmisiCo2Listrik(household);
                household.GasEmision = fnHousehold.EmisiCo2Gas(household);
                household.PeopleEmision = fnHousehold.EmisiCo2Person(household);
                //household.PeriodeId = household.PeriodeId;
                _context.Entry(household).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HouseholdExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok($"Data Household with ID {id} updated!");
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }

        }

        // DELETE api/<HouseholdController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Retrieve the access token from cache
            if (_memoryCache.TryGetValue("AccessToken", out string accessToken))
            {
                // Use the access token in your API call
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var household = await _context.TblTHousehold.FindAsync(id);
                if (household == null)
                {
                    return NotFound($"Data with ID {id} is not found!");
                }

                _context.TblTHousehold.Remove(household);
                await _context.SaveChangesAsync();

                return Ok($"Data with id = {id} deleted!");
            }
            else
            {
                // Handle the case where the access token is not available
                return Unauthorized("Access token required please login.");
            }
        }

        private bool HouseholdExists(int id)
        {
            return _context.TblTHousehold.Any(e => e.HouseholdId == id);
        }
    }
}
