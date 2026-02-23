# Release Process

This document describes how to create and publish a new NdcApp release.

---

## 🏷️ Versioning

NdcApp uses **Semantic Versioning** (`MAJOR.MINOR.PATCH`):

| Component | When to increment |
|---|---|
| `MAJOR` | Breaking changes or major feature milestones |
| `MINOR` | New backward-compatible features |
| `PATCH` | Bug fixes and small improvements |

---

## 🚀 Automated Release Pipeline

The release workflow (`.github/workflows/release.yml`) triggers automatically on a version tag push or can be triggered manually via `workflow_dispatch`.

### Trigger: Git tag

```bash
git tag v1.2.3
git push origin v1.2.3
```

### Trigger: Manual dispatch

Navigate to **Actions → Release → Run workflow** and enter the version number (e.g., `1.2.3`).

---

## 🔨 What the Pipeline Does

The workflow runs three parallel build jobs and then creates a GitHub Release:

### 1. Build Windows MSIX

Runs on `windows-latest`:

```bash
dotnet workload install maui
dotnet publish NdcApp/NdcApp.csproj \
  -f net8.0-windows10.0.19041.0 \
  -c Release \
  -p:BuildingForRelease=true \
  -p:GenerateAppxPackageOnBuild=true \
  -p:AppxPackageSigningEnabled=false
```

Artifact: `windows-msix` (`.msix` package)

### 2. Build Android APK

Runs on `ubuntu-latest`:

```bash
dotnet workload install maui-android
dotnet publish NdcApp/NdcApp.csproj \
  -f net8.0-android \
  -c Release \
  -p:BuildingForRelease=true
```

Artifact: `android-apk` (`.apk` file)

### 3. Build iOS IPA

Runs on `macos-latest`:

```bash
dotnet workload install maui-ios
dotnet publish NdcApp/NdcApp.csproj \
  -f net8.0-ios \
  -c Release \
  -p:BuildingForRelease=true
```

Artifact: `ios-ipa` (`.ipa` file — unsigned)

### 4. Create GitHub Release

After all build jobs succeed, a release is created with:
- Release notes generated from the Git tag message
- All platform artifacts attached

---

## 📋 Pre-Release Checklist

Before tagging a release, confirm:

- [ ] All CI checks pass on `main` (`ci.yml`)
- [ ] PR validation pipeline passes (`pr-validation.yml`)
- [ ] All 99+ tests pass (`dotnet test`)
- [ ] `CHANGELOG.md` updated with new version entry
- [ ] Version numbers consistent in `.csproj` files if bumped manually
- [ ] Preview environment tested (`docker compose -f docker-compose.preview.yml up`)

---

## 🔐 Code Signing

The automated pipeline builds unsigned packages for development/testing. For production distribution:

| Platform | Requirement |
|---|---|
| Android | Configure `ANDROID_KEYSTORE` and `ANDROID_SIGNING_*` secrets in GitHub |
| Windows | Configure code-signing certificate secret; remove `-p:AppxPackageSigningEnabled=false` |
| iOS | Apple Developer certificates and provisioning profiles required |

See [Security](../project/SECURITY.md) for credential storage guidance.

---

## ♻️ Rollback

To roll back a bad release, see [Rollback Strategy](ROLLBACK_STRATEGY.md).

---

## 🔗 See Also

- [Build Guide](BUILD.md)
- [Deployment](DEPLOYMENT.md)
- [Rollback Strategy](ROLLBACK_STRATEGY.md)
- [Changelog](../project/CHANGELOG.md)
