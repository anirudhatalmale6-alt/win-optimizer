# WinOptimizer

Windows 10/11 optimization suite with 130+ tweaks across performance, privacy, network, gaming, visual, services, and debloat categories.

## Features
- Toggle individual tweaks on/off with instant apply/revert
- Dark themed UI with sidebar navigation
- HWID-locked licensing system with separate admin panel
- System restore point creation before changes
- Detailed activity logging
- Self-contained single-file executables (no runtime required)

## Build
Requires .NET 8 SDK on Windows.

```
dotnet restore
publish.bat
```

Output in `publish/` folder:
- `WinOptimizer.exe` - Main optimization tool (run as admin)
- `WinOptimizer.Admin.exe` - License administration panel

## Architecture
- `WinOptimizer.Core` - Shared licensing/crypto library
- `WinOptimizer` - Main WPF application
- `WinOptimizer.Admin` - License management WPF app

## License
Proprietary. All rights reserved.
