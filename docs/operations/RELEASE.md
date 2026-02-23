# Release Process

This guide covers the end-to-end workflow for tagging, building, and publishing a new NdcApp release.

## Overview

Releases are fully automated via `.github/workflows/release.yml`. A release is triggered by either:

1. **Pushing a version tag** — the standard path for planned releases.
2. **Workflow dispatch** — for out-of-band or hotfix releases from the GitHub Actions UI.

```
git tag v1.x.y  →  push  →  release.yml  →  multi-platform builds  →  GitHub Release
```

---

## Versioning

NdcApp follows [Semantic Versioning](https://semver.org/):

| Part | When to increment | Example |
|---|---|---|
| **Major** | Breaking changes, major redesigns | `2.0.0` |
| **Minor** | New features, backward-compatible | `1.1.0` |
| **Patch** | Bug fixes, minor improvements | `1.0.1` |

Pre-release suffixes are allowed: `v1.2.0-beta.1`, `v1.2.0-rc.1`.

---

## Pre-Release Checklist

Before creating a tag, verify that:

- [ ] All tests pass on `main`: `dotnet test`
- [ ] No open blocking issues for this milestone
- [ ] `CHANGELOG.md` updated with the new version entry
- [ ] Documentation updated for any user-facing changes
- [ ] Version numbers updated in project files (if applicable)

---

## Creating a Release

### Step 1: Tag the Commit

```bash
# Ensure you are on main and up to date
git checkout main
git pull origin main

# Create an annotated tag
git tag -a v1.2.0 -m "Release v1.2.0"

# Push the tag to trigger the release workflow
git push origin v1.2.0
```

### Step 2: Monitor the Workflow

Navigate to **Actions → release.yml** in the GitHub repository to watch build progress.

The workflow:
1. Builds the Android APK on Ubuntu.
2. Builds the Windows MSIX on Windows.
3. Builds the iOS `.app` bundle on macOS.
4. Creates a GitHub Release and attaches all three artifacts.

Expected duration: 15–30 minutes depending on runner queue times.

### Step 3: Verify the Release

Once the workflow completes:

1. Open the [Releases page](https://github.com/rsutter98/NdcApp/releases).
2. Confirm the new release is present with all three platform artifacts.
3. Download each artifact and perform a smoke test on the target platform.
4. Verify the auto-generated release notes list the expected commits.

---

## Artifacts

| Platform | Artifact | Location in build output |
|---|---|---|
| Android | `NdcApp.apk` | `NdcApp/bin/Release/net8.0-android/publish/` |
| Windows | `NdcApp.msix` | `NdcApp/bin/Release/net8.0-windows10.0.19041.0/win10-x64/AppPackages/` |
| iOS | `NdcApp.app` | `NdcApp/bin/Release/net8.0-ios/` |

All artifacts are uploaded to the GitHub Release and available for direct download.

---

## Hotfix Release

For urgent fixes to a production release:

```bash
# Create a hotfix branch from the affected tag
git checkout -b hotfix/v1.0.1 v1.0.0

# Apply the minimal fix, commit, and push the branch
git commit -m "fix: description of hotfix"
git push origin hotfix/v1.0.1

# Create a PR to merge the hotfix branch into main
# After merge, tag the hotfix on main
git checkout main && git pull
git tag -a v1.0.1 -m "Hotfix release v1.0.1"
git push origin v1.0.1
```

---

## Rollback

If a release contains a critical defect:

1. **Communicate** — update the GitHub Release description to flag the issue.
2. **Identify the last good tag:**
   ```bash
   git tag --list --sort=-version:refname | head -10
   ```
3. **Re-release** — create a new patch tag pointing to the last good commit, or follow the hotfix process above.
4. See [Rollback Strategy](ROLLBACK_STRATEGY.md) for platform-specific rollback steps.

---

## Distribution Channels

| Platform | Development | Production |
|---|---|---|
| Android | Direct APK sideload | Google Play Store (requires signing keys) |
| Windows | MSIX sideload (Developer Mode) | Microsoft Store or enterprise distribution |
| iOS | TestFlight | App Store (requires Apple Developer Account) |

### Required Secrets for Production Signing

Store signing credentials as [GitHub Secrets](https://docs.github.com/actions/security-for-github-actions/security-guides/using-secrets-in-github-actions):

| Secret | Used for |
|---|---|
| `ANDROID_KEYSTORE` | Android release signing |
| `ANDROID_KEY_ALIAS` | Keystore alias |
| `ANDROID_KEY_PASSWORD` | Key password |
| `IOS_CERTIFICATE` | iOS distribution certificate |
| `IOS_PROVISIONING_PROFILE` | iOS provisioning profile |

---

## See Also

- [Deployment Guide](DEPLOYMENT.md) — Full deployment and build documentation
- [Rollback Strategy](ROLLBACK_STRATEGY.md) — Recovering from a bad release
- [Build Guide](BUILD.md) — Building locally and in CI
