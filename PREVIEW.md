# NDC Conference Planner - Preview Deployment

This document describes the preview deployment functionality for the NDC Conference Planner app.

## Overview

The preview deployment provides a **web-based version** of the MAUI app that can be accessed through a browser for demonstration and testing purposes. This allows stakeholders to interact with the app functionality without needing to install the native MAUI application.

## Architecture

```
┌─────────────────┐    ┌──────────────────┐    ┌─────────────────┐
│   MAUI App      │    │  Preview App     │    │   Docker        │
│   (Original)    │───▶│  (Blazor Server) │───▶│   Container     │
│                 │    │                  │    │                 │
│ • XAML UI       │    │ • Web UI         │    │ • Linux Alpine  │
│ • Native        │    │ • Same Logic     │    │ • ASP.NET Core  │
│ • Cross-platform│    │ • Browser-based  │    │ • Port 80/8080  │
└─────────────────┘    └──────────────────┘    └─────────────────┘
```

## Components

### NdcApp.Preview
- **Type**: Blazor Server application
- **Purpose**: Web-based replica of MAUI app functionality
- **Technology**: ASP.NET Core 8.0, SignalR, Blazor Server
- **UI**: Responsive web interface mimicking the MAUI app design

### Core Functionality Replicated
✅ **Conference Talk Browsing** - View all available talks with details  
✅ **Personal Planning** - Add/remove talks from personal schedule  
✅ **Talk Ratings** - Rate talks with 1-5 star system  
✅ **Filtering & Search** - Filter by day, category, or search text  
✅ **Real-time Updates** - Live statistics and UI updates  
✅ **Responsive Design** - Works on desktop, tablet, and mobile  

### Shared Services
The preview app reuses the core business logic:
- `ITalkService` - Talk data loading and parsing
- `IConferencePlanService` - Personal schedule management  
- `ITalkRatingService` - Rating system
- `ITalkFilterService` - Search and filtering
- `ILoggerService` - Logging functionality

## Deployment Methods

### 1. GitHub Actions Pipeline (Automated)

The preview deployment is automatically triggered by:
- **Pull Requests** - Builds preview for testing
- **Push to main/develop** - Deploys to preview environment
- **Manual Workflow Dispatch** - On-demand deployment

```yaml
# .github/workflows/preview.yml
- Builds preview app
- Creates Docker image
- Deploys to container registry
- Comments on PRs with preview info
```

### 2. Docker Deployment (Local/Server)

```bash
# Using Docker Compose (Recommended)
docker-compose -f docker-compose.preview.yml up -d

# Using Docker directly
docker build -f NdcApp.Preview/Dockerfile -t ndcapp-preview .
docker run -p 8080:80 ndcapp-preview
```

### 3. Direct .NET Deployment

```bash
# Development mode
cd NdcApp.Preview
dotnet run

# Production mode
dotnet publish -c Release -o ./publish
cd publish
dotnet NdcApp.Preview.dll
```

## Access URLs

| Environment | URL | Purpose |
|-------------|-----|---------|
| Local Development | http://localhost:5000 | Development and testing |
| Docker Local | http://localhost:8080 | Local containerized testing |
| GitHub Actions | Variable | CI/CD pipeline testing |
| Production Preview | TBD | Stakeholder demonstrations |

## Configuration

### Environment Variables
```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:80
ASPNETCORE_LOGGING__LOGLEVEL__DEFAULT=Information
```

### Docker Configuration
- **Base Image**: `mcr.microsoft.com/dotnet/aspnet:8.0`
- **Exposed Ports**: 80, 443
- **Health Check**: Built-in ASP.NET Core health checks
- **Resource Requirements**: 
  - Memory: 512MB minimum, 1GB recommended
  - CPU: 0.5 vCPU minimum, 1 vCPU recommended

## Features Demonstration

### 1. Home Page
- Welcome screen with NDC branding
- Real-time current time display
- Conference statistics overview
- Navigation to conference planning

