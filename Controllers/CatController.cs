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

        [HttpGet("pick")]
        public ActionResult<Cat[]> PickOpponents()
        {

            List<Cat> cats = _catService.Get();
            int count = cats.Count();
            var ran = new System.Random();

            int indexOne = ran.Next(count);
            int indexTwo;
            do
            {
                indexTwo = ran.Next(count);
            }
            while (indexOne == indexTwo);

            var catOne = cats[indexOne];
            var catTwo = cats[indexTwo];

           
            return new Cat[] { catOne, catTwo };
        }

        [HttpPost]
        public ActionResult<Cat> Create(Cat cat)
        {
            cat.Elo = 1000;
            cat.Occurences = 0;
            _catService.Create(cat);

            return CreatedAtRoute("GetCat", new { id = cat.Id.ToString() }, cat);
        }

        [HttpPatch("{idWinner}/mashes/{idLoser}")]
        public ActionResult<Cat> Mashes(string idWinner, string idLoser)
        {

            if (idWinner == idLoser)
            {
                return BadRequest("Cannot mash a cat against himself");
            }

            var catWinner= _catService.Get(idWinner);
            var catLoser = _catService.Get(idLoser);

            int delta = EloService.CalculateDelta(catWinner.Elo, catLoser.Elo);

            if (catLoser == null || catWinner == null)
            {
                System.Diagnostics.Debug.WriteLine(catLoser);
                return NotFound();
            }

            return Ok(delta);

            //return CreatedAtRoute("GetCat", new { id = cat.Id.ToString() }, cat);


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