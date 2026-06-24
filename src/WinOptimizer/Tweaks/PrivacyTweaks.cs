using Microsoft.Win32;
using WinOptimizer.Models;

namespace WinOptimizer.Tweaks;

public static class PrivacyTweaks
{
    public static List<Tweak> GetTweaks()
    {
        return new List<Tweak>
        {
            // 1. Disable Telemetry
            new Tweak
            {
                Id = "privacy-disable-telemetry",
                Name = "Disable Telemetry",
                Description = "Prevents Windows from sending diagnostic and usage data to Microsoft. Sets telemetry level to zero (Security only).",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection",
                RegistryValueName = "AllowTelemetry",
                OptimizedValue = 0,
                DefaultValue = 3,
                ValueKind = RegistryValueKind.DWord
            },

            // 2. Disable DiagTrack Service
            new Tweak
            {
                Id = "privacy-disable-diagtrack",
                Name = "Disable DiagTrack Service",
                Description = "Disables the Connected User Experiences and Telemetry service that collects and transmits diagnostic data.",
                Category = "Privacy",
                Type = TweakType.Service,
                Risk = TweakRisk.Low,
                ServiceName = "DiagTrack",
                OptimizedStartType = "Disabled",
                DefaultStartType = "Automatic"
            },

            // 3. Disable dmwappushservice
            new Tweak
            {
                Id = "privacy-disable-dmwappushservice",
                Name = "Disable dmwappushservice",
                Description = "Disables the Device Management WAP Push message routing service used for telemetry data routing.",
                Category = "Privacy",
                Type = TweakType.Service,
                Risk = TweakRisk.Low,
                ServiceName = "dmwappushservice",
                OptimizedStartType = "Disabled",
                DefaultStartType = "Automatic"
            },

            // 4. Disable Advertising ID
            new Tweak
            {
                Id = "privacy-disable-advertising-id",
                Name = "Disable Advertising ID",
                Description = "Prevents apps from using your unique advertising ID for cross-app ad targeting and tracking.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKCU\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo",
                RegistryValueName = "Enabled",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 5. Disable Activity History
            new Tweak
            {
                Id = "privacy-disable-activity-history",
                Name = "Disable Activity History",
                Description = "Stops Windows from collecting your activity history including apps used, files opened, and websites browsed.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\System",
                RegistryValueName = "EnableActivityFeed",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 6. Disable Activity History Upload
            new Tweak
            {
                Id = "privacy-disable-activity-history-upload",
                Name = "Disable Activity History Upload",
                Description = "Prevents Windows from uploading your activity history to Microsoft cloud servers.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\System",
                RegistryValueName = "UploadUserActivities",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 7. Disable Location Tracking
            new Tweak
            {
                Id = "privacy-disable-location-tracking",
                Name = "Disable Location Tracking",
                Description = "Disables the Windows location service, preventing apps and system services from accessing your physical location.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors",
                RegistryValueName = "DisableLocation",
                OptimizedValue = 1,
                DefaultValue = 0,
                ValueKind = RegistryValueKind.DWord
            },

            // 8. Disable Cortana
            new Tweak
            {
                Id = "privacy-disable-cortana",
                Name = "Disable Cortana",
                Description = "Disables the Cortana virtual assistant to prevent voice data collection and cloud-based query processing.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search",
                RegistryValueName = "AllowCortana",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 9. Disable Web Search in Start
            new Tweak
            {
                Id = "privacy-disable-web-search",
                Name = "Disable Web Search in Start Menu",
                Description = "Prevents the Start menu search from sending queries to Bing and displaying web results.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search",
                RegistryValueName = "DisableWebSearch",
                OptimizedValue = 1,
                DefaultValue = 0,
                ValueKind = RegistryValueKind.DWord
            },

            // 10. Disable Connected Search Suggestions
            new Tweak
            {
                Id = "privacy-disable-connected-search",
                Name = "Disable Connected Search Suggestions",
                Description = "Stops Windows Search from using the internet to provide web-based suggestions and results.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search",
                RegistryValueName = "ConnectedSearchUseWeb",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 11. Disable Clipboard History
            new Tweak
            {
                Id = "privacy-disable-clipboard-history",
                Name = "Disable Clipboard History",
                Description = "Prevents Windows from storing a history of items you have copied to the clipboard.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\System",
                RegistryValueName = "AllowClipboardHistory",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 12. Disable Cloud Clipboard
            new Tweak
            {
                Id = "privacy-disable-cloud-clipboard",
                Name = "Disable Cloud Clipboard Sync",
                Description = "Prevents clipboard contents from being synced across devices through your Microsoft account.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Policies\Microsoft\Windows\System",
                RegistryValueName = "AllowCrossDeviceClipboard",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 13. Disable Feedback Frequency
            new Tweak
            {
                Id = "privacy-disable-feedback",
                Name = "Disable Feedback Frequency",
                Description = "Prevents Windows from periodically asking for feedback through the Feedback Hub app.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKCU\SOFTWARE\Microsoft\Siuf\Rules",
                RegistryValueName = "NumberOfSIUFInPeriod",
                OptimizedValue = 0,
                DefaultValue = null,
                ValueKind = RegistryValueKind.DWord
            },

            // 14. Disable CEIP
            new Tweak
            {
                Id = "privacy-disable-ceip",
                Name = "Disable Customer Experience Improvement Program",
                Description = "Opts out of the Customer Experience Improvement Program that collects hardware and software usage data.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Microsoft\SQMClient\Windows",
                RegistryValueName = "CEIPEnable",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 15. Disable App Launch Tracking
            new Tweak
            {
                Id = "privacy-disable-app-launch-tracking",
                Name = "Disable App Launch Tracking",
                Description = "Stops Windows from tracking which apps you launch to personalize the Start menu.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                RegistryValueName = "Start_TrackProgs",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 16. Disable Tailored Experiences
            new Tweak
            {
                Id = "privacy-disable-tailored-experiences",
                Name = "Disable Tailored Experiences",
                Description = "Prevents Microsoft from using your diagnostic data to provide personalized tips, ads, and recommendations.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Privacy",
                RegistryValueName = "TailoredExperiencesWithDiagnosticDataEnabled",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 17. Disable Inking Personalization
            new Tweak
            {
                Id = "privacy-disable-inking-personalization",
                Name = "Disable Inking and Typing Personalization",
                Description = "Prevents Windows from collecting inking and typing data to improve language recognition for you.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKCU\Software\Microsoft\InputPersonalization",
                RegistryValueName = "RestrictImplicitInkCollection",
                OptimizedValue = 1,
                DefaultValue = 0,
                ValueKind = RegistryValueKind.DWord
            },

            // 18. Disable Text Suggestions (Harvest Contacts)
            new Tweak
            {
                Id = "privacy-disable-text-suggestions",
                Name = "Disable Text Input Personalization",
                Description = "Stops Windows from harvesting your contacts and calendar data to improve text suggestions.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKCU\Software\Microsoft\InputPersonalization\TrainedDataStore",
                RegistryValueName = "HarvestContacts",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 19. Disable Online Speech Recognition
            new Tweak
            {
                Id = "privacy-disable-speech-recognition",
                Name = "Disable Online Speech Recognition",
                Description = "Disables cloud-based speech recognition that sends voice data to Microsoft servers for processing.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKCU\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy",
                RegistryValueName = "HasAccepted",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 20. Disable Camera App Access
            new Tweak
            {
                Id = "privacy-disable-camera-access",
                Name = "Disable Camera App Access",
                Description = "Blocks apps from accessing the camera. Individual app permissions can still be managed in Settings.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Medium,
                RegistryPath = @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\webcam",
                RegistryValueName = "Value",
                OptimizedValue = "Deny",
                DefaultValue = "Allow",
                ValueKind = RegistryValueKind.String
            },

            // 21. Disable Microphone App Access
            new Tweak
            {
                Id = "privacy-disable-microphone-access",
                Name = "Disable Microphone App Access",
                Description = "Blocks apps from accessing the microphone. Individual app permissions can still be managed in Settings.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Medium,
                RegistryPath = @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\microphone",
                RegistryValueName = "Value",
                OptimizedValue = "Deny",
                DefaultValue = "Allow",
                ValueKind = RegistryValueKind.String
            },

            // 22. Disable Notification Access
            new Tweak
            {
                Id = "privacy-disable-notification-access",
                Name = "Disable Notification Access",
                Description = "Prevents apps from reading your notifications, which may contain sensitive information.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userNotificationListener",
                RegistryValueName = "Value",
                OptimizedValue = "Deny",
                DefaultValue = "Allow",
                ValueKind = RegistryValueKind.String
            },

            // 23. Disable Account Info Access
            new Tweak
            {
                Id = "privacy-disable-account-info-access",
                Name = "Disable Account Info Access",
                Description = "Prevents apps from accessing your account name, picture, and other Microsoft account information.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userAccountInformation",
                RegistryValueName = "Value",
                OptimizedValue = "Deny",
                DefaultValue = "Allow",
                ValueKind = RegistryValueKind.String
            },

            // 24. Disable Background Apps
            new Tweak
            {
                Id = "privacy-disable-background-apps",
                Name = "Disable Background Apps",
                Description = "Prevents apps from running in the background, reducing data collection and improving performance.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKCU\Software\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications",
                RegistryValueName = "GlobalUserDisabled",
                OptimizedValue = 1,
                DefaultValue = 0,
                ValueKind = RegistryValueKind.DWord
            },

            // 25. Disable Suggestions in Start Menu
            new Tweak
            {
                Id = "privacy-disable-start-suggestions",
                Name = "Disable Suggestions in Start Menu",
                Description = "Removes suggested apps and content from the Start menu that Microsoft promotes based on your usage.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                RegistryValueName = "SubscribedContent-338388Enabled",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 26. Disable Tips and Tricks Notifications
            new Tweak
            {
                Id = "privacy-disable-tips-notifications",
                Name = "Disable Tips and Tricks Notifications",
                Description = "Stops Windows from showing tips, tricks, and suggestion notifications that track your usage patterns.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                RegistryValueName = "SubscribedContent-338389Enabled",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 27. Disable Welcome Experience
            new Tweak
            {
                Id = "privacy-disable-welcome-experience",
                Name = "Disable Windows Welcome Experience",
                Description = "Disables the post-update Welcome Experience page that promotes Microsoft apps and services.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                RegistryValueName = "SubscribedContent-310093Enabled",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 28. Disable Suggested Apps (OEM Pre-installed)
            new Tweak
            {
                Id = "privacy-disable-suggested-apps",
                Name = "Disable Suggested and Pre-installed Apps",
                Description = "Prevents Windows from automatically installing suggested apps and OEM-promoted software.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager",
                RegistryValueName = "OemPreInstalledAppsEnabled",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 29. Disable WiFi Sense HotSpot Sharing
            new Tweak
            {
                Id = "privacy-disable-wifi-sense",
                Name = "Disable WiFi Sense HotSpot Sharing",
                Description = "Prevents Windows from automatically connecting to suggested open hotspots and shared networks.",
                Category = "Privacy",
                Type = TweakType.Registry,
                Risk = TweakRisk.Low,
                RegistryPath = @"HKLM\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config",
                RegistryValueName = "AutoConnectAllowedOEM",
                OptimizedValue = 0,
                DefaultValue = 1,
                ValueKind = RegistryValueKind.DWord
            },

            // 30. Disable Telemetry Scheduled Tasks
            new Tweak
            {
                Id = "privacy-disable-telemetry-tasks",
                Name = "Disable Telemetry Scheduled Tasks",
                Description = "Disables the Microsoft Compatibility Appraiser scheduled task that collects telemetry data about installed software.",
                Category = "Privacy",
                Type = TweakType.ScheduledTask,
                Risk = TweakRisk.Low,
                TaskPath = @"\Microsoft\Windows\Application Experience\Microsoft Compatibility Appraiser",
                TaskShouldBeEnabled = false
            }
        };
    }
}
