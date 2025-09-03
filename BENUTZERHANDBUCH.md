# NdcApp - Benutzerhandbuch

Eine ausführliche Anleitung zur Nutzung der NdcApp für Konferenzteilnehmer.

## 📱 Schnellstart

### 1. Installation
- **Windows**: Laden Sie die `.msix` Datei herunter und installieren Sie sie
- **Android**: Laden Sie die `.apk` Datei herunter und aktivieren Sie "Installation aus unbekannten Quellen"
- **Apple/iOS**: Verwenden Sie TestFlight für Beta-Installation oder installieren Sie die `.ipa` Datei über Xcode

### 2. Erster Start
1. Öffnen Sie die NdcApp
2. Sie sehen die Hauptseite mit dem NDC Copenhagen Design
3. Tippen Sie auf "Go to My Conference Plan"

## 🎯 Hauptfunktionen

### Talk-Suche und -Auswahl

**Talks finden:**
- Nutzen Sie die Suchleiste für Titel, Sprecher oder Kategorien
- Beispiele:
  - "AI" → findet alle KI-bezogenen Talks
  - "Jodie Burchell" → findet Talks dieser Sprecherin
  - "Backend" → findet Backend-Development Talks

**Talks zu Ihrem Plan hinzufügen:**
- **Methode 1**: Tippen Sie auf "Select Talk"
- **Methode 2**: Wischen Sie nach rechts über einen Talk
- **Entfernen**: Tippen Sie auf "Deselect" oder wischen nach links

### Persönlichen Konferenzplan verwalten

**Ihren Plan anzeigen:**
- Tippen Sie auf "Meine Talks" um nur ausgewählte Talks zu sehen
- Die Hauptseite zeigt immer Ihren nächsten geplanten Talk

**Plan organisieren:**
- Sortieren Sie nach:
  - **Standard**: Chronologische Reihenfolge
  - **Presenter**: Alphabetisch nach Sprecher
  - **Category**: Gruppiert nach Themenbereichen
  - **Rating**: Nach Bewertungen sortiert

### Bewertungen und Empfehlungen

**Talks bewerten:**
- Tippen Sie auf die Sterne unter einem Talk
- Bewerten Sie von 1-5 Sternen
- Ihre Bewertungen werden automatisch gespeichert

**Empfehlungen nutzen:**
- Basierend auf Ihren Bewertungen erhalten Sie passende Talk-Vorschläge
- Hoch bewertete Talks werden in der Sortierung bevorzugt

### Benachrichtigungen

**Push-Notifications aktivieren:**
- Tippen Sie auf das 🔔 Symbol
- Aktivieren Sie Benachrichtigungen für:
  - Anstehende Talks (15 und 5 Minuten vorher)
  - Programm-Änderungen
  - Raum-Wechsel

## 💡 Tipps für optimale Nutzung

### Vorbereitung vor der Konferenz
1. **Vollständigen Plan erstellen**: Durchsuchen Sie alle Talks und wählen Sie interessante aus
2. **Zeitkonflikte prüfen**: Achten Sie auf überlappende Zeiten
3. **Backup-Talks**: Wählen Sie alternative Talks für jeden Zeitslot
4. **Benachrichtigungen testen**: Stellen Sie sicher, dass Push-Notifications funktionieren

### Während der Konferenz
1. **Morgen-Routine**: Checken Sie Ihren Tagesplan mit "Meine Talks"
2. **Zwischen Talks**: Bewerten Sie besuchte Talks sofort
3. **Spontane Änderungen**: Nutzen Sie die Suche für Last-Minute-Entscheidungen
4. **Nächster Talk**: Die Hauptseite zeigt immer was als nächstes ansteht

### Nach jedem Talk
1. **Sofort bewerten**: Geben Sie 1-5 Sterne ab solange der Eindruck frisch ist
2. **Plan anpassen**: Fügen Sie neue interessante Talks hinzu oder entfernen Sie weniger relevante

## 🛠️ Problemlösung

### Häufige Probleme

**Talk-Liste ist leer oder veraltet:**
- Ziehen Sie die Liste nach unten (Pull-to-Refresh)
- Überprüfen Sie Ihre Internetverbindung
- Starten Sie die App neu

**Benachrichtigungen kommen nicht an:**
- Überprüfen Sie die App-Berechtigungen
- Deaktivieren Sie "Nicht stören" während der Konferenz
- Prüfen Sie die Benachrichtigungseinstellungen in der App

**Bewertungen werden nicht gespeichert:**
- Warten Sie kurz nach der Bewertung bevor Sie zur nächsten Aktion wechseln
- Bei anhaltenden Problemen: App komplett schließen und neu starten

**App reagiert langsam:**
- Schließen Sie andere Apps um Speicher freizugeben
- Überprüfen Sie verfügbaren Speicherplatz auf dem Gerät

### Kontakt bei Problemen
- GitHub Issues: [Bug Report erstellen](https://github.com/rsutter98/NdcApp/issues)
- Für allgemeine Fragen: [GitHub Discussions](https://github.com/rsutter98/NdcApp/discussions)

## 📊 Datenformat

Die App arbeitet mit CSV-Dateien im folgenden Format:

```csv
Datum,Startzeit,Endzeit,Raum,Titel,Speaker,Kategorie
Wednesday,09:00,10:00,1,Keynote: AI is having its moment ... again,Jodie Burchell,Talk
Wednesday,10:20,11:20,1,Java Sucks (So C# Didn't Have To),Adele Carpenter,Talk
```

**Spalten-Erklärung:**
- **Datum**: Wochentag (Monday, Tuesday, Wednesday, ...)
- **Startzeit**: Format HH:MM (z.B. 09:00)
- **Endzeit**: Format HH:MM (z.B. 10:00)
- **Raum**: Raum-Nummer oder -Name
- **Titel**: Vollständiger Talk-Titel
- **Speaker**: Name des Sprechers/der Sprecherin
- **Kategorie**: Themenbereich (Talk, Workshop, Keynote, ...)

## 🎨 App-Design

Die NdcApp verwendet das offizielle NDC Copenhagen Design:
- **Primärfarbe**: Dunkles Blau (#0A2342)
- **Akzentfarbe**: NDC-Gelb (#FFB400)
- **Textfarbe**: Weiß (#FFFFFF)
- **Schriftart**: System-Standard mit verschiedenen Gewichtungen

Dies sorgt für eine konsistente und wiedererkennbare Nutzererfahrung im NDC-Stil.

## 📈 Statistiken

Die App bietet folgende Metriken:
- Anzahl ausgewählter Talks
- Durchschnittliche Bewertung Ihrer besuchten Talks
- Kategorien-Verteilung Ihrer Auswahl
- Bewertungshistorie

## 🔄 Updates und Synchronisation

**Automatische Updates:**
- Talk-Daten werden beim Pull-to-Refresh aktualisiert
- Bewertungen und Auswahlen werden lokal gespeichert
- Bei verfügbaren App-Updates erhalten Sie eine Benachrichtigung

**Daten-Backup:**
- Alle Ihre Auswahlen und Bewertungen werden lokal gespeichert
- Bei App-Neuinstallation gehen diese Daten verloren
- Zukünftig: Cloud-Synchronisation geplant

---

**Version**: 1.0.0-preview  
**Letzte Aktualisierung**: Januar 2025  
**Support**: [GitHub Repository](https://github.com/rsutter98/NdcApp)