using CatMash.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatMashApi.Controllers
{
    [Route("api/cats")]
    [ApiController]
    public class CatController : ControllerBase
    {
        private readonly CatContext _context;

        public CatController(CatContext context)
        {
            _context = context;

            if (_context.Cats.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.Cats.Add(new Cat { Id = "Item1" });
                _context.SaveChanges();
            }
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cat>>> GetTodoItems()
        {
            return await _context.Cats.ToListAsync();
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cat>> GetTodoItem(long id)
        {
            var cat = await _context.Cats.FindAsync(id);

            if (cat == null)
            {
                return NotFound();
            }

            return cat;
        }
    }
}