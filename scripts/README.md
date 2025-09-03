# GitHub Issues aus Roadmap erstellen

Dieses Repository enthält Tools zur automatischen Erstellung von GitHub Issues basierend auf der [Roadmap](../ROADMAP.md).

## 📋 Übersicht

Aus der Roadmap wurden **22 Issues** generiert, die alle noch nicht erledigten Aufgaben (- [ ]) abdecken:

- **6 hohe Priorität Issues** - Sofortige Umsetzung empfohlen
- **11 mittlere Priorität Issues** - Kurz- bis mittelfristig
- **5 niedrige Priorität Issues** - Langfristige Ziele

## 🚀 Schnellstart

### Option 1: Automatische Erstellung mit GitHub CLI

```bash
# 1. In das scripts Verzeichnis wechseln
cd scripts/generated-issues

# 2. GitHub CLI installieren (falls nicht vorhanden)
# https://cli.github.com/

# 3. Bei GitHub anmelden
gh auth login

# 4. Alle Issues automatisch erstellen
./create-issues.sh
```

### Option 2: Manuelle Erstellung

1. Gehe zu den [generierten Issue-Dateien](./generated-issues/)
2. Öffne eine Issue-Datei (z.B. `issue-001-...md`)
3. Kopiere den Inhalt
4. Erstelle neue GitHub Issue
5. Füge den Inhalt ein und setze die Labels

### Option 3: Selektive Erstellung

```bash
# Nur hohe Priorität Issues erstellen
cd scripts/generated-issues

# Einzelne Issues erstellen
gh issue create \
  --title "Nullable Reference Warnings beheben" \
  --body-file "issue-001-nullable-reference-warnings-beheben.md" \
  --label "enhancement,priority-high,roadmap"
```

## 📊 Issue-Kategorien

### Hohe Priorität (Sofort) 🔥
1. **Nullable Reference Warnings beheben** - Code-Qualität
2. **XAML-Binding-Optimierung** - Performance 
3. **README.md erstellen** - Dokumentation
4. **Screenshots hinzufügen** - Dokumentation
5. **Platform-spezifische Builds** - Deployment
6. **Release-Pipeline** - CI/CD

### Mittlere Priorität (1-2 Monate) 🎯
- Datenquellen erweitern
- Benutzerinteraktion verbessern  
- Benachrichtigungen
- Responsive Design
- Accessibility
- Performance-Optimierungen
- Dependency Injection
- Error Handling

### Niedrige Priorität (3+ Monate) 📅
- Multi-Konferenz-Support
- Cloud-Synchronisation
- Real-time Updates
- Code-Analyse-Tools

## 🏷️ Labels-System

Jede Issue hat automatisch passende Labels:

- **enhancement** - Feature-Verbesserungen
- **code-quality** - Code-Qualitäts-Verbesserungen  
- **documentation** - Dokumentations-Tasks
- **deployment** - Deployment-bezogene Tasks
- **priority-high/medium/low** - Prioritätsstufen
- **roadmap** - Alle aus Roadmap generierten Issues

## 🔧 Anpassung der Issues

### Issue-Titel und -Beschreibung anpassen

1. Bearbeite die entsprechende `.md` Datei im `generated-issues/` Verzeichnis
2. Erstelle die Issue manuell mit dem angepassten Inhalt

### Labels anpassen

Editiere die Labels im `create-issues.sh` Script oder beim manuellen Erstellen.

### Prioritäten ändern

Die Prioritäten basieren auf der Roadmap-Struktur. Bei Bedarf können sie vor der Erstellung angepasst werden.

## 📝 Issue-Templates

Das Repository enthält auch vordefinierte Issue-Templates in `.github/ISSUE_TEMPLATE/`:

- **roadmap-feature.md** - Allgemeine Roadmap-Features
- **code-quality.md** - Code-Qualitäts-Verbesserungen  
- **documentation.md** - Dokumentations-Tasks
- **deployment.md** - Deployment-bezogene Aufgaben

## 🔄 Workflow-Empfehlung

1. **Phase 1: Sofortige Fixes** (Diese Woche)
   - Nullable Reference Warnings beheben
   - README.md erstellen  
   - Screenshots hinzufügen

2. **Phase 2: Deployment Setup** (Nächste Woche)
   - Platform-spezifische Builds
   - Release-Pipeline einrichten

3. **Phase 3: Code-Qualität** (Laufend)
   - XAML-Binding-Optimierung
   - Dependency Injection
   - Error Handling

4. **Phase 4: Features** (Nach Business-Value priorisieren)
   - Benutzerinteraktion verbessern
   - Benachrichtigungen
   - Responsive Design

## 📋 Nachverfolgung

### Issue-Progress verfolgen

```bash
# Alle Roadmap-Issues anzeigen
gh issue list --label roadmap

# Issues nach Priorität filtern  
gh issue list --label priority-high
gh issue list --label priority-medium
gh issue list --label priority-low

# Status-Übersicht
gh issue list --label roadmap --state all
```

### Roadmap aktualisieren

Wenn Issues abgeschlossen sind:

1. Markiere sie in der `ROADMAP.md` als erledigt: `- [x]`
2. Regeneriere Issues falls nötig: `npm run generate`
3. Erstelle neue Issues für zusätzliche Tasks

## 🤖 Automatisierung

### GitHub Actions Integration

Erstelle `.github/workflows/roadmap-sync.yml`:

```yaml
name: Roadmap Sync
on:
  push:
    paths: ['ROADMAP.md']
    
jobs:
  sync-issues:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Generate Issues
        run: |
          cd scripts
          node generate-issues.js
      # Optional: Auto-create new issues
```

### Periodische Reviews

- **Wöchentlich:** Prioritäten überprüfen
- **Monatlich:** Roadmap aktualisieren  
- **Quarterly:** Issue-Kategorien überdenken

## 🆘 Troubleshooting

### GitHub CLI Probleme

```bash
# GitHub CLI Status prüfen
gh auth status

# Neu anmelden
gh auth login --web

# Repository-Kontext prüfen
gh repo view
```

### Script-Fehler

```bash
# Node.js Version prüfen
node --version  # Mindestens v14

# Script-Berechtigungen
chmod +x create-issues.sh

# Trockenlauf ohne tatsächliche Erstellung
# Kommentiere die gh-Befehle im Script aus
```

### Rate Limiting

Das Script enthält automatische Pausen. Bei Problemen:

- Reduziere die Anzahl paralleler Issues
- Erhöhe die Sleep-Zeit im Script
- Verwende persönliche Access Tokens

## 📞 Support

Bei Fragen oder Problemen:

1. Prüfe die [GitHub CLI Dokumentation](https://cli.github.com/manual/)
2. Erstelle eine Issue in diesem Repository
3. Überprüfe die Roadmap auf Updates

---

**Generiert:** $(date)  
**Version:** 1.0.0  
**Issues:** 22 total (6 high, 11 medium, 5 low priority)