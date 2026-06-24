using WinOptimizer.Models;

namespace WinOptimizer.Tweaks;

public static class TweakDefinitions
{
    public static List<TweakCategory> GetAllCategories()
    {
        return new List<TweakCategory>
        {
            new TweakCategory
            {
                Id = "performance", Name = "Performance", Icon = "",
                Description = "Memory, CPU, disk and boot optimizations",
                Tweaks = PerformanceTweaks.GetTweaks()
            },
            new TweakCategory
            {
                Id = "privacy", Name = "Privacy", Icon = "",
                Description = "Telemetry, tracking and data collection controls",
                Tweaks = PrivacyTweaks.GetTweaks()
            },
            new TweakCategory
            {
                Id = "network", Name = "Network", Icon = "",
                Description = "TCP/IP, DNS and connection optimizations",
                Tweaks = NetworkTweaks.GetTweaks()
            },
            new TweakCategory
            {
                Id = "gaming", Name = "Gaming", Icon = "",
                Description = "GPU, input latency and game-specific tweaks",
                Tweaks = GamingTweaks.GetTweaks()
            },
            new TweakCategory
            {
                Id = "visual", Name = "Visual", Icon = "",
                Description = "UI effects, theme and explorer customizations",
                Tweaks = VisualTweaks.GetTweaks()
            },
            new TweakCategory
            {
                Id = "services", Name = "Services", Icon = "",
                Description = "Disable unnecessary Windows services",
                Tweaks = ServicesTweaks.GetTweaks()
            },
            new TweakCategory
            {
                Id = "debloat", Name = "Debloat", Icon = "",
                Description = "Remove pre-installed bloatware apps",
                Tweaks = DebloatTweaks.GetTweaks()
            }
        };
    }
}
