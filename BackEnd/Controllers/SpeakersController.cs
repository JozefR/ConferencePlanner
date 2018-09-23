using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using BackEnd.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackEnd.Models;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public SpeakersController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: api/Speakers
        [HttpGet]
        public async Task<IActionResult> GetSpeakers()
        {
            var speakers = await _db.Speakers.AsNoTracking()
                .Include(s => s.SessionSpeakers)
                .ThenInclude(ss => ss.Session)
                .ToListAsync();

            var result = speakers.Select(s => s.MapSpeakerResponse());
            return Ok(result);
        }

        // GET: api/Speakers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpeaker([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var speaker = await _db.Speakers.FindAsync(id);

            if (speaker == null)
            {
                return NotFound();
            }

            return Ok(speaker);
        }

        // PUT: api/Speakers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSpeaker([FromRoute] int id, [FromBody] Speaker speaker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != speaker.ID)
            {
                return BadRequest();
            }

            _db.Entry(speaker).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpeakerExists(id))
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

        // POST: api/Speakers
        [HttpPost]
        public async Task<IActionResult> PostSpeaker([FromBody] Speaker speaker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _db.Speakers.Add(speaker);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetSpeaker", new { id = speaker.ID }, speaker);
        }

        // DELETE: api/Speakers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpeaker([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var speaker = await _db.Speakers.FindAsync(id);
            if (speaker == null)
            {
                return NotFound();
            }

            _db.Speakers.Remove(speaker);
            await _db.SaveChangesAsync();

            return Ok(speaker);
        }

        private bool SpeakerExists(int id)
        {
            return _db.Speakers.Any(e => e.ID == id);
        }
    }
}