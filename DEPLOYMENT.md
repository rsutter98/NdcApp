# Deployment Guide

> **📚 Note**: This document has been moved to the new documentation structure.  
> **🔗 Latest Version**: See [docs/operations/DEPLOYMENT.md](docs/operations/DEPLOYMENT.md)

This document describes the deployment pipeline and build processes for the NdcApp.

## Overview

The NdcApp is a .NET MAUI cross-platform application that can be deployed to:
- **Android** (APK)
- **Windows** (MSIX Package)
- **iOS** (App Store/Enterprise)
- **Web Preview** (Blazor Server - for demonstration)

## Continuous Integration (CI)

### Pipeline Overview
The CI/CD system now includes comprehensive validation workflows:

| Workflow | Purpose | Triggers | Status Check |
|----------|---------|----------|--------------|
| `ci.yml` | Core build and test validation | Push/PR to main/develop | `Build and Test` |
| `pr-validation.yml` | Comprehensive PR validation | PR events | `PR Status Check` |
| `preview.yml` | Preview deployment validation | Push/PR to main/develop | `Build Preview App` |
| `release.yml` | Release builds for all platforms | Git tags/manual | N/A |

### Pull Request Validation
Every pull request now triggers comprehensive validation:
- ✅ **Build verification** across all projects
- ✅ **Unit test execution** (99+ tests)
- ✅ **Dockerfile validation** for preview deployment
- ✅ **Test result artifacts** uploaded for review
- ✅ **Status checks** that block merging until all pass

**Primary Workflows:** 
- `.github/workflows/ci.yml` - Core CI validation
- `.github/workflows/pr-validation.yml` - Comprehensive PR checks

### Branch Protection
To ensure code quality, configure branch protection rules:
- **Required status checks**: All CI workflows must pass
- **Required reviews**: Code must be reviewed before merge
- **Up-to-date branches**: PRs must be current with target branch

📋 **Setup Guide**: [docs/BRANCH_PROTECTION_SETUP.md](docs/BRANCH_PROTECTION_SETUP.md)
🔧 **Configuration Script**: [scripts/setup-branch-protection.md](scripts/setup-branch-protection.md)  
✅ **Verification Tool**: [scripts/verify-branch-protection.sh](scripts/verify-branch-protection.sh)
🔄 **Rollback Strategy**: [docs/operations/ROLLBACK_STRATEGY.md](docs/operations/ROLLBACK_STRATEGY.md)

### Branches
- `main` - Production branch (protected)
- `develop` - Development branch (protected)
- Feature branches trigger full validation on PR

### Recent Fixes (Issue #47)
🔧 **Permission Issues Resolved**: 
- Removed auto-commit functionality that caused 403 errors
- Status updates now handled without repository write permissions
- All pipelines must be green before PR completion

## Continuous Deployment (CD)

### Release Process
Releases are triggered by:
1. **Git Tags**: Push a tag like `v1.0.0`
2. **Manual Trigger**: Use GitHub Actions manual dispatch

**Workflow:** `.github/workflows/release.yml`

## Preview Deployment (Web)

### Web-Based Preview
A Blazor Server application that replicates the MAUI app functionality for browser-based demonstration:
- **Framework:** `net8.0` (ASP.NET Core)
- **Output:** Docker container
- **Location:** `NdcApp.Preview/`
- **Runner:** Docker container
- **Access:** http://localhost:8080 (local), configurable for production

#### Features
- ✅ Conference talk browsing and filtering
- ✅ Personal schedule management
- ✅ Talk rating system (1-5 stars)
- ✅ Real-time statistics and updates
- ✅ Responsive web design
- ✅ Same core business logic as MAUI app

**See:** [PREVIEW.md](PREVIEW.md) for detailed preview deployment documentation


### Platform Builds

#### Android APK
- **Framework:** `net8.0-android`

- **Output:** APK file
- **Location:** `NdcApp/bin/Release/net8.0-android/publish/`
- **Runner:** Ubuntu Latest

