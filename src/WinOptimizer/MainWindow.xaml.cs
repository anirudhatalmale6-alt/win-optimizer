using System.Windows;
using System.Windows.Input;
using WinOptimizer.ViewModels;

namespace WinOptimizer;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm;

    public MainWindow()
    {
        InitializeComponent();
        _vm = new MainViewModel();
        DataContext = _vm;
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (!_vm.CheckLicense())
        {
            ActivationOverlay.Visibility = Visibility.Visible;
            return;
        }
        await _vm.InitializeAsync();
    }

    private void CategoryButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement fe && fe.DataContext is CategoryViewModel cat)
        {
            _vm.SelectedCategory = cat;
        }
    }

    private void ActivateButton_Click(object sender, RoutedEventArgs e)
    {
        var key = LicenseKeyBox.Text.Trim();
        if (string.IsNullOrEmpty(key))
        {
            ActivationError.Text = "Please enter a license key.";
            return;
        }

        if (_vm.ActivateLicense(key))
        {
            ActivationOverlay.Visibility = Visibility.Collapsed;
            _ = _vm.InitializeAsync();
        }
        else
        {
            ActivationError.Text = "Invalid license key or hardware mismatch.";
        }
    }

    private void ShowLog_Click(object sender, RoutedEventArgs e)
    {
        LogOverlay.Visibility = Visibility.Visible;
    }

    private void CloseLog_Click(object sender, RoutedEventArgs e)
    {
        LogOverlay.Visibility = Visibility.Collapsed;
    }

    private void BtnMinimize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void BtnMaximize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        if (e.GetPosition(this).Y <= 40)
            DragMove();
    }
}
