using BiblioManager.API.Interfaces;
using BiblioManager.API.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioManager.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdherentController : ControllerBase
    {
        private readonly IAdherentRepository _repo;
        private readonly IAdherentService _service;

        public AdherentController(IAdherentRepository repo, IAdherentService service)
        {
            _repo = repo;
            _service = service;
        }
        [HttpGet("me")]
        public async Task<IActionResult> GetMyAdherent()
        {
            var idAdherentClaim = User.FindFirst("IdAdherent");

            if (idAdherentClaim == null)
                return Forbid();

            if (!int.TryParse(idAdherentClaim.Value, out var idAdherent))
                return Unauthorized();

            var adherent = await _repo.GetAdherentById(idAdherent);

            if (adherent == null)
                return NotFound();
            if (adherent.Actif == false)
                return Forbid();
            return Ok(adherent.ToAdherentDto());
        }
        [HttpGet("{idAdherent:int}")]
        [Authorize(Roles = "Admin,Employe")]
        public async Task<IActionResult> GetAdherentById(int idAdherent)
        {
            var adherent = await _repo.GetAdherentById(idAdherent);

            if (adherent == null)
                return NotFound();

            return Ok(adherent.ToAdherentDto());
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employe")]
        public async Task<IActionResult> GetAdherents()
        {
            var adherents = await _repo.GetAdherents();
            return Ok(adherents?.Select(x => x.ToAdherentDto()).ToList());
        }
        [HttpGet("actifs")]
        [Authorize(Roles = "Admin,Employe")]
        public async Task<IActionResult> GetAdherentsActifs()
        {
            var adherents = await _repo.GetAdherentActifs();
            return Ok(adherents?.Select(x => x.ToAdherentDto()).ToList());
        }
        [HttpGet("{idAdherent}/verifier")]
        [Authorize(Roles = "Admin,Employe")]
        public async Task<IActionResult> VerifierAdherent(int idAdherent)
        {
            await _service.VerifierAdherentActif(idAdherent);

            return Ok(new
            {
                Message = "Adhérent actif."
            });
        }
        [HttpPut("desactiver/{id}")]
        [Authorize(Roles = "Admin,Employe")]
        public async Task<IActionResult> DesactiverAdhesion(int id)
        {
            var IsInactive = await _service.DesactiverAdhesion(id);
            if (!IsInactive)
                return Conflict("L'adhésion est déjà active.");
            return Ok(new
            {
                Message = "Adhésion désactivée."
            });
        }
        [HttpGet("utilisateur/{userId}")]
        [Authorize(Roles = "Admin,Employe")]
        public async Task<IActionResult> UserIsAdherent(int userId)
        {
            var existe = await _repo.UserIsAdherent(userId);
            return Ok(existe);
        }
    }
}
