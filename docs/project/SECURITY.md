# Security

> The authoritative security policy, including vulnerability reporting instructions, lives in [`SECURITY.md`](../../SECURITY.md) at the repository root.

This page supplements that policy with developer-focused security guidance.

---

## Supported Versions

| Version | Security updates |
|---|---|
| 1.0.x | ✅ Supported |

---

## Reporting a Vulnerability

**Do not open a public issue.** Vulnerabilities should be reported privately:

1. Email the repository maintainers with a description of the issue.
2. Include steps to reproduce, potential impact, and a suggested fix if available.
3. Allow reasonable time for a fix before public disclosure.

---

## Dependency Security

Dependabot is configured to scan NuGet packages weekly (`.github/dependabot.yml`). Maintainers should review and merge Dependabot PRs promptly, especially for packages with known CVEs.

Key dependencies with elevated security relevance:

| Package | Risk area |
|---|---|
| `Plugin.LocalNotification` | OS notification permissions |
| `Microsoft.Maui.Controls` | Platform API surface |

---

## Code Signing

Platform-specific signing is required for production distribution:

| Platform | Requirement |
|---|---|
| Android | Signing keystore stored as GitHub Secret (`ANDROID_KEYSTORE`) |
| Windows | Certificate required for MSIX packages |
| iOS | Apple Developer certificates and provisioning profiles |

Never commit signing keys or certificates to the repository. Use [GitHub Secrets](https://docs.github.com/actions/security-for-github-actions/security-guides/using-secrets-in-github-actions) for all sensitive values.

---

## Build Pipeline Security

- All CI runs execute in isolated, ephemeral GitHub Actions runners.
- Dependencies are restored exclusively from official NuGet sources.
- No secrets or credentials appear in build logs.
- Release artifacts are generated without embedded credentials.

---

## Data Storage

NdcApp stores user data (selected talks and ratings) using platform `Preferences`, which maps to:

- **Android:** `SharedPreferences` (private to the app)
- **iOS:** `NSUserDefaults` (private to the app)
- **Windows:** Application data folder

No network transmission of personal data occurs. The app does not send analytics or telemetry.

---

## Notifications

Push notification content is generated locally and scheduled via `Plugin.LocalNotification`. No notification payload is sent to a remote server.

---

## See Also

- [`SECURITY.md`](../../SECURITY.md) — Root security policy
- [Monitoring](../operations/MONITORING.md) — Dependabot and code scanning
- [Release Process](../operations/RELEASE.md) — Signing and distribution
