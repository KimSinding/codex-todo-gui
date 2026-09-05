using System.Windows;

namespace TodoList;

public partial class MainWindow : Window
{
    private readonly MainViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new MainViewModel();
        _viewModel.EditRequested += OpenEditDialog;
        DataContext = _viewModel;
    }

    private void OpenEditDialog(TodoTask task)
    {
        var dialog = new EditTaskWindow
        {
            Owner = this,
            DataContext = new EditTaskViewModel(task)
        };

        dialog.ShowDialog();
    }
}
