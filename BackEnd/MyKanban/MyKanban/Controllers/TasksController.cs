using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyKanban.Data;
using MyKanban.Models;

namespace MyKanban.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly MyKanbanDbContext _db;

    public TasksController(MyKanbanDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<KanbanTask>>> GetAll()
    {
        var items = await _db.Tasks.AsNoTracking().ToListAsync();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<KanbanTask>> GetById(int id)
    {
        var item = await _db.Tasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<KanbanTask>> Create(KanbanTask input)
    {
        _db.Tasks.Add(input);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = input.Id }, input);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, KanbanTask input)
    {
        if (id != input.Id)
        {
            return BadRequest();
        }

        var exists = await _db.Tasks.AnyAsync(x => x.Id == id);
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
        var item = await _db.Tasks.FindAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        _db.Tasks.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
