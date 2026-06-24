namespace WinOptimizer.Models;

public enum TweakType
{
    Registry,
    Service,
    ScheduledTask,
    PowerShell,
    AppRemoval
}

public enum TweakRisk
{
    Low,
    Medium,
    High
}

public enum TweakStatus
{
    Unknown,
    Applied,
    NotApplied,
    PartiallyApplied,
    Error
}

public class Tweak
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public TweakType Type { get; set; }
    public TweakRisk Risk { get; set; }
    public TweakStatus Status { get; set; } = TweakStatus.Unknown;
    public bool IsEnabled { get; set; }
    public bool SupportsWindows10 { get; set; } = true;
    public bool SupportsWindows11 { get; set; } = true;

    // Registry tweaks
    public string? RegistryPath { get; set; }
    public string? RegistryValueName { get; set; }
    public object? OptimizedValue { get; set; }
    public object? DefaultValue { get; set; }
    public Microsoft.Win32.RegistryValueKind ValueKind { get; set; } = Microsoft.Win32.RegistryValueKind.DWord;

    // Service tweaks
    public string? ServiceName { get; set; }
    public string? OptimizedStartType { get; set; }
    public string? DefaultStartType { get; set; }

    // Scheduled task tweaks
    public string? TaskPath { get; set; }
    public bool? TaskShouldBeEnabled { get; set; }

    // PowerShell tweaks
    public string? ApplyScript { get; set; }
    public string? RevertScript { get; set; }
    public string? DetectScript { get; set; }

    // App removal tweaks
    public string? PackageName { get; set; }
}