#### Windows MSIX
- **Framework:** `net8.0-windows10.0.19041.0`
- **Output:** MSIX package
- **Location:** `NdcApp/bin/Release/net8.0-windows10.0.19041.0/win10-x64/AppPackages/`
- **Runner:** Windows Latest
- **Requirements:** Windows 10 version 19041.0 or later

#### iOS App
- **Framework:** `net8.0-ios`
- **Output:** .app bundle
- **Location:** `NdcApp/bin/Release/net8.0-ios/`
- **Runner:** macOS Latest
- **Note:** Requires Apple Developer Account for App Store deployment

## Local Development Builds

### Prerequisites
- .NET 8.0 SDK
- MAUI workload: `dotnet workload install maui`

### Build Scripts
Located in `scripts/build/`:

#### Android
```bash
./scripts/build/build-android.sh
```

#### Windows
```cmd
scripts\build\build-windows.bat
```

#### iOS
```bash
./scripts/build/build-ios.sh
```

### Manual Commands

#### Build All Platforms
```bash
dotnet build -c Release
```

#### Build Specific Platform
```bash
# Android
dotnet publish NdcApp/NdcApp.csproj -f net8.0-android -c Release

# Windows
dotnet publish NdcApp/NdcApp.csproj -f net8.0-windows10.0.19041.0 -c Release

# iOS
dotnet build NdcApp/NdcApp.csproj -f net8.0-ios -c Release
```

## Release Artifacts

### Automated Release Creation
When a tag is pushed, the release workflow:
1. Builds all platforms
2. Creates GitHub release
3. Uploads all artifacts
4. Generates release notes

### Artifact Structure
```
Release Assets:
├── android-apk/
│   └── NdcApp.apk
├── windows-msix/
│   └── NdcApp.msix
└── ios-app/
    └── NdcApp.app
```

## Distribution

### Android
- **Development:** Direct APK installation
- **Production:** Google Play Store (requires signing)

### Windows
- **Development:** Sideload MSIX package
- **Production:** Microsoft Store or enterprise distribution

### iOS
- **Development:** TestFlight or direct installation (requires provisioning)
- **Production:** App Store (requires Apple Developer Account)

## Rollback Strategy

### Git-Based Rollback
1. **Identify Last Good Release:**
   ```bash
   git tag --list --sort=-version:refname
   ```

2. **Create Rollback Release:**
   ```bash
   git tag v1.0.1-rollback [previous-good-commit]
   git push origin v1.0.1-rollback
   ```

3. **Emergency Hotfix:**
   - Create hotfix branch from last good release
   - Apply minimal fix
   - Create new release tag

### Platform-Specific Rollback

#### Android
- Revert to previous APK version
- Use Google Play Console rollback (if published)

#### Windows
- Uninstall current MSIX
- Install previous MSIX version
- Use Microsoft Store rollback (if published)

#### iOS
- Use App Store rollback functionality
- Deploy previous IPA for enterprise

### Monitoring
- Monitor release workflows in GitHub Actions
- Check build status before deployment
- Test artifacts before distribution

## Security Considerations

### Code Signing
- **Android:** Configure signing keys for Play Store
- **Windows:** Certificate required for production MSIX
- **iOS:** Apple Developer certificates required

### Secrets Management
Store sensitive data in GitHub Secrets:
- `ANDROID_KEYSTORE` - Android signing keystore
- `ANDROID_KEY_ALIAS` - Keystore alias
- `ANDROID_KEY_PASSWORD` - Key password
- `IOS_CERTIFICATE` - iOS distribution certificate
- `IOS_PROVISIONING_PROFILE` - iOS provisioning profile

## Troubleshooting

### Common Build Issues

#### MAUI Workload Missing
```bash
dotnet workload install maui
```

#### Platform SDK Missing
- **Android:** Install Android SDK via Visual Studio or Android Studio
- **iOS:** Install Xcode on macOS
- **Windows:** Windows SDK included with Visual Studio

#### Build Failures
1. Check GitHub Actions logs
2. Verify project file syntax
3. Ensure all dependencies are restored
4. Check platform-specific requirements

### Support
- GitHub Issues for build problems
- Documentation updates via pull requests
- Release process improvements via GitHub Discussions

---

*Last updated: January 2025*
