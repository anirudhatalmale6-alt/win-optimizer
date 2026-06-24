@echo off
echo ========================================
echo  WinOptimizer Build Script
echo ========================================
echo.

:: Clean previous builds
if exist "publish" rmdir /s /q "publish"
mkdir publish

echo [1/4] Restoring packages...
dotnet restore WinOptimizer.sln
if errorlevel 1 goto :error

echo.
echo [2/4] Building WinOptimizer (main app)...
dotnet publish src\WinOptimizer\WinOptimizer.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false -p:IncludeNativeLibrariesForSelfExtract=true -o publish\WinOptimizer
if errorlevel 1 goto :error

echo.
echo [3/4] Building WinOptimizer Admin (license panel)...
dotnet publish src\WinOptimizer.Admin\WinOptimizer.Admin.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false -p:IncludeNativeLibrariesForSelfExtract=true -o publish\WinOptimizer.Admin
if errorlevel 1 goto :error

echo.
echo [4/4] Build complete!
echo.
echo Output:
echo   Main App:    publish\WinOptimizer\WinOptimizer.exe
echo   Admin Panel: publish\WinOptimizer.Admin\WinOptimizer.Admin.exe
echo.
echo Both executables are self-contained (no .NET runtime required).
echo The main app must be run as Administrator.
echo.
pause
exit /b 0

:error
echo.
echo BUILD FAILED! Check the errors above.
pause
exit /b 1
