using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyKanban.Data;
using MyKanban.Models;

namespace MyKanban.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LabelsController : ControllerBase
{
    private readonly MyKanbanDbContext _db;

    public LabelsController(MyKanbanDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Label>>> GetAll()
    {
        var items = await _db.Labels.AsNoTracking().ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Label>> GetById(int id)
    {
        var item = await _db.Labels.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<Label>> Create(Label input)
    {
        _db.Labels.Add(input);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = input.Id }, input);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Label input)
    {
        if (id != input.Id)
        {
            return BadRequest();
        }

        var exists = await _db.Labels.AnyAsync(x => x.Id == id);
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
        var item = await _db.Labels.FindAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        _db.Labels.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
