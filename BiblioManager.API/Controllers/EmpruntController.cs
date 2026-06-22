using BiblioManager.API.Interfaces;
using BiblioManager.API.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace BiblioManager.API.Controllers
{
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
        public async Task<IActionResult> EmprunterLivre(int idAdherent, int idLivre)
        {
            await _empruntService.EmprunterLivre(idAdherent, idLivre);
            return Ok(new
            {
                Message = "Livre emprunté avec succès."
            });
        }
        [HttpPut("{idEmprunt}/retour")]
        public async Task<IActionResult> RetournerLivre(int idEmprunt)
        {
            await _empruntService.RetournerLivre(idEmprunt);
            return Ok(new
            {
                Message = "Livre retourné avec succès."
            });
        }

        [HttpGet("historique/{idAdherent}")]
        public async Task<IActionResult> GetHistorique(int idAdherent)
        {
            var emprunts = await _empruntRepository.GetHistoriqueByAdherent(idAdherent);
            return Ok(emprunts.Select(x=> x.ToEmpruntDto()).ToList());
        }

        [HttpGet("retards")]
        public async Task<IActionResult> GetEmpruntsEnRetard()
        {
            var emprunts = await _empruntRepository.GetEmpruntsEnRetard();
            return Ok(emprunts.Select(x=> x.ToEmpruntDto()).ToList());
        }
    }
}
