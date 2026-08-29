using BiblioManager.API.Dtos.Utilisateur;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Mappers;
using BiblioManager.API.Models;
using BiblioManager.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Xml.Linq;
namespace BiblioManager.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateurController : ControllerBase
    {
        private readonly IUtilisateurRepository _repo;
        private readonly IUtilisateurService _repoSer;

        public UtilisateurController(IUtilisateurRepository repo, IUtilisateurService repoSer)
        {
            _repo = repo;
            _repoSer = repoSer;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employe")]
        public async Task<IActionResult> GetAll()
        {
            var utilisateurs = await _repo.GetAllAsync();
            var utilisateurdtos = utilisateurs.Select(u => u.ToUtilisateurDto()).ToList();
            return Ok(utilisateurdtos);
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            if (User.IsInRole("Utilisateur") || User.IsInRole("Adherent"))
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized();
                if (!int.TryParse(userIdClaim.Value, out var userId))
                    return Unauthorized();
                if (userId != id)
                    return Forbid();
            }
            var utilisateur = await _repo.GetByIdAsync(id);
            if (utilisateur == null)
                return NotFound();
            return Ok(utilisateur.ToUtilisateurDto());
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUtilisateurDto createUtilisateurDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var utilisateurModel = createUtilisateurDto.ToUtilisateurFromCreateUtilisateurDto();
            if (utilisateurModel.RoleUtilisateur == RoleUtilisateurEnum.Employe)
            {
                if (!User.IsInRole("Admin"))
                {
                    return Forbid();
                }
            }
            var utilisateur = await _repo.CreateAsync(utilisateurModel);
            return CreatedAtAction(nameof(GetById), new { id = utilisateur.IdUtilisateur }, utilisateur.ToUtilisateurDto());
        }
        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUtilisateurDto updateUtilisateurDto)
        {
            if (User.IsInRole("Utilisateur") || User.IsInRole("Adherent"))
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (!int.TryParse(userIdClaim!.Value, out var userId))
                    return Unauthorized();
                if (userId != id)
                    return Forbid();
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var userModel = updateUtilisateurDto.ToUtilisateurFromUpdateUtilisateurDto();
            var user = await _repo.UpdateAsync(id, userModel);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user.ToUtilisateurDto());
        }
        [Authorize(Roles = "Admin,Employe")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userDeleted = await _repo.DeleteAsync(id);
            if (!userDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id:int}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ModifierRole(int id, [FromBody] UpdateRoleUtilisateurDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (dto.Role == RoleUtilisateurEnum.Admin)
                return Forbid();

            await _repoSer.ModifierRoleAsync(id, dto.Role);

            return Ok();
        }
    }
}
