#!/bin/bash

# GitHub Issues Auto-Creation Script
# Generiert: 2025-09-03T16:07:15.944Z

set -e

echo "🚀 Erstelle GitHub Issues aus Roadmap..."

echo "📋 Erstelle high Priorität Issues..."

echo "Creating issue 1/22: Nullable Reference Warnings beheben"
gh issue create \
  --title "Nullable Reference Warnings beheben" \
  --body-file "issue-001-nullable-reference-warnings-beheben.md" \
  --label "enhancement","priority-high","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Nullable Reference Warnings beheben"

echo "Creating issue 2/22: XAML-Binding-Optimierung"
gh issue create \
  --title "XAML-Binding-Optimierung" \
  --body-file "issue-002-xaml-binding-optimierung.md" \
  --label "enhancement","priority-high","roadmap" \
  --assignee "@me" || echo "Failed to create issue: XAML-Binding-Optimierung"

echo "Creating issue 3/22: README.md erstellen"
gh issue create \
  --title "README.md erstellen" \
  --body-file "issue-003-readme-md-erstellen.md" \
  --label "enhancement","priority-high","roadmap" \
  --assignee "@me" || echo "Failed to create issue: README.md erstellen"

echo "Creating issue 4/22: Screenshots hinzufügen"
gh issue create \
  --title "Screenshots hinzufügen" \
  --body-file "issue-004-screenshots-hinzufuegen.md" \
  --label "enhancement","priority-high","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Screenshots hinzufügen"

echo "Creating issue 5/22: Platform-spezifische Builds"
gh issue create \
  --title "Platform-spezifische Builds" \
  --body-file "issue-005-platform-spezifische-builds.md" \
  --label "enhancement","priority-high","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Platform-spezifische Builds"

echo "Creating issue 6/22: Release-Pipeline"
gh issue create \
  --title "Release-Pipeline" \
  --body-file "issue-006-release-pipeline.md" \
  --label "enhancement","priority-high","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Release-Pipeline"

echo "📋 Erstelle medium Priorität Issues..."

echo "Creating issue 7/22: Datenquellen erweitern"
gh issue create \
  --title "Datenquellen erweitern" \
  --body-file "issue-007-datenquellen-erweitern.md" \
  --label "enhancement","priority-medium","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Datenquellen erweitern"

echo "Creating issue 8/22: Benutzerinteraktion verbessern"
gh issue create \
  --title "Benutzerinteraktion verbessern" \
  --body-file "issue-008-benutzerinteraktion-verbessern.md" \
  --label "enhancement","priority-medium","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Benutzerinteraktion verbessern"

echo "Creating issue 9/22: Benachrichtigungen"
gh issue create \
  --title "Benachrichtigungen" \
  --body-file "issue-009-benachrichtigungen.md" \
  --label "enhancement","priority-medium","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Benachrichtigungen"

echo "Creating issue 10/22: Responsive Design"
gh issue create \
  --title "Responsive Design" \
  --body-file "issue-010-responsive-design.md" \
  --label "enhancement","priority-medium","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Responsive Design"

echo "⏸️  Kurze Pause für Rate Limiting..."
sleep 5

echo "Creating issue 11/22: Accessibility"
gh issue create \
  --title "Accessibility" \
  --body-file "issue-011-accessibility.md" \
  --label "enhancement","priority-medium","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Accessibility"

echo "Creating issue 12/22: Große Datenmengen"
gh issue create \
  --title "Große Datenmengen" \
  --body-file "issue-012-grosse-datenmengen.md" \
  --label "enhancement","priority-medium","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Große Datenmengen"

echo "Creating issue 13/22: Startup-Zeit"
gh issue create \
  --title "Startup-Zeit" \
  --body-file "issue-013-startup-zeit.md" \
  --label "enhancement","priority-medium","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Startup-Zeit"

echo "Creating issue 14/22: Dependency Injection"
gh issue create \
  --title "Dependency Injection" \
  --body-file "issue-019-dependency-injection.md" \
  --label "tech-debt","priority-medium","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Dependency Injection"

echo "Creating issue 15/22: Error Handling"
gh issue create \
  --title "Error Handling" \
  --body-file "issue-020-error-handling.md" \
  --label "tech-debt","priority-medium","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Error Handling"

echo "Creating issue 16/22: Code-Architektur"
gh issue create \
  --title "Code-Architektur" \
  --body-file "issue-021-code-architektur.md" \
  --label "tech-debt","priority-medium","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Code-Architektur"

echo "Creating issue 17/22: Code-Analyse"
gh issue create \
  --title "Code-Analyse" \
  --body-file "issue-022-code-analyse.md" \
  --label "tech-debt","priority-medium","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Code-Analyse"

echo "📋 Erstelle low Priorität Issues..."

echo "Creating issue 18/22: Konferenz-Management"
gh issue create \
  --title "Konferenz-Management" \
  --body-file "issue-014-konferenz-management.md" \
  --label "enhancement","priority-low","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Konferenz-Management"

echo "Creating issue 19/22: Cloud-Synchronisation"
gh issue create \
  --title "Cloud-Synchronisation" \
  --body-file "issue-015-cloud-synchronisation.md" \
  --label "enhancement","priority-low","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Cloud-Synchronisation"

echo "Creating issue 20/22: Talk-Bewertungen"
gh issue create \
  --title "Talk-Bewertungen" \
  --body-file "issue-016-talk-bewertungen.md" \
  --label "enhancement","priority-low","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Talk-Bewertungen"

echo "⏸️  Kurze Pause für Rate Limiting..."
sleep 5

echo "Creating issue 21/22: Conference-Management Portal"
gh issue create \
  --title "Conference-Management Portal" \
  --body-file "issue-017-conference-management-portal.md" \
  --label "enhancement","priority-low","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Conference-Management Portal"

echo "Creating issue 22/22: Real-time Updates"
gh issue create \
  --title "Real-time Updates" \
  --body-file "issue-018-real-time-updates.md" \
  --label "enhancement","priority-low","roadmap" \
  --assignee "@me" || echo "Failed to create issue: Real-time Updates"

echo "✅ Issue-Erstellung abgeschlossen!"
echo "📊 Überprüfe die erstellten Issues: gh issue list"
