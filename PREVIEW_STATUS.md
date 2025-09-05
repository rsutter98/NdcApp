# NDC Conference Planner - Live Preview Status

> **🎯 Diese Seite wird automatisch nach jedem Build aktualisiert**

## 📱 iPhone Zugang - Einfacher Zugang

### Aktueller Status
**Status:** ⏳ Warten auf ersten Deployment  
**Letztes Update:** Noch kein Build  
**Preview URL:** Noch nicht verfügbar  

### 📱 iPhone Setup - Schritt für Schritt

#### 1. Netzwerk vorbereiten
- ✅ iPhone und Computer müssen im **gleichen WLAN** sein
- ✅ Computer muss Preview App laufen lassen

#### 2. Computer IP-Adresse finden

**Windows:**
```cmd
ipconfig
```

**macOS/Linux:**
```bash
ifconfig | grep "inet " | grep -v 127.0.0.1
```

**Beispiel Ausgabe:** `192.168.1.100`

#### 3. Preview App starten

```bash
# Docker Compose (Empfohlen)
docker-compose -f docker-compose.preview.yml up -d

# Oder direkt mit .NET
cd NdcApp.Preview
dotnet run --urls=http://0.0.0.0:5000
```

#### 4. iPhone Browser öffnen

**URL Format:** `http://[IHRE-IP]:8080`  
**Beispiel:** `http://192.168.1.100:8080`

## 🖥️ Alle Zugangsoptionen

| Plattform | URL | Anleitung |
|-----------|-----|-----------|
| **iPhone/iPad** | `http://[IP]:8080` | Safari Browser verwenden |
| **Android** | `http://[IP]:8080` | Chrome Browser verwenden |
| **Computer (lokal)** | `http://localhost:8080` | Beliebiger Browser |
| **Computer (Netzwerk)** | `http://[IP]:8080` | Beliebiger Browser |

## 🔧 Deployment Methoden

### 1. 🐳 Docker (Empfohlen für iPhone)
```bash
# Auto-Build und Starten
docker-compose -f docker-compose.preview.yml up -d

# Status prüfen
docker ps

# Stoppen
docker-compose -f docker-compose.preview.yml down
```

### 2. 💻 Direkt mit .NET
```bash
# Entwicklungsmodus
cd NdcApp.Preview
dotnet run --urls=http://0.0.0.0:5000

# Production Build
dotnet publish -c Release
cd bin/Release/net8.0/publish
dotnet NdcApp.Preview.dll --urls=http://0.0.0.0:5000
```

### 3. ☁️ GitHub Actions (Automatisch)
- **Pull Request:** Erstellt Preview Build automatisch
- **Push to main/develop:** Deployed automatisch
- **Manual Trigger:** `Actions` → `Preview Deployment` → `Run workflow`

## 🌟 Features verfügbar

- ✅ **Konferenz Talks durchsuchen** - Alle verfügbaren Talks anzeigen
- ✅ **Persönliche Planung** - Talks zur persönlichen Agenda hinzufügen/entfernen
- ✅ **Talk Bewertungen** - Talks mit 1-5 Sternen bewerten
- ✅ **Filtern & Suchen** - Nach Tag, Kategorie oder Text filtern
- ✅ **Responsive Design** - Funktioniert auf Desktop und Mobile
- ✅ **Real-time Updates** - Live Statistiken und Interaktionen

## 🔍 Troubleshooting

### iPhone kann nicht zugreifen
1. **WLAN prüfen** - Beide Geräte im gleichen Netzwerk?
2. **IP-Adresse korrekt** - `ipconfig`/`ifconfig` nochmals ausführen
3. **Port verfügbar** - Ist Port 8080 frei?
4. **Firewall** - Windows/macOS Firewall kann Port blockieren
5. **VPN/Proxy** - Temporär deaktivieren

### Docker Probleme
```bash
# Ports prüfen
netstat -an | grep 8080

# Container Status
docker ps -a

# Logs anzeigen
docker logs ndcapp-preview

# Neustart
docker-compose -f docker-compose.preview.yml restart
```

### Performance Probleme
- **Mobile Safari:** Private Browsing verwenden
- **Cache leeren:** Safari → Einstellungen → Daten löschen
- **Netzwerk:** 5GHz WLAN für bessere Performance

## 📊 Aktuelle Deployment Info

**Branch:** Noch kein Build  
**Commit:** -  
**Build Zeit:** -  
**Docker Image:** -  
**Container Status:** Noch nicht deployed  

---

## 🚀 Quick Start für iPhone

1. **Terminal öffnen** (Windows: CMD, macOS: Terminal)
2. **IP-Adresse finden:**
   ```bash
   # Windows
   ipconfig
   
   # macOS/Linux  
   ifconfig
   ```
3. **Preview starten:**
   ```bash
   docker-compose -f docker-compose.preview.yml up -d
   ```
4. **iPhone Safari öffnen:** `http://[IHRE-IP]:8080`
5. **NDC Conference Planner testen!** 🎉

---

**💡 Tipp:** Bookmark diese URL auf iPhone für schnellen Zugang!

*Automatisch generiert am: {{ TIMESTAMP }}*  
*Nächstes Update: Nach dem nächsten Build*