using BiblioManager.API.Dtos;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Mappers;
using BiblioManager.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [HttpPost("{id}/devenir-adherent")]
        [Authorize(Roles = "Admin,Employe,Utilisateur")]
        public async Task<IActionResult> DevenirAdherent(int id)
        {
            if (User.IsInRole("Utilisateur"))
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                if (userId != id)
                {
                    return Forbid();
                }
            }
            await _service.DevenirAdherent(id);
            return Ok("Utilisateur est maintenant Adhérent");

        }
        [HttpGet("{idAdherent:int}")]
        [Authorize(Roles = "Admin,Employe,Adherent")]
        public async Task<IActionResult> GetAdherentById(int idAdherent)
        {
            if (User.IsInRole("Adherent"))
            {
                var idAdherentClaim = User.FindFirst("IdAdherent");

                if (idAdherentClaim == null)
                    return Unauthorized();

                if (!int.TryParse(idAdherentClaim.Value, out var idAd))
                    return Unauthorized();

                if (idAdherent != idAd)
                    return Forbid();
            }
            var adherent = await _repo.GetAdherentById(idAdherent);
            if (adherent == null)
            {
                return NotFound();
            }
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
            return Ok(adherents?.Select(x=>x.ToAdherentDto()).ToList());
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
            var IsActif = await _service.DesactiverAdhesion(id);
            if (IsActif)
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
        [HttpPut("renouveler/{idAdherent}")]
        [Authorize(Roles = "Admin,Employe,Adherent")]
        public async Task<IActionResult> RenouvelerAbonnement(int idAdherent)
        {
            if (User.IsInRole("Adherent"))
            {
                var idAdherentClaim = User.FindFirst("IdAdherent");

                if (idAdherentClaim == null)
                    return Unauthorized();

                if (!int.TryParse(idAdherentClaim.Value, out var idAd))
                    return Unauthorized();

                if (idAdherent != idAd)
                    return Forbid();
            }
            await _service.RenouvelerAbonnement(idAdherent);
            return Ok(new
            {
                Message = "Abonnement renouvelé avec succès."
            });
        }
    }
}
