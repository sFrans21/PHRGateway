using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Function;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarbonAbsorbtion : ControllerBase
    {
        public readonly APIDBContext _context;

        public CarbonAbsorbtion(APIDBContext context)
        {
            _context = context;
        }

        // GET: api/<HouseholdController>
        [HttpGet]
        public List<TblTCarbonAbsorption> Get()
        {
            return _context.TblTCarbonAbsorption.ToList();
        }

        // GET api/<HouseholdController>/5
        [HttpGet("{id}")]
        public TblTCarbonAbsorption GetById(int id)
        {
            var carbon = _context.TblTCarbonAbsorption.Where(a => a.CarbonAbsorptionId == id).SingleOrDefault();
            return carbon;
        }

        // POST api/<HouseholdController>
        [HttpPost]
        public IActionResult Post([FromBody] TblTCarbonAbsorption carbon, [FromForm] FnCarbonAbs fnCarbonAbs)
        {            
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            
            carbon.CreateDate = DateTime.Now;            
           
            _context.TblTCarbonAbsorption.Add(carbon);
            _context.SaveChanges();
            return Ok();
        }

        // PUT api/<HouseholdController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TblTCarbonAbsorption carbon)
        {
            if (id != carbon.CarbonAbsorptionId)
            {
                return BadRequest();
            }

            _context.Entry(carbon).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarbonExists(id))
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

        // DELETE api/<CarbonAbsorbtion>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var carbon = await _context.TblTCarbonAbsorption.FindAsync(id);
            if (carbon == null)
            {
                return NotFound();
            }

            _context.TblTCarbonAbsorption.Remove(carbon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarbonExists(int id)
        {
            return _context.TblTCarbonAbsorption.Any(e => e.CarbonAbsorptionId == id);
        }
    }
}
