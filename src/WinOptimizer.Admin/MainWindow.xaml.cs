using System.Windows;
using System.Windows.Input;
using WinOptimizer.Admin.ViewModels;
using WinOptimizer.Core.Licensing;

namespace WinOptimizer.Admin;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new AdminViewModel();

        try
        {
            LocalHwidText.Text = HwidHelper.GetHardwareId();
        }
        catch
        {
            LocalHwidText.Text = "Could not detect";
        }
    }

    private void CopyLocalHwid_Click(object sender, MouseButtonEventArgs e)
    {
        try
        {
            Clipboard.SetText(LocalHwidText.Text);
            MessageBox.Show("Hardware ID copied to clipboard!", "Copied", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch { }
    }
}
