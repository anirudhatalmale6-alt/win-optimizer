using Microsoft.Win32;
using WinOptimizer.Models;

namespace WinOptimizer.Tweaks;

public static class PerformanceTweaks
{
    public static List<Tweak> GetTweaks()
    {
        return new List<Tweak>
        {
            // ── Memory Management ────────────────────────────────────────

            new Tweak
            {
                Id = "perf-disable-paging-executive",
                Name = "Disable Paging Executive",
                Description = "Keeps the kernel and drivers in physical memory instead of paging them to disk, improving responsiveness on systems with sufficient RAM.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Medium,
                RegistryPath = @"HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                RegistryValueName = "DisablePagingExecutive",
                OptimizedValue = 1,
                DefaultValue = 0,
                ValueKind = RegistryValueKind.DWord
            },

            new Tweak
            {
                Id = "perf-large-system-cache",
                Name = "Large System Cache",
                Description = "Allocates more memory to the file system cache, improving disk I/O performance for file-heavy workloads.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Medium,
                RegistryPath = @"HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                RegistryValueName = "LargeSystemCache",
                OptimizedValue = 1,
                DefaultValue = 0,
                ValueKind = RegistryValueKind.DWord
            },

            new Tweak
            {
                Id = "perf-clear-pagefile-shutdown",
                Name = "Clear Page File at Shutdown",
                Description = "Zeroes out the page file on shutdown, preventing sensitive data from persisting on disk. Slightly increases shutdown time.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Medium,
                RegistryPath = @"HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management",
                RegistryValueName = "ClearPageFileAtShutdown",
                OptimizedValue = 1,
                DefaultValue = 0,
                ValueKind = RegistryValueKind.DWord
            },

            new Tweak
            {
                Id = "perf-disable-prefetcher",
                Name = "Disable Prefetcher",
                Description = "Disables the Windows Prefetcher which preloads application data. On SSD systems, the Prefetcher provides negligible benefit and wastes I/O.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Medium,
                RegistryPath = @"HKLM\SYSTEM\CurrentControlSet\Control\Session Manager\Memory Management\PrefetchParameters",
                RegistryValueName = "EnablePrefetcher",
                OptimizedValue = 0,
                DefaultValue = 3,
                ValueKind = RegistryValueKind.DWord
            },

            // ── Services ─────────────────────────────────────────────────

            new Tweak
            {
                Id = "perf-disable-sysmain",
                Name = "Disable SysMain (Superfetch)",
                Description = "Disables the SysMain service (formerly Superfetch) which preloads frequently used applications into memory. Reduces disk thrashing on SSDs.",
                Category = "Performance",
                Type = TweakType.Service,
                Risk = TweakRisk.Low,
                ServiceName = "SysMain",
                OptimizedStartType = "Disabled",
                DefaultStartType = "Automatic"
            },

            // ── File System ──────────────────────────────────────────────

            new Tweak
            {
                Id = "perf-ntfs-disable-8dot3",
                Name = "NTFS Disable 8.3 Name Creation",
                Description = "Disables automatic generation of legacy 8.3 short file names on NTFS volumes, reducing overhead on file creation operations.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SYSTEM\CurrentControlSet\Control\FileSystem",
                RegistryValueName = "NtfsDisable8dot3NameCreation",
                OptimizedValue = 1,
                DefaultValue = 0,
                ValueKind = RegistryValueKind.DWord
            },

            new Tweak
            {
                Id = "perf-ntfs-disable-last-access",
                Name = "NTFS Disable Last Access Update",
                Description = "Disables updating the last-access timestamp on files and directories, reducing disk writes on every file read operation.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SYSTEM\CurrentControlSet\Control\FileSystem",
                RegistryValueName = "NtfsDisableLastAccessUpdate",
                OptimizedValue = unchecked((int)0x80000001),
                DefaultValue = unchecked((int)0x80000000),
                ValueKind = RegistryValueKind.DWord
            },

            // ── Multimedia / MMCSS ───────────────────────────────────────

            new Tweak
            {
                Id = "perf-mmcss-system-responsiveness",
                Name = "MMCSS System Responsiveness",
                Description = "Reduces the percentage of CPU resources reserved for background tasks from 20% to 10%, giving foreground applications more processing power.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                RegistryValueName = "SystemResponsiveness",
                OptimizedValue = 10,
                DefaultValue = 20,
                ValueKind = RegistryValueKind.DWord
            },

            new Tweak
            {
                Id = "perf-mmcss-network-throttling",
                Name = "Disable Network Throttling",
                Description = "Removes the network throttling applied during multimedia playback, allowing full network throughput at all times.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                RegistryValueName = "NetworkThrottlingIndex",
                OptimizedValue = unchecked((int)0xFFFFFFFF),
                DefaultValue = 10,
                ValueKind = RegistryValueKind.DWord
            },

            new Tweak
            {
                Id = "perf-mmcss-gpu-priority",
                Name = "MMCSS GPU Priority for Games",
                Description = "Sets the GPU scheduling priority for game processes to maximum (8), ensuring games receive preferential GPU time.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                RegistryValueName = "GPU Priority",
                OptimizedValue = 8,
                DefaultValue = 8,
                ValueKind = RegistryValueKind.DWord
            },

            new Tweak
            {
                Id = "perf-mmcss-games-priority",
                Name = "MMCSS Games Thread Priority",
                Description = "Raises the thread priority for game processes from 2 (Normal) to 6 (High), improving game responsiveness.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                RegistryValueName = "Priority",
                OptimizedValue = 6,
                DefaultValue = 2,
                ValueKind = RegistryValueKind.DWord
            },

            new Tweak
            {
                Id = "perf-mmcss-games-scheduling",
                Name = "MMCSS Games Scheduling Category",
                Description = "Sets the MMCSS scheduling category for games to High, granting elevated CPU scheduling priority to game threads.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                RegistryValueName = "Scheduling Category",
                OptimizedValue = "High",
                DefaultValue = "Medium",
                ValueKind = RegistryValueKind.String
            },

            new Tweak
            {
                Id = "perf-mmcss-games-sfio",
                Name = "MMCSS Games SFIO Priority",
                Description = "Sets the Scheduled File I/O priority for game processes to High, improving disk access speed for games.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                RegistryValueName = "SFIO Priority",
                OptimizedValue = "High",
                DefaultValue = "Normal",
                ValueKind = RegistryValueKind.String
            },

            // ── Priority & Scheduling ────────────────────────────────────

            new Tweak
            {
                Id = "perf-win32-priority-separation",
                Name = "Optimize Process Priority Separation",
                Description = "Sets foreground process priority boost to short, variable, with high foreground bias (38 = 0x26), making interactive applications more responsive.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Medium,
                RegistryPath = @"HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl",
                RegistryValueName = "Win32PrioritySeparation",
                OptimizedValue = 38,
                DefaultValue = 2,
                ValueKind = RegistryValueKind.DWord
            },

            // ── UI Responsiveness ────────────────────────────────────────

            new Tweak
            {
                Id = "perf-menu-show-delay",
                Name = "Reduce Menu Show Delay",
                Description = "Eliminates the delay before menus appear, making the UI feel instantly responsive.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKCU\Control Panel\Desktop",
                RegistryValueName = "MenuShowDelay",
                OptimizedValue = "0",
                DefaultValue = "400",
                ValueKind = RegistryValueKind.String
            },

            new Tweak
            {
                Id = "perf-wait-to-kill-service-timeout",
                Name = "Reduce Service Shutdown Timeout",
                Description = "Reduces the time Windows waits for services to stop during shutdown from 5 seconds to 2 seconds, speeding up the shutdown process.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Medium,
                RegistryPath = @"HKLM\SYSTEM\CurrentControlSet\Control",
                RegistryValueName = "WaitToKillServiceTimeout",
                OptimizedValue = "2000",
                DefaultValue = "5000",
                ValueKind = RegistryValueKind.String
            },

            new Tweak
            {
                Id = "perf-hung-app-timeout",
                Name = "Reduce Hung App Timeout",
                Description = "Reduces the time before Windows considers an application as not responding from 5 seconds to 1 second.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKCU\Control Panel\Desktop",
                RegistryValueName = "HungAppTimeout",
                OptimizedValue = "1000",
                DefaultValue = "5000",
                ValueKind = RegistryValueKind.String
            },

            new Tweak
            {
                Id = "perf-auto-end-tasks",
                Name = "Auto End Unresponsive Tasks",
                Description = "Automatically closes unresponsive applications at shutdown instead of prompting the user, speeding up the shutdown process.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKCU\Control Panel\Desktop",
                RegistryValueName = "AutoEndTasks",
                OptimizedValue = "1",
                DefaultValue = "0",
                ValueKind = RegistryValueKind.String
            },

            new Tweak
            {
                Id = "perf-wait-to-kill-app-timeout",
                Name = "Reduce App Kill Timeout",
                Description = "Reduces the time Windows waits before terminating an application during shutdown from 20 seconds to 2 seconds.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKCU\Control Panel\Desktop",
                RegistryValueName = "WaitToKillAppTimeout",
                OptimizedValue = "2000",
                DefaultValue = "20000",
                ValueKind = RegistryValueKind.String
            },

            // ── Power Management ─────────────────────────────────────────

            new Tweak
            {
                Id = "perf-disable-hibernation",
                Name = "Disable Hibernation",
                Description = "Disables hibernation and removes the hiberfil.sys file, freeing disk space equal to your RAM size. Recommended for desktops and SSD-equipped laptops.",
                Category = "Performance",
                Type = TweakType.PowerShell,
                Risk = TweakRisk.Low,
                ApplyScript = "powercfg /h off",
                RevertScript = "powercfg /h on",
                DetectScript = "if ((powercfg /a) -match 'Hibernation') { 'False' } else { 'True' }"
            },

            new Tweak
            {
                Id = "perf-ultimate-performance-plan",
                Name = "Enable Ultimate Performance Power Plan",
                Description = "Activates the hidden Ultimate Performance power plan which eliminates micro-latencies from power management, ideal for high-performance workstations.",
                Category = "Performance",
                Type = TweakType.PowerShell,
                Risk = TweakRisk.Low,
                ApplyScript = "powercfg /duplicatescheme e9a42b02-d5df-448d-aa00-03f14749eb61",
                RevertScript = "powercfg /restoredefaultschemes",
                DetectScript = @"if ((powercfg /list) -match 'Ultimate Performance') { 'True' } else { 'False' }"
            },

            new Tweak
            {
                Id = "perf-disable-core-parking",
                Name = "Disable CPU Core Parking",
                Description = "Prevents Windows from parking CPU cores to save power, ensuring all cores remain active and available for immediate use.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.Medium,
                RegistryPath = @"HKLM\SYSTEM\CurrentControlSet\Control\Power\PowerSettings\54533251-82be-4824-96c1-47b60b740d00\0cc5b647-c1df-4637-891a-dec35c318583",
                RegistryValueName = "ValueMax",
                OptimizedValue = 0,
                DefaultValue = 100,
                ValueKind = RegistryValueKind.DWord
            },

            // ── Service Tweaks ───────────────────────────────────────────

            new Tweak
            {
                Id = "perf-disable-windows-search",
                Name = "Disable Windows Search Indexer",
                Description = "Disables the Windows Search indexing service which constantly scans files in the background. Eliminates disk and CPU overhead from indexing.",
                Category = "Performance",
                Type = TweakType.Service,
                Risk = TweakRisk.Low,
                ServiceName = "WSearch",
                OptimizedStartType = "Disabled",
                DefaultStartType = "Automatic"
            },

            new Tweak
            {
                Id = "perf-disable-pcasvc",
                Name = "Disable Program Compatibility Assistant",
                Description = "Disables the Program Compatibility Assistant service which monitors programs for compatibility issues. Reduces background CPU usage.",
                Category = "Performance",
                Type = TweakType.Service,
                Risk = TweakRisk.Low,
                ServiceName = "PcaSvc",
                OptimizedStartType = "Disabled",
                DefaultStartType = "Automatic"
            },

            // ── IRQ ──────────────────────────────────────────────────────

            new Tweak
            {
                Id = "perf-irq8-priority-boost",
                Name = "IRQ8 Priority Boost",
                Description = "Increases the priority of the real-time clock interrupt (IRQ8), improving system timer precision for latency-sensitive workloads.",
                Category = "Performance",
                Type = TweakType.Registry,
                Risk = TweakRisk.High,
                RegistryPath = @"HKLM\SYSTEM\CurrentControlSet\Control\PriorityControl",
                RegistryValueName = "IRQ8Priority",
                OptimizedValue = 1,
                DefaultValue = 0,
                ValueKind = RegistryValueKind.DWord
            }
        };
    }
}
