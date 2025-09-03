#!/usr/bin/env node

/**
 * GitHub Issue Generator für NdcApp Roadmap
 * 
 * Dieses Script parst die ROADMAP.md und generiert formatierte GitHub Issues
 * für alle noch nicht erledigten Aufgaben.
 */

const fs = require('fs');
const path = require('path');

// Konfiguration
const ROADMAP_FILE = '../ROADMAP.md';
const OUTPUT_DIR = './generated-issues';
const MAX_ISSUES_PER_BATCH = 20; // GitHub Rate Limiting berücksichtigen

// Issue-Kategorien basierend auf Roadmap-Struktur
const ISSUE_CATEGORIES = {
  'code-quality': {
    label: 'code-quality',
    template: 'code-improvement',
    priority: 'high'
  },
  'documentation': {
    label: 'documentation',
    template: 'documentation',
    priority: 'medium'
  },
  'deployment': {
    label: 'deployment',
    template: 'feature',
    priority: 'high'
  },
  'feature': {
    label: 'enhancement',
    template: 'feature',
    priority: 'medium'
  },
  'ui-ux': {
    label: 'ui/ux',
    template: 'feature',
    priority: 'medium'
  },
  'performance': {
    label: 'performance',
    template: 'code-improvement',
    priority: 'high'
  },
  'technical-debt': {
    label: 'tech-debt',
    template: 'code-improvement',
    priority: 'low'
  }
};

// Prioritäts-Mapping
const PRIORITY_MAPPING = {
  'Kurzzeitige Ziele': 'high',
  'Mittelfristige Ziele': 'medium', 
  'Langfristige Ziele': 'low',
  'Technische Schulden': 'medium'
};

class IssueGenerator {
  constructor() {
    this.roadmapContent = '';
    this.issues = [];
    this.currentSection = '';
    this.currentSubsection = '';
  }

  /**
   * Hauptfunktion zur Generierung aller Issues
   */
  async generateAllIssues() {
    try {
      console.log('🚀 Starte GitHub Issue Generierung...');
      
      this.loadRoadmap();
      this.parseRoadmap();
      this.createOutputDirectory();
      this.generateIssueFiles();
      this.generateSummaryReport();
      this.generateBatchCommands();
      
      console.log(`✅ ${this.issues.length} Issues erfolgreich generiert!`);
      console.log(`📁 Ausgabeverzeichnis: ${OUTPUT_DIR}`);
      
    } catch (error) {
      console.error('❌ Fehler bei der Issue-Generierung:', error.message);
      process.exit(1);
    }
  }

  /**
   * Lädt die Roadmap-Datei
   */
  loadRoadmap() {
    const roadmapPath = path.resolve(__dirname, ROADMAP_FILE);
    
    if (!fs.existsSync(roadmapPath)) {
      throw new Error(`Roadmap-Datei nicht gefunden: ${roadmapPath}`);
    }
    
    this.roadmapContent = fs.readFileSync(roadmapPath, 'utf8');
    console.log('📖 Roadmap-Datei geladen');
  }

  /**
   * Parst die Roadmap und extrahiert alle Tasks
   */
  parseRoadmap() {
    const lines = this.roadmapContent.split('\n');
    let currentSection = '';
    let currentSubsection = '';
    let currentCategory = 'feature';
    let isInTaskList = false;
    
    for (let i = 0; i < lines.length; i++) {
      const line = lines[i].trim();
      
      // Section-Header erkennen (## oder ###)
      if (line.startsWith('## ')) {
        currentSection = line.replace('## ', '').replace(/[🎯🚀🏗️🔧📊🚦📞💡]/g, '').trim();
        currentCategory = this.determineCategoryFromSection(currentSection);
        isInTaskList = this.shouldProcessSection(currentSection);
        continue;
      }
      
      if (line.startsWith('### ')) {
        currentSubsection = line.replace('### ', '').replace(/[0-9.]/g, '').trim();
        continue;
      }
      
      // Tasks erkennen (- [ ] Format)
      if (isInTaskList && line.startsWith('- [ ] ')) {
        const task = this.parseTask(line, lines, i, currentSection, currentSubsection, currentCategory);
        if (task) {
          this.issues.push(task);
        }
      }
    }
    
    console.log(`📋 ${this.issues.length} Tasks aus Roadmap extrahiert`);
  }

  /**
   * Bestimmt die Kategorie basierend auf dem Section-Namen
   */
  determineCategoryFromSection(section) {
    const sectionLower = section.toLowerCase();
    
    if (sectionLower.includes('code') || sectionLower.includes('qualität')) {
      return 'code-quality';
    }
    if (sectionLower.includes('dokumentation') || sectionLower.includes('readme')) {
      return 'documentation';
    }
    if (sectionLower.includes('deployment') || sectionLower.includes('build')) {
      return 'deployment';
    }
    if (sectionLower.includes('ui') || sectionLower.includes('ux') || sectionLower.includes('design')) {
      return 'ui-ux';
    }
    if (sectionLower.includes('performance') || sectionLower.includes('optimierung')) {
      return 'performance';
    }
    if (sectionLower.includes('schulden') || sectionLower.includes('technical debt')) {
      return 'technical-debt';
    }
    
    return 'feature';
  }

  /**
   * Prüft ob eine Section verarbeitet werden soll
   */
  shouldProcessSection(section) {
    const sectionsToProcess = [
      'Kurzzeitige Ziele',
      'Mittelfristige Ziele', 
      'Langfristige Ziele',
      'Technische Schulden',
      'Nächste Schritte'
    ];
    
    return sectionsToProcess.some(s => section.includes(s));
  }

  /**
   * Parst einen einzelnen Task
   */
  parseTask(line, lines, index, section, subsection, category) {
    const taskText = line.replace('- [ ] ', '').replace(/\*\*(.*?)\*\*/g, '$1').trim();
    
    if (!taskText || taskText.length < 10) {
      return null;
    }

    // Sammle zusätzliche Details aus nachfolgenden Zeilen
    const details = this.collectTaskDetails(lines, index + 1);
    
    return {
      title: this.generateIssueTitle(taskText),
      body: this.generateIssueBody(taskText, details, section, subsection),
      labels: this.generateLabels(category, section),
      priority: this.determinePriority(section),
      category: category,
      section: section,
      subsection: subsection,
      rawTask: taskText
    };
  }

  /**
   * Sammelt zusätzliche Details für einen Task
   */
  collectTaskDetails(lines, startIndex) {
    const details = [];
    
    for (let i = startIndex; i < Math.min(startIndex + 5, lines.length); i++) {
      const line = lines[i].trim();
      
      // Stoppe bei nächster Task oder Section
      if (line.startsWith('- [ ]') || line.startsWith('- [x]') || 
          line.startsWith('#') || line === '') {
        break;
      }
      
      // Sammle Sub-Items
      if (line.startsWith('  - ') || line.startsWith('    - ')) {
        details.push(line.replace(/^\s*- /, ''));
      }
    }
    
    return details;
  }

  /**
   * Generiert Issue-Titel
   */
  generateIssueTitle(taskText) {
    // Entferne Formatierung und kürze auf max 80 Zeichen
    let title = taskText.replace(/\*\*(.*?)\*\*/g, '$1');
    
    if (title.length > 80) {
      title = title.substring(0, 77) + '...';
    }
    
    return title;
  }

  /**
   * Generiert Issue-Body
   */
  generateIssueBody(taskText, details, section, subsection) {
    let body = `## Beschreibung\n\n${taskText}\n\n`;
    
    if (subsection) {
      body += `**Bereich:** ${subsection}\n`;
    }
    
    body += `**Roadmap-Kategorie:** ${section}\n\n`;
    
    if (details.length > 0) {
      body += `## Details\n\n`;
      details.forEach(detail => {
        body += `- ${detail}\n`;
      });
      body += '\n';
    }
    
    body += `## Akzeptanzkriterien\n\n`;
    body += `- [ ] Feature/Verbesserung ist implementiert\n`;
    body += `- [ ] Tests sind vorhanden (falls anwendbar)\n`;
    body += `- [ ] Dokumentation ist aktualisiert\n`;
    body += `- [ ] Code Review ist durchgeführt\n\n`;
    
    body += `## Zusätzliche Informationen\n\n`;
    body += `Diese Issue wurde automatisch aus der [Roadmap](../ROADMAP.md) generiert.\n\n`;
    body += `Siehe auch: [Vollständige Roadmap](../ROADMAP.md)`;
    
    return body;
  }

