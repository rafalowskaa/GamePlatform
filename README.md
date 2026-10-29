# GamePlatform

A Windows desktop game platform with a WPF interface, local accounts, game management and social features.

**C# · .NET 8 · WPF · MVVM · Entity Framework Core · SQLite · WebView2**

[Selected projects](https://github.com/rafalowskaa/BrowserGamesPlatform/blob/main/PORTFOLIO.md)

## Features represented in the source

- Account registration and login, user profiles and avatars.
- A game catalogue with upload and administrator review interfaces.
- Friends, chat, notifications and user reports.
- Scores, ranks and statistics views.
- Polish and English localisation.
- An optional remembered login name; passwords and sessions are not saved.
- Embedded game presentation using WebView2.

## Structure

| Project | Responsibility |
| --- | --- |
| `GamePlatform.Desktop` | WPF views, view models, services and resources |
| `GamePlatform.Shared` | Models, validation and EF Core persistence |

## Local setup

Requirements: Windows, .NET 8 SDK and Microsoft Edge WebView2 Runtime.

```powershell
git clone https://github.com/rafalowskaa/GamePlatform.git
cd GamePlatform
dotnet restore GamePlatform.sln
dotnet build GamePlatform.sln --configuration Release --no-restore
cd GamePlatform.Desktop
dotnet run --project GamePlatform.Desktop.csproj
```

The app uses `Data Source=games.db`, relative to its working directory. Launching from the desktop project directory matches the location of the checked-in database.

## Current limitations

This is a portfolio application under development. A local SQLite database is included in the repository; it should be reviewed before redistribution. Startup runs EF migrations and creates a development administrator when no users exist. That behaviour is intended for a local demo and needs review before any wider use.

The repository does not contain a dedicated automated test project.

A separate web implementation is available in [BrowserGamesPlatform](https://github.com/rafalowskaa/BrowserGamesPlatform).

## Licence

Original project source code is licensed under [MIT](LICENSE). Third-party libraries and bundled media remain subject to their own rights and licences.
