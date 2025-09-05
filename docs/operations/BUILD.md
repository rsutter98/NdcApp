# Build & Deployment Pipeline

Quick reference for building and deploying the NdcApp.

## 🚀 Quick Start

### Prerequisites
```bash
# Install .NET 8.0 SDK
# Install MAUI workloads
dotnet workload install maui
```

### Local Development Build
```bash
# Standard build (single target framework)
dotnet build

# Release build (all platforms)
dotnet build -c Release -p:BuildingForRelease=true
```

### Platform-Specific Builds

#### Android APK
```bash
./scripts/build/build-android.sh
# or
dotnet publish NdcApp/NdcApp.csproj -f net8.0-android -c Release -p:BuildingForRelease=true
```

#### Windows MSIX
```cmd
scripts\build\build-windows.bat
# or
dotnet publish NdcApp/NdcApp.csproj -f net8.0-windows10.0.19041.0 -c Release -p:BuildingForRelease=true
```

#### iOS
```bash
./scripts/build/build-ios.sh
# or (requires macOS)
dotnet build NdcApp/NdcApp.csproj -f net8.0-ios -c Release -p:BuildingForRelease=true
```

## 🔄 CI/CD Pipeline

### Pull Request Workflow
- **Trigger:** Pull requests to `main` or `develop`
- **Actions:** Build, test, upload test results
- **File:** `.github/workflows/ci.yml`

### Release Workflow
- **Trigger:** Git tags (`v*`) or manual dispatch
- **Actions:** Multi-platform builds, create GitHub release
- **File:** `.github/workflows/release.yml`

### Creating a Release
```bash
# Tag and push for automated release
git tag v1.0.0
git push origin v1.0.0

# Or use GitHub web interface for manual dispatch
```

## 📦 Artifacts

### Output Locations
- **Android APK:** `NdcApp/bin/Release/net8.0-android/publish/`
- **Windows MSIX:** `NdcApp/bin/Release/net8.0-windows10.0.19041.0/win10-x64/AppPackages/`
- **iOS App:** `NdcApp/bin/Release/net8.0-ios/`

### GitHub Release Assets
- Automatically attached to GitHub releases
- Available for download from Releases page

## 🔧 Configuration

### Project File Features
- Multi-targeting for release builds
- Platform-specific optimizations
- MSIX packaging for Windows
- APK generation for Android

### Environment Variables
- `BuildingForRelease=true` - Enables multi-targeting

## 📚 Documentation

- **Full Guide:** [DEPLOYMENT.md](DEPLOYMENT.md)
- **Project README:** [README.md](README.md)
- **Roadmap:** [ROADMAP.md](ROADMAP.md)

---
*For detailed instructions, troubleshooting, and rollback procedures, see [DEPLOYMENT.md](DEPLOYMENT.md)*