# WinOptimizer User Guide

## Requirements
- Windows 10 or Windows 11 (x64)
- Administrator privileges

## Installation

### Option A: Installer
Run `WinOptimizer_Setup.exe` and follow the wizard. It creates a Start Menu shortcut and desktop icon.

### Option B: Portable
Copy `WinOptimizer.exe` to any folder and run it directly. No installation needed.

## First Launch

1. Right-click `WinOptimizer.exe` and select **Run as administrator**
2. Enter your license key when prompted (provided by your admin)
3. The app will scan your system and show the current status of all tweaks

## Using the App

### Navigation
The left sidebar shows tweak categories:
- **Performance** - Memory, CPU, disk, and boot optimizations
- **Privacy** - Telemetry, tracking, and data collection controls
- **Network** - TCP/IP, DNS, and connection optimizations
- **Gaming** - GPU, input latency, and game-specific tweaks
- **Visual** - UI effects, theme, and Explorer customizations
- **Services** - Disable unnecessary Windows services
- **Debloat** - Remove pre-installed bloatware apps

### Applying Tweaks
1. Select a category from the sidebar
2. Toggle ON the tweaks you want to apply
3. Click **Apply Selected** to apply only the toggled tweaks
4. Each tweak shows its status: APPLIED, NOT APPLIED, or ERROR

### Reverting Tweaks
1. Toggle ON the tweaks you want to revert
2. Click **Revert Selected** to restore original Windows defaults

### Quick Actions
- **Select All / Deselect All** - Toggle all tweaks in the current category
- **Create Restore Point** - Creates a Windows System Restore point before changes

### Risk Levels
- **LOW** (green) - Safe tweaks with no side effects
- **MED** (yellow) - May affect specific features if you use them
- **HIGH** (red) - Can impact system behavior; apply with caution

### Activity Log
Click **View Log** in the sidebar to see a detailed log of all operations.

## Recommendations

1. **Create a restore point** before applying tweaks for the first time
2. **Start with LOW risk** tweaks and test for a day before applying higher risk ones
3. **Don't disable services** you're unsure about - read the description first
4. **Debloat carefully** - removed apps cannot be easily reinstalled

## License Administration

The separate `WinOptimizer.Admin.exe` app manages licenses:

1. Get the user's **Hardware ID** (shown in the activation dialog of WinOptimizer)
2. Paste it into the Admin panel's HWID field
3. Enter a name/label for the license
4. Optionally set an expiry period
5. Click **Generate Key** - the key is copied to your clipboard
6. Send the key to the user

### Managing Licenses
- **Revoke** - Immediately invalidates a license
- **Renew** - Generates a new key with extended expiry
- **Delete** - Permanently removes the license record
- **Copy Key** - Copies the full license key to clipboard

## Troubleshooting

**"Administrator Required" error**
Right-click the exe and select "Run as administrator", or create a shortcut and set it to always run as admin.

**"Invalid license key or hardware mismatch"**
The key was generated for a different machine. Get the correct HWID from the activation dialog and generate a new key.

**A tweak shows ERROR status**
The tweak could not be applied, usually due to Windows edition restrictions (some tweaks require Pro/Enterprise). Check the Activity Log for details.

**Windows Update broke after tweaks**
Use the Windows System Restore point you created before applying tweaks. Or open WinOptimizer and revert the Services category tweaks.

## Uninstalling
Run the uninstaller from Add/Remove Programs, or delete the exe. App data is stored in `%LOCALAPPDATA%\WinOptimizer` and can be manually deleted.
