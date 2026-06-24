using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using WinOptimizer.Core;
using WinOptimizer.Engine;
using WinOptimizer.Licensing;
using WinOptimizer.Models;
using WinOptimizer.Tweaks;

namespace WinOptimizer.ViewModels;

public class TweakItemViewModel : BaseViewModel
{
    private readonly Tweak _tweak;

    public TweakItemViewModel(Tweak tweak) { _tweak = tweak; }

    public Tweak Tweak => _tweak;
    public string Id => _tweak.Id;
    public string Name => _tweak.Name;
    public string Description => _tweak.Description;
    public TweakRisk Risk => _tweak.Risk;
    public TweakType Type => _tweak.Type;

    public bool IsEnabled
    {
        get => _tweak.IsEnabled;
        set { _tweak.IsEnabled = value; OnPropertyChanged(); }
    }

    private TweakStatus _status = TweakStatus.Unknown;
    public TweakStatus Status
    {
        get => _status;
        set { SetProperty(ref _status, value); OnPropertyChanged(nameof(StatusText)); OnPropertyChanged(nameof(StatusColor)); }
    }

    public string StatusText => Status switch
    {
        TweakStatus.Applied => "APPLIED",
        TweakStatus.NotApplied => "NOT APPLIED",
        TweakStatus.Error => "ERROR",
        _ => "CHECKING..."
    };

    public string StatusColor => Status switch
    {
        TweakStatus.Applied => "#22C55E",
        TweakStatus.NotApplied => "#9898B0",
        TweakStatus.Error => "#EF4444",
        _ => "#606080"
    };

    public string RiskText => Risk switch
    {
        TweakRisk.Low => "LOW",
        TweakRisk.Medium => "MED",
        TweakRisk.High => "HIGH",
        _ => ""
    };

    public string RiskColor => Risk switch
    {
        TweakRisk.Low => "#22C55E",
        TweakRisk.Medium => "#F59E0B",
        TweakRisk.High => "#EF4444",
        _ => "#606080"
    };
}

public class CategoryViewModel : BaseViewModel
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Icon { get; set; } = "";
    public ObservableCollection<TweakItemViewModel> Tweaks { get; set; } = new();

    public int AppliedCount => Tweaks.Count(t => t.Status == TweakStatus.Applied);
    public int TotalCount => Tweaks.Count;
    public int EnabledCount => Tweaks.Count(t => t.IsEnabled);

    public void Refresh()
    {
        OnPropertyChanged(nameof(AppliedCount));
        OnPropertyChanged(nameof(TotalCount));
        OnPropertyChanged(nameof(EnabledCount));
    }
}

public class MainViewModel : BaseViewModel
{
    private readonly TweakEngine _engine = new();
    private readonly LicenseValidator _license = new();
    private AppConfig _config = new();

    public MainViewModel()
    {
        Categories = new ObservableCollection<CategoryViewModel>();
        LogMessages = new ObservableCollection<string>();

        ApplySelectedCommand = new AsyncRelayCommand(ApplySelected, () => !IsBusy);
        RevertSelectedCommand = new AsyncRelayCommand(RevertSelected, () => !IsBusy);
        ApplyAllInCategoryCommand = new AsyncRelayCommand(ApplyAllInCategory, () => !IsBusy);
        RevertAllInCategoryCommand = new AsyncRelayCommand(RevertAllInCategory, () => !IsBusy);
        SelectAllCommand = new RelayCommand(SelectAll);
        DeselectAllCommand = new RelayCommand(DeselectAll);
        RefreshStatusCommand = new AsyncRelayCommand(RefreshAllStatuses);
        CreateRestorePointCommand = new AsyncRelayCommand(CreateRestorePoint, () => !IsBusy);

        _engine.OnLog += msg => Application.Current.Dispatcher.Invoke(() =>
        {
            LogMessages.Insert(0, $"[{DateTime.Now:HH:mm:ss}] {msg}");
            if (LogMessages.Count > 500) LogMessages.RemoveAt(LogMessages.Count - 1);
        });
    }

