using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleEmisionController : ControllerBase
    {
        public readonly APIDBContext _context;

        public VehicleEmisionController(APIDBContext context)
        {
            _context = context;
        }
        // GET: api/<VehicleEmisionController>
        [HttpGet]
        public List<TblTVehicleEmision> Get()
        {
            return _context.TblTVehicleEmision.ToList();
        }

        // GET api/<VehicleEmisionController>/5
        [HttpGet("{id}")]
        public TblTVehicleEmision Get(int id)
        {
            var vehicleemision = _context.TblTVehicleEmision.Where(a => a.VehicleEmisionId == id).SingleOrDefault();
            return vehicleemision;
        }

        // POST api/<VehicleEmisionController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<VehicleEmisionController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<VehicleEmisionController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
