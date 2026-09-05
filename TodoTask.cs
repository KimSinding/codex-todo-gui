namespace TodoList;

public sealed class TodoTask : ObservableObject
{
    private string _text = string.Empty;
    private bool _isDone;
    private string _priority = "Medium";

    public int Id { get; set; }
    public string Text { get => _text; set => SetProperty(ref _text, value); }
    public bool IsDone { get => _isDone; set => SetProperty(ref _isDone, value); }
    public string Priority { get => _priority; set => SetProperty(ref _priority, value); }
}
