# Changelog

All notable changes to NdcApp are documented here. This project follows [Semantic Versioning](https://semver.org/).

Format based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

---

## [Unreleased]

### Added
- Agentic `update-docs` workflow to automatically synchronize documentation with code changes on every push to `main` (`.github/workflows/update-docs.lock.yml`).
- Full documentation structure under `docs/` following the Diátaxis framework.
- `docs/developer-guide/API.md` — complete API reference for all service interfaces.
- `docs/developer-guide/ARCHITECTURE.md` — layered architecture overview.
- `docs/operations/RELEASE.md` — release process and pipeline documentation.
- `docs/operations/MONITORING.md` — monitoring and observability guide.
- `docs/project/SECURITY.md` — consolidated security policy.
- `docs/project/CHANGELOG.md` — this file.

---

## [1.0.0] — Initial Release

### Added

#### Core Application (`NdcApp`)
- .NET MAUI cross-platform app targeting Windows, Android, and iOS.
- `MainPage` — Landing page displaying current time and next scheduled talk.
- `ConferencePlanPage` — Full session browser with selection, sorting, and search.
- `AppShell` — Single-level navigation shell.

#### Business Logic (`NdcApp.Core`)
- `IConferencePlanService` / `ConferencePlanService` — Talk selection, sorting, serialization, and recommendations.
- `ITalkService` / `TalkService` — CSV talk data loading and parsing.
- `ITalkFilterService` / `TalkFilterService` — Full-text search across title, speaker, category, and room.
- `ITalkRatingService` / `TalkRatingService` — Per-talk ratings with average and recommendation engine.
- `INotificationService` / `TalkNotificationService` — Local notification scheduling for upcoming talks.
- `ILoggerService` / `LoggerService` — Structured application logging.
- `IErrorHandlingService` / `ErrorHandlingService` — Centralized exception handling with user-friendly messages.
- `GlobalExceptionHandler` — Top-level unhandled exception capture.

#### Features
- CSV import of conference sessions from bundled `ndc.csv`.
- Personal schedule builder with one-talk-per-time-slot enforcement.
- Talk sorting by speaker, category, rating, or chronological order.
- Advanced search across all talk fields.
- Star ratings (1–5) with comment support.
- Recommendation engine based on user ratings.
- Local push notifications for upcoming selected talks.
- Auto-save of selections and ratings via platform Preferences.
- NDC Copenhagen branding and color scheme.

#### Preview App (`NdcApp.Preview`)
- Blazor Server application rendering the conference plan in a browser.
- Docker support via `docker-compose.preview.yml`.
- Health status page at `/status.html`.

#### CI/CD
- GitHub Actions workflows: `ci.yml`, `pr-validation.yml`, `preview.yml`, `release.yml`.
- Dependabot configuration for NuGet and Actions dependency updates.
- Branch protection requiring all checks to pass before merging.

#### Documentation
- `README.md` with feature overview, quick start, and architecture summary.
- `BENUTZERHANDBUCH.md` — detailed German user guide.
- `docs/` structured documentation (user guide, developer guide, operations, project).
- `BUILD.md`, `DEPLOYMENT.md`, `ROADMAP.md`, `FEATURES.md`, `SECURITY.md`.

#### Tests (`NdcApp.Tests`)
- 99+ xUnit tests covering unit, integration, and performance scenarios.
- Test files: `ConferencePlanServiceTests`, `TalkRatingServiceTests`, `TalkNotificationServiceTests`, `IntegrationTests`, `PerformanceTests`, and more.

---

[Unreleased]: https://github.com/rsutter98/NdcApp/compare/v1.0.0...HEAD
[1.0.0]: https://github.com/rsutter98/NdcApp/releases/tag/v1.0.0
