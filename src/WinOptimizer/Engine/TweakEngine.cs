using Microsoft.Win32;
using WinOptimizer.Models;

namespace WinOptimizer.Engine;

public class TweakEngine
{
    public event Action<string>? OnLog;
    public event Action<Tweak, TweakStatus>? OnTweakStatusChanged;

    public async Task<TweakStatus> DetectTweakStatusAsync(Tweak tweak)
    {
        try
        {
            return tweak.Type switch
            {
                TweakType.Registry => DetectRegistryTweak(tweak),
                TweakType.Service => DetectServiceTweak(tweak),
                TweakType.ScheduledTask => await DetectScheduledTask(tweak),
                TweakType.PowerShell => await DetectPowerShellTweak(tweak),
                TweakType.AppRemoval => await DetectAppRemoval(tweak),
                _ => TweakStatus.Unknown
            };
        }
        catch (Exception ex)
        {
            Log($"Error detecting {tweak.Name}: {ex.Message}");
            return TweakStatus.Unknown;
        }
    }

    public async Task<bool> ApplyTweakAsync(Tweak tweak)
    {
        try
        {
            Log($"Applying: {tweak.Name}");
            var result = tweak.Type switch
            {
                TweakType.Registry => ApplyRegistryTweak(tweak),
                TweakType.Service => ApplyServiceTweak(tweak),
                TweakType.ScheduledTask => await ApplyScheduledTask(tweak),
                TweakType.PowerShell => await ApplyPowerShellTweak(tweak),
                TweakType.AppRemoval => await ApplyAppRemoval(tweak),
                _ => false
            };

            tweak.Status = result ? TweakStatus.Applied : TweakStatus.Error;
            OnTweakStatusChanged?.Invoke(tweak, tweak.Status);
            Log(result ? $"Applied: {tweak.Name}" : $"Failed: {tweak.Name}");
            return result;
        }
        catch (Exception ex)
        {
            tweak.Status = TweakStatus.Error;
            OnTweakStatusChanged?.Invoke(tweak, tweak.Status);
            Log($"Error applying {tweak.Name}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RevertTweakAsync(Tweak tweak)
    {
        try
        {
            Log($"Reverting: {tweak.Name}");
            var result = tweak.Type switch
            {
                TweakType.Registry => RevertRegistryTweak(tweak),
                TweakType.Service => RevertServiceTweak(tweak),
                TweakType.ScheduledTask => await RevertScheduledTask(tweak),
                TweakType.PowerShell => await RevertPowerShellTweak(tweak),
                TweakType.AppRemoval => false, // Can't reinstall removed apps easily
                _ => false
            };

            tweak.Status = result ? TweakStatus.NotApplied : TweakStatus.Error;
            OnTweakStatusChanged?.Invoke(tweak, tweak.Status);
            Log(result ? $"Reverted: {tweak.Name}" : $"Failed reverting: {tweak.Name}");
            return result;
        }
        catch (Exception ex)
        {
            tweak.Status = TweakStatus.Error;
            OnTweakStatusChanged?.Invoke(tweak, tweak.Status);
            Log($"Error reverting {tweak.Name}: {ex.Message}");
            return false;
        }
    }

    public async Task<int> ApplyAllAsync(IEnumerable<Tweak> tweaks, IProgress<(int current, int total, string name)>? progress = null)
    {
        var list = tweaks.Where(t => t.IsEnabled).ToList();
        int success = 0;
        for (int i = 0; i < list.Count; i++)
        {
            progress?.Report((i + 1, list.Count, list[i].Name));
            if (await ApplyTweakAsync(list[i]))
                success++;
        }
        return success;
    }

    public async Task<int> RevertAllAsync(IEnumerable<Tweak> tweaks, IProgress<(int current, int total, string name)>? progress = null)
    {
        var list = tweaks.Where(t => t.Status == TweakStatus.Applied).ToList();
        int success = 0;
        for (int i = 0; i < list.Count; i++)
        {
            progress?.Report((i + 1, list.Count, list[i].Name));
            if (await RevertTweakAsync(list[i]))
                success++;
        }
        return success;
    }

    private TweakStatus DetectRegistryTweak(Tweak tweak)
    {
        if (string.IsNullOrEmpty(tweak.RegistryPath) || string.IsNullOrEmpty(tweak.RegistryValueName))
            return TweakStatus.Unknown;

        var current = RegistryHelper.GetValue(tweak.RegistryPath, tweak.RegistryValueName);
        if (current == null) return TweakStatus.NotApplied;

        return current.ToString() == tweak.OptimizedValue?.ToString()
            ? TweakStatus.Applied
            : TweakStatus.NotApplied;
    }

    private bool ApplyRegistryTweak(Tweak tweak)
    {
        if (string.IsNullOrEmpty(tweak.RegistryPath) || string.IsNullOrEmpty(tweak.RegistryValueName))
            return false;
        return RegistryHelper.SetValue(tweak.RegistryPath, tweak.RegistryValueName,
            tweak.OptimizedValue!, tweak.ValueKind);
    }

    private bool RevertRegistryTweak(Tweak tweak)
    {
        if (string.IsNullOrEmpty(tweak.RegistryPath) || string.IsNullOrEmpty(tweak.RegistryValueName))
            return false;
        if (tweak.DefaultValue == null)
            return RegistryHelper.DeleteValue(tweak.RegistryPath, tweak.RegistryValueName);
        return RegistryHelper.SetValue(tweak.RegistryPath, tweak.RegistryValueName,
            tweak.DefaultValue, tweak.ValueKind);
    }

    private TweakStatus DetectServiceTweak(Tweak tweak)
    {
        if (string.IsNullOrEmpty(tweak.ServiceName)) return TweakStatus.Unknown;
        if (!ServiceHelper.ServiceExists(tweak.ServiceName)) return TweakStatus.Unknown;
        var current = ServiceHelper.GetStartupType(tweak.ServiceName);
        return current?.ToLowerInvariant() == tweak.OptimizedStartType?.ToLowerInvariant()
            ? TweakStatus.Applied
            : TweakStatus.NotApplied;
    }

    private bool ApplyServiceTweak(Tweak tweak)
    {
        if (string.IsNullOrEmpty(tweak.ServiceName) || string.IsNullOrEmpty(tweak.OptimizedStartType))
            return false;

        if (tweak.OptimizedStartType.Equals("Disabled", StringComparison.OrdinalIgnoreCase))
            ServiceHelper.StopService(tweak.ServiceName, TimeSpan.FromSeconds(10));

        return ServiceHelper.SetStartupType(tweak.ServiceName, tweak.OptimizedStartType);
    }

    private bool RevertServiceTweak(Tweak tweak)
    {
        if (string.IsNullOrEmpty(tweak.ServiceName) || string.IsNullOrEmpty(tweak.DefaultStartType))
            return false;

        var result = ServiceHelper.SetStartupType(tweak.ServiceName, tweak.DefaultStartType);
        if (result && !tweak.DefaultStartType.Equals("Disabled", StringComparison.OrdinalIgnoreCase))
            ServiceHelper.StartService(tweak.ServiceName, TimeSpan.FromSeconds(10));

        return result;
    }

    private async Task<TweakStatus> DetectScheduledTask(Tweak tweak)
    {
        if (string.IsNullOrEmpty(tweak.TaskPath)) return TweakStatus.Unknown;
        var script = $"(Get-ScheduledTask -TaskPath '{tweak.TaskPath.Replace("'", "''")}' -ErrorAction SilentlyContinue).State";
        var (success, output, _) = await PowerShellHelper.ExecuteAsync(script);
        if (!success) return TweakStatus.Unknown;
        var isDisabled = output.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
        return (tweak.TaskShouldBeEnabled == false && isDisabled) ? TweakStatus.Applied : TweakStatus.NotApplied;
    }

    private async Task<bool> ApplyScheduledTask(Tweak tweak)
    {
        if (string.IsNullOrEmpty(tweak.TaskPath)) return false;
        var action = tweak.TaskShouldBeEnabled == false ? "Disable" : "Enable";
        var script = $"{action}-ScheduledTask -TaskPath '{tweak.TaskPath.Replace("'", "''")}' -ErrorAction SilentlyContinue";
        var (success, _, _) = await PowerShellHelper.ExecuteAsync(script);
        return success;
    }

    private async Task<bool> RevertScheduledTask(Tweak tweak)
    {
        if (string.IsNullOrEmpty(tweak.TaskPath)) return false;
        var action = tweak.TaskShouldBeEnabled == false ? "Enable" : "Disable";
        var script = $"{action}-ScheduledTask -TaskPath '{tweak.TaskPath.Replace("'", "''")}' -ErrorAction SilentlyContinue";
        var (success, _, _) = await PowerShellHelper.ExecuteAsync(script);
        return success;
    }

    private async Task<TweakStatus> DetectPowerShellTweak(Tweak tweak)
    {
        if (string.IsNullOrEmpty(tweak.DetectScript)) return TweakStatus.Unknown;
        var (success, output, _) = await PowerShellHelper.ExecuteAsync(tweak.DetectScript);
        if (!success) return TweakStatus.Unknown;
        return output.Trim().Equals("True", StringComparison.OrdinalIgnoreCase)
            ? TweakStatus.Applied
            : TweakStatus.NotApplied;
    }

    private async Task<bool> ApplyPowerShellTweak(Tweak tweak)
    {
        if (string.IsNullOrEmpty(tweak.ApplyScript)) return false;
        var (success, _, _) = await PowerShellHelper.ExecuteAsync(tweak.ApplyScript, 60000);
        return success;
    }

    private async Task<bool> RevertPowerShellTweak(Tweak tweak)
    {
        if (string.IsNullOrEmpty(tweak.RevertScript)) return false;
        var (success, _, _) = await PowerShellHelper.ExecuteAsync(tweak.RevertScript, 60000);
        return success;
    }

    private async Task<TweakStatus> DetectAppRemoval(Tweak tweak)
    {
        if (string.IsNullOrEmpty(tweak.PackageName)) return TweakStatus.Unknown;
        var script = $"if (Get-AppxPackage -Name '{tweak.PackageName}' -ErrorAction SilentlyContinue) {{ 'Installed' }} else {{ 'Removed' }}";
        var (success, output, _) = await PowerShellHelper.ExecuteAsync(script);
        if (!success) return TweakStatus.Unknown;
        return output.Trim() == "Removed" ? TweakStatus.Applied : TweakStatus.NotApplied;
    }

    private async Task<bool> ApplyAppRemoval(Tweak tweak)
    {
        if (string.IsNullOrEmpty(tweak.PackageName)) return false;
        var script = $@"
            Get-AppxPackage -Name '{tweak.PackageName}' -AllUsers | Remove-AppxPackage -AllUsers -ErrorAction SilentlyContinue
            Get-AppxProvisionedPackage -Online | Where-Object {{ $_.PackageName -like '*{tweak.PackageName}*' }} | Remove-AppxProvisionedPackage -Online -ErrorAction SilentlyContinue
        ";
        var (success, _, _) = await PowerShellHelper.ExecuteAsync(script, 60000);
        return success;
    }

    private void Log(string message) => OnLog?.Invoke(message);
}
