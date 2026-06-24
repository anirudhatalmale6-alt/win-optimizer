using System.Management;
using System.Text;

namespace WinOptimizer.Core.Licensing;

public static class HwidHelper
{
    public static string GetHardwareId()
    {
        var sb = new StringBuilder();
        sb.Append(GetWmiValue("Win32_Processor", "ProcessorId"));
        sb.Append('|');
        sb.Append(GetWmiValue("Win32_BaseBoard", "SerialNumber"));
        sb.Append('|');
        sb.Append(GetDiskSerial());
        return CryptoHelper.ComputeHash(sb.ToString());
    }

    private static string GetWmiValue(string wmiClass, string property)
    {
        try
        {
            using var searcher = new ManagementObjectSearcher($"SELECT {property} FROM {wmiClass}");
            foreach (var obj in searcher.Get())
            {
                var val = obj[property]?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(val) && val != "To Be Filled By O.E.M.")
                    return val;
            }
        }
        catch { }
        return "UNKNOWN";
    }

    private static string GetDiskSerial()
    {
        try
        {
            using var searcher = new ManagementObjectSearcher(
                "SELECT SerialNumber FROM Win32_DiskDrive WHERE MediaType='Fixed hard disk media'");
            foreach (var obj in searcher.Get())
            {
                var serial = obj["SerialNumber"]?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(serial))
                    return serial;
            }
        }
        catch { }
        return "UNKNOWN";
    }
}
