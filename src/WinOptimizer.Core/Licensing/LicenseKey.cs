using System.Text.Json;

namespace WinOptimizer.Core.Licensing;

public class LicenseKey
{
    public string HardwareId { get; set; } = string.Empty;
    public string KeyCode { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }
    public string IssuedTo { get; set; } = string.Empty;

    public bool IsExpired => ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;
    public bool IsValid => !IsRevoked && !IsExpired;

    public string Serialize()
    {
        return JsonSerializer.Serialize(this);
    }

    public static LicenseKey? Deserialize(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<LicenseKey>(json);
        }
        catch
        {
            return null;
        }
    }
}

public class LicenseFile
{
    public string EncryptedPayload { get; set; } = string.Empty;
    public string HwidHash { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;

    public string Serialize()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }

    public static LicenseFile? Deserialize(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<LicenseFile>(json);
        }
        catch
        {
            return null;
        }
    }
}
