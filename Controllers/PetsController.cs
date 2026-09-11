using ChallengePetApi.Data;
using ChallengePetApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChallengePetApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PetsController : ControllerBase
{
    private readonly AppDbContext _context;

    public PetsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pet>>> GetPets()
    {
        return Ok(await _context.Pets.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Pet>> GetPet(int id)
    {
        var pet = await _context.Pets.FindAsync(id);

        if (pet == null)
            return NotFound();

        return Ok(pet);
    }

    [HttpGet("nome/{nome}")]
    public async Task<ActionResult<IEnumerable<Pet>>> BuscarPorNome(string nome)
    {
        var pets = await _context.Pets
            .Where(p => p.Nome != null && p.Nome.Contains(nome))
            .ToListAsync();

        return Ok(pets);
    }

    [HttpGet("risco/{risco}")]
    public async Task<ActionResult<IEnumerable<Pet>>> BuscarPorRisco(string risco)
    {
        var pets = await _context.Pets
            .Where(p => p.NivelRisco == risco)
            .ToListAsync();

        return Ok(pets);
    }

    [HttpPost]
    public async Task<ActionResult> PostPet(Pet pet)
    {
        _context.Pets.Add(pet);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPet),
            new { id = pet.Id }, pet);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> PutPet(int id, Pet pet)
    {
        if (id != pet.Id)
            return BadRequest();

        _context.Entry(pet).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePet(int id)
    {
        var pet = await _context.Pets.FindAsync(id);

        if (pet == null)
            return NotFound();

        _context.Pets.Remove(pet);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}