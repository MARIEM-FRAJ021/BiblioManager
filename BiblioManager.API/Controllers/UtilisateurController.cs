using BiblioManager.API.Dtos.Auteur;
using BiblioManager.API.Dtos.Utilisateur;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BiblioManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateurController : ControllerBase
    {
        private readonly IUtilisateurRepository _repo;

        public UtilisateurController(IUtilisateurRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var utilisateurs = await _repo.GetAllAsync();
            var utilisateurdtos = utilisateurs.Select(u => u.ToUtilisateurDto()).ToList();
            return Ok(utilisateurdtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var utilisateur = await _repo.GetByIdAsync(id);
            if (utilisateur == null)
                return NotFound();
            return Ok(utilisateur.ToUtilisateurDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUtilisateurDto createUtilisateurDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var utilisateurModel = createUtilisateurDto.ToUtilisateurFromCreateUtilisateurDto();
            var utilisateur = await _repo.CreateAsync(utilisateurModel);
            return CreatedAtAction(nameof(GetById), new { id = utilisateur.IdUtilisateur }, utilisateur);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromBody] UpdateUtilisateurDto updateUtilisateurDto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userModel = updateUtilisateurDto.ToUtilisateurFromUpdateUtilisateurDto();
            var user = await _repo.UpdateAsync(id, userModel);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var userDeleted = await _repo.DeleteAsync(id);
                if (!userDeleted)
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
