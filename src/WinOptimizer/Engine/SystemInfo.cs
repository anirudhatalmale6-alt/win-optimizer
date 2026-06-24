using Microsoft.Win32;

namespace WinOptimizer.Engine;

public static class SystemInfo
{
    public static bool IsWindows10 => Environment.OSVersion.Version.Major == 10 &&
                                       Environment.OSVersion.Version.Build < 22000;

    public static bool IsWindows11 => Environment.OSVersion.Version.Major == 10 &&
                                       Environment.OSVersion.Version.Build >= 22000;

    public static string WindowsVersion
    {
        get
        {
            if (IsWindows11) return "Windows 11";
            if (IsWindows10) return "Windows 10";
            return $"Windows {Environment.OSVersion.Version}";
        }
    }

    public static int BuildNumber => Environment.OSVersion.Version.Build;

    public static string Edition
    {
        get
        {
            try
            {
                return Registry.GetValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                    "EditionID", "Unknown")?.ToString() ?? "Unknown";
            }
            catch { return "Unknown"; }
        }
    }

    public static string DisplayVersion
    {
        get
        {
            try
            {
                return Registry.GetValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion",
                    "DisplayVersion", "Unknown")?.ToString() ?? "Unknown";
            }
            catch { return "Unknown"; }
        }
    }

    public static bool IsAdmin
    {
        get
        {
            using var identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            var principal = new System.Security.Principal.WindowsPrincipal(identity);
            return principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
        }
    }

    public static long TotalRamMb
    {
        get
        {
            try
            {
                var val = Registry.GetValue(
                    @"HKEY_LOCAL_MACHINE\HARDWARE\RESOURCEMAP\System Resources\Physical Memory",
                    ".Translated", null);
                if (val != null) return 0;
                var result = PowerShellHelper.Execute(
                    "(Get-CimInstance Win32_PhysicalMemory | Measure-Object Capacity -Sum).Sum / 1MB");
                if (result.Success && long.TryParse(result.Output.Trim(), out var mb))
                    return mb;
            }
            catch { }
            return 0;
        }
    }

    public static string ProcessorName
    {
        get
        {
            try
            {
                return Registry.GetValue(
                    @"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\CentralProcessor\0",
                    "ProcessorNameString", "Unknown")?.ToString()?.Trim() ?? "Unknown";
            }
            catch { return "Unknown"; }
        }
    }
}
