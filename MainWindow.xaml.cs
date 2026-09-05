using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TodoList;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<TodoTask> _tasks;

    public MainWindow()
    {
        InitializeComponent();
        _tasks = new ObservableCollection<TodoTask>(TaskStorage.Load());
        TasksListBox.ItemsSource = _tasks;
    }

    private void AddButton_Click(object sender, RoutedEventArgs e) => AddTask();

    private void NewTaskTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            AddTask();
        }
    }

    private void AddTask()
    {
        var text = NewTaskTextBox.Text.Trim();
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        var nextId = _tasks.Count == 0 ? 1 : _tasks.Max(task => task.Id) + 1;
        _tasks.Add(new TodoTask { Id = nextId, Text = text, IsDone = false });
        NewTaskTextBox.Clear();
        TaskStorage.Save(_tasks);
    }

    private void DoneButton_Click(object sender, RoutedEventArgs e)
    {
        if (TasksListBox.SelectedItem is TodoTask task && !task.IsDone)
        {
            task.IsDone = true;
            TasksListBox.Items.Refresh();
            TaskStorage.Save(_tasks);
        }
    }

    private void TaskCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        if (IsLoaded)
        {
            TaskStorage.Save(_tasks);
        }
    }
}
