using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyKanban.Data;
using MyKanban.Models;

namespace MyKanban.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusesController : ControllerBase
{
    private readonly MyKanbanDbContext _db;

    public StatusesController(MyKanbanDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Status>>> GetAll()
    {
        var items = await _db.Statuses.AsNoTracking().ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Status>> GetById(int id)
    {
        var item = await _db.Statuses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Status>> Create(Status input)
    {
        _db.Statuses.Add(input);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = input.Id }, input);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Status input)
    {
        if (id != input.Id)
        {
            return BadRequest();
        }

        var exists = await _db.Statuses.AnyAsync(x => x.Id == id);
        if (!exists)
        {
            return NotFound();
        }

        _db.Entry(input).State = EntityState.Modified;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _db.Statuses.FindAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        _db.Statuses.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
