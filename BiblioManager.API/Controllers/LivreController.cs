using BiblioManager.API.Dtos.Livre;
using BiblioManager.API.Interfaces;
using BiblioManager.API.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BiblioManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivreController : ControllerBase
    {
        private readonly ILivreRepository _repo;

        public LivreController (ILivreRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var livres = await _repo.GetAllAsync();
            var livreDtos = livres.Select(l=> l.ToLivreDto()).ToList();
            return Ok(livreDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var livre = await _repo.GetByIdAsync(id);
            if (livre == null)
                return NotFound();
            return Ok(livre.ToLivreDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLivreDto createLivreDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var livreModel = createLivreDto.ToLivreFromCreateLivreDto();
            var livre = await _repo.CreateAsync(livreModel);
            return CreatedAtAction(nameof(GetById), new { id = livre.IdLivre }, livre);
        }

    }
}
