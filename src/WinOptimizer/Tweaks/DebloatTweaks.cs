using System.Collections.Generic;
using WinOptimizer.Models;

namespace WinOptimizer.Tweaks
{
    public static class DebloatTweaks
    {
        public static List<Tweak> GetTweaks()
        {
            return new List<Tweak>
            {
                // 1. 3D Viewer
                new Tweak
                {
                    Id = "debloat-3d-viewer",
                    Name = "Remove 3D Viewer",
                    Description = "Removes the 3D Viewer app used for viewing 3D models. Rarely used by most users.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    PackageName = "Microsoft.Microsoft3DViewer"
                },

                // 2. Get Help
                new Tweak
                {
                    Id = "debloat-get-help",
                    Name = "Remove Get Help",
                    Description = "Removes the Get Help app that connects to Microsoft support. Online help is still available through the browser.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    PackageName = "Microsoft.GetHelp"
                },

                // 3. Mixed Reality Portal
                new Tweak
                {
                    Id = "debloat-mixed-reality",
                    Name = "Remove Mixed Reality Portal",
                    Description = "Removes the Mixed Reality Portal app used for Windows Mixed Reality headsets.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    PackageName = "Microsoft.MixedReality.Portal"
                },

                // 4. Feedback Hub
                new Tweak
                {
                    Id = "debloat-feedback-hub",
                    Name = "Remove Feedback Hub",
                    Description = "Removes the Feedback Hub app used to send feedback and bug reports to Microsoft.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    PackageName = "Microsoft.WindowsFeedbackHub"
                },

                // 5. Maps
                new Tweak
                {
                    Id = "debloat-maps",
                    Name = "Remove Windows Maps",
                    Description = "Removes the built-in Windows Maps application. Most users prefer Google Maps or other web-based alternatives.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    PackageName = "Microsoft.WindowsMaps"
                },

                // 6. Movies & TV
                new Tweak
                {
                    Id = "debloat-movies-tv",
                    Name = "Remove Movies & TV",
                    Description = "Removes the Movies & TV (Zune Video) app. Media files can still be played with other players like VLC.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    PackageName = "Microsoft.ZuneVideo"
                },

                // 7. Groove Music
                new Tweak
                {
                    Id = "debloat-groove-music",
                    Name = "Remove Groove Music",
                    Description = "Removes the Groove Music app. Music files can still be played with other players.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    PackageName = "Microsoft.ZuneMusic"
                },

                // 8. News
                new Tweak
                {
                    Id = "debloat-news",
                    Name = "Remove Bing News",
                    Description = "Removes the Microsoft News (Bing News) app from the system.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    PackageName = "Microsoft.BingNews"
                },

                // 9. Weather
                new Tweak
                {
                    Id = "debloat-weather",
                    Name = "Remove Bing Weather",
                    Description = "Removes the Microsoft Weather (Bing Weather) app. Weather can still be checked via the browser.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    PackageName = "Microsoft.BingWeather"
                },

                // 10. People
                new Tweak
                {
                    Id = "debloat-people",
                    Name = "Remove People",
                    Description = "Removes the People contacts app. Contact management can be done through Outlook or other apps.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    PackageName = "Microsoft.People"
                },

                // 11. Sticky Notes
                new Tweak
                {
                    Id = "debloat-sticky-notes",
                    Name = "Remove Sticky Notes",
                    Description = "Removes the Sticky Notes app. Consider keeping this if you actively use desktop sticky notes.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    PackageName = "Microsoft.MicrosoftStickyNotes"
                },

                // 12. Tips
                new Tweak
                {
                    Id = "debloat-tips",
                    Name = "Remove Tips",
                    Description = "Removes the Tips (Get Started) app that shows Windows tips and tutorials.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    PackageName = "Microsoft.Getstarted"
                },

                // 13. Voice Recorder
                new Tweak
                {
                    Id = "debloat-voice-recorder",
                    Name = "Remove Voice Recorder",
                    Description = "Removes the Windows Voice Recorder (Sound Recorder) app.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    PackageName = "Microsoft.WindowsSoundRecorder"
                },

                // 14. Paint 3D (Win10 only)
                new Tweak
                {
                    Id = "debloat-paint-3d",
                    Name = "Remove Paint 3D",
                    Description = "Removes the Paint 3D app. Classic Paint (mspaint.exe) remains available. Only present on Windows 10.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    SupportsWindows10 = true,
                    SupportsWindows11 = false,
                    PackageName = "Microsoft.MSPaint"
                },

                // 15. Solitaire Collection
                new Tweak
                {
                    Id = "debloat-solitaire",
                    Name = "Remove Solitaire Collection",
                    Description = "Removes the Microsoft Solitaire Collection including Klondike, Spider, FreeCell, and other card games.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    PackageName = "Microsoft.MicrosoftSolitaireCollection"
                },

                // 16. Office Hub
                new Tweak
                {
                    Id = "debloat-office-hub",
                    Name = "Remove Office Hub",
                    Description = "Removes the My Office Hub app that promotes Office 365 subscriptions. Does not affect installed Office apps.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    PackageName = "Microsoft.MicrosoftOfficeHub"
                },

                // 17. Skype
                new Tweak
                {
                    Id = "debloat-skype",
                    Name = "Remove Skype",
                    Description = "Removes the built-in Skype app. The desktop version of Skype can still be installed separately if needed.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    PackageName = "Microsoft.SkypeApp"
                },

                // 18. Your Phone
                new Tweak
                {
                    Id = "debloat-your-phone",
                    Name = "Remove Your Phone",
                    Description = "Removes the Your Phone (Phone Link) app that connects your Android or iOS device to Windows.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    PackageName = "Microsoft.YourPhone"
                },

                // 19. OneNote
                new Tweak
                {
                    Id = "debloat-onenote",
                    Name = "Remove OneNote",
                    Description = "Removes the built-in OneNote UWP app. The full desktop version of OneNote (via Office) is not affected.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    PackageName = "Microsoft.Office.OneNote"
                },

                // 20. Clipchamp (Win11 only)
                new Tweak
                {
                    Id = "debloat-clipchamp",
                    Name = "Remove Clipchamp",
                    Description = "Removes Clipchamp, the video editor bundled with Windows 11. Other video editors can be used instead.",
                    Category = "Debloat",
                    Type = TweakType.AppRemoval,
                    Risk = TweakRisk.Low,
                    SupportsWindows10 = false,
                    SupportsWindows11 = true,
                    PackageName = "Clipchamp.Clipchamp"
                }
            };
        }
    }
}
