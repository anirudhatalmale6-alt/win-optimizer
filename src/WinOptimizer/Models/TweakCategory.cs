namespace WinOptimizer.Models;

public class TweakCategory
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public List<Tweak> Tweaks { get; set; } = new();
    public int AppliedCount => Tweaks.Count(t => t.Status == TweakStatus.Applied);
    public int TotalCount => Tweaks.Count;
}

public class AppConfig
{
    public Dictionary<string, bool> TweakStates { get; set; } = new();
    public bool CreateRestorePoint { get; set; } = true;
    public bool ShowRiskyTweaks { get; set; } = false;
    public DateTime? LastApplied { get; set; }
}
