namespace MyKanban.Models;

public class KanbanTask
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public int LabelId { get; set; }
    public Label? Label { get; set; }

    public int StatusId { get; set; }
    public Status? Status { get; set; }
}
