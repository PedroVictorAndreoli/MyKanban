// Hubs/KanbanHub.cs
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MyKanban.Data;
using MyKanban.Models;

namespace MyKanban.Hubs;

public class KanbanHub : Hub
{
    private readonly MyKanbanDbContext _db;

    public KanbanHub(MyKanbanDbContext db)
    {
        _db = db;
    }

    // Tasks
    public async Task<IEnumerable<KanbanTask>> GetAllTasks()
    {
        return await _db.Tasks.AsNoTracking().ToListAsync();
    }

    public async Task<KanbanTask?> GetTaskById(int id)
    {
        return await _db.Tasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<KanbanTask> CreateTask(KanbanTask input)
    {
        _db.Tasks.Add(input);
        await _db.SaveChangesAsync();

        // Notifica todos os clientes conectados
        await Clients.All.SendAsync("TaskCreated", input);
        return input;
    }

    public async Task<bool> UpdateTask(int id, KanbanTask input)
    {
        if (id != input.Id)
            return false;

        var exists = await _db.Tasks.AnyAsync(x => x.Id == id);
        if (!exists)
            return false;

        _db.Entry(input).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        // Notifica todos os clientes conectados
        await Clients.All.SendAsync("TaskUpdated", input);
        return true;
    }

    public async Task<bool> DeleteTask(int id)
    {
        var item = await _db.Tasks.FindAsync(id);
        if (item is null)
            return false;

        _db.Tasks.Remove(item);
        await _db.SaveChangesAsync();

        // Notifica todos os clientes conectados
        await Clients.All.SendAsync("TaskDeleted", id);
        return true;
    }

    // Labels
    public async Task<IEnumerable<Label>> GetAllLabels()
    {
        return await _db.Labels.AsNoTracking().ToListAsync();
    }

    public async Task<Label?> GetLabelById(int id)
    {
        return await _db.Labels.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Label> CreateLabel(Label input)
    {
        _db.Labels.Add(input);
        await _db.SaveChangesAsync();

        await Clients.All.SendAsync("LabelCreated", input);
        return input;
    }

    public async Task<bool> UpdateLabel(int id, Label input)
    {
        if (id != input.Id)
            return false;

        var exists = await _db.Labels.AnyAsync(x => x.Id == id);
        if (!exists)
            return false;

        _db.Entry(input).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        await Clients.All.SendAsync("LabelUpdated", input);
        return true;
    }

    public async Task<bool> DeleteLabel(int id)
    {
        var item = await _db.Labels.FindAsync(id);
        if (item is null)
            return false;

        _db.Labels.Remove(item);
        await _db.SaveChangesAsync();

        await Clients.All.SendAsync("LabelDeleted", id);
        return true;
    }

    // Statuses
    public async Task<IEnumerable<Status>> GetAllStatuses()
    {
        return await _db.Statuses.AsNoTracking().ToListAsync();
    }

    public async Task<Status?> GetStatusById(int id)
    {
        return await _db.Statuses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Status> CreateStatus(Status input)
    {
        _db.Statuses.Add(input);
        await _db.SaveChangesAsync();

        await Clients.All.SendAsync("StatusCreated", input);
        return input;
    }

    public async Task<bool> UpdateStatus(int id, Status input)
    {
        if (id != input.Id)
            return false;

        var exists = await _db.Statuses.AnyAsync(x => x.Id == id);
        if (!exists)
            return false;

        _db.Entry(input).State = EntityState.Modified;
        await _db.SaveChangesAsync();

        await Clients.All.SendAsync("StatusUpdated", input);
        return true;
    }

    public async Task<bool> DeleteStatus(int id)
    {
        var item = await _db.Statuses.FindAsync(id);
        if (item is null)
            return false;

        _db.Statuses.Remove(item);
        await _db.SaveChangesAsync();

        await Clients.All.SendAsync("StatusDeleted", id);
        return true;
    }
}