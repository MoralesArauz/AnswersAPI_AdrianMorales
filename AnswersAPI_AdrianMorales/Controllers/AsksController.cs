﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnswersAPI_AdrianMorales.Models;
using AnswersAPI_AdrianMorales.Attributes;

namespace AnswersAPI_AdrianMorales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class AsksController : ControllerBase
    {
        private readonly AnswerDBContext _context;

        public AsksController(AnswerDBContext context)
        {
            _context = context;
        }

        // GET: api/Asks
        [HttpGet("GetQuestionsListByUserID")]
        public async Task<ActionResult<IEnumerable<Ask>>> GetQuestionsListByUserID(int pUserID)
        {
            // Estp es el equivalente a un select con where
            List<Ask> QList = await _context.Asks.Where(x => x.UserId == pUserID).ToListAsync();

            if (QList == null)
            {
                return NotFound();
            }
            return QList;
        }


        // GET: api/Asks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ask>>> GetAsks()
        {
            return await _context.Asks.ToListAsync();
        }

        // GET: api/Asks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ask>> GetAsk(long id)
        {
            var ask = await _context.Asks.FindAsync(id);

            if (ask == null)
            {
                return NotFound();
            }

            return ask;
        }

        // PUT: api/Asks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsk(long id, Ask ask)
        {
            if (id != ask.AskId)
            {
                return BadRequest();
            }

            _context.Entry(ask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AskExists(id))
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

        // POST: api/Asks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ask>> PostAsk(Ask ask)
        {
            _context.Asks.Add(ask);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAsk", new { id = ask.AskId }, ask);
        }

        // DELETE: api/Asks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsk(long id)
        {
            var ask = await _context.Asks.FindAsync(id);
            if (ask == null)
            {
                return NotFound();
            }

            _context.Asks.Remove(ask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AskExists(long id)
        {
            return _context.Asks.Any(e => e.AskId == id);
        }
    }
}
