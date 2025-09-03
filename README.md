# NdcApp - Conference Planning Application

Eine .NET MAUI-Anwendung zur Planung und Verwaltung von Konferenzteilnahmen.

## 🚀 Features

- **CSV-Import**: Lade Konferenz-Talks aus CSV-Dateien
- **Talk-Auswahl**: Wähle interessante Talks für deinen persönlichen Plan
- **Sortierung**: Sortiere nach Sprecher, Kategorie oder chronologisch
- **Persistierung**: Deine Auswahl wird automatisch gespeichert
- **Nächster Talk**: Siehe auf einen Blick, welcher Talk als nächstes ansteht
- **NDC-Design**: Ansprechendes UI im NDC-Corporate-Design

## 📱 Screenshots

*Screenshots folgen*

## 🏗️ Projektstruktur

```
NdcApp/
├── NdcApp/                 # Haupt-MAUI-Anwendung
│   ├── MainPage.xaml       # Startseite mit Next-Talk-Info
│   ├── ConferencePlanPage.xaml # Talk-Auswahl und -Verwaltung
│   └── Converters/         # UI-Konverter
├── NdcApp.Core/           # Business Logic
│   ├── Models/            # Talk-Datenmodell
│   └── Services/          # CSV-Service, Plan-Service
└── NdcApp.Tests/          # Umfangreiche Test-Suite (52 Tests)
```

## 🛠️ Setup und Installation

### Voraussetzungen

- .NET 8.0 SDK
- Visual Studio 2022 mit MAUI-Workload oder
- Visual Studio Code mit C# Dev Kit

### Entwicklungsumgebung einrichten

1. **Repository klonen**
   ```bash
   git clone https://github.com/rsutter98/NdcApp.git
   cd NdcApp
   ```

2. **Dependencies installieren**
   ```bash
   dotnet restore
   ```

3. **Build ausführen**
   ```bash
   dotnet build
   ```

4. **Tests ausführen**
   ```bash
   dotnet test
   ```

5. **App starten** (für Windows)
   ```bash
   dotnet run --project NdcApp --framework net8.0-windows10.0.19041.0
   ```

### CSV-Datenformat

Die App erwartet CSV-Dateien mit folgendem Format:

```csv
Day,StartTime,EndTime,Room,Title,Speaker,Category
Monday,09:00,10:00,Room A,Keynote: The Future of AI,John Doe,Keynote
Monday,10:30,11:30,Room B,Building Modern APIs,Jane Smith,Backend
...
```

**Spalten:**
- `Day`: Wochentag (Monday, Tuesday, ...)
- `StartTime`: Startzeit (HH:MM Format)
- `EndTime`: Endzeit (HH:MM Format)
- `Room`: Raum-Bezeichnung
- `Title`: Talk-Titel
- `Speaker`: Sprecher-Name
- `Category`: Kategorie/Track

## 🧪 Testing

Das Projekt verfügt über eine umfangreiche Test-Suite:

- **52 Tests** decken alle Core-Funktionalitäten ab
- **Unit Tests** für Business Logic
- **Integration Tests** für End-to-End-Szenarien
- **UI-Converter Tests** für XAML-Bindings

```bash
# Alle Tests ausführen
dotnet test

# Tests mit detaillierter Ausgabe
dotnet test --verbosity detailed

# Spezifische Test-Klasse ausführen
dotnet test --filter "ClassName=TalkServiceTests"
```

## 🚀 Deployment

### Android
```bash
dotnet publish NdcApp -f net8.0-android -c Release
```

### Windows
```bash
dotnet publish NdcApp -f net8.0-windows10.0.19041.0 -c Release
```

## 📋 Bekannte Probleme

- XAML-Binding-Warnungen (geplant zu beheben)
- Noch keine iOS-Unterstützung konfiguriert
- Performance bei sehr großen CSV-Dateien nicht getestet

## 🗺️ Roadmap

Siehe [ROADMAP.md](ROADMAP.md) für detaillierte Informationen zu geplanten Features und nächsten Schritten.

### Nächste Features
- Such-/Filter-Funktionalität
- Push-Benachrichtigungen für anstehende Talks
- Dark Mode
- Multi-Konferenz-Unterstützung

## 🤝 Beitragen

1. Fork das Repository
2. Erstelle einen Feature-Branch (`git checkout -b feature/amazing-feature`)
3. Committe deine Änderungen (`git commit -m 'Add amazing feature'`)
4. Push zum Branch (`git push origin feature/amazing-feature`)
5. Öffne eine Pull Request

### Entwicklungsrichtlinien

- Alle neuen Features benötigen Tests
- Code-Style entsprechend bestehender Konventionen
- XAML-Bindings mit x:DataType für Performance
- Deutsche Kommentare und UI-Texte (NDC Copenhagen Context)

## 📄 Lizenz

*Lizenz-Information folgt*

## 📞 Support

Bei Fragen oder Problemen erstelle bitte ein Issue im GitHub-Repository.

---

**Status**: Aktive Entwicklung  
**Version**: 1.0.0-preview  
**Zielplattformen**: Android, Windows  
**Letzte Aktualisierung**: Januar 2025