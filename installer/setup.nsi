; WinOptimizer NSIS Installer Script
; Requires NSIS 3.x (https://nsis.sourceforge.io/)

!include "MUI2.nsh"

; General
Name "WinOptimizer"
OutFile "..\publish\WinOptimizer_Setup.exe"
InstallDir "$PROGRAMFILES64\WinOptimizer"
RequestExecutionLevel admin
Unicode True

; UI
!define MUI_ICON "..\src\WinOptimizer\Resources\app.ico"
!define MUI_ABORTWARNING
!define MUI_WELCOMEPAGE_TITLE "WinOptimizer Setup"
!define MUI_WELCOMEPAGE_TEXT "This wizard will install WinOptimizer on your computer.$\r$\n$\r$\nWinOptimizer is a Windows 10/11 optimization suite that helps improve performance, privacy, and visual customization.$\r$\n$\r$\nClick Next to continue."

; Pages
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

!insertmacro MUI_LANGUAGE "English"

; Install
Section "Install"
    SetOutPath "$INSTDIR"

    ; Main application
    File "..\publish\WinOptimizer\WinOptimizer.exe"

    ; Create shortcuts
    CreateDirectory "$SMPROGRAMS\WinOptimizer"
    CreateShortcut "$SMPROGRAMS\WinOptimizer\WinOptimizer.lnk" "$INSTDIR\WinOptimizer.exe"
    CreateShortcut "$DESKTOP\WinOptimizer.lnk" "$INSTDIR\WinOptimizer.exe"

    ; Uninstaller
    WriteUninstaller "$INSTDIR\Uninstall.exe"

    ; Registry for Add/Remove Programs
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WinOptimizer" "DisplayName" "WinOptimizer"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WinOptimizer" "UninstallString" '"$INSTDIR\Uninstall.exe"'
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WinOptimizer" "Publisher" "WinOptimizer"
    WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WinOptimizer" "DisplayVersion" "1.0.0"
    WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WinOptimizer" "NoModify" 1
    WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WinOptimizer" "NoRepair" 1
SectionEnd

; Uninstall
Section "Uninstall"
    Delete "$INSTDIR\WinOptimizer.exe"
    Delete "$INSTDIR\Uninstall.exe"
    RMDir "$INSTDIR"

    Delete "$SMPROGRAMS\WinOptimizer\WinOptimizer.lnk"
    RMDir "$SMPROGRAMS\WinOptimizer"
    Delete "$DESKTOP\WinOptimizer.lnk"

    DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\WinOptimizer"

    ; Remove app data (optional)
    RMDir /r "$LOCALAPPDATA\WinOptimizer"
SectionEnd