  /**
   * Generiert Labels für Issue
   */
  generateLabels(category, section) {
    const labels = [];
    
    // Basis-Label aus Kategorie
    if (ISSUE_CATEGORIES[category]) {
      labels.push(ISSUE_CATEGORIES[category].label);
    }
    
    // Prioritäts-Label
    const priority = this.determinePriority(section);
    labels.push(`priority-${priority}`);
    
    // Roadmap-Label
    labels.push('roadmap');
    
    return labels;
  }

  /**
   * Bestimmt Priorität basierend auf Section
   */
  determinePriority(section) {
    for (const [key, priority] of Object.entries(PRIORITY_MAPPING)) {
      if (section.includes(key)) {
        return priority;
      }
    }
    return 'medium';
  }

  /**
   * Erstellt Ausgabe-Verzeichnis
   */
  createOutputDirectory() {
    const outputPath = path.resolve(__dirname, OUTPUT_DIR);
    
    if (!fs.existsSync(outputPath)) {
      fs.mkdirSync(outputPath, { recursive: true });
    }
    
    console.log(`📁 Ausgabe-Verzeichnis erstellt: ${outputPath}`);
  }

  /**
   * Generiert Issue-Dateien
   */
  generateIssueFiles() {
    this.issues.forEach((issue, index) => {
      const filename = `issue-${String(index + 1).padStart(3, '0')}-${this.slugify(issue.title)}.md`;
      const filepath = path.join(OUTPUT_DIR, filename);
      
      const content = this.formatIssueFile(issue);
      fs.writeFileSync(filepath, content, 'utf8');
    });
    
    console.log(`📝 ${this.issues.length} Issue-Dateien generiert`);
  }

  /**
   * Formatiert Issue als Markdown-Datei
   */
  formatIssueFile(issue) {
    let content = `# ${issue.title}\n\n`;
    content += `**Labels:** ${issue.labels.join(', ')}\n`;
    content += `**Priority:** ${issue.priority}\n`;
    content += `**Category:** ${issue.category}\n\n`;
    content += `---\n\n`;
    content += issue.body;
    
    return content;
  }

  /**
   * Generiert zusammenfassenden Report
   */
  generateSummaryReport() {
    const summaryPath = path.join(OUTPUT_DIR, 'SUMMARY.md');
    
    let content = `# GitHub Issues Summary\n\n`;
    content += `Generiert: ${new Date().toLocaleString('de-DE')}\n\n`;
    content += `## Übersicht\n\n`;
    content += `- **Gesamt Issues:** ${this.issues.length}\n`;
    
    // Gruppierung nach Priorität
    const byPriority = this.groupBy(this.issues, 'priority');
    content += `- **Hohe Priorität:** ${(byPriority.high || []).length}\n`;
    content += `- **Mittlere Priorität:** ${(byPriority.medium || []).length}\n`;
    content += `- **Niedrige Priorität:** ${(byPriority.low || []).length}\n\n`;
    
    // Gruppierung nach Kategorie
    const byCategory = this.groupBy(this.issues, 'category');
    content += `## Issues nach Kategorie\n\n`;
    
    Object.keys(byCategory).forEach(category => {
      content += `### ${category} (${byCategory[category].length})\n\n`;
      byCategory[category].forEach((issue, index) => {
        const filename = `issue-${String(this.issues.indexOf(issue) + 1).padStart(3, '0')}-${this.slugify(issue.title)}.md`;
        content += `${index + 1}. [${issue.title}](${filename})\n`;
      });
      content += '\n';
    });
    
    content += `## Empfohlene Reihenfolge\n\n`;
    content += `1. **Hohe Priorität zuerst** - Kritische Fixes und Deployment\n`;
    content += `2. **Code-Qualität** - Warnings beheben\n`;
    content += `3. **Dokumentation** - README und Screenshots\n`;
    content += `4. **Features** - Nach Business-Value priorisieren\n\n`;
    
    content += `## GitHub CLI Commands\n\n`;
    content += `Siehe \`create-issues.sh\` für automatisierte Issue-Erstellung.\n`;
    
    fs.writeFileSync(summaryPath, content, 'utf8');
    console.log(`📊 Summary-Report erstellt: ${summaryPath}`);
  }

