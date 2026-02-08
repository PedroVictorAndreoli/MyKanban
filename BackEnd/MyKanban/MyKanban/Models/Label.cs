namespace MyKanban.Models;

public class Label
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ColorBg { get; set; } = string.Empty;
    public string ColorText { get; set; } = string.Empty;

    public ICollection<KanbanTask> Tasks { get; set; } = new List<KanbanTask>();
}
