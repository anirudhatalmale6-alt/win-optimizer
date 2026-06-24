using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using WinOptimizer.Admin.Models;
using WinOptimizer.Core;
using WinOptimizer.Core.Licensing;

namespace WinOptimizer.Admin.ViewModels;

public class AdminViewModel : INotifyPropertyChanged
{
    private const string LicenseDbFile = "licenses.json";
    private readonly string _dbPath;

    public AdminViewModel()
    {
        _dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WinOptimizer.Admin", LicenseDbFile);

        Licenses = new ObservableCollection<LicenseEntry>();
        GenerateCommand = new AdminRelayCommand(GenerateLicense);
        RevokeCommand = new AdminRelayCommand(RevokeLicense, () => SelectedLicense != null);
        RenewCommand = new AdminRelayCommand(RenewLicense, () => SelectedLicense != null);
        CopyKeyCommand = new AdminRelayCommand(CopyKey, () => SelectedLicense != null);
        DeleteCommand = new AdminRelayCommand(DeleteLicense, () => SelectedLicense != null);

        LoadLicenses();
    }

    public ObservableCollection<LicenseEntry> Licenses { get; }

    private LicenseEntry? _selectedLicense;
    public LicenseEntry? SelectedLicense
    {
        get => _selectedLicense;
        set
        {
            _selectedLicense = value;
            OnPropertyChanged();
            CommandManager.InvalidateRequerySuggested();
        }
    }

    private string _hardwareId = string.Empty;
    public string HardwareId
    {
        get => _hardwareId;
        set { _hardwareId = value; OnPropertyChanged(); }
    }

    private string _issuedTo = string.Empty;
    public string IssuedTo
    {
        get => _issuedTo;
        set { _issuedTo = value; OnPropertyChanged(); }
    }

    private bool _hasExpiry;
    public bool HasExpiry
    {
        get => _hasExpiry;
        set { _hasExpiry = value; OnPropertyChanged(); }
    }

    private int _expiryDays = 365;
    public int ExpiryDays
    {
        get => _expiryDays;
        set { _expiryDays = value; OnPropertyChanged(); }
    }

    private string _lastGeneratedKey = string.Empty;
    public string LastGeneratedKey
    {
        get => _lastGeneratedKey;
        set { _lastGeneratedKey = value; OnPropertyChanged(); }
    }

    private string _statusMessage = string.Empty;
    public string StatusMessage
    {
        get => _statusMessage;
        set { _statusMessage = value; OnPropertyChanged(); }
    }

    public ICommand GenerateCommand { get; }
    public ICommand RevokeCommand { get; }
    public ICommand RenewCommand { get; }
    public ICommand CopyKeyCommand { get; }
    public ICommand DeleteCommand { get; }

    private void GenerateLicense()
    {
        if (string.IsNullOrWhiteSpace(HardwareId))
        {
            StatusMessage = "Hardware ID is required.";
            return;
        }

        var key = new LicenseKey
        {
            HardwareId = HardwareId.Trim(),
            KeyCode = Guid.NewGuid().ToString("N"),
            IssuedAt = DateTime.UtcNow,
            ExpiresAt = HasExpiry ? DateTime.UtcNow.AddDays(ExpiryDays) : null,
            IssuedTo = IssuedTo.Trim(),
            IsRevoked = false
        };

        var serialized = key.Serialize();
        var encrypted = CryptoHelper.Encrypt(serialized, Constants.LicensePassphrase);

        var entry = new LicenseEntry
        {
            KeyCode = encrypted,
            HardwareId = HardwareId.Trim(),
            IssuedTo = IssuedTo.Trim(),
            IssuedAt = key.IssuedAt,
            ExpiresAt = key.ExpiresAt,
            IsRevoked = false
        };

        Licenses.Add(entry);
        LastGeneratedKey = encrypted;
        SaveLicenses();

        Clipboard.SetText(encrypted);
        StatusMessage = $"License generated for {(string.IsNullOrEmpty(IssuedTo) ? HardwareId[..Math.Min(16, HardwareId.Length)] : IssuedTo)} and copied to clipboard.";

        HardwareId = string.Empty;
        IssuedTo = string.Empty;
    }

    private void RevokeLicense()
    {
        if (SelectedLicense == null) return;
        SelectedLicense.IsRevoked = true;
        OnPropertyChanged(nameof(Licenses));
        SaveLicenses();
        StatusMessage = $"License for {SelectedLicense.IssuedTo} has been revoked.";

        var items = Licenses.ToList();
        Licenses.Clear();
        foreach (var item in items) Licenses.Add(item);
    }

    private void RenewLicense()
    {
        if (SelectedLicense == null) return;
        SelectedLicense.IsRevoked = false;
        SelectedLicense.ExpiresAt = HasExpiry ? DateTime.UtcNow.AddDays(ExpiryDays) : null;

        var key = new LicenseKey
        {
            HardwareId = SelectedLicense.HardwareId,
            KeyCode = Guid.NewGuid().ToString("N"),
            IssuedAt = DateTime.UtcNow,
            ExpiresAt = SelectedLicense.ExpiresAt,
            IssuedTo = SelectedLicense.IssuedTo,
            IsRevoked = false
        };

        var serialized = key.Serialize();
        var encrypted = CryptoHelper.Encrypt(serialized, Constants.LicensePassphrase);
        SelectedLicense.KeyCode = encrypted;

        LastGeneratedKey = encrypted;
        Clipboard.SetText(encrypted);
        SaveLicenses();
        StatusMessage = $"License renewed and new key copied to clipboard.";

        var items = Licenses.ToList();
        Licenses.Clear();
        foreach (var item in items) Licenses.Add(item);
    }

    private void CopyKey()
    {
        if (SelectedLicense == null) return;
        Clipboard.SetText(SelectedLicense.KeyCode);
        StatusMessage = "License key copied to clipboard.";
    }

    private void DeleteLicense()
    {
        if (SelectedLicense == null) return;
        var name = SelectedLicense.IssuedTo;
        Licenses.Remove(SelectedLicense);
        SelectedLicense = null;
        SaveLicenses();
        StatusMessage = $"License for {name} deleted.";
    }

    private void LoadLicenses()
    {
        try
        {
            if (!File.Exists(_dbPath)) return;
            var json = File.ReadAllText(_dbPath);
            var entries = JsonSerializer.Deserialize<List<LicenseEntry>>(json);
            if (entries == null) return;
            foreach (var entry in entries) Licenses.Add(entry);
        }
        catch { }
    }

    private void SaveLicenses()
    {
        try
        {
            var dir = Path.GetDirectoryName(_dbPath);
            if (dir != null) Directory.CreateDirectory(dir);
            var json = JsonSerializer.Serialize(Licenses.ToList(), new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_dbPath, json);
        }
        catch { }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public class AdminRelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public AdminRelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute;
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;
    public void Execute(object? parameter) => _execute();
}
