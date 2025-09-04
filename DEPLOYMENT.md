# Deployment Guide - NDC Conference Planner

## Übersicht

Diese Dokumentation beschreibt den Deployment-Prozess für die NDC Conference Planner App (.NET MAUI).

## Overview

The NdcApp is a .NET MAUI cross-platform application that can be deployed to:
- **Android** (APK)
- **Windows** (MSIX Package)
- **iOS** (App Store/Enterprise)

## 🏗️ CI/CD Pipeline

### Automatisierte Builds

Die CI/CD-Pipeline ist über GitHub Actions konfiguriert und unterstützt:

- **Kontinuierliche Integration**: Automatische Builds und Tests bei Pull Requests
- **Kontinuierliche Bereitstellung**: Automatische Erstellung von Release-Artifacts
- **Multi-Platform Support**: Windows (MSIX), Android (APK), und iOS (App)

### Continuous Integration (CI)

#### Pull Request Testing
Every pull request triggers automated testing:
- Build verification across all projects
- Unit test execution (99+ tests)
- Test result artifacts

**Workflow:** `.github/workflows/ci.yml`

#### Branches
- `main` - Production branch
- `develop` - Development branch
- Feature branches trigger CI on PR

### Pipeline-Trigger

| Trigger | Aktion | Beschreibung |
|---------|--------|--------------|
| Push zu `main` | Build + Test | CI-Validierung |
| Push zu `develop` | Build + Test | Nur Validierung |
| Pull Request | Build + Test | Code-Validierung |
| Git Tags (`v*`) | Build + Test + Deploy + Publish | Release-Erstellung |
| Manual Trigger | Build + Test + Deploy + Publish | Manuelle Release-Erstellung |

## Continuous Deployment (CD)

### Release Process
Releases are triggered by:
1. **Git Tags**: Push a tag like `v1.0.0`
2. **Manual Trigger**: Use GitHub Actions manual dispatch

**Workflow:** `.github/workflows/release.yml`

## 🎯 Zielplattformen

### Platform Builds

#### Android APK
- **Framework:** `net8.0-android`
- **Format**: APK Package
- **Minimum Version**: Android 5.0 (API Level 21)
- **Target Version**: Android 14 (API Level 34)
- **Installation**: Sideloading (APK direkt installieren)
- **Output:** APK file
- **Location:** `NdcApp/bin/Release/net8.0-android/publish/`
- **Runner:** Ubuntu Latest

#### Windows MSIX
- **Framework:** `net8.0-windows10.0.19041.0`
- **Format**: MSIX Package
- **Minimum Version**: Windows 10 Build 19041 (Version 2004)
- **Installation**: Über Microsoft Store oder Sideloading
- **Updates**: Automatisch über Store oder manuell
- **Output:** MSIX package
- **Location:** `NdcApp/bin/Release/net8.0-windows10.0.19041.0/win10-x64/AppPackages/`
- **Runner:** Windows Latest

#### iOS App
- **Framework:** `net8.0-ios`
- **Output:** .app bundle
- **Location:** `NdcApp/bin/Release/net8.0-ios/`
- **Runner:** macOS Latest
- **Note:** Requires Apple Developer Account for App Store deployment

## 🚀 Deployment-Prozess

### 1. Entwicklung und Testing

```bash
# Lokale Entwicklung
dotnet build
dotnet test

# Platform-spezifische Builds (erfordert Windows + MAUI Workload)
dotnet publish -f net8.0-windows10.0.19041.0 -c Release
dotnet publish -f net8.0-android -c Release
```

### 2. Local Development Builds

#### Prerequisites
- .NET 8.0 SDK
- MAUI workload: `dotnet workload install maui`

#### Build Scripts
Located in `scripts/build/`:

##### Android
```bash
./scripts/build/build-android.sh
```

##### Windows
```cmd
scripts\build\build-windows.bat
```

##### iOS
```bash
./scripts/build/build-ios.sh
```

#### Manual Commands

##### Build All Platforms
```bash
dotnet build -c Release
```

##### Build Specific Platform
```bash
# Android
dotnet publish NdcApp/NdcApp.csproj -f net8.0-android -c Release

# Windows
dotnet publish NdcApp/NdcApp.csproj -f net8.0-windows10.0.19041.0 -c Release

# iOS
dotnet build NdcApp/NdcApp.csproj -f net8.0-ios -c Release
```

### 3. Continuous Integration

Bei jedem Push wird automatisch ausgeführt:

1. **Code Checkout**: Repository wird geladen
2. **Environment Setup**: .NET 8.0 SDK installiert
3. **Dependencies**: NuGet-Pakete wiederhergestellt
4. **Build**: Solution kompiliert (Release-Konfiguration)
5. **Tests**: Alle 99 Tests ausgeführt
6. **Artifacts**: Test-Ergebnisse gespeichert

