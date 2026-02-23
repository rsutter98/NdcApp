# Monitoring

Guidance for observing NdcApp's health, catching errors, and diagnosing issues in production.

## What to Monitor

| Signal | Tool | Where to look |
|---|---|---|
| Build and test status | GitHub Actions | Repository → Actions tab |
| Release artifact health | GitHub Releases | Repository → Releases page |
| Crash reports | Application logs | Device log (see below) |
| Dependency vulnerabilities | Dependabot | Repository → Security → Dependabot |
| Code quality | GitHub Code Scanning | Repository → Security → Code scanning |

---

## Application Logging

NdcApp uses `ILoggerService` throughout the core library. The default `LoggerService` implementation delegates to `Microsoft.Extensions.Logging`.

### Log Levels

| Level | Used for |
|---|---|
| `Debug` | Detailed diagnostic output (Debug builds only) |
| `Information` | Normal operation milestones (e.g., "Loaded 120 talks") |
| `Warning` | Non-fatal anomalies (e.g., malformed CSV line skipped) |
| `Error` | Exceptions and failures with full stack traces |

Log level thresholds set in `MauiProgram.cs`:

```csharp
#if DEBUG
    builder.Logging.SetMinimumLevel(LogLevel.Debug);
#else
    builder.Logging.SetMinimumLevel(LogLevel.Information);
#endif
```

### Viewing Logs

**Windows:** Logs appear in the Visual Studio Output window (debug builds) or the Windows Event Viewer (release builds with `AddEventLog`).

**Android:** Use `adb logcat` with a tag filter:
```bash
adb logcat -s NdcApp
```

**iOS:** Open **Xcode → Window → Devices and Simulators**, select the device, and click **Open Console**.

---

## Error Handling

Unhandled exceptions are caught by `GlobalExceptionHandler`, which hooks two runtime events at app startup:

- `AppDomain.CurrentDomain.UnhandledException` — catches synchronous exceptions that escape all handlers.
- `TaskScheduler.UnobservedTaskException` — catches fire-and-forget async failures; marks them observed to prevent a crash.

Both routes log at `Critical` level via `ILogger<GlobalExceptionHandler>` and delegate to `IErrorHandlingService.HandleErrorAsync`.

### Error Message Mapping

`ErrorHandlingService.GetUserFriendlyMessage` translates exception types to user-readable strings. Extend the `switch` expression in that method to handle new exception types.

---

## GitHub Actions Health

Monitor CI/CD pipelines in the **Actions** tab:

| Workflow | Trigger | Healthy state |
|---|---|---|
| `ci.yml` | Pull request to `main` | All jobs green |
| `release.yml` | Tag push (`v*`) | All platform builds succeed |
| `preview.yml` | Push to `main` | Docker image built and pushed |
| `pr-validation.yml` | Pull request | Lint and validation pass |
| `update-docs.yml` | Push to `main` | Documentation PR created (if needed) |

Set up [GitHub notifications](https://docs.github.com/account-and-profile/managing-subscriptions-and-notifications-on-github) or [Slack/email integrations](https://docs.github.com/actions/monitoring-and-troubleshooting-workflows/monitoring-workflows/about-monitoring-workflows) to be alerted on failures.

---

## Dependabot

Dependabot is configured in `.github/dependabot.yml` to scan NuGet dependencies weekly. Review and merge Dependabot PRs promptly to keep the dependency graph secure.

Critical packages to watch:

- `Plugin.LocalNotification` — notification scheduling
- `Microsoft.Maui.Controls` — MAUI framework
- `xunit` / `Microsoft.NET.Test.Sdk` — test infrastructure

---

## Performance Baselines

`NdcApp.Tests/PerformanceTests.cs` and `NdcApp.Tests/PerformanceBenchmarkTests.cs` capture timing assertions for common operations. If CI performance tests start failing, investigate:

1. Regressions in CSV parsing (`TalkService`)
2. Sorting algorithms in `ConferencePlanService`
3. Recommendation score computation in `TalkRatingService`

---

## Preview Application

The Blazor preview (`NdcApp.Preview`) exposes a `/status` endpoint served from `wwwroot/status.html`. Use this for basic liveness checks when running locally or in Docker:

```bash
# Start the preview container
docker-compose -f docker-compose.preview.yml up -d

# Liveness check
curl -f http://localhost:8080/status || echo "Preview is down"
```

---

## Alerting Recommendations

For production deployments:

1. **Set up GitHub Actions failure notifications** via email or Slack webhook.
2. **Enable Dependabot security alerts** so critical CVEs surface immediately.
3. **Monitor Docker image size** for the Blazor preview to catch accidental dependency bloat.
4. **Review code scanning alerts** (Security → Code Scanning) after each push to `main`.

---

## See Also

- [Deployment Guide](DEPLOYMENT.md) — How to deploy the app
- [Rollback Strategy](ROLLBACK_STRATEGY.md) — Recovering from incidents
- [Release Process](RELEASE.md) — How releases are cut and distributed
