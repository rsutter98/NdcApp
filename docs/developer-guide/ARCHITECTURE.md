# Architecture Overview

This document explains how NdcApp is structured, why that structure was chosen, and how the layers interact.

---

## 🏗️ High-Level Structure

NdcApp follows a **layered architecture** that separates concerns across three projects:

```
NdcApp.sln
├── NdcApp/            — .NET MAUI UI layer (views, view-models, platform code)
├── NdcApp.Core/       — Platform-agnostic business logic (models, services)
├── NdcApp.Preview/    — Blazor Server preview app (browser-based UI for development)
└── NdcApp.Tests/      — xUnit test suite
```

### Dependency graph

```
NdcApp  ──depends on──▶  NdcApp.Core
NdcApp.Preview  ───────▶  NdcApp.Core
NdcApp.Tests  ──────────▶  NdcApp.Core
```

`NdcApp.Core` has **no dependency** on any UI or platform framework. This makes it independently testable and reusable.

---

## 📁 Project Breakdown

### NdcApp.Core

Contains all business logic, domain models, and service contracts.

```
NdcApp.Core/
├── Models/
│   ├── Talk.cs                    — Core domain model
│   ├── TalkRating.cs              — Rating value object
│   ├── TalkRecommendation.cs      — Recommendation result
│   └── NotificationRequest.cs     — Notification payload
└── Services/
    ├── IConferencePlanService.cs  — Schedule management contract
    ├── ConferencePlanService.cs   — Schedule management implementation
    ├── ITalkService.cs            — CSV loading contract
    ├── TalkService.cs             — CSV loading implementation
    ├── ITalkFilterService.cs      — Search/filter contract
    ├── TalkFilterService.cs       — Search/filter implementation
    ├── ITalkRatingService.cs      — Rating contract
    ├── TalkRatingService.cs       — Rating implementation
    ├── INotificationService.cs    — Notification scheduling contract
    ├── TalkNotificationService.cs — Orchestrates talk reminders
    ├── ILoggerService.cs          — Logging contract
    ├── LoggerService.cs           — Logging implementation
    ├── IErrorHandlingService.cs   — Error handling contract
    └── ErrorHandlingService.cs    — Error handling implementation
```

### NdcApp (MAUI app)

The .NET MAUI project that produces runnable apps for Windows, Android, and iOS.

```
NdcApp/
├── App.xaml / App.xaml.cs         — Application entry point
├── AppShell.xaml                  — Navigation shell
├── MainPage.xaml / .cs            — Landing page (next talk, navigation)
├── ConferencePlanPage.xaml / .cs  — Main scheduling view
├── MauiProgram.cs                 — Dependency injection setup
├── Converters/                    — IValueConverter implementations
├── Services/                      — MAUI-specific service implementations
│   ├── LocalNotificationService.cs
│   ├── TalkNotificationService.cs (re-uses Core service)
│   └── GlobalExceptionHandler.cs
├── Platforms/                     — Per-platform entry points & manifests
│   ├── Android/
│   ├── iOS/
│   ├── MacCatalyst/
│   ├── Windows/
│   └── Tizen/
└── Resources/
    ├── Raw/ndc.csv                — Bundled conference data
    └── Styles/                    — XAML resource dictionaries
```

### NdcApp.Preview (Blazor)

A Blazor Server application that renders the conference plan in a browser. Used for visual development and demonstration without requiring a physical device or emulator.

```
NdcApp.Preview/
├── Pages/
│   ├── Index.razor               — Preview landing page
│   └── ConferencePlan.razor      — Full plan view in Blazor
└── Services/
    └── PreviewNotificationService.cs  — In-memory stub for notifications
```

### NdcApp.Tests

xUnit test project covering unit, integration, and performance tests. All tests target `NdcApp.Core` directly.

---

## 🔄 Data Flow

```
ndc.csv (bundled resource)
    │
    ▼
TalkService.LoadTalks()
    │  parses CSV → List<Talk>
    ▼
ConferencePlanPage / Blazor view
    │  user interactions
    ▼
IConferencePlanService   ◄───►  ITalkRatingService
    │  persists via Preferences
    ▼
TalkNotificationService
    │  schedules reminders
    ▼
INotificationService (LocalNotificationService)
    │
    ▼
OS notification system
```

---

## 💉 Dependency Injection

Services are registered in `NdcApp/MauiProgram.cs` as singletons:

| Interface | Implementation | Scope |
|---|---|---|
| `IConferencePlanService` | `ConferencePlanService` | Singleton |
| `ITalkService` | `TalkService` | Singleton |
| `ITalkFilterService` | `TalkFilterService` | Singleton |
| `ITalkRatingService` | `TalkRatingService` | Singleton |
| `INotificationService` | `LocalNotificationService` | Singleton |
| `ILoggerService` | `LoggerService` | Singleton |
| `IErrorHandlingService` | `ErrorHandlingService` | Singleton |
| `TalkNotificationService` | — | Singleton |
| `GlobalExceptionHandler` | — | Singleton |

In tests, interfaces are replaced with mocks (see `NdcApp.Tests/Mocks/MockLoggerService.cs`).

In the Preview project, `PreviewNotificationService` implements `INotificationService` as an in-memory stub.

---

## 🧱 Key Design Decisions

### Interface-first services
Every service exposes a contract interface. This enforces loose coupling, enables mock-based testing, and allows platform-specific implementations (e.g., `INotificationService`).

### Singleton scope
All services are singletons because the app's state (selected talks, ratings) must be consistent across pages for the lifetime of a single app session.

### NdcApp.Core is framework-free
The core library targets `net8.0` without MAUI or Blazor references. This means tests run on any .NET-capable platform without an emulator or device.

### Preferences for persistence
User selections and ratings are persisted using `Microsoft.Maui.Storage.Preferences` (key-value store backed by platform-native storage). No database or file I/O is required.

---

## 📐 Supported Platforms

| Platform | Target Framework | Entry Point |
|---|---|---|
| Windows | `net8.0-windows10.0.19041.0` | `NdcApp/Platforms/Windows/App.xaml.cs` |
| Android | `net8.0-android` | `NdcApp/Platforms/Android/MainActivity.cs` |
| iOS | `net8.0-ios` | `NdcApp/Platforms/iOS/Program.cs` |
| macOS (Catalyst) | `net8.0-maccatalyst` | `NdcApp/Platforms/MacCatalyst/Program.cs` |
| Tizen | `net8.0-tizen` | `NdcApp/Platforms/Tizen/Main.cs` |

---

## 🔗 See Also

- [API Reference](API.md)
- [Developer Guide](README.md)
- [Testing Guide](TESTING.md)
