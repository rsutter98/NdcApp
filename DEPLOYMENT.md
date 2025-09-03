# Deployment Guide - NDC Conference Planner

## Übersicht

Diese Dokumentation beschreibt den Deployment-Prozess für die NDC Conference Planner App (.NET MAUI).

## 🏗️ CI/CD Pipeline

### Automatisierte Builds

Die CI/CD-Pipeline ist über GitHub Actions konfiguriert und unterstützt:

- **Kontinuierliche Integration**: Automatische Builds und Tests bei Pull Requests
- **Kontinuierliche Bereitstellung**: Automatische Erstellung von Release-Artifacts
- **Multi-Platform Support**: Windows (MSIX) und Android (APK)

### Pipeline-Trigger

| Trigger | Aktion | Beschreibung |
|---------|--------|--------------|
| Push zu `main` | Build + Test + Deploy | Vollständige Pipeline |
| Push zu `develop` | Build + Test | Nur Validierung |
| Pull Request | Build + Test | Code-Validierung |
| Release Published | Build + Test + Deploy + Publish | Release-Erstellung |

## 🎯 Zielplattformen

### Windows 10/11
- **Format**: MSIX Package
- **Minimum Version**: Windows 10 Build 17763 (Version 1809)
- **Installation**: Über Microsoft Store oder Sideloading
- **Updates**: Automatisch über Store oder manuell

### Android
- **Format**: APK Package
- **Minimum Version**: Android 5.0 (API Level 21)
- **Target Version**: Android 14 (API Level 34)
- **Installation**: Sideloading (APK direkt installieren)

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

### 2. Continuous Integration

Bei jedem Push wird automatisch ausgeführt:

1. **Code Checkout**: Repository wird geladen
2. **Environment Setup**: .NET 8.0 SDK installiert
3. **Dependencies**: NuGet-Pakete wiederhergestellt
4. **Build**: Solution kompiliert (Release-Konfiguration)
5. **Tests**: Alle 99 Tests ausgeführt
6. **Artifacts**: Test-Ergebnisse gespeichert

### 3. Platform Builds (bei Main-Push oder Release)

#### Windows Build
- Läuft auf: `windows-latest` Runner
- MAUI Workload installiert
- MSIX Package erstellt
- Artifacts gespeichert

#### Android Build
- Läuft auf: `windows-latest` Runner
- MAUI Workload installiert
- Java JDK 11 Setup
- APK Package erstellt
- Artifacts gespeichert

### 4. Release-Erstellung

Bei einem GitHub Release:

1. **Download**: Platform-Artifacts heruntergeladen
2. **Package**: ZIP-Archive erstellt
3. **Upload**: Artifacts zum Release hinzugefügt
4. **Documentation**: Release Notes automatisch generiert

## 📦 Installation

### Windows

1. **MSIX Package herunterladen**
2. **Doppelklick auf .msix Datei**
3. **"Installieren" klicken**
4. **App erscheint im Startmenü**

**Voraussetzungen:**
- Windows 10 Version 1809 oder neuer
- Developer Mode aktiviert (für Sideloading)

### Android

1. **APK Datei herunterladen**
2. **"Installation aus unbekannten Quellen" aktivieren**
3. **APK Datei öffnen**
4. **"Installieren" bestätigen**

**Voraussetzungen:**
- Android 5.0 oder neuer
- Mindestens 100 MB freier Speicherplatz

## 🔄 Update-Strategie

### Automatische Updates
- **Windows**: Über Microsoft Store (wenn veröffentlicht)
- **Android**: Über Google Play Store (wenn veröffentlicht)

### Manuelle Updates
- **Windows**: Neue MSIX installieren (überschreibt alte Version)
- **Android**: Neue APK installieren (überschreibt alte Version)

## ⚡ Rollback-Strategie

### Windows Rollback

1. **Deinstallation**:
   - `Einstellungen > Apps > NDC Conference Planner > Deinstallieren`
   
2. **Vorherige Version installieren**:
   - Vorherige MSIX-Datei aus Releases herunterladen
   - Normal installieren

### Android Rollback

1. **Deinstallation**:
   - `Einstellungen > Apps > NDC Conference Planner > Deinstallieren`
   
2. **Vorherige Version installieren**:
   - Vorherige APK-Datei installieren
   - Daten bleiben erhalten (außer bei App-Data-Incompatibilität)

### Datenbank-Migration

Bei Breaking Changes in der Datenstruktur:

```csharp
// Beispiel für Datenbank-Migration
public void MigrateFromVersion(string fromVersion, string toVersion)
{
    if (fromVersion == "1.0.0" && toVersion == "1.1.0")
    {
        // Migration Logic hier
    }
}
```

## 🔧 Troubleshooting

### Häufige Probleme

#### Windows

**Problem**: MSIX Installation schlägt fehl
- **Lösung**: Developer Mode aktivieren oder Zertifikat installieren

**Problem**: App startet nicht
- **Lösung**: Windows Update durchführen, .NET Desktop Runtime installieren

#### Android

**Problem**: APK Installation blockiert
- **Lösung**: "Unbekannte Quellen" in den Sicherheitseinstellungen aktivieren

**Problem**: App crasht beim Start
- **Lösung**: Android Version prüfen (mind. 5.0), Speicherplatz freigeben

### Logs und Debugging

#### Lokale Logs
- **Windows**: `%LOCALAPPDATA%\Packages\com.ndcapp.conferenceplanner_*\LocalState\logs`
- **Android**: Via `adb logcat` oder in der App über Debug-Menü

#### CI/CD Logs
- GitHub Actions: Workflow-Logs in Repository
- Test Results: Downloadbare Artifacts nach jedem Build

## 📊 Monitoring

### Build-Status
- Badge im README.md zeigt aktuellen Pipeline-Status
- GitHub Actions Dashboard für detaillierte Logs

### Releases
- GitHub Releases für Download-Statistiken
- Artifact-Download-Counter

### Performance
- App-Start-Zeit: < 3 Sekunden
- Memory Usage: < 200 MB
- Package-Größe: Windows ~50MB, Android ~30MB

## 🛡️ Security Considerations

### Code Signing
- **Windows**: MSIX signiert mit verfügbarem Zertifikat
- **Android**: APK signiert mit Debug-Key (für Entwicklung)

**Produktionshinweis**: Für Store-Veröffentlichung sind entsprechende Certificates/Keys erforderlich.

### Permissions
- **Windows**: Standardberechtigungen (Dateisystem, Netzwerk)
- **Android**: Minimalberechtigungen (Speicher, Benachrichtigungen)

## 🔗 Weiterführende Links

- [.NET MAUI Dokumentation](https://docs.microsoft.com/en-us/dotnet/maui/)
- [GitHub Actions Dokumentation](https://docs.github.com/en/actions)
- [MSIX Packaging](https://docs.microsoft.com/en-us/windows/msix/)
- [Android APK Deployment](https://developer.android.com/studio/publish)