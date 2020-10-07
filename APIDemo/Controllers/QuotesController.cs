using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIDemo.Data;
using APIDemo.Models;

namespace APIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //podle id quote, vrátí tagy
        [HttpGet("{id}/tags")]
        public async Task<ActionResult<IEnumerable<Tag>>> GetQuoteTags(int id)
        {
            var quote = _context.Quotes.Where(q => q.Id == id)
                    .Include(s => s.QuoteTags)
                    .ThenInclude(tag => tag.Tag);

            if (quote == null)
            {
                return NotFound();
            }

            return Ok(quote.Select(item => item.QuoteTags.Select(tag => tag.Tag)).ToList());
        }

        //vytvoří nové mezitabulky
        //ke quote přidá další tagy
        [HttpPost("{id}/tags")]
        public async Task<ActionResult<Quote>> PostTags(int id, [FromBody] IEnumerable<int> tagIds)
        {
            IList<QuoteTag> quoteTags = new List<QuoteTag>();
            foreach (var item in tagIds)
            {
                QuoteTag newQuote = new QuoteTag
                {
                    QuoteId = id,
                    TagId = item
                };
                quoteTags.Add(newQuote);
            }
            _context.QuoteTags.AddRange(quoteTags);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTags", quoteTags);
        }


        // GET: api/Quotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quote>>> GetQuotes()
        {
            return await _context.Quotes.ToListAsync();
        }

        // GET: api/Quotes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Quote>> GetQuote(int id)
        {
            var quote = await _context.Quotes.FindAsync(id);

            if (quote == null)
            {
                return NotFound();
            }

            return quote;
        }

        // PUT: api/Quotes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuote(int id, Quote quote)
        {
            if (id != quote.Id)
            {
                return BadRequest();
            }

            _context.Entry(quote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuoteExists(id))
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

        // POST: api/Quotes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Quote>> PostQuote(Quote quote)
        {
            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuote", new { id = quote.Id }, quote);
        }

        // DELETE: api/Quotes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Quote>> DeleteQuote(int id)
        {
            var quote = await _context.Quotes.FindAsync(id);
            if (quote == null)
            {
                return NotFound();
            }

            _context.Quotes.Remove(quote);
            await _context.SaveChangesAsync();

            return quote;
        }

        private bool QuoteExists(int id)
        {
            return _context.Quotes.Any(e => e.Id == id);
        }
    }
}