    public ObservableCollection<CategoryViewModel> Categories { get; }
    public ObservableCollection<string> LogMessages { get; }

    private CategoryViewModel? _selectedCategory;
    public CategoryViewModel? SelectedCategory
    {
        get => _selectedCategory;
        set { SetProperty(ref _selectedCategory, value); }
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set { SetProperty(ref _isBusy, value); OnPropertyChanged(nameof(IsNotBusy)); }
    }
    public bool IsNotBusy => !_isBusy;

    private string _statusText = "Ready";
    public string StatusText
    {
        get => _statusText;
        set => SetProperty(ref _statusText, value);
    }

    private double _progress;
    public double Progress
    {
        get => _progress;
        set => SetProperty(ref _progress, value);
    }

    private bool _showProgress;
    public bool ShowProgress
    {
        get => _showProgress;
        set => SetProperty(ref _showProgress, value);
    }

    public string WindowsVersion => SystemInfo.WindowsVersion;
    public string WindowsBuild => $"Build {SystemInfo.BuildNumber}";
    public string WindowsEdition => SystemInfo.Edition;
    public string Processor => SystemInfo.ProcessorName;
    public bool IsLicensed => _license.IsLicensed;
    public string LicenseHolder => _license.LicenseHolder ?? "Unlicensed";

    public ICommand ApplySelectedCommand { get; }
    public ICommand RevertSelectedCommand { get; }
    public ICommand ApplyAllInCategoryCommand { get; }
    public ICommand RevertAllInCategoryCommand { get; }
    public ICommand SelectAllCommand { get; }
    public ICommand DeselectAllCommand { get; }
    public ICommand RefreshStatusCommand { get; }
    public ICommand CreateRestorePointCommand { get; }

    public int TotalTweaks => Categories.Sum(c => c.TotalCount);
    public int TotalApplied => Categories.Sum(c => c.AppliedCount);

    public async Task InitializeAsync()
    {
        _license.Validate();
        OnPropertyChanged(nameof(IsLicensed));
        OnPropertyChanged(nameof(LicenseHolder));

        LoadConfig();
        LoadCategories();
        if (Categories.Count > 0)
            SelectedCategory = Categories[0];

        await RefreshAllStatuses();
    }

    public bool CheckLicense() => _license.Validate();

    public bool ActivateLicense(string key) => _license.ActivateWithKey(key);

    private void LoadCategories()
    {
        var allCategories = TweakDefinitions.GetAllCategories();
        var isWin10 = SystemInfo.IsWindows10;
        var isWin11 = SystemInfo.IsWindows11;

        foreach (var cat in allCategories)
        {
            var vm = new CategoryViewModel
            {
                Id = cat.Id,
                Name = cat.Name,
                Description = cat.Description,
                Icon = cat.Icon
            };

            foreach (var tweak in cat.Tweaks)
            {
                if (isWin10 && !tweak.SupportsWindows10) continue;
                if (isWin11 && !tweak.SupportsWindows11) continue;

                var item = new TweakItemViewModel(tweak);
                if (_config.TweakStates.TryGetValue(tweak.Id, out var enabled))
                    item.IsEnabled = enabled;

                vm.Tweaks.Add(item);
            }

            Categories.Add(vm);
        }
    }

    private async Task RefreshAllStatuses()
    {
        StatusText = "Scanning system...";
        IsBusy = true;
        ShowProgress = true;
        int total = Categories.Sum(c => c.Tweaks.Count);
        int current = 0;

        foreach (var cat in Categories)
        {
            foreach (var item in cat.Tweaks)
            {
                current++;
                Progress = (double)current / total * 100;
                var status = await Task.Run(() => _engine.DetectTweakStatus(item.Tweak));
                item.Status = status;
            }
            cat.Refresh();
        }

        OnPropertyChanged(nameof(TotalApplied));
        OnPropertyChanged(nameof(TotalTweaks));
        ShowProgress = false;
        IsBusy = false;
        StatusText = $"Ready - {TotalApplied}/{TotalTweaks} tweaks applied";
    }

