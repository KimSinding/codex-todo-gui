using System.Windows;

namespace TodoList;

public partial class EditTaskWindow : Window
{
    public EditTaskWindow()
    {
        InitializeComponent();
        DataContextChanged += (_, _) => { if (DataContext is EditTaskViewModel viewModel) viewModel.CloseRequested += Close; };
    }
}
