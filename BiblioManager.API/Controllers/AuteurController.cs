using BiblioManager.API.Dtos.Auteur;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioManager.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuteurController : ControllerBase
    {
        private readonly IAuteurRepository _repo;

        public AuteurController(IAuteurRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employe,Adherent")]
        public async Task<IActionResult> GetAll()
        {
            var auteurs = await _repo.GetAllAsync();
            var auteurdtos = auteurs.Select(c => c.ToAuteurDto()).ToList();
            return Ok(auteurdtos);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Employe,Adherent")]
        public async Task<IActionResult> GetById(int id)
        {
            var auteur = await _repo.GetByIdAsync(id);
            if (auteur == null)
                return NotFound();
            return Ok(auteur.ToAuteurDto());
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employe")]
        public async Task<IActionResult> Create([FromBody] CreateAuteurDto createAuteurDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var auteurModel = createAuteurDto.ToAuteurFromCreateAuteurDto();
            var auteur = await _repo.CreateAsync(auteurModel);
            return CreatedAtAction(nameof(GetById), new { id = auteur.IdAuteur }, auteur.ToAuteurDto());
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Employe")]
        public async Task<IActionResult> Update([FromBody] UpdateAuteurDto updateAuteurDto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var auteurModel = updateAuteurDto.ToAuteurFromUpdateAuteurDto();
            var auteur = await _repo.UpdateAsync(id, auteurModel);
            if (auteur == null)
            {
                return NotFound();
            }

            return Ok(auteur.ToAuteurDto());
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Employe")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var auteurDeleted = await _repo.DeleteAsync(id);
            if (!auteurDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
