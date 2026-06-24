using System.Collections.Generic;
using Microsoft.Win32;
using WinOptimizer.Models;

namespace WinOptimizer.Tweaks
{
    public static class VisualTweaks
    {
        public static List<Tweak> GetTweaks()
        {
            return new List<Tweak>
            {
                // 1. Disable Animations
                new Tweak
                {
                    Id = "visual-disable-animations",
                    Name = "Disable Animations",
                    Description = "Disables window minimize/maximize animations for a snappier feel.",
                    Category = "Visual",
                    Type = TweakType.Registry,
                    Risk = TweakRisk.Low,
                    RegistryPath = @"HKCU\Control Panel\Desktop\WindowMetrics",
                    RegistryValueName = "MinAnimate",
                    OptimizedValue = "0",
                    DefaultValue = "1",
                    ValueKind = RegistryValueKind.String
                },

                // 2. Disable Transparency
                new Tweak
                {
                    Id = "visual-disable-transparency",
                    Name = "Disable Transparency",
                    Description = "Disables transparency effects on the taskbar, Start menu, and Action Center to improve performance.",
                    Category = "Visual",
                    Type = TweakType.Registry,
                    Risk = TweakRisk.Low,
                    RegistryPath = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                    RegistryValueName = "EnableTransparency",
                    OptimizedValue = 0,
                    DefaultValue = 1,
                    ValueKind = RegistryValueKind.DWord
                },

                // 3. Classic Context Menu (Win11)
                new Tweak
                {
                    Id = "visual-classic-context-menu",
                    Name = "Classic Context Menu (Win11)",
                    Description = "Restores the full classic right-click context menu in Windows 11 instead of the simplified modern menu.",
                    Category = "Visual",
                    Type = TweakType.PowerShell,
                    Risk = TweakRisk.Low,
                    SupportsWindows11 = true,
                    SupportsWindows10 = false,
                    ApplyScript = @"
New-Item -Path 'HKCU:\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32' -Force | Out-Null
Set-ItemProperty -Path 'HKCU:\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32' -Name '(Default)' -Value '' -Force
Stop-Process -Name explorer -Force -ErrorAction SilentlyContinue
",
                    RevertScript = @"
Remove-Item -Path 'HKCU:\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}' -Recurse -Force -ErrorAction SilentlyContinue
Stop-Process -Name explorer -Force -ErrorAction SilentlyContinue
",
                    DetectScript = @"
try {
    $path = 'HKCU:\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32'
    if (Test-Path $path) { return $true } else { return $false }
} catch { return $false }
"
                },

                // 4. Show File Extensions
                new Tweak
                {
                    Id = "visual-show-file-extensions",
                    Name = "Show File Extensions",
                    Description = "Shows file name extensions in Explorer, making it easier to identify file types and spot suspicious files.",
                    Category = "Visual",
                    Type = TweakType.Registry,
                    Risk = TweakRisk.Low,
                    RegistryPath = @"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    RegistryValueName = "HideFileExt",
                    OptimizedValue = 0,
                    DefaultValue = 1,
                    ValueKind = RegistryValueKind.DWord
                },

                // 5. Show Hidden Files
                new Tweak
                {
                    Id = "visual-show-hidden-files",
                    Name = "Show Hidden Files",
                    Description = "Makes hidden files and folders visible in File Explorer.",
                    Category = "Visual",
                    Type = TweakType.Registry,
                    Risk = TweakRisk.Low,
                    RegistryPath = @"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    RegistryValueName = "Hidden",
                    OptimizedValue = 1,
                    DefaultValue = 2,
                    ValueKind = RegistryValueKind.DWord
                },

                // 6. Show Super Hidden Files
                new Tweak
                {
                    Id = "visual-show-super-hidden",
                    Name = "Show Super Hidden Files",
                    Description = "Shows protected operating system files that are normally hidden even when 'Show Hidden Files' is enabled.",
                    Category = "Visual",
                    Type = TweakType.Registry,
                    Risk = TweakRisk.Medium,
                    RegistryPath = @"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    RegistryValueName = "ShowSuperHidden",
                    OptimizedValue = 1,
                    DefaultValue = 0,
                    ValueKind = RegistryValueKind.DWord
                },

                // 7. Disable Taskbar Search Box
                new Tweak
                {
                    Id = "visual-disable-search-box",
                    Name = "Disable Taskbar Search Box",
                    Description = "Removes the search box from the taskbar to free up space. You can still search by pressing the Windows key.",
                    Category = "Visual",
                    Type = TweakType.Registry,
                    Risk = TweakRisk.Low,
                    RegistryPath = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Search",
                    RegistryValueName = "SearchboxTaskbarMode",
                    OptimizedValue = 0,
                    DefaultValue = 2,
                    ValueKind = RegistryValueKind.DWord
                },

                // 8. Small Taskbar Icons (Win10 only)
                new Tweak
                {
                    Id = "visual-small-taskbar-icons",
                    Name = "Small Taskbar Icons",
                    Description = "Uses smaller icons on the taskbar to save space and fit more pinned applications.",
                    Category = "Visual",
                    Type = TweakType.Registry,
                    Risk = TweakRisk.Low,
                    SupportsWindows10 = true,
                    SupportsWindows11 = false,
                    RegistryPath = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    RegistryValueName = "TaskbarSmallIcons",
                    OptimizedValue = 1,
                    DefaultValue = 0,
                    ValueKind = RegistryValueKind.DWord
                },

                // 9. Disable Lock Screen
                new Tweak
                {
                    Id = "visual-disable-lock-screen",
                    Name = "Disable Lock Screen",
                    Description = "Bypasses the lock screen and goes straight to the login prompt, saving a click on every startup and wake.",
                    Category = "Visual",
                    Type = TweakType.Registry,
                    Risk = TweakRisk.Low,
                    RegistryPath = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\Personalization",
                    RegistryValueName = "NoLockScreen",
                    OptimizedValue = 1,
                    DefaultValue = 0,
                    ValueKind = RegistryValueKind.DWord
                },

                // 10. Dark Mode Apps
                new Tweak
                {
                    Id = "visual-dark-mode-apps",
                    Name = "Dark Mode Apps",
                    Description = "Switches applications to dark color mode, reducing eye strain in low-light environments.",
                    Category = "Visual",
                    Type = TweakType.Registry,
                    Risk = TweakRisk.Low,
                    RegistryPath = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                    RegistryValueName = "AppsUseLightTheme",
                    OptimizedValue = 0,
                    DefaultValue = 1,
                    ValueKind = RegistryValueKind.DWord
                },

                // 11. Dark Mode System
                new Tweak
                {
                    Id = "visual-dark-mode-system",
                    Name = "Dark Mode System",
                    Description = "Switches the Windows system UI (taskbar, Start menu, Action Center) to dark color mode.",
                    Category = "Visual",
                    Type = TweakType.Registry,
                    Risk = TweakRisk.Low,
                    RegistryPath = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                    RegistryValueName = "SystemUsesLightTheme",
                    OptimizedValue = 0,
                    DefaultValue = 1,
                    ValueKind = RegistryValueKind.DWord
                },

                // 12. Disable Snap Assist
                new Tweak
                {
                    Id = "visual-disable-snap-assist",
                    Name = "Disable Snap Assist",
                    Description = "Disables the Snap Assist overlay that suggests windows to fill remaining screen space after snapping a window.",
                    Category = "Visual",
                    Type = TweakType.Registry,
                    Risk = TweakRisk.Low,
                    RegistryPath = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    RegistryValueName = "SnapAssist",
                    OptimizedValue = 0,
                    DefaultValue = 1,
                    ValueKind = RegistryValueKind.DWord
                },

                // 13. Verbose Logon Messages
                new Tweak
                {
                    Id = "visual-verbose-logon",
                    Name = "Verbose Logon Messages",
                    Description = "Shows detailed status messages during startup, shutdown, logon, and logoff instead of generic text.",
                    Category = "Visual",
                    Type = TweakType.Registry,
                    Risk = TweakRisk.Low,
                    RegistryPath = @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
                    RegistryValueName = "VerboseStatus",
                    OptimizedValue = 1,
                    DefaultValue = 0,
                    ValueKind = RegistryValueKind.DWord
                },

                // 14. Disable Aero Shake
                new Tweak
                {
                    Id = "visual-disable-aero-shake",
                    Name = "Disable Aero Shake",
                    Description = "Prevents the Aero Shake gesture from minimizing all other windows when you shake a window's title bar.",
                    Category = "Visual",
                    Type = TweakType.Registry,
                    Risk = TweakRisk.Low,
                    RegistryPath = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    RegistryValueName = "DisallowShaking",
                    OptimizedValue = 1,
                    DefaultValue = 0,
                    ValueKind = RegistryValueKind.DWord
                },

                // 15. Wallpaper Quality
                new Tweak
                {
                    Id = "visual-wallpaper-quality",
                    Name = "Wallpaper Quality",
                    Description = "Sets desktop wallpaper JPEG compression quality to maximum (100%), eliminating visible artifacts on JPEG wallpapers.",
                    Category = "Visual",
                    Type = TweakType.Registry,
                    Risk = TweakRisk.Low,
                    RegistryPath = @"HKCU\Control Panel\Desktop",
                    RegistryValueName = "JPEGImportQuality",
                    OptimizedValue = 100,
                    DefaultValue = 75,
                    ValueKind = RegistryValueKind.DWord
                }
            };
        }
    }
}
