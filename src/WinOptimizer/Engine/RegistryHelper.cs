using Microsoft.Win32;

namespace WinOptimizer.Engine;

public static class RegistryHelper
{
    public static object? GetValue(string path, string valueName)
    {
        try
        {
            var (root, subKey) = ParsePath(path);
            using var key = root.OpenSubKey(subKey, false);
            return key?.GetValue(valueName);
        }
        catch
        {
            return null;
        }
    }

    public static bool SetValue(string path, string valueName, object value, RegistryValueKind kind)
    {
        try
        {
            var (root, subKey) = ParsePath(path);
            using var key = root.CreateSubKey(subKey, true);
            if (key == null) return false;
            key.SetValue(valueName, value, kind);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool DeleteValue(string path, string valueName)
    {
        try
        {
            var (root, subKey) = ParsePath(path);
            using var key = root.OpenSubKey(subKey, true);
            if (key == null) return false;
            key.DeleteValue(valueName, false);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool CreateKey(string path)
    {
        try
        {
            var (root, subKey) = ParsePath(path);
            using var key = root.CreateSubKey(subKey, true);
            return key != null;
        }
        catch
        {
            return false;
        }
    }

    public static bool DeleteKey(string path)
    {
        try
        {
            var (root, subKey) = ParsePath(path);
            root.DeleteSubKeyTree(subKey, false);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static bool KeyExists(string path)
    {
        try
        {
            var (root, subKey) = ParsePath(path);
            using var key = root.OpenSubKey(subKey, false);
            return key != null;
        }
        catch
        {
            return false;
        }
    }

    public static Dictionary<string, object?> BackupKey(string path)
    {
        var backup = new Dictionary<string, object?>();
        try
        {
            var (root, subKey) = ParsePath(path);
            using var key = root.OpenSubKey(subKey, false);
            if (key == null) return backup;
            foreach (var name in key.GetValueNames())
            {
                backup[name] = key.GetValue(name);
            }
        }
        catch { }
        return backup;
    }

    private static (RegistryKey Root, string SubKey) ParsePath(string path)
    {
        var parts = path.Split('\\', 2);
        var rootName = parts[0].ToUpperInvariant();
        var subKey = parts.Length > 1 ? parts[1] : string.Empty;

        var root = rootName switch
        {
            "HKEY_LOCAL_MACHINE" or "HKLM" => Registry.LocalMachine,
            "HKEY_CURRENT_USER" or "HKCU" => Registry.CurrentUser,
            "HKEY_CLASSES_ROOT" or "HKCR" => Registry.ClassesRoot,
            "HKEY_USERS" or "HKU" => Registry.Users,
            "HKEY_CURRENT_CONFIG" or "HKCC" => Registry.CurrentConfig,
            _ => throw new ArgumentException($"Unknown registry root: {rootName}")
        };

        return (root, subKey);
    }
}
