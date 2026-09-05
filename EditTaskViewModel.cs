using System;

namespace TodoList;

public sealed class EditTaskViewModel : ObservableObject
{
    private readonly TodoTask _task;
    private string _text;
    public EditTaskViewModel(TodoTask task) { _task = task; _text = task.Text; SaveCommand = new RelayCommand(Save, () => !string.IsNullOrWhiteSpace(Text)); }
    public event Action? CloseRequested;
    public RelayCommand SaveCommand { get; }
    public string Text { get => _text; set { if (SetProperty(ref _text, value)) SaveCommand.RaiseCanExecuteChanged(); } }
    private void Save() { _task.Text = Text.Trim(); CloseRequested?.Invoke(); }
}
