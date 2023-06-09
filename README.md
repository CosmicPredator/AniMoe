# AniMoe for AniList
 A Windows 10 & 11 Client for AniList. Track and Discover anime and manga with ease.

 [![WinUI3 Packaged MSIX App](https://github.com/CosmicPredator/AniMoe/actions/workflows/ci-packaged.yml/badge.svg)](https://github.com/CosmicPredator/AniMoe/actions/workflows/ci-packaged.yml)

> Note: This is a Work in Progress Application

# Build from Source
- Install Visual Studio 2022.
- Install ".NET Desktop Development" along with
Windows App SDK C# Templates.
- Clone the repository.
- Add WinUi3 Labs NuGet Repository.
	```
	https://pkgs.dev.azure.com/dotnet/CommunityToolkit/_packaging/CommunityToolkit-Labs/nuget/v3/index.json
	```
- Open the solution with Visual Studio
- Build it.

# NuGet Packages Used
- `CommunityToolkit.MVVM`
- `CommunityToolkit.WinUI.UI.Behaviours`
- `Microsoft.Extensions.Configuration.Abstractions`
- `Microsoft.Extensions.DependencyInjection`
- `Microsoft.Windows.SDK.BuildTools`
- `Microsoft.WindowsAppSDK`
- `Newtonsoft.Json`
- `CommunityToolkit.Labs.WinUI.SegmentedControl`
- `CommunityToolkit.Labs.WinUI.SettingsControls`
- `CommunityToolkit.Labs.WinUI.Shimmer`
- `CommunityToolkit.WinUI.UI.Controls`
- `CommunityToolkit.WinUI.UI.Media`
- `LiveChartsCore.SkiaSharpView.WinUI`
- `Markdig`
- `PInvoke.User32`
- `Serilog`
- `Serilog.Sinks.Debug`
- `Serilog.Sinks.File`
