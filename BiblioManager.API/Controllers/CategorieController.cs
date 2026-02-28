using BiblioManager.API.Dtos.Auteur;
using BiblioManager.API.Dtos.Categorie;
using BiblioManager.API.Dtos.Livre;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BiblioManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategorieController : ControllerBase
    {
        private readonly ICategorieRepository _repo;

        public CategorieController(ICategorieRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categs = await _repo.GetAllAsync();
            var categdtos = categs.Select(c => c.ToCategorieDto()).ToList();
            return Ok(categdtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var categ = await _repo.GetByIdAsync(id);
            if (categ == null)
                return NotFound();
            return Ok(categ.ToCategorieDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategorieDto createCategorieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var categModel = createCategorieDto.ToCategorieFromCreateCategorieDto();
            var categ = await _repo.CreateAsync(categModel);
            return CreatedAtAction(nameof(GetById), new { id = categ.Id }, categ);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCategorieDto updateCategorieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var categModel = updateCategorieDto.ToCategorieFromUpdateCategorieDto();
            var categ = await _repo.UpdateAsync(id, categModel);
            if (categ == null)
            {
                return NotFound();
            }
            return Ok(categ);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var categDeleted = await _repo.DeleteAsync(id);
                if (!categDeleted)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

    }
}
