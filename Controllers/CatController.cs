using CatMash.Models;
using CatMash.Services;
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
        private readonly CatService _catService;

        public CatController(CatService service)
        {
            _catService = service;

        }

        // GET: api/Todo
        [HttpGet]
        public ActionResult<List<Cat>> Get() =>
           _catService.Get();

        // GET: api/Todo/5
        [HttpGet("{id}", Name = "GetCat")]
        public ActionResult<Cat> Get(string id)
        {
            var cat = _catService.Get(id);

            if (cat == null)
            {
                return NotFound();
            }

            return cat;
        }

        [HttpPost]
        public ActionResult<Cat> Create(Cat cat)
        {
            _catService.Create(cat);

            return CreatedAtRoute("GetCat", new { id = cat.Id.ToString() }, cat);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, Cat catIn)
        {
            var cat = _catService.Get(id);

            if (cat == null)
            {
                return NotFound();
            }

            _catService.Update(id, catIn);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var cat = _catService.Get(id);

            if (cat == null)
            {
                return NotFound();
            }

            _catService.Remove(cat.Id);

            return NoContent();
        }

    }
}