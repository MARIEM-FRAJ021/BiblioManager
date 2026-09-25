using BiblioManager.API.Dtos.Paiement;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Mappers;
using BiblioManager.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace BiblioManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaiementController : ControllerBase
    {
        private readonly IPaimentRepository _paiementRepository;
        private readonly IPaiementService _paiementService;
        private readonly IConfiguration _configuration;


        public PaiementController(IPaimentRepository paiementRepository, IPaiementService paiementService, IConfiguration configuration)
        {
            _paiementRepository = paiementRepository;
            _paiementService = paiementService;
            _configuration = configuration;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Employe,Adherent")]
        public async Task<IActionResult> GetPaiementsUtilisateur(int idUtilisateur)
        {
            if (User.IsInRole("Adherent"))
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized();
                }
                var userId = int.Parse(userIdClaim.Value);
                if (idUtilisateur != userId)
                {
                    return Forbid();
                }
            }
            var ListPaiements = await _paiementRepository.GetPaiementsUtilisateur(idUtilisateur);
            if (ListPaiements == null || !ListPaiements.Any())
                return NotFound("Aucun paiement trouvé");
            return Ok(ListPaiements.Select(x => x.ToPaiementDto()).ToList());
        }
        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Employe")]
        public async Task<IActionResult> GetById(int id)
        {
            var paiement = await _paiementRepository.GetById(id);
            if (paiement == null) return NotFound();
            return Ok(paiement.ToPaiementDto());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> InitierPaiementCarte([FromBody] CreatePaiementDto createPaiementDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized();

            if (!int.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stripeUrl = await _paiementService.InitierPaiementCarte(createPaiementDto.Type, userId);

            return Ok(stripeUrl);
        }

        [HttpPost("traiter-paiement")]
        [AllowAnonymous] 
        public async Task<IActionResult> TraiterPaiementCarte()
        {
            Request.EnableBuffering(); 
            var json = await new StreamReader(HttpContext.Request.Body, leaveOpen: true).ReadToEndAsync();
            Request.Body.Position = 0;

            var signature = Request.Headers["Stripe-Signature"].FirstOrDefault();
            if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(json))
                return BadRequest();

            var webhookSecret = _configuration["Stripe:webhookSecret"];
            if (string.IsNullOrEmpty(webhookSecret))
                return StatusCode(StatusCodes.Status500InternalServerError, "Stripe webhook Secret non configuré");

            Event stripeEvent = EventUtility.ConstructEvent(json, signature, webhookSecret);
            if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted
                && stripeEvent.Data.Object is Session session)
            {
                await _paiementService.TraiterPaiementCarte(session);
            }
            
            return Ok();
        }
    }
}
