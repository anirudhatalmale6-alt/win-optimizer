using System.Windows;
using System.Windows.Threading;

namespace WinOptimizer;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        DispatcherUnhandledException += OnUnhandledException;
        AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
    }

    private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show(
            $"An error occurred:\n\n{e.Exception.Message}\n\n{e.Exception.StackTrace}",
            "WinOptimizer Error",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
        e.Handled = true;
    }

    private void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
        {
            MessageBox.Show(
                $"A fatal error occurred:\n\n{ex.Message}\n\n{ex.StackTrace}",
                "WinOptimizer Fatal Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}
