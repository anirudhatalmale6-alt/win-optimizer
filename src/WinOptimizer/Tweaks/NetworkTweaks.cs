using Microsoft.Win32;
using WinOptimizer.Models;

namespace WinOptimizer.Tweaks;

public static class NetworkTweaks
{
    public static List<Tweak> GetTweaks()
    {
        return new List<Tweak>
        {
            // 1. Disable Nagle's Algorithm (PowerShell — must iterate all interfaces)
            new Tweak
            {
                Id = "NET-001",
                Name = "Disable Nagle's Algorithm",
                Description = "Disables TCP packet batching (Nagle) on all network interfaces for lower latency. Sets TcpNoDelay and TcpAckFrequency to 1 on every interface.",
                Category = "Network",
                Type = TweakType.PowerShell,
                Risk = TweakRisk.Low,
                ApplyScript = "Get-ChildItem 'HKLM:\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\Interfaces' | ForEach-Object { Set-ItemProperty -Path $_.PSPath -Name 'TcpNoDelay' -Value 1 -Type DWord -Force; Set-ItemProperty -Path $_.PSPath -Name 'TcpAckFrequency' -Value 1 -Type DWord -Force }",
                RevertScript = "Get-ChildItem 'HKLM:\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\Interfaces' | ForEach-Object { Remove-ItemProperty -Path $_.PSPath -Name 'TcpNoDelay' -ErrorAction SilentlyContinue; Remove-ItemProperty -Path $_.PSPath -Name 'TcpAckFrequency' -ErrorAction SilentlyContinue }",
                DetectScript = "if ((Get-ChildItem 'HKLM:\\SYSTEM\\CurrentControlSet\\Services\\Tcpip\\Parameters\\Interfaces' | Where-Object { (Get-ItemProperty $_.PSPath -Name 'TcpNoDelay' -ErrorAction SilentlyContinue).TcpNoDelay -eq 1 }).Count -gt 0) { 'True' } else { 'False' }"
            },

            // 2. Enable TCP Timestamps (RFC 1323)
            new Tweak
            {
                Id = "NET-002",
                Name = "Enable TCP Timestamps",
                Description = "Enables RFC 1323 TCP timestamps and window scaling for improved throughput on high-latency connections.",
                Category = "Network",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters",
                RegistryValueName = "Tcp1323Opts",
                OptimizedValue = 1,
                DefaultValue = 0,
                ValueKind = RegistryValueKind.DWord
            },

            // 3. Increase Max User Ports
            new Tweak
            {
                Id = "NET-003",
                Name = "Increase Maximum User Ports",
                Description = "Raises the maximum number of ephemeral ports from 5000 to 65534, preventing port exhaustion under heavy network load.",
                Category = "Network",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters",
                RegistryValueName = "MaxUserPort",
                OptimizedValue = 65534,
                DefaultValue = 5000,
                ValueKind = RegistryValueKind.DWord
            },

            // 4. Reduce TCP Timed Wait Delay
            new Tweak
            {
                Id = "NET-004",
                Name = "Reduce TCP Timed Wait Delay",
                Description = "Reduces the TIME_WAIT state duration from 120 to 30 seconds, freeing closed connections faster.",
                Category = "Network",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters",
                RegistryValueName = "TcpTimedWaitDelay",
                OptimizedValue = 30,
                DefaultValue = 120,
                ValueKind = RegistryValueKind.DWord
            },

            // 5. Optimize Default TTL
            new Tweak
            {
                Id = "NET-005",
                Name = "Optimize Default TTL",
                Description = "Sets the default Time To Live to 64 (Linux standard) instead of 128, reducing unnecessary hops and slightly masking the OS fingerprint.",
                Category = "Network",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters",
                RegistryValueName = "DefaultTTL",
                OptimizedValue = 64,
                DefaultValue = 128,
                ValueKind = RegistryValueKind.DWord
            },

            // 6. Increase Max Free TCBs
            new Tweak
            {
                Id = "NET-006",
                Name = "Increase Max Free TCBs",
                Description = "Increases the maximum number of free TCP control blocks from 2000 to 65536, allowing more simultaneous connections.",
                Category = "Network",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters",
                RegistryValueName = "MaxFreeTcbs",
                OptimizedValue = 65536,
                DefaultValue = 2000,
                ValueKind = RegistryValueKind.DWord
            },

            // 7. Increase Max Hash Table Size
            new Tweak
            {
                Id = "NET-007",
                Name = "Increase Max Hash Table Size",
                Description = "Increases the TCP hash table size from 512 to 65536 for faster connection lookups under heavy load.",
                Category = "Network",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters",
                RegistryValueName = "MaxHashTableSize",
                OptimizedValue = 65536,
                DefaultValue = 512,
                ValueKind = RegistryValueKind.DWord
            },

            // 8. Disable TCP Auto-Tuning
            new Tweak
            {
                Id = "NET-008",
                Name = "Disable TCP Auto-Tuning",
                Description = "Disables the TCP receive window auto-tuning level. Can fix connectivity issues with older routers and firewalls that don't handle window scaling properly.",
                Category = "Network",
                Type = TweakType.PowerShell,
                Risk = TweakRisk.Medium,
                ApplyScript = "netsh int tcp set global autotuninglevel=disabled",
                RevertScript = "netsh int tcp set global autotuninglevel=normal",
                DetectScript = "if ((netsh int tcp show global) -match 'disabled') { 'True' } else { 'False' }"
            },

            // 9. Disable ECN Capability
            new Tweak
            {
                Id = "NET-009",
                Name = "Disable ECN Capability",
                Description = "Disables Explicit Congestion Notification. Some older network equipment drops ECN-marked packets, causing connectivity problems.",
                Category = "Network",
                Type = TweakType.PowerShell,
                Risk = TweakRisk.Low,
                ApplyScript = "netsh int tcp set global ecncapability=disabled",
                RevertScript = "netsh int tcp set global ecncapability=default",
                DetectScript = "if ((netsh int tcp show global) -match 'ecncapability.*disabled') { 'True' } else { 'False' }"
            },

            // 10. Disable Receive-Side Scaling (RSS)
            new Tweak
            {
                Id = "NET-010",
                Name = "Disable Receive-Side Scaling",
                Description = "Disables RSS which distributes network receive processing across multiple CPUs. Disabling can reduce latency on systems with a single high-speed NIC.",
                Category = "Network",
                Type = TweakType.PowerShell,
                Risk = TweakRisk.Low,
                ApplyScript = "netsh int tcp set global rss=disabled",
                RevertScript = "netsh int tcp set global rss=enabled",
                DetectScript = "if ((netsh int tcp show global) -match 'rss.*disabled') { 'True' } else { 'False' }"
            },

            // 11. Disable Delivery Optimization Service
            new Tweak
            {
                Id = "NET-011",
                Name = "Disable Delivery Optimization",
                Description = "Disables the Windows Delivery Optimization service (DoSvc) which uses peer-to-peer bandwidth to distribute updates to other PCs.",
                Category = "Network",
                Type = TweakType.Service,
                Risk = TweakRisk.Low,
                ServiceName = "DoSvc",
                OptimizedStartType = "Disabled",
                DefaultStartType = "Automatic"
            },

            // 12. Disable LLMNR
            new Tweak
            {
                Id = "NET-012",
                Name = "Disable LLMNR",
                Description = "Disables Link-Local Multicast Name Resolution, which is a security risk that can be exploited for credential theft via MITM attacks.",
                Category = "Network",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient",
                RegistryValueName = "EnableMulticast",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 13. Disable NetBIOS over TCP/IP
            new Tweak
            {
                Id = "NET-013",
                Name = "Disable NetBIOS over TCP/IP",
                Description = "Disables NetBIOS over TCP/IP on all network adapters. NetBIOS is a legacy protocol that exposes the system to name-spoofing attacks.",
                Category = "Network",
                Type = TweakType.PowerShell,
                Risk = TweakRisk.Low,
                ApplyScript = "Get-ChildItem 'HKLM:\\SYSTEM\\CurrentControlSet\\Services\\NetBT\\Parameters\\Interfaces' | ForEach-Object { Set-ItemProperty -Path $_.PSPath -Name 'NetbiosOptions' -Value 2 -Type DWord -Force }",
                RevertScript = "Get-ChildItem 'HKLM:\\SYSTEM\\CurrentControlSet\\Services\\NetBT\\Parameters\\Interfaces' | ForEach-Object { Set-ItemProperty -Path $_.PSPath -Name 'NetbiosOptions' -Value 0 -Type DWord -Force }",
                DetectScript = "if ((Get-ChildItem 'HKLM:\\SYSTEM\\CurrentControlSet\\Services\\NetBT\\Parameters\\Interfaces' | Where-Object { (Get-ItemProperty $_.PSPath -Name 'NetbiosOptions' -ErrorAction SilentlyContinue).NetbiosOptions -eq 2 }).Count -gt 0) { 'True' } else { 'False' }"
            },

            // 14. Disable IPv6 on All Adapters
            new Tweak
            {
                Id = "NET-014",
                Name = "Disable IPv6 on All Adapters",
                Description = "Disables IPv6 on all network adapters. Can improve network performance on IPv4-only networks and reduces attack surface, but may break certain Windows features.",
                Category = "Network",
                Type = TweakType.PowerShell,
                Risk = TweakRisk.Medium,
                ApplyScript = "Get-NetAdapterBinding -ComponentID 'ms_tcpip6' -ErrorAction SilentlyContinue | Disable-NetAdapterBinding -ComponentID 'ms_tcpip6' -Confirm:$false",
                RevertScript = "Get-NetAdapterBinding -ComponentID 'ms_tcpip6' -ErrorAction SilentlyContinue | Enable-NetAdapterBinding -ComponentID 'ms_tcpip6' -Confirm:$false",
                DetectScript = "if ((Get-NetAdapterBinding -ComponentID 'ms_tcpip6' -ErrorAction SilentlyContinue | Where-Object { $_.Enabled -eq $false }).Count -gt 0) { 'True' } else { 'False' }"
            },

            // 15. Enable DNS over HTTPS (DoH)
            new Tweak
            {
                Id = "NET-015",
                Name = "Enable DNS over HTTPS",
                Description = "Enables automatic DNS over HTTPS (DoH) support in Windows, encrypting DNS queries to prevent eavesdropping and manipulation.",
                Category = "Network",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters",
                RegistryValueName = "EnableAutoDoh",
                OptimizedValue = 2,
                DefaultValue = 0,
                ValueKind = RegistryValueKind.DWord
            }
        };
    }
}
