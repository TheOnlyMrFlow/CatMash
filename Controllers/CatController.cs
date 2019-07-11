using CatMash.Models;
using CatMash.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatMashApi.Controllers
{
    [Route("cats")]
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
        public ActionResult<List<Cat>> Get()
        {

            
            string sortBy = HttpContext.Request.Query["sortBy"].ToString();

            switch (sortBy)
            {
                case "elo":
                    return _catService.Get(CatService.SortBy.elo, true);

                case "occurence":
                    return _catService.Get(CatService.SortBy.occurence);

                default:
                    return _catService.Get();
            }


        }

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


            List<Cat> cats = _catService.Get(CatService.SortBy.occurence);
            int count = cats.Count();
            var ran = new System.Random();

            // linear distribution between 0 and 1
            double ranNum = ran.NextDouble();

            // makes it more likely to be close to 0, i.e the begining of the list, i.e a cat that has not much occurence yet
            ranNum = System.Math.Pow(ranNum, 3);

            // transpose index from [0,1] to [0, length of list]
            int indexOne = (int)System.Math.Floor(ranNum * (count - 1));


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

        [HttpPost("admin")]
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


            if (catLoser == null || catWinner == null)
            {
                System.Diagnostics.Debug.WriteLine(catLoser);
                return NotFound();
            }

            int delta = EloService.CalculateDelta(catWinner.Elo, catLoser.Elo);

            catWinner.Elo += delta;
            catLoser.Elo -= delta;
            catWinner.Occurences++;
            catLoser.Occurences++;

            _catService.Update(catWinner.Id, catWinner);
            _catService.Update(catLoser.Id, catLoser);

            return Ok(delta);

            //return CreatedAtRoute("GetCat", new { id = cat.Id.ToString() }, cat);


        }

        [HttpPut("admin/{id}")]
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

        [HttpDelete("admin/{id}")]
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