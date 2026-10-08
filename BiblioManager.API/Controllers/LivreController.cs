using BiblioManager.API.Dtos.Livre;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Mappers;
using BiblioManager.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioManager.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LivreController : ControllerBase
    {
        private readonly ILivreRepository _repo;
        private readonly IAdherentRepository _repoAdherent;

        public LivreController(ILivreRepository repo, IAdherentRepository repoAdherent)
        {
            _repo = repo;
            _repoAdherent = repoAdherent;
        }
        [HttpGet]
        [Authorize]
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
            var livres = await _repo.GetAllAsync();
            var livreDtos = livres.Select(l => l.ToLivreDto()).ToList();
            return Ok(livreDtos);
        }

        [HttpGet("{id:int}")]
        [Authorize]
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
            var livre = await _repo.GetByIdAsync(id);
            if (livre == null)
                return NotFound();
            return Ok(livre.ToLivreDto());
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employe")]
        public async Task<IActionResult> Create([FromBody] CreateLivreDto createLivreDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var livreModel = createLivreDto.ToLivreFromCreateLivreDto();
            var livre = await _repo.CreateAsync(livreModel);
            return CreatedAtAction(nameof(GetById), new { id = livre.IdLivre }, livre.ToLivreDto());
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Employe")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateLivreDto updatelivreDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var livreModel = updatelivreDto.ToLivreFromUpdateLivreDto();
            var livre = await _repo.UpdateAsync(id, livreModel);
            if (livre == null)
            {
                return NotFound();
            }
            return Ok(livre.ToLivreDto());
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
