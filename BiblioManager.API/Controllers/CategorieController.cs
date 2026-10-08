using BiblioManager.API.Dtos.Categorie;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioManager.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategorieController : ControllerBase
    {
        private readonly ICategorieRepository _repo;
        private readonly IAdherentRepository _repoAdherent;

        public CategorieController(ICategorieRepository repo, IAdherentRepository repoAdherent)
        {
            _repo = repo;
            _repoAdherent = repoAdherent;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (User.IsInRole("Utilisateur"))
            {
                var adherentIdClaim = User.FindFirst("IdAdherent");

                if (adherentIdClaim == null)
                    return Forbid();

                if (!int.TryParse(adherentIdClaim.Value, out var adherentId))
                    return Unauthorized();
                
                var adherent = await _repoAdherent.GetAdherentById(adherentId);

                if (!adherent!.Actif)
                    return Forbid();
            }
            var categs = await _repo.GetAllAsync();
            var categdtos = categs.Select(c => c.ToCategorieDto()).ToList();
            return Ok(categdtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (User.IsInRole("Utilisateur"))
            {
                var adherentIdClaim = User.FindFirst("IdAdherent");

                if (adherentIdClaim == null)
                    return Forbid();

                if (!int.TryParse(adherentIdClaim.Value, out var adherentId))
                    return Unauthorized();
                var adherent = await _repoAdherent.GetAdherentById(adherentId);

                if (!adherent!.Actif)
                    return Forbid();
            }
            var categ = await _repo.GetByIdAsync(id);
            if (categ == null)
                return NotFound();
            return Ok(categ.ToCategorieDto());
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employe")]
        public async Task<IActionResult> Create([FromBody] CreateCategorieDto createCategorieDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var categModel = createCategorieDto.ToCategorieFromCreateCategorieDto();
            var categ = await _repo.CreateAsync(categModel);
            return CreatedAtAction(nameof(GetById), new { id = categ.Id }, categ.ToCategorieDto());
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Employe")]
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
            return Ok(categ.ToCategorieDto());
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Employe")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var categDeleted = await _repo.DeleteAsync(id);
            if (!categDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
