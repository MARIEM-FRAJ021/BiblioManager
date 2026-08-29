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

        public EmpruntController(IEmpruntService empruntService, IEmpruntRepository empruntRepository)
        {
            _empruntService = empruntService;
            _empruntRepository = empruntRepository;
        }

        [HttpPost("{idAdherent}/{idLivre}")]
        [Authorize(Roles = "Admin,Employe,Adherent")]
        public async Task<IActionResult> EmprunterLivre(int idAdherent, int idLivre)
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
            await _empruntService.EmprunterLivre(idAdherent, idLivre);
            return Ok(new
            {
                Message = "Livre emprunté avec succès."
            });
        }
        [HttpPut("{idAdherent}/{idEmprunt}/retour")]
        [Authorize(Roles = "Admin,Employe,Adherent")]
        public async Task<IActionResult> RetournerLivre(int idAdherent, int idEmprunt)
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

            await _empruntService.RetournerLivre(idAdherent, idEmprunt);
            return Ok(new
            {
                Message = "Livre retourné avec succès."
            });
        }

        [HttpGet("historique/{idAdherent}")]
        [Authorize(Roles = "Admin,Employe,Adherent")]
        public async Task<IActionResult> GetHistorique(int idAdherent)
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
