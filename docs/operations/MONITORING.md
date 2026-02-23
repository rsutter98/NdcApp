# Monitoring

This document describes how to observe the health and behavior of NdcApp in production and development.

---

## 📊 What to Monitor

NdcApp is a client-side mobile/desktop application. There is no server-side component to monitor at runtime. Observability therefore focuses on:

1. **CI/CD pipeline health** — Are builds and tests passing?
2. **Release artifact integrity** — Are published packages valid?
3. **Crash and error reports** — Are users encountering unhandled errors?
4. **Preview environment** — Is the Blazor preview instance healthy?

---

## ✅ CI/CD Pipeline Health

Monitor pipeline status via GitHub Actions:

| Workflow | File | Purpose |
|---|---|---|
| CI | `ci.yml` | Builds, runs all 99+ tests on every push/PR |
| PR Validation | `pr-validation.yml` | Validates PR quality and code standards |
| Preview | `preview.yml` | Deploys Blazor preview environment |
| Release | `release.yml` | Produces signed/unsigned artifacts |
| Update Docs | `update-docs.lock.yml` | Keeps documentation synchronized with code |

**Quick check:** The CI badge in `README.md` reflects the latest `main` branch build status.

```
[![CI/CD Pipeline](https://github.com/rsutter98/NdcApp/actions/workflows/ci.yml/badge.svg)](...)
```

**Alert threshold:** Any failed workflow run on `main` requires immediate investigation.

---

## 🖥️ Preview Environment

The Blazor preview app runs in Docker and is deployed by `preview.yml`. To verify locally:

```bash
# Start the preview environment
docker compose -f docker-compose.preview.yml up

# Check health endpoint
curl http://localhost:5000/status.html
```

The `status.html` page at `NdcApp.Preview/wwwroot/status.html` provides a self-contained health dashboard.

---

## 🪵 Application Logging

NdcApp uses structured logging via `ILoggerService` and Microsoft.Extensions.Logging.

| Build | Minimum Level | Output |
|---|---|---|
| Debug | `Debug` | Console + Debug output |
| Release | `Information` | Console |

Log levels in order of severity: `Debug → Info → Warning → Error`.

To view logs during development:

```bash
# Run with debug output visible
dotnet run --project NdcApp/NdcApp.csproj -f net8.0-windows10.0.19041.0
```

On Android/iOS, use the platform device log tools:
- **Android**: `adb logcat` (filter by app package)
- **iOS**: Xcode Devices & Simulators console

---

## 💥 Error Handling & Crash Reporting

`GlobalExceptionHandler` catches unhandled exceptions at the top level:

```csharp
AppDomain.CurrentDomain.UnhandledException  // background threads
TaskScheduler.UnobservedTaskException       // async task failures
```

All unhandled errors are routed through `IErrorHandlingService`, which logs the stack trace and displays a user-friendly alert.

**To review recent errors:** Check the platform device log after an unexpected app termination.

**To add external crash reporting** (e.g., App Center, Sentry): implement `ILoggerService` with your chosen SDK and register it in `MauiProgram.cs`.

---

## 📦 Release Artifact Verification

After a release pipeline run, verify the artifacts in the GitHub Release page:

1. Download the artifact for the target platform.
2. Check file size is non-zero and within expected range.
3. For Android: run `aapt dump badging *.apk` to verify package metadata.
4. For Windows: attempt to install the MSIX on a test machine.

---

## 🔔 Alerting Recommendations

| Event | Recommended Action |
|---|---|
| CI pipeline failure on `main` | Investigate immediately; block releases |
| Release workflow failure | Re-trigger after fixing the root cause |
| Preview environment down | Re-run `preview.yml` workflow |
| Spike in unhandled errors | Review device logs; open a bug report |

---

## 🔗 See Also

- [Deployment](DEPLOYMENT.md)
- [Build Guide](BUILD.md)
- [Rollback Strategy](ROLLBACK_STRATEGY.md)
