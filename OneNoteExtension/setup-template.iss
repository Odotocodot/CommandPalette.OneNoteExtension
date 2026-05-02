; TEMPLATE: Inno Setup Script for Command Palette Extensions
;
; To use this template for a new extension:
; 1. Copy this file to your extension's project folder as "setup-template.iss"
; 2. Replace EXTENSION_NAME with your extension name (e.g., CmdPalMyExtension)
; 3. Replace DISPLAY_NAME with your extension's display name (e.g., My Extension)
; 4. Replace DEVELOPER_NAME with your name (e.g., Your Name Here)
; 5. Replace CLSID-HERE with extensions CLSID
; 6. Update the default version to match your project file

#define AppVersion "0.0.1.0"

[Setup]
AppId={{2586d13f-78bd-4923-bffe-c61d6e0d2263}}
AppName=OneNote for Command Palette
AppVersion={#AppVersion}
AppPublisher=Odotocodot
DefaultDirName={autopf}\OneNoteExtension
OutputDir=bin\Release\installer
OutputBaseFilename=OneNoteExtension-Setup-{#AppVersion}
Compression=lzma
SolidCompression=yes
MinVersion=10.0.19041

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "bin\Release\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\OneNote for Command Palette"; Filename: "{app}\OneNoteExtension.exe"

[Registry]
Root: HKCU; Subkey: "SOFTWARE\Classes\CLSID\{{2586d13f-78bd-4923-bffe-c61d6e0d2263}}"; ValueData: "OneNoteExtension"
Root: HKCU; Subkey: "SOFTWARE\Classes\CLSID\{{2586d13f-78bd-4923-bffe-c61d6e0d2263}}\LocalServer32"; ValueData: "{app}\OneNoteExtension.exe -RegisterProcessAsComServer"