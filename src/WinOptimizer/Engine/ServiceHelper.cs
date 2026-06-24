using System.ServiceProcess;
using Microsoft.Win32;

namespace WinOptimizer.Engine;

public static class ServiceHelper
{
    public static ServiceControllerStatus? GetServiceStatus(string serviceName)
    {
        try
        {
            using var sc = new ServiceController(serviceName);
            return sc.Status;
        }
        catch
        {
            return null;
        }
    }

    public static string? GetStartupType(string serviceName)
    {
        try
        {
            var path = $@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\{serviceName}";
            var val = RegistryHelper.GetValue(path, "Start");
            return val switch
            {
                0 => "Boot",
                1 => "System",
                2 => "Automatic",
                3 => "Manual",
                4 => "Disabled",
                _ => null
            };
        }
        catch
        {
            return null;
        }
    }

    public static bool SetStartupType(string serviceName, string startupType)
    {
        int value = startupType.ToLowerInvariant() switch
        {
            "automatic" => 2,
            "manual" => 3,
            "disabled" => 4,
            _ => -1
        };

        if (value < 0) return false;

        var path = $@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\{serviceName}";
        return RegistryHelper.SetValue(path, "Start", value, RegistryValueKind.DWord);
    }

    public static bool StopService(string serviceName, TimeSpan timeout)
    {
        try
        {
            using var sc = new ServiceController(serviceName);
            if (sc.Status == ServiceControllerStatus.Running ||
                sc.Status == ServiceControllerStatus.StartPending)
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool StartService(string serviceName, TimeSpan timeout)
    {
        try
        {
            using var sc = new ServiceController(serviceName);
            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool ServiceExists(string serviceName)
    {
        try
        {
            using var sc = new ServiceController(serviceName);
            _ = sc.Status;
            return true;
        }
        catch
        {
            return false;
        }
    }
}
