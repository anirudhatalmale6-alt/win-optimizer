using WinOptimizer.Models;

namespace WinOptimizer.Tweaks;

public static class ServicesTweaks
{
    public static List<Tweak> GetTweaks()
    {
        return new List<Tweak>
        {
            new Tweak
            {
                Id = "svc-spooler", Name = "Disable Print Spooler",
                Description = "Disables the Print Spooler service. Only disable if you don't use printers or print-to-PDF.",
                Category = "Services", Type = TweakType.Service, Risk = TweakRisk.Medium,
                ServiceName = "Spooler", OptimizedStartType = "Disabled", DefaultStartType = "Automatic"
            },
            new Tweak
            {
                Id = "svc-fax", Name = "Disable Fax Service",
                Description = "Disables the Windows Fax service. Safe to disable unless you send/receive faxes.",
                Category = "Services", Type = TweakType.Service, Risk = TweakRisk.Low,
                ServiceName = "Fax", OptimizedStartType = "Disabled", DefaultStartType = "Manual"
            },
            new Tweak
            {
                Id = "svc-maps", Name = "Disable Maps Broker",
                Description = "Disables the Downloaded Maps Manager for offline map data.",
                Category = "Services", Type = TweakType.Service, Risk = TweakRisk.Low,
                ServiceName = "MapsBroker", OptimizedStartType = "Disabled", DefaultStartType = "Automatic"
            },
            new Tweak
            {
                Id = "svc-geolocation", Name = "Disable Geolocation Service",
                Description = "Disables the Geolocation service that monitors your device's position.",
                Category = "Services", Type = TweakType.Service, Risk = TweakRisk.Low,
                ServiceName = "lfsvc", OptimizedStartType = "Disabled", DefaultStartType = "Manual"
            },
            new Tweak
            {
                Id = "svc-remote-registry", Name = "Disable Remote Registry",
                Description = "Prevents remote users from modifying registry settings on this computer.",
                Category = "Services", Type = TweakType.Service, Risk = TweakRisk.Low,
                ServiceName = "RemoteRegistry", OptimizedStartType = "Disabled", DefaultStartType = "Manual"
            },
            new Tweak
            {
                Id = "svc-iphelper", Name = "Disable IP Helper (IPv6)",
                Description = "Disables IPv6 tunnel connectivity. Safe if your network doesn't require IPv6 tunneling.",
                Category = "Services", Type = TweakType.Service, Risk = TweakRisk.Medium,
                ServiceName = "iphlpsvc", OptimizedStartType = "Disabled", DefaultStartType = "Automatic"
            },
            new Tweak
            {
                Id = "svc-tablet", Name = "Disable Tablet Input Service",
                Description = "Disables touch keyboard and handwriting panel. Safe on non-touch desktops.",
                Category = "Services", Type = TweakType.Service, Risk = TweakRisk.Low,
                ServiceName = "TabletInputService", OptimizedStartType = "Disabled", DefaultStartType = "Manual"
            },
            new Tweak
            {
                Id = "svc-smartcard", Name = "Disable Smart Card Service",
                Description = "Disables smart card reader access. Safe if you don't use smart card authentication.",
                Category = "Services", Type = TweakType.Service, Risk = TweakRisk.Low,
                ServiceName = "SCardSvr", OptimizedStartType = "Disabled", DefaultStartType = "Manual"
            },
            new Tweak
            {
                Id = "svc-smartcard-enum", Name = "Disable Smart Card Enumeration",
                Description = "Disables smart card device node creation.",
                Category = "Services", Type = TweakType.Service, Risk = TweakRisk.Low,
                ServiceName = "ScDeviceEnum", OptimizedStartType = "Disabled", DefaultStartType = "Manual"
            },
            new Tweak
            {
                Id = "svc-biometric", Name = "Disable Windows Biometric",
                Description = "Disables fingerprint and facial recognition. Only disable if you don't use biometric login.",
                Category = "Services", Type = TweakType.Service, Risk = TweakRisk.Medium,
                ServiceName = "WbioSrvc", OptimizedStartType = "Disabled", DefaultStartType = "Manual"
            },
            new Tweak
            {
                Id = "svc-phone", Name = "Disable Phone Service",
                Description = "Disables telephony state management. Safe if you don't use the Your Phone app.",
                Category = "Services", Type = TweakType.Service, Risk = TweakRisk.Low,
                ServiceName = "PhoneSvc", OptimizedStartType = "Disabled", DefaultStartType = "Manual"
            },
            new Tweak
            {
                Id = "svc-cdp", Name = "Disable Connected Devices Platform",
                Description = "Disables cross-device experiences, Bluetooth, and connected device scenarios.",
                Category = "Services", Type = TweakType.Service, Risk = TweakRisk.Medium,
                ServiceName = "CDPSvc", OptimizedStartType = "Disabled", DefaultStartType = "Automatic"
            }
        };
    }
}
