using System.Diagnostics;

namespace WinOptimizer.Engine;

public static class PowerShellHelper
{
    public static async Task<(bool Success, string Output, string Error)> ExecuteAsync(string script, int timeoutMs = 30000)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = $"-NoProfile -NonInteractive -ExecutionPolicy Bypass -Command \"{EscapeScript(script)}\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = psi };
        process.Start();

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();

        var completed = await Task.Run(() => process.WaitForExit(timeoutMs));
        if (!completed)
        {
            try { process.Kill(true); } catch { }
            return (false, string.Empty, "Process timed out");
        }

        var output = await outputTask;
        var error = await errorTask;

        return (process.ExitCode == 0, output.Trim(), error.Trim());
    }

    public static (bool Success, string Output, string Error) Execute(string script, int timeoutMs = 30000)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "powershell.exe",
            Arguments = $"-NoProfile -NonInteractive -ExecutionPolicy Bypass -Command \"{EscapeScript(script)}\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        using var process = new Process { StartInfo = psi };
        process.Start();

        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();

        process.WaitForExit(timeoutMs);
        return (process.ExitCode == 0, output.Trim(), error.Trim());
    }

    private static string EscapeScript(string script)
    {
        return script.Replace("\"", "\\\"");
    }
}
