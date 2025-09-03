#!/usr/bin/env python3
"""
GitHub Issue Generator für NdcApp Roadmap (Python Version)

Einfachere Alternative zum Node.js Script für Benutzer die Python bevorzugen.
"""

import os
import re
import json
from pathlib import Path
from typing import List, Dict, Any
from datetime import datetime


class RoadmapIssueGenerator:
    def __init__(self, roadmap_path: str = "../ROADMAP.md", output_dir: str = "./python-generated-issues"):
        self.roadmap_path = Path(roadmap_path)
        self.output_dir = Path(output_dir)
        self.issues = []
        
    def generate_issues(self):
        """Hauptfunktion zur Generierung aller Issues"""
        print("🚀 Starte GitHub Issue Generierung (Python)...")
        
        # Roadmap laden und parsen
        roadmap_content = self._load_roadmap()
        self.issues = self._parse_roadmap(roadmap_content)
        
        # Ausgabe-Verzeichnis erstellen
        self.output_dir.mkdir(exist_ok=True, parents=True)
        
        # Issues generieren
        self._generate_issue_files()
        self._generate_summary()
        self._generate_github_cli_script()
        
        print(f"✅ {len(self.issues)} Issues erfolgreich generiert!")
        print(f"📁 Ausgabeverzeichnis: {self.output_dir}")
        
    def _load_roadmap(self) -> str:
        """Lädt die Roadmap-Datei"""
        if not self.roadmap_path.exists():
            raise FileNotFoundError(f"Roadmap-Datei nicht gefunden: {self.roadmap_path}")
        
        with open(self.roadmap_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        print("📖 Roadmap-Datei geladen")
        return content
        
    def _parse_roadmap(self, content: str) -> List[Dict[str, Any]]:
        """Parst die Roadmap und extrahiert Tasks"""
        lines = content.split('\n')
        issues = []
        current_section = ""
        current_subsection = ""
        
        for i, line in enumerate(lines):
            line = line.strip()
            
            # Section-Header erkennen
            if line.startswith('## '):
                current_section = re.sub(r'[🎯🚀🏗️🔧📊🚦📞💡]', '', line.replace('## ', '')).strip()
                continue
                
            if line.startswith('### '):
                current_subsection = re.sub(r'[0-9.]', '', line.replace('### ', '')).strip()
                continue
                
            # Tasks erkennen (- [ ] Format)
            if line.startswith('- [ ] ') and self._should_process_section(current_section):
                task_text = line.replace('- [ ] ', '').replace('**', '').strip()
                
                if len(task_text) >= 10:  # Mindestlänge für valide Tasks
                    issue = {
                        'title': self._clean_title(task_text),
                        'body': self._generate_body(task_text, current_section, current_subsection),
                        'labels': self._generate_labels(current_section),
                        'priority': self._determine_priority(current_section),
                        'section': current_section,
                        'subsection': current_subsection
                    }
                    issues.append(issue)
        
        print(f"📋 {len(issues)} Tasks aus Roadmap extrahiert")
        return issues
        
    def _should_process_section(self, section: str) -> bool:
        """Prüft ob eine Section verarbeitet werden soll"""
        process_sections = [
            'Kurzzeitige Ziele',
            'Mittelfristige Ziele',
            'Langfristige Ziele', 
            'Technische Schulden',
            'Nächste Schritte'
        ]
        return any(s in section for s in process_sections)
        
    def _clean_title(self, title: str) -> str:
        """Bereinigt und kürzt den Titel"""
        title = re.sub(r'\*\*(.*?)\*\*', r'\1', title)  # Bold formatting entfernen
        if len(title) > 80:
            title = title[:77] + '...'
        return title
        
    def _generate_body(self, task_text: str, section: str, subsection: str) -> str:
        """Generiert den Issue-Body"""
        body = f"## Beschreibung\n\n{task_text}\n\n"
        
        if subsection:
            body += f"**Bereich:** {subsection}\n"
        body += f"**Roadmap-Kategorie:** {section}\n\n"
        
        body += "## Akzeptanzkriterien\n\n"
        body += "- [ ] Feature/Verbesserung ist implementiert\n"
        body += "- [ ] Tests sind vorhanden (falls anwendbar)\n"
        body += "- [ ] Dokumentation ist aktualisiert\n"
        body += "- [ ] Code Review ist durchgeführt\n\n"
        
        body += "## Zusätzliche Informationen\n\n"
        body += "Diese Issue wurde automatisch aus der [Roadmap](../ROADMAP.md) generiert.\n\n"
        body += "Siehe auch: [Vollständige Roadmap](../ROADMAP.md)"
        
        return body
        
    def _generate_labels(self, section: str) -> List[str]:
        """Generiert Labels basierend auf Section"""
        labels = ['roadmap']
        
        section_lower = section.lower()
        if 'code' in section_lower or 'qualität' in section_lower:
            labels.append('code-quality')
        elif 'dokumentation' in section_lower:
            labels.append('documentation')
        elif 'deployment' in section_lower:
            labels.append('deployment')
        else:
            labels.append('enhancement')
            
        # Priorität hinzufügen
        priority = self._determine_priority(section)
        labels.append(f'priority-{priority}')
        
        return labels
        
    def _determine_priority(self, section: str) -> str:
        """Bestimmt Priorität basierend auf Section"""
        if 'Kurzzeitige Ziele' in section:
            return 'high'
        elif 'Mittelfristige Ziele' in section or 'Technische Schulden' in section:
            return 'medium'
        elif 'Langfristige Ziele' in section:
            return 'low'
        return 'medium'
        
    def _generate_issue_files(self):
        """Generiert Issue-Dateien"""
        for i, issue in enumerate(self.issues):
            filename = f"issue-{i+1:03d}-{self._slugify(issue['title'])}.md"
            filepath = self.output_dir / filename
            
            content = f"# {issue['title']}\n\n"
            content += f"**Labels:** {', '.join(issue['labels'])}\n"
            content += f"**Priority:** {issue['priority']}\n\n"
            content += "---\n\n"
            content += issue['body']
            
            with open(filepath, 'w', encoding='utf-8') as f:
                f.write(content)
                
        print(f"📝 {len(self.issues)} Issue-Dateien generiert")
        
    def _generate_summary(self):
        """Generiert Summary-Report"""
        summary_path = self.output_dir / "SUMMARY.md"
        
        # Gruppierung nach Priorität
        by_priority = {}
        for issue in self.issues:
            priority = issue['priority']
            if priority not in by_priority:
                by_priority[priority] = []
            by_priority[priority].append(issue)
            
        content = "# GitHub Issues Summary (Python Generated)\n\n"
        content += f"Generiert: {datetime.now().strftime('%d.%m.%Y, %H:%M:%S')}\n\n"
        content += "## Übersicht\n\n"
        content += f"- **Gesamt Issues:** {len(self.issues)}\n"
        content += f"- **Hohe Priorität:** {len(by_priority.get('high', []))}\n"
        content += f"- **Mittlere Priorität:** {len(by_priority.get('medium', []))}\n"
        content += f"- **Niedrige Priorität:** {len(by_priority.get('low', []))}\n\n"
        
        for priority in ['high', 'medium', 'low']:
            if priority in by_priority:
                content += f"## {priority.title()} Priority Issues\n\n"
                for j, issue in enumerate(by_priority[priority]):
                    filename = f"issue-{self.issues.index(issue)+1:03d}-{self._slugify(issue['title'])}.md"
                    content += f"{j+1}. [{issue['title']}]({filename})\n"
                content += "\n"
                
        content += "## GitHub CLI Commands\n\n"
        content += "Siehe `create-issues.sh` für automatisierte Issue-Erstellung.\n"
        
        with open(summary_path, 'w', encoding='utf-8') as f:
            f.write(content)
            
        print(f"📊 Summary-Report erstellt: {summary_path}")
        
    def _generate_github_cli_script(self):
        """Generiert GitHub CLI Script"""
        script_path = self.output_dir / "create-issues.sh"
        
        script = "#!/bin/bash\n\n"
        script += "# GitHub Issues Auto-Creation Script (Python Generated)\n"
        script += f"# Generiert: {datetime.now().isoformat()}\n\n"
        script += "set -e\n\n"
        script += 'echo "🚀 Erstelle GitHub Issues aus Roadmap..."\n\n'
        
        for i, issue in enumerate(self.issues):
            filename = f"issue-{i+1:03d}-{self._slugify(issue['title'])}.md"
            title_escaped = issue['title'].replace('"', '\\"')
            labels_str = '","'.join(issue['labels'])
            
            script += f'echo "Creating issue {i+1}/{len(self.issues)}: {issue["title"]}"\n'
            script += "gh issue create \\\n"
            script += f'  --title "{title_escaped}" \\\n'
            script += f'  --body-file "{filename}" \\\n'
            script += f'  --label "{labels_str}" \\\n'
            script += f'  --assignee "@me" || echo "Failed to create issue: {issue["title"]}"\n\n'
            
            # Rate limiting
            if (i + 1) % 10 == 0:
                script += 'echo "⏸️  Kurze Pause für Rate Limiting..."\n'
                script += "sleep 5\n\n"
                
        script += 'echo "✅ Issue-Erstellung abgeschlossen!"\n'
        
        with open(script_path, 'w', encoding='utf-8') as f:
            f.write(script)
            
        # Ausführbar machen
        os.chmod(script_path, 0o755)
        
        print(f"🔧 Batch-Script erstellt: {script_path}")
        
    def _slugify(self, text: str) -> str:
        """Erstellt URL-freundlichen Slug"""
        # Deutsche Umlaute ersetzen
        replacements = {'ä': 'ae', 'ö': 'oe', 'ü': 'ue', 'ß': 'ss'}
        for old, new in replacements.items():
            text = text.replace(old, new)
            
        # Nur alphanumerische Zeichen und Bindestriche
        text = re.sub(r'[^a-zA-Z0-9]+', '-', text.lower())
        text = text.strip('-')
        
        return text[:50]  # Maximale Länge


if __name__ == "__main__":
    generator = RoadmapIssueGenerator()
    generator.generate_issues()