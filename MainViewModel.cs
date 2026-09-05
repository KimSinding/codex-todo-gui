using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace TodoList;

public sealed class MainViewModel : ObservableObject
{
    private string _newTaskText = string.Empty;
    private string _newTaskPriority = "Medium";
    private TodoTask? _selectedTask;

    public MainViewModel()
    {
        Tasks = new ObservableCollection<TodoTask>(SortTasks(TaskStorage.Load()));
        foreach (var task in Tasks) WatchTask(task);
        AddTaskCommand = new RelayCommand(AddTask, () => !string.IsNullOrWhiteSpace(NewTaskText));
        MarkDoneCommand = new RelayCommand(MarkSelectedTaskDone, () => SelectedTask is { IsDone: false });
        EditTaskCommand = new RelayCommand(EditSelectedTask, () => SelectedTask is not null);
    }

    public ObservableCollection<TodoTask> Tasks { get; }
    public IReadOnlyList<string> Priorities { get; } = ["Low", "Medium", "High"];
    public RelayCommand AddTaskCommand { get; }
    public RelayCommand MarkDoneCommand { get; }
    public RelayCommand EditTaskCommand { get; }
    public event Action<TodoTask>? EditRequested;
    public string NewTaskText { get => _newTaskText; set { if (SetProperty(ref _newTaskText, value)) AddTaskCommand.RaiseCanExecuteChanged(); } }
    public string NewTaskPriority { get => _newTaskPriority; set => SetProperty(ref _newTaskPriority, value); }
    public TodoTask? SelectedTask { get => _selectedTask; set { if (SetProperty(ref _selectedTask, value)) { MarkDoneCommand.RaiseCanExecuteChanged(); EditTaskCommand.RaiseCanExecuteChanged(); } } }

    private void AddTask()
    {
        var task = new TodoTask { Id = Tasks.Count == 0 ? 1 : Tasks.Max(item => item.Id) + 1, Text = NewTaskText.Trim(), Priority = NewTaskPriority };
        WatchTask(task);
        Tasks.Add(task);
        SortTaskList();
        NewTaskText = string.Empty;
        SaveTasks();
    }

    private void MarkSelectedTaskDone() { if (SelectedTask is not null) SelectedTask.IsDone = true; }
    private void EditSelectedTask() { if (SelectedTask is not null) EditRequested?.Invoke(SelectedTask); }
    private void WatchTask(TodoTask task) => task.PropertyChanged += Task_PropertyChanged;
    private void Task_PropertyChanged(object? sender, PropertyChangedEventArgs e) { if (e.PropertyName is nameof(TodoTask.Text) or nameof(TodoTask.IsDone) or nameof(TodoTask.Priority)) { SaveTasks(); MarkDoneCommand.RaiseCanExecuteChanged(); } }
    private void SaveTasks() => TaskStorage.Save(Tasks);
    private void SortTaskList() { var sorted = SortTasks(Tasks).ToList(); Tasks.Clear(); foreach (var task in sorted) Tasks.Add(task); }
    private static IEnumerable<TodoTask> SortTasks(IEnumerable<TodoTask> tasks) => tasks.OrderBy(task => task.Priority switch { "High" => 0, "Medium" => 1, "Low" => 2, _ => 1 }).ThenBy(task => task.Id);
}
