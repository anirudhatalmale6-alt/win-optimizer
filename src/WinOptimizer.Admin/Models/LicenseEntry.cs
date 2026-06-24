namespace WinOptimizer.Admin.Models;

public class LicenseEntry
{
    public string KeyCode { get; set; } = string.Empty;
    public string HardwareId { get; set; } = string.Empty;
    public string IssuedTo { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }

    public string StatusText => IsRevoked ? "REVOKED" : ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value ? "EXPIRED" : "ACTIVE";
    public string StatusColor => StatusText switch
    {
        "ACTIVE" => "#22C55E",
        "EXPIRED" => "#F59E0B",
        "REVOKED" => "#EF4444",
        _ => "#606080"
    };

    public string ShortKey => KeyCode.Length > 40 ? KeyCode[..40] + "..." : KeyCode;
    public string ShortHwid => HardwareId.Length > 16 ? HardwareId[..16] + "..." : HardwareId;
}