### 4. Platform Builds (bei Tags oder Manual Trigger)

#### Windows Build
- Läuft auf: `windows-latest` Runner
- MAUI Workload installiert
- MSIX Package erstellt
- Artifacts gespeichert

#### Android Build
- Läuft auf: `ubuntu-latest` Runner
- MAUI Workload installiert
- APK Package erstellt
- Artifacts gespeichert

#### iOS Build
- Läuft auf: `macos-latest` Runner
- MAUI Workload installiert
- App Bundle erstellt
- Artifacts gespeichert

### 5. Release-Erstellung

Bei einem GitHub Release:

1. **Download**: Platform-Artifacts heruntergeladen
2. **Release Creation**: GitHub Release mit allen Artifacts
3. **Documentation**: Release Notes automatisch generiert

## 📦 Installation

### Windows

1. **MSIX Package herunterladen**
2. **Doppelklick auf .msix Datei**
3. **"Installieren" klicken**
4. **App erscheint im Startmenü**

**Voraussetzungen:**
- Windows 10 Version 2004 oder neuer
- .NET Runtime wird automatisch installiert

### Android

1. **APK Package herunterladen**
2. **"Unbekannte Quellen" aktivieren** (Einstellungen > Sicherheit)
3. **APK-Datei antippen** und "Installieren" wählen
4. **App erscheint in der App-Übersicht**

**Voraussetzungen:**
- Android 5.0 (API Level 21) oder neuer
- Mindestens 100 MB freier Speicherplatz

### iOS

**App Store Distribution:**
1. App über TestFlight oder App Store installieren

**Enterprise/Sideloading:**
1. Developer Certificate erforderlich
2. Provisioning Profile konfigurieren
3. App über Xcode oder Apple Configurator installieren

**Voraussetzungen:**
- iOS 11.0 oder neuer
- Apple Developer Account (für Entwicklung/Distribution)

## 🔧 Troubleshooting

### Häufige Probleme

#### Windows Installation schlägt fehl
- **Problem**: "Diese App kann nicht installiert werden"
- **Lösung**: Developer-Modus aktivieren oder Certificate vertrauen

#### Android APK Installation blockiert
- **Problem**: "Installation aus unbekannten Quellen blockiert"
- **Lösung**: Sicherheitseinstellungen prüfen und "Unbekannte Quellen" aktivieren

#### Build Fehler in CI/CD
- **Problem**: MAUI Workload nicht gefunden
- **Lösung**: Pipeline Script überprüfen, Workload Installation sicherstellen

## 🔄 Rollback-Strategie

### Windows
1. **App deinstallieren** über Windows-Einstellungen
2. **Vorherige Version installieren** (falls verfügbar)
3. **App-Daten bleiben erhalten** (LocalApplicationData)

### Android
1. **App deinstallieren** über Android-Einstellungen
2. **Vorherige APK installieren**
3. **App-Daten gehen verloren** (Backup empfohlen)

### iOS
1. **App löschen** vom Homescreen
2. **Vorherige Version über TestFlight/App Store** installieren
3. **iCloud Backup** kann Daten wiederherstellen

## 📊 Pipeline Monitoring

### GitHub Actions
- **Dashboard**: Repository → Actions Tab
- **Logs**: Detaillierte Build-Logs pro Workflow
- **Artifacts**: Download verfügbar für 90 Tage

### Pipeline Validation
```bash
# Pipeline Validierung lokal ausführen
./scripts/validate-pipeline.sh
```

### Metriken
- **Build Zeit**: Durchschnittlich 15-20 Minuten
- **Test Coverage**: 99+ Tests
- **Artifact Größe**: 
  - Windows MSIX: ~50-80 MB
  - Android APK: ~30-50 MB
  - iOS App: ~40-60 MB

## 🛡️ Sicherheit

### Code Signing
- **Windows**: MSIX Pakete können signiert werden
- **Android**: APK Signierung optional für Sideloading
- **iOS**: Zwingend erforderlich für Distribution

### Dependency Management
- **Dependabot**: Automatische Updates für NuGet und GitHub Actions
- **Security Alerts**: GitHub Security Advisories aktiviert
- **Vulnerability Scanning**: Integriert in CI/CD Pipeline

## 📚 Weitere Ressourcen

- **Build Documentation**: `BUILD.md`
- **Security Policy**: `SECURITY.md`
- **User Manual**: `BENUTZERHANDBUCH.md`
- **Feature Overview**: `FEATURES.md`
- **Roadmap**: `ROADMAP.md`

---

*Für Support oder Fragen zur Deployment-Pipeline, bitte ein Issue auf GitHub erstellen.*