using System.Windows;
using WinOptimizer.Admin.ViewModels;

namespace WinOptimizer.Admin;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new AdminViewModel();
    }
}