### 2. Conference Plan Page
- **Talk Browsing**: Grid view of all available talks
- **Filtering**: Dropdown filters for day and category
- **Search**: Real-time text search across talk titles and speakers
- **Selection**: Add/remove talks from personal plan
- **Rating**: 5-star rating system with modal dialog
- **Statistics**: Live counters for selected vs available talks

### 3. Interactive Features
- **State Management**: Persistent selections across page refreshes
- **Real-time UI**: Instant feedback for user actions
- **Responsive Design**: Mobile-friendly interface
- **Error Handling**: Graceful fallbacks for missing data

## Data Source

The preview app uses the same data as the MAUI app:
- **Source**: `NdcApp/Resources/Raw/ndc.csv`
- **Format**: CSV with conference talk information
- **Fallback**: Sample data for demonstration if CSV not found
- **Fields**: Day, Start Time, End Time, Room, Title, Speaker, Category

## Development Workflow

### Adding New Features
1. Implement in `NdcApp.Core` (shared logic)
2. Add to `NdcApp` (MAUI app)
3. Replicate in `NdcApp.Preview` (web interface)
4. Test in both environments
5. Deploy via pipeline

### Testing Preview
1. **Local**: `dotnet run` in NdcApp.Preview folder
2. **Docker**: `docker-compose -f docker-compose.preview.yml up`
3. **CI/CD**: Automatic build on PR/push
4. **Browser Testing**: Chrome, Firefox, Safari, Edge, Mobile

## Monitoring and Logging

### Application Logs
- ASP.NET Core built-in logging
- Custom `ILoggerService` integration
- Console and file output support

### Health Checks
- Built-in ASP.NET Core health checks
- Container readiness probes
- Application startup verification

### Metrics
- Request/response times
- User interaction tracking
- Error rate monitoring

## Security Considerations

### Web Application
- HTTPS redirection in production
- Content Security Policy headers
- XSS protection enabled
- CSRF protection for forms

### Container Security
- Non-root user execution
- Minimal base image (Alpine Linux)
- Security scanning in CI/CD
- Secrets management via environment variables

## Troubleshooting

### Common Issues

**"Conference data not found"**
```bash
# Ensure CSV file is copied to Docker image
COPY ["NdcApp/Resources/Raw/ndc.csv", "Data/"]
```

**Docker build fails**
```bash
# Check .dockerignore file
# Verify network connectivity
# Use Docker BuildKit
DOCKER_BUILDKIT=1 docker build ...
```

**Port conflicts**
```bash
# Change port mapping
docker run -p 8081:80 ndcapp-preview
```

**SignalR connection issues**
```bash
# Check firewall settings
# Verify WebSocket support
# Enable CORS if needed
```

### Debug Mode
```bash
# Enable detailed logging
export ASPNETCORE_ENVIRONMENT=Development
export ASPNETCORE_LOGGING__LOGLEVEL__DEFAULT=Debug
```

## Future Enhancements

### Planned Features
- [ ] **Real-time Collaboration** - Multiple users viewing same plan
- [ ] **Push Notifications** - Browser notifications for talk reminders
- [ ] **Offline Support** - Progressive Web App capabilities
- [ ] **Analytics Integration** - User interaction tracking
- [ ] **A/B Testing** - Feature flag support

### Infrastructure Improvements
- [ ] **Auto-scaling** - Kubernetes deployment
- [ ] **Load Balancing** - Multiple instance support
- [ ] **Monitoring** - Application Performance Monitoring
- [ ] **Backup/Recovery** - Data persistence strategies

---

## Quick Start Commands

```bash
# Clone and run locally
git clone <repository>
cd NdcApp
dotnet run --project NdcApp.Preview

# Build and run with Docker
docker-compose -f docker-compose.preview.yml up -d

# Access the application
open http://localhost:8080
```

For more information, see the main [README.md](../README.md) and [DEPLOYMENT.md](../DEPLOYMENT.md) files.