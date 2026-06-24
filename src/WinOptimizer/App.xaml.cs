using System.Windows;
using WinOptimizer.Engine;

namespace WinOptimizer;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        if (!SystemInfo.IsAdmin)
        {
            MessageBox.Show(
                "WinOptimizer requires administrator privileges to apply system tweaks.\n\nPlease right-click and select 'Run as administrator'.",
                "Administrator Required",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);
            Shutdown(1);
            return;
        }

        if (!SystemInfo.IsWindows10 && !SystemInfo.IsWindows11)
        {
            MessageBox.Show(
                "WinOptimizer is designed for Windows 10 and Windows 11 only.",
                "Unsupported OS",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            Shutdown(1);
        }
    }
}
