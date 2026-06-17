using BiblioManager.API.Dtos;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Mappers;
using BiblioManager.API.Models;
using BiblioManager.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace BiblioManager.API.Controllers
{
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
        public async Task<IActionResult> DevenirAdherent(int id)
        {
            await _service.DevenirAdherent(id);
            return Ok("Utilisateur est maintenant Adhérent");

        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAdherentById(int id)
        {
            var adherent = await _repo.GetAdherentById(id);
            if(adherent == null)
            {
                return NotFound();
            }
            return Ok(adherent.ToAdherentDto());
        }

        [HttpGet]
        public async Task<IActionResult> GetAdherents()
        {
            var adherents = await _repo.GetAdherents();
            return Ok(adherents);
        }
        [HttpGet("actifs")]
        public async Task<IActionResult> GetAdherentsActifs()
        {
            var adherents = await _repo.GetAdherentActifs();
            return Ok(adherents);
        }
        [HttpGet("{idAdherent}/verifier")]
        public async Task<IActionResult> VerifierAdherent(int idAdherent)
        {
            await _service.VerifierAdherentActif(idAdherent);

            return Ok(new
            {
                Message = "Adhérent actif."
            });
        }
        [HttpPut("desactiver/{id}")]
        public async Task<IActionResult> DesactiverAdhesion (int id)
        {
            var IsActif = await _service.DesactiverAdhesion(id);
            if (IsActif)
                return Conflict("L'adhésion est déjà active.");
            return Ok( new
            {
                Message ="Adhésion désactivée."
            });
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdherent(int id, [FromBody] AdherentDto adherentDto )
        {
            await _repo.UpdateAdherent(id, adherentDto.ToAdherentFromAdherentDto());
            return Ok(new
            {
                Message = "Adhérent mis à jour."
            });
        }
        [HttpGet("utilisateur/{userId}")]
        public async Task<IActionResult> UserIsAdherent(int userId)
        {
            var existe = await _repo.UserIsAdherent(userId);
            return Ok(existe);
        }
        [HttpPut("renouveler/{id}")]
        public async Task<IActionResult> RenouvelerAbonnement(int id)
        {
            await _service.RenouvelerAbonnement(id);
            return Ok(new
            {
                Message = "Abonnement renouvelé avec succès."
            });
        }
    }
}
