namespace MyKanban.Models;

public class Status
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public ICollection<KanbanTask> Tasks { get; set; } = new List<KanbanTask>();
}
