using BiblioManager.API.Dtos.Paiement;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace BiblioManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaiementController : ControllerBase
    {
        private readonly IPaimentRepository _paiementRepository;
        private readonly IPaiementService _paiementService;

        public PaiementController(IPaimentRepository paiementRepository, IPaiementService paiementService)
        {
            _paiementRepository = paiementRepository;
            _paiementService = paiementService;
        }
        [HttpGet]
        public async Task<IActionResult> GetPaiementsUtilisateur(int idUtilisateur)
        {
            var ListPaiements = await _paiementRepository.GetPaiementsUtilisateur(idUtilisateur);
            if (ListPaiements == null || !ListPaiements.Any())
                return NotFound("Aucun paiement trouvé");
            return Ok(ListPaiements.Select(x => x.ToPaiementDto()).ToList());
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var paiement = await _paiementRepository.GetById(id);
            if (paiement == null) return NotFound();
            return Ok(paiement.ToPaiementDto());
        }

        [HttpPost]
        public async Task<IActionResult> InitierPaiementCarte([FromBody] CreatePaiementDto createPaiementDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var paiement = await _paiementService.InitierPaiementCarte(createPaiementDto.ToPaiementFromCreatePaiementDto());
            return CreatedAtAction(nameof(GetById), new { id = paiement.IdPaiement }, paiement);
        }

        [HttpPut]
        public async Task<IActionResult> TraiterPaiementCarte([FromBody] StripeWebhookDto stripeWebhookDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                bool paiementReussi = stripeWebhookDto.Status.Equals("paid");
                await _paiementService.TraiterPaiementCarte(
                    stripeWebhookDto.StripeSessionId, paiementReussi);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
