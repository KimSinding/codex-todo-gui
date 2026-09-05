namespace TodoList;

public sealed class TodoTask
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsDone { get; set; }
}
