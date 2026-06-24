using Microsoft.Win32;
using WinOptimizer.Models;

namespace WinOptimizer.Tweaks;

public static class GamingTweaks
{
    public static List<Tweak> GetTweaks()
    {
        return new List<Tweak>
        {
            // 1. GPU Hardware Scheduling
            new Tweak
            {
                Id = "gaming_gpu_hw_scheduling",
                Name = "GPU Hardware Scheduling",
                Description = "Enables hardware-accelerated GPU scheduling, allowing the GPU to manage its own video memory directly. Can reduce latency and improve frame consistency in supported games. Requires a compatible GPU (NVIDIA 10-series+ / AMD 5000-series+) and Windows 10 2004+.",
                Category = "Gaming",
                Type = TweakType.Registry,
                Risk = TweakRisk.Medium,
                RegistryPath = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers",
                RegistryValueName = "HwSchMode",
                OptimizedValue = 2,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 2. Disable Fullscreen Optimizations
            new Tweak
            {
                Id = "gaming_disable_fse_optimizations",
                Name = "Disable Fullscreen Optimizations",
                Description = "Disables Windows fullscreen optimizations (FSE) which force games into a borderless windowed mode. Disabling this can reduce input lag and improve frame pacing for games that benefit from exclusive fullscreen.",
                Category = "Gaming",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKEY_CURRENT_USER\System\GameConfigStore",
                RegistryValueName = "GameDVR_FSEBehaviorMode",
                OptimizedValue = 2,
                DefaultValue = 0,
                ValueKind = RegistryValueKind.DWord
            },

            // 3. Honor User Fullscreen Exclusive Behavior
            new Tweak
            {
                Id = "gaming_honor_user_fse",
                Name = "Honor User FSE Behavior",
                Description = "Forces Windows to respect per-application fullscreen exclusive settings configured by the user, preventing the OS from overriding them with its own optimizations.",
                Category = "Gaming",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKEY_CURRENT_USER\System\GameConfigStore",
                RegistryValueName = "GameDVR_HonorUserFSEBehaviorMode",
                OptimizedValue = 1,
                DefaultValue = 0,
                ValueKind = RegistryValueKind.DWord
            },

            // 4. Disable Game DVR
            new Tweak
            {
                Id = "gaming_disable_game_dvr",
                Name = "Disable Game DVR",
                Description = "Disables the Windows Game DVR background recording feature. Game DVR continuously records gameplay in the background, consuming GPU and CPU resources even when not actively capturing clips.",
                Category = "Gaming",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKEY_CURRENT_USER\System\GameConfigStore",
                RegistryValueName = "GameDVR_Enabled",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 5. Disable Game Bar Capture
            new Tweak
            {
                Id = "gaming_disable_game_bar_capture",
                Name = "Disable Game Bar Capture",
                Description = "Disables the Xbox Game Bar's background app capture service. This prevents the overlay from hooking into games and consuming resources for screenshot and recording functionality.",
                Category = "Gaming",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR",
                RegistryValueName = "AppCaptureEnabled",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 6. Disable Game Bar Tips
            new Tweak
            {
                Id = "gaming_disable_game_bar_tips",
                Name = "Disable Game Bar Tips",
                Description = "Disables the Game Bar startup panel and tips overlay that appears when launching games. Removes the 'Press Win+G to open Game Bar' notification and related popups.",
                Category = "Gaming",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\GameBar",
                RegistryValueName = "ShowStartupPanel",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 7. Enable Game Mode
            new Tweak
            {
                Id = "gaming_enable_game_mode",
                Name = "Enable Game Mode",
                Description = "Enables Windows Game Mode which prioritizes system resources for the active game. Prevents Windows Update from performing driver installations and suppresses restart notifications during gameplay.",
                Category = "Gaming",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\GameBar",
                RegistryValueName = "AllowAutoGameMode",
                OptimizedValue = 1,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 8. Mouse Data Queue Size
            new Tweak
            {
                Id = "gaming_mouse_data_queue",
                Name = "Mouse Data Queue Size",
                Description = "Reduces the mouse input data queue size from 100 to 20 entries. A smaller queue means mouse movements are processed more immediately by the system, reducing perceived input lag in fast-paced games.",
                Category = "Gaming",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\mouclass\Parameters",
                RegistryValueName = "MouseDataQueueSize",
                OptimizedValue = 20,
                DefaultValue = 100,
                ValueKind = RegistryValueKind.DWord
            },

            // 9. Keyboard Data Queue Size
            new Tweak
            {
                Id = "gaming_keyboard_data_queue",
                Name = "Keyboard Data Queue Size",
                Description = "Reduces the keyboard input data queue size from 100 to 20 entries. A smaller queue reduces the buffer between keypress and system response, improving input responsiveness for competitive gaming.",
                Category = "Gaming",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\kbdclass\Parameters",
                RegistryValueName = "KeyboardDataQueueSize",
                OptimizedValue = 20,
                DefaultValue = 100,
                ValueKind = RegistryValueKind.DWord
            },

            // 10. Disable Mouse Acceleration (PowerShell - sets 3 registry values)
            new Tweak
            {
                Id = "gaming_disable_mouse_acceleration",
                Name = "Disable Mouse Acceleration",
                Description = "Disables Windows enhanced pointer precision (mouse acceleration) by setting MouseSpeed to 0 and both acceleration thresholds to 0. Provides a 1:1 linear mouse response critical for consistent aiming in FPS games.",
                Category = "Gaming",
                Type = TweakType.PowerShell,
                Risk = TweakRisk.Low,
                ApplyScript = @"Set-ItemProperty -Path 'HKCU:\Control Panel\Mouse' -Name 'MouseSpeed' -Value '0'
Set-ItemProperty -Path 'HKCU:\Control Panel\Mouse' -Name 'MouseThreshold1' -Value '0'
Set-ItemProperty -Path 'HKCU:\Control Panel\Mouse' -Name 'MouseThreshold2' -Value '0'",
                RevertScript = @"Set-ItemProperty -Path 'HKCU:\Control Panel\Mouse' -Name 'MouseSpeed' -Value '1'
Set-ItemProperty -Path 'HKCU:\Control Panel\Mouse' -Name 'MouseThreshold1' -Value '6'
Set-ItemProperty -Path 'HKCU:\Control Panel\Mouse' -Name 'MouseThreshold2' -Value '10'",
                DetectScript = @"$speed = (Get-ItemProperty -Path 'HKCU:\Control Panel\Mouse' -Name 'MouseSpeed' -ErrorAction SilentlyContinue).MouseSpeed
$t1 = (Get-ItemProperty -Path 'HKCU:\Control Panel\Mouse' -Name 'MouseThreshold1' -ErrorAction SilentlyContinue).MouseThreshold1
$t2 = (Get-ItemProperty -Path 'HKCU:\Control Panel\Mouse' -Name 'MouseThreshold2' -ErrorAction SilentlyContinue).MouseThreshold2
if ($speed -eq '0' -and $t1 -eq '0' -and $t2 -eq '0') { 'Applied' } else { 'NotApplied' }"
            },

            // 11. Disable Xbox Auth Manager Service
            new Tweak
            {
                Id = "gaming_disable_xbl_auth",
                Name = "Disable Xbox Live Auth Manager",
                Description = "Disables the Xbox Live Authentication Manager service. This service handles Xbox Live sign-in and authentication. Disabling it frees resources but prevents Xbox Live features and achievements from functioning.",
                Category = "Gaming",
                Type = TweakType.Service,
                Risk = TweakRisk.Low,
                ServiceName = "XblAuthManager",
                OptimizedStartType = "Disabled",
                DefaultStartType = "Manual"
            },

            // 12. Disable Xbox Game Save Service
            new Tweak
            {
                Id = "gaming_disable_xbl_gamesave",
                Name = "Disable Xbox Game Save",
                Description = "Disables the Xbox Game Monitoring and cloud save synchronization service. Prevents background cloud save syncing for Xbox-enabled titles, reducing network and disk I/O during gameplay.",
                Category = "Gaming",
                Type = TweakType.Service,
                Risk = TweakRisk.Low,
                ServiceName = "XblGameSave",
                OptimizedStartType = "Disabled",
                DefaultStartType = "Manual"
            },

            // 13. Disable Xbox Networking Service
            new Tweak
            {
                Id = "gaming_disable_xbox_networking",
                Name = "Disable Xbox Networking Service",
                Description = "Disables the Xbox Networking API service responsible for Xbox Live multiplayer networking and NAT traversal. Frees system resources but disables Xbox Live multiplayer connectivity.",
                Category = "Gaming",
                Type = TweakType.Service,
                Risk = TweakRisk.Low,
                ServiceName = "XboxNetApiSvc",
                OptimizedStartType = "Disabled",
                DefaultStartType = "Manual"
            },

            // 14. Disable HPET (High Precision Event Timer)
            new Tweak
            {
                Id = "gaming_disable_hpet",
                Name = "Disable HPET",
                Description = "Removes the platform clock override forcing the High Precision Event Timer. On modern systems, the TSC (Time Stamp Counter) is more efficient. Disabling HPET can improve frame times and reduce micro-stuttering. Requires reboot. Revert if system becomes unstable.",
                Category = "Gaming",
                Type = TweakType.PowerShell,
                Risk = TweakRisk.High,
                ApplyScript = "bcdedit /deletevalue useplatformclock",
                RevertScript = "bcdedit /set useplatformclock true",
                DetectScript = @"$output = bcdedit /enum '{current}' 2>&1
if ($output -match 'useplatformclock\s+Yes') { 'NotApplied' } else { 'Applied' }"
            },

            // 15. Optimize Timer Resolution
            new Tweak
            {
                Id = "gaming_timer_resolution",
                Name = "Optimize Timer Resolution",
                Description = "Enables platform tick and disables dynamic tick to force a consistent high-resolution timer across the system. Improves frame pacing and reduces timing jitter in games. Requires reboot. May slightly increase idle power consumption.",
                Category = "Gaming",
                Type = TweakType.PowerShell,
                Risk = TweakRisk.High,
                ApplyScript = @"bcdedit /set useplatformtick yes
bcdedit /set disabledynamictick yes",
                RevertScript = @"bcdedit /deletevalue useplatformtick
bcdedit /deletevalue disabledynamictick",
                DetectScript = @"$output = bcdedit /enum '{current}' 2>&1
$platformTick = $output -match 'useplatformtick\s+Yes'
$dynamicTick = $output -match 'disabledynamictick\s+Yes'
if ($platformTick -and $dynamicTick) { 'Applied' } else { 'NotApplied' }"
            }
        };
    }
}
