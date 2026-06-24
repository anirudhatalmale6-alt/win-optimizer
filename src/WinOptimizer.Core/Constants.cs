namespace WinOptimizer.Core;

public static class Constants
{
    public const string AppName = "WinOptimizer";
    public const string AppVersion = "1.0.0";
    public const string LicenseFileName = "license.dat";
    public const string LicensePassphrase = "W1nOpt!miz3r_Lic3ns3_K3y_2024!@#";
    public const string SignatureSecret = "W1nOpt_S1gn@tur3_S3cr3t_K3y!";
    public const string BackupFolderName = "WinOptimizer_Backups";
    public const string ConfigFileName = "config.json";

    public static string AppDataPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        AppName);

    public static string LicenseFilePath => Path.Combine(AppDataPath, LicenseFileName);
    public static string ConfigFilePath => Path.Combine(AppDataPath, ConfigFileName);
    public static string BackupPath => Path.Combine(AppDataPath, BackupFolderName);
}
