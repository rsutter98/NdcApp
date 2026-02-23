# Changelog

All notable changes to NdcApp are documented here.

This project adheres to [Semantic Versioning](https://semver.org/) and the format follows [Keep a Changelog](https://keepachangelog.com/).

---

## [Unreleased]

> Changes merged to `main` but not yet tagged as a release.

---

## [1.0.0] — 2026-02-23

Initial public release. All core functionality established from the ground up.

### Added

#### Architecture
- `NdcApp.Core` portable class library separating business logic from MAUI UI
- `NdcApp.Preview` Blazor Server web application for browser-based demonstration
- `NdcApp.Tests` xUnit test suite with 92 tests across 15 test files
- Clean dependency-injection architecture via `MauiProgram.CreateMauiApp()`

#### Data Models
- `Talk` model with `Day`, `StartTime`, `EndTime`, `Room`, `Title`, `Speaker`, `Category`, `AverageRating`, `RatingCount`, and computed `Id`
- `TalkRating` model (1–5 stars, optional comment, `UserId`, `RatingDate`)
- `TalkRecommendation` model with `Score` (0.0–1.0) and `Reason` string
- `NotificationRequest` model with `NotificationType` enum (`TalkReminder`, `TalkStarting`, `ScheduleUpdate`)

#### Services
- `TalkService` — CSV file and string parsing for talk data
- `TalkFilterService` — case-insensitive full-text search across all talk fields
- `TalkRatingService` — in-memory rating storage, averages, and recommendation engine (weighted scoring: rating 40 %, popularity 20 %, category trend 20 %, recency 20 %)
- `ConferencePlanService` — talk selection with time-slot conflict resolution, serialisation/deserialisation via `Preferences`, four sort modes (standard, speaker, category, rating), next-talk lookup
- `TalkNotificationService` — orchestrates multi-reminder scheduling (15 min / 5 min / 1 min before each selected talk)
- `ErrorHandlingService` — maps exception types to user-friendly messages
- `LoggerService` — `ILoggerService` implementation wrapping `Microsoft.Extensions.Logging`
- `LocalNotificationService` — `INotificationService` platform implementation using `Plugin.LocalNotification`
- `GlobalExceptionHandler` — hooks `AppDomain.UnhandledException` and `TaskScheduler.UnobservedTaskException`

#### UI (MAUI)
- `MainPage` — displays current time and next selected talk from `Preferences`
- `ConferencePlanPage` — search bar, All / My Talks filter, sort dropdown, notification toggle, talk cards with 5-star rating widget
- `InverseBoolConverter`, `StarRatingConverter`, `SelectedConverters` XAML value converters
- NDC Copenhagen branding (blue/yellow theme, custom fonts)
- Multi-platform support: Android 5.0+, iOS 15+, Windows 10 19041+

#### Conference Data
- Bundled `ndc.csv` raw resource with NDC Copenhagen talk data

#### Notifications
- Permission request flow
- Talk reminder notifications at 1, 5, and 15 minutes before selected talks
- Automatic re-scheduling when the selection changes

#### CI/CD
- `ci.yml` — build and test on every pull request
- `release.yml` — multi-platform builds triggered by version tags
- `preview.yml` — Docker image build for the Blazor preview
- `pr-validation.yml` — pull request linting and validation
- `update-docs.yml` — automated documentation drift detection

#### Documentation
- Full `docs/` tree: user guide, developer guide, operations, project
- German user manual (`BENUTZERHANDBUCH.md`)
- API reference, architecture guide, testing guide, contributing guide
- Installation, quick-start, and FAQ guides for end users

---

## How to Update This Changelog

When merging a pull request to `main`, add an entry under `[Unreleased]`:

```markdown
### Added
- Brief description of new features.

### Changed
- Breaking or notable behaviour changes.

### Fixed
- Bug fixes with issue references where available.

### Removed
- Features or APIs removed in this release.
```

When cutting a release, move the `[Unreleased]` block to a new versioned section:

```markdown
## [1.1.0] — YYYY-MM-DD
```

and add a fresh empty `[Unreleased]` block above it.

---

[Unreleased]: https://github.com/rsutter98/NdcApp/compare/v1.0.0...HEAD
[1.0.0]: https://github.com/rsutter98/NdcApp/releases/tag/v1.0.0
