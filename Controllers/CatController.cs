using CatMash.Models;
using CatMash.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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

        [HttpGet]
        public ActionResult<IEnumerable<Cat>> Get()
        {
            string sortBy = HttpContext.Request.Query["sortBy"].ToString();

            switch (sortBy)
            {
                case "elo":
                    return Ok(_catService.Get(CatService.SortBy.elo, true));

                case "occurence":
                    return Ok(_catService.Get(CatService.SortBy.occurence));

                default:
                    return Ok(_catService.Get());
            }
        }

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
            return _catService.GetRandomMatch();
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
                return NotFound();
            }

            _catService.SaveMatchResult(catWinner, catLoser);

            return Ok();
        }

        [HttpPost("admin")]
        public ActionResult<Cat> Create(Cat cat)
        {
            _catService.Create(cat);
            return CreatedAtRoute("GetCat", new { id = cat.Id.ToString() }, cat);
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
            return Ok();
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

        [HttpPatch("admin/reset/elo")]
        public IActionResult ResetElo(string id)
        {
            _catService.ResetElo();
            return Ok("Elo reset");
        }

        [HttpPatch("admin/reset/occurence")]
        public IActionResult ResetOccurence(string id)
        {
            _catService.ResetOccurence();
            return Ok("Occurence reset");
        }
    }
}