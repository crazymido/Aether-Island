# Aether Island — Windows app

A new native Windows desktop companion, implemented independently in C# and WPF on .NET 10 LTS.

## First shell
- Floating, always-on-top capsule
- Original Aether Island visual identity
- Expand/collapse and drag positioning
- Live clock and date
- 25-minute focus countdown
- Quick launch for Downloads and Notepad
- Single-instance guard
- Per-user JSON settings
- Windows CI build

Media controls, notifications, tray behavior, configurable shortcuts, settings UI, startup integration and installer are future milestones, not implemented yet.

## Requirements
- Windows 10/11 x64
- .NET 10 SDK
- Visual Studio with .NET desktop development, or VS Code + C# Dev Kit

## Run
From this folder:
`dotnet run --project src/AetherIsland.App/AetherIsland.App.csproj`

## Build
`dotnet restore AetherIsland.sln`
`dotnet build AetherIsland.sln --configuration Release`

Publish self-contained x64:
`dotnet publish src/AetherIsland.App/AetherIsland.App.csproj -c Release -r win-x64 --self-contained true -o artifacts/publish/win-x64`

Preferences are stored in the current Windows user's AppData under Aether Island. The first shell makes no network requests and collects no telemetry.