  /**
   * Generiert Batch-Commands für GitHub CLI
   */
  generateBatchCommands() {
    const scriptPath = path.join(OUTPUT_DIR, 'create-issues.sh');
    
    let script = `#!/bin/bash\n\n`;
    script += `# GitHub Issues Auto-Creation Script\n`;
    script += `# Generiert: ${new Date().toISOString()}\n\n`;
    script += `set -e\n\n`;
    script += `echo "🚀 Erstelle GitHub Issues aus Roadmap..."\n\n`;
    
    // Gruppiere nach Priorität für bessere Reihenfolge
    const priorityOrder = ['high', 'medium', 'low'];
    const byPriority = this.groupBy(this.issues, 'priority');
    
    let issueCount = 0;
    
    priorityOrder.forEach(priority => {
      if (!byPriority[priority]) return;
      
      script += `echo "📋 Erstelle ${priority} Priorität Issues..."\n\n`;
      
      byPriority[priority].forEach((issue, index) => {
        issueCount++;
        const filename = `issue-${String(this.issues.indexOf(issue) + 1).padStart(3, '0')}-${this.slugify(issue.title)}.md`;
        
        script += `echo "Creating issue ${issueCount}/${this.issues.length}: ${issue.title}"\n`;
        script += `gh issue create \\\n`;
        script += `  --title "${issue.title.replace(/"/g, '\\"')}" \\\n`;
        script += `  --body-file "${filename}" \\\n`;
        script += `  --label "${issue.labels.join('","')}" \\\n`;
        script += `  --assignee "@me" || echo "Failed to create issue: ${issue.title}"\n\n`;
        
        // Rate Limiting: Pause nach bestimmter Anzahl
        if (issueCount % 10 === 0) {
          script += `echo "⏸️  Kurze Pause für Rate Limiting..."\n`;
          script += `sleep 5\n\n`;
        }
      });
    });
    
    script += `echo "✅ Issue-Erstellung abgeschlossen!"\n`;
    script += `echo "📊 Überprüfe die erstellten Issues: gh issue list"\n`;
    
    fs.writeFileSync(scriptPath, script, 'utf8');
    fs.chmodSync(scriptPath, 0o755); // Ausführbar machen
    
    console.log(`🔧 Batch-Script erstellt: ${scriptPath}`);
  }

  /**
   * Hilfsfunktion: Gruppiert Array nach Property
   */
  groupBy(array, property) {
    return array.reduce((groups, item) => {
      const key = item[property];
      groups[key] = groups[key] || [];
      groups[key].push(item);
      return groups;
    }, {});
  }

  /**
   * Hilfsfunktion: Erstellt URL-freundlichen Slug
   */
  slugify(text) {
    return text
      .toLowerCase()
      .replace(/[äöüß]/g, match => ({
        'ä': 'ae', 'ö': 'oe', 'ü': 'ue', 'ß': 'ss'
      }[match]))
      .replace(/[^a-z0-9]+/g, '-')
      .replace(/^-+|-+$/g, '')
      .substring(0, 50);
  }
}

// Script ausführen wenn direkt aufgerufen
if (require.main === module) {
  const generator = new IssueGenerator();
  generator.generateAllIssues();
}

module.exports = IssueGenerator;