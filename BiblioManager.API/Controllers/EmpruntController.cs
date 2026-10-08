using BiblioManager.API.Interfaces;
using BiblioManager.API.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiblioManager.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmpruntController : ControllerBase
    {
        private readonly IEmpruntService _empruntService;
        private readonly IEmpruntRepository _empruntRepository;
        private readonly IAdherentRepository _repoAdherent;

        public EmpruntController(IEmpruntService empruntService, IEmpruntRepository empruntRepository, IAdherentRepository repoAdherent)
        {
            _empruntService = empruntService;
            _empruntRepository = empruntRepository;
            _repoAdherent = repoAdherent;
        }

        [HttpPost("{idLivre}")]
        [Authorize]
        public async Task<IActionResult> EmprunterLivre(int idLivre)
        {
            var idAdherentClaim = User.FindFirst("IdAdherent");

            if (idAdherentClaim == null)
                return Forbid();

            if (!int.TryParse(idAdherentClaim.Value, out var idAdherent))
                return Unauthorized();

            var adherent = await _repoAdherent.GetAdherentById(idAdherent);

            if (!adherent!.Actif)
                return Forbid();

            await _empruntService.EmprunterLivre(idAdherent, idLivre);
            return Ok(new
            {
                Message = "Livre emprunté avec succès."
            });
        }

        [HttpPut("{idEmprunt}/retour")]
        [Authorize]
        public async Task<IActionResult> RetournerLivre(int idEmprunt)
        {
            var idAdherentClaim = User.FindFirst("IdAdherent");

            if (idAdherentClaim == null)
                return Forbid();

            if (!int.TryParse(idAdherentClaim.Value, out var idAdherent))
                return Unauthorized();

            var adherent = await _repoAdherent.GetAdherentById(idAdherent);

            if (!adherent!.Actif)
                return Forbid();

            await _empruntService.RetournerLivre(idAdherent, idEmprunt);
            return Ok(new
            {
                Message = "Livre retourné avec succès."
            });
        }

        [HttpGet("me/historique")]
        [Authorize]
        public async Task<IActionResult> GetMonHistorique()
        {
            var idAdherentClaim = User.FindFirst("IdAdherent");

            if (idAdherentClaim == null)
                return Forbid();

            if (!int.TryParse(idAdherentClaim.Value, out var idAdherent))
                return Unauthorized();
            
            var adherent = await _repoAdherent.GetAdherentById(idAdherent);

            if (!adherent!.Actif)
                return Forbid();

            var emprunts = await _empruntRepository.GetHistoriqueByAdherent(idAdherent);
            return Ok(emprunts.Select(x => x.ToEmpruntDto()).ToList());
        }

        [HttpGet("historique/{idAdherent}")]
        [Authorize(Roles = "Admin,Employe")]
        public async Task<IActionResult> GetHistorique(int idAdherent)
        {
            var emprunts = await _empruntRepository.GetHistoriqueByAdherent(idAdherent);
            return Ok(emprunts.Select(x => x.ToEmpruntDto()).ToList());
        }

        [HttpGet("retards")]
        [Authorize(Roles = "Admin,Employe")]
        public async Task<IActionResult> GetEmpruntsEnRetard()
        {
            var emprunts = await _empruntRepository.GetEmpruntsEnRetard();
            return Ok(emprunts.Select(x => x.ToEmpruntDto()).ToList());
        }
    }
}