    private async Task ApplySelected()
    {
        if (SelectedCategory == null) return;
        var enabled = SelectedCategory.Tweaks.Where(t => t.IsEnabled && t.Status != TweakStatus.Applied).ToList();
        if (enabled.Count == 0) { StatusText = "No tweaks selected to apply"; return; }

        IsBusy = true;
        ShowProgress = true;
        StatusText = $"Applying {enabled.Count} tweaks...";

        for (int i = 0; i < enabled.Count; i++)
        {
            Progress = (double)(i + 1) / enabled.Count * 100;
            StatusText = $"Applying ({i + 1}/{enabled.Count}): {enabled[i].Name}";
            var result = await _engine.ApplyTweakAsync(enabled[i].Tweak);
            enabled[i].Status = enabled[i].Tweak.Status;
        }

        SelectedCategory.Refresh();
        SaveConfig();
        OnPropertyChanged(nameof(TotalApplied));
        ShowProgress = false;
        IsBusy = false;
        StatusText = $"Done - {TotalApplied}/{TotalTweaks} tweaks applied";
    }

    private async Task RevertSelected()
    {
        if (SelectedCategory == null) return;
        var applied = SelectedCategory.Tweaks.Where(t => t.IsEnabled && t.Status == TweakStatus.Applied).ToList();
        if (applied.Count == 0) { StatusText = "No applied tweaks selected to revert"; return; }

        IsBusy = true;
        ShowProgress = true;

        for (int i = 0; i < applied.Count; i++)
        {
            Progress = (double)(i + 1) / applied.Count * 100;
            StatusText = $"Reverting ({i + 1}/{applied.Count}): {applied[i].Name}";
            await _engine.RevertTweakAsync(applied[i].Tweak);
            applied[i].Status = applied[i].Tweak.Status;
        }

        SelectedCategory.Refresh();
        SaveConfig();
        OnPropertyChanged(nameof(TotalApplied));
        ShowProgress = false;
        IsBusy = false;
        StatusText = $"Done - {TotalApplied}/{TotalTweaks} tweaks applied";
    }

    private async Task ApplyAllInCategory()
    {
        if (SelectedCategory == null) return;
        foreach (var t in SelectedCategory.Tweaks) t.IsEnabled = true;
        await ApplySelected();
    }

    private async Task RevertAllInCategory()
    {
        if (SelectedCategory == null) return;
        foreach (var t in SelectedCategory.Tweaks) t.IsEnabled = true;
        await RevertSelected();
    }

    private void SelectAll(object? _)
    {
        if (SelectedCategory == null) return;
        foreach (var t in SelectedCategory.Tweaks) t.IsEnabled = true;
    }

    private void DeselectAll(object? _)
    {
        if (SelectedCategory == null) return;
        foreach (var t in SelectedCategory.Tweaks) t.IsEnabled = false;
    }

    private async Task CreateRestorePoint()
    {
        IsBusy = true;
        StatusText = "Creating system restore point...";
        var success = await RestorePointHelper.CreateRestorePointAsync("WinOptimizer Backup");
        StatusText = success ? "Restore point created" : "Failed to create restore point";
        IsBusy = false;
    }

    private void LoadConfig()
    {
        try
        {
            if (File.Exists(Constants.ConfigFilePath))
            {
                var json = File.ReadAllText(Constants.ConfigFilePath);
                _config = JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
            }
        }
        catch { _config = new AppConfig(); }
    }

    private void SaveConfig()
    {
        try
        {
            _config.TweakStates.Clear();
            foreach (var cat in Categories)
                foreach (var t in cat.Tweaks)
                    _config.TweakStates[t.Id] = t.IsEnabled;
            _config.LastApplied = DateTime.UtcNow;

            Directory.CreateDirectory(Constants.AppDataPath);
            File.WriteAllText(Constants.ConfigFilePath,
                JsonSerializer.Serialize(_config, new JsonSerializerOptions { WriteIndented = true }));
        }
        catch { }
    }
}
