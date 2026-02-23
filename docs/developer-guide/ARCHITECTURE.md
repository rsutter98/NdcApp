# Architecture

This document explains the high-level design of NdcApp, the decisions behind it, and how the pieces fit together.

## Overview

NdcApp is a **.NET MAUI** cross-platform application for planning conference schedules. It also ships a **Blazor Server preview** that mirrors the same functionality in a browser.

```
┌──────────────────────────────────┐   ┌──────────────────────────┐
│  NdcApp (MAUI UI)                │   │  NdcApp.Preview (Blazor)  │
│  Android · iOS · Windows         │   │  Browser / Docker         │
└──────────────┬───────────────────┘   └────────────┬─────────────┘
               │ depends on                          │ depends on
               ▼                                     ▼
      ┌──────────────────────────────────────────────────┐
      │               NdcApp.Core (Class Library)         │
      │  Models · Services · Interfaces                   │
      └──────────────────────────────────────────────────┘
               │ tested by
               ▼
      ┌──────────────────────────┐
      │  NdcApp.Tests (xUnit)    │
      └──────────────────────────┘
```

---

## Projects

| Project | Target Framework | Purpose |
|---|---|---|
| `NdcApp` | `net8.0-android`, `net8.0-ios`, `net8.0-windows10.0.19041.0` | Native MAUI UI application |
| `NdcApp.Core` | `net8.0` | Portable business logic and data models |
| `NdcApp.Preview` | `net8.0` | Blazor Server web preview |
| `NdcApp.Tests` | `net8.0` | xUnit test suite |

---

## Layered Design

```
┌────────────────────────────────────────────────────────────────┐
│  Presentation Layer                                             │
│  MAUI pages (XAML + code-behind) · Blazor Razor components     │
│  Value converters · Data binding                               │
├────────────────────────────────────────────────────────────────┤
│  Application / Service Layer  (NdcApp.Core/Services)           │
│  ConferencePlanService · TalkService · TalkFilterService        │
│  TalkRatingService · TalkNotificationService                   │
│  ErrorHandlingService · LoggerService                          │
├────────────────────────────────────────────────────────────────┤
│  Domain / Model Layer  (NdcApp.Core/Models)                    │
│  Talk · TalkRating · TalkRecommendation · NotificationRequest  │
├────────────────────────────────────────────────────────────────┤
│  Infrastructure Layer  (NdcApp/Services)                       │
│  LocalNotificationService (Plugin.LocalNotification)           │
│  GlobalExceptionHandler                                        │
└────────────────────────────────────────────────────────────────┘
```

**Key rule:** `NdcApp.Core` has **no dependency** on MAUI or any UI framework. This keeps all business logic portable and fully unit-testable without a device or emulator.

---

## Component Responsibilities

### Domain Models (`NdcApp.Core/Models`)

Plain C# records/classes with no behaviour beyond property access:

- **`Talk`** — All session metadata plus aggregated rating fields and a computed `Id`.
- **`TalkRating`** — A single star rating with an optional comment.
- **`TalkRecommendation`** — A `Talk` with a `Score` and a `Reason` string, produced by the recommendation engine.
- **`NotificationRequest`** — Describes a scheduled notification (time, title, body, linked talk).

### Service Layer (`NdcApp.Core/Services`)

All services are registered behind interfaces to support dependency injection and testability.

| Service | Responsibility |
|---|---|
| `ITalkService` | Load and parse CSV talk data. |
| `ITalkFilterService` | Case-insensitive full-text search across talk fields. |
| `ITalkRatingService` | Store ratings in memory; compute averages; generate recommendations. |
| `IConferencePlanService` | Maintain the user's selected talks, handle conflicts, serialise/deserialise, sort, and delegate to the rating service. |
| `INotificationService` | Platform-agnostic notification scheduling interface. |
| `TalkNotificationService` | Orchestrate multi-reminder scheduling (1 / 5 / 15 min) for selected talks. |
| `IErrorHandlingService` | Map exceptions to user-friendly messages and log them. |
| `ILoggerService` | Thin logging abstraction over `Microsoft.Extensions.Logging`. |

### Platform Services (`NdcApp/Services`)

| Service | Responsibility |
|---|---|
| `LocalNotificationService` | `INotificationService` implementation using [Plugin.LocalNotification](https://github.com/thudugala/Plugin.LocalNotification). |
| `GlobalExceptionHandler` | Registers `AppDomain.UnhandledException` and `TaskScheduler.UnobservedTaskException` hooks. |

### UI Layer (`NdcApp/`)

- **`MainPage`** — Displays the current time and the next selected talk loaded from `Preferences`.
- **`ConferencePlanPage`** — Main scheduling UI: search bar, filter buttons (All / My Talks), sort dropdown, notification toggle, and a scrollable list of `Talk` cards with star ratings.
- **Converters** — XAML value converters (`InverseBoolConverter`, `StarRatingConverter`, `SelectedConverters`) bridge model data to visual state without logic in code-behind.

---

## Data Flow

### Loading Talks

```
CSV file (Resources/Raw/ndc.csv)
  └─→ TalkService.LoadTalks()
        └─→ List<Talk>
              └─→ ConferencePlanPage (displayed in CollectionView)
```

### Selecting a Talk

```
User taps "Select Talk"
  └─→ ConferencePlanService.SelectTalk(talk)
        ├─→ selectedTalks["{day}|{startTime}"] = talk  (in-memory)
        └─→ ILoggerService.LogInfo(...)

On navigation away:
  └─→ ConferencePlanService.SerializeSelectedTalks()
        └─→ Preferences.Set("SelectedTalks", serialized)
```

### Scheduling Notifications

```
User enables notifications
  └─→ TalkNotificationService.RequestNotificationPermissionAsync()
  └─→ TalkNotificationService.ScheduleNotificationsForSelectedTalksAsync()
        ├─→ INotificationService.CancelAllNotificationsAsync()
        └─→ For each selected talk × {15, 5, 1} min:
              └─→ INotificationService.ScheduleNotificationAsync(NotificationRequest)
```

---

## Design Decisions

### Why a Separate `NdcApp.Core` Project?

MAUI applications cannot be unit tested without a running device or emulator. By extracting all business logic into a standalone `net8.0` class library, the entire service layer can be tested with a standard xUnit runner on any platform. The UI layer is kept thin and delegates to core services.

### Why Interfaces for Every Service?

Interfaces allow each service to be swapped for a test double (mock or stub) in unit tests. For example, `INotificationService` is replaced with a mock in `TalkNotificationServiceTests` to verify scheduling logic without triggering real OS notifications.

### Why Singletons?

Rating data and selected talks are held in in-memory dictionaries within the service instances. Using `AddSingleton` ensures that state accumulated during a session is consistent across pages without explicit state transfer.

### Time Slot Conflict Resolution

`ConferencePlanService` uses `"{Day}|{StartTime}"` as the dictionary key for selected talks. Selecting a new talk for an occupied slot silently replaces the previous one — a deliberate UX choice to make re-scheduling frictionless.

### Day Ordering

Sorting by day uses an explicit `Dictionary<string, int>` mapping day names to integers (Monday = 1 … Sunday = 7). This avoids relying on `DayOfWeek` enum ordering and handles conferences that start mid-week naturally.

---

## Preview Application

`NdcApp.Preview` is a Blazor Server app that exposes the same core business logic through a web UI. It:

- References `NdcApp.Core` directly (same models and services).
- Provides a `PreviewNotificationService` that replaces `LocalNotificationService` with an in-memory stub.
- Runs in Docker (`NdcApp.Preview/Dockerfile`) and is deployed via `docker-compose.preview.yml`.

This is useful for quick demonstrations and CI-based visual regression testing without requiring a device.

---

## See Also

- [API Reference](API.md) — Detailed method signatures and parameters
- [Testing](TESTING.md) — Test suite structure and coverage strategy
- [Build Guide](../operations/BUILD.md) — How to compile for each platform
