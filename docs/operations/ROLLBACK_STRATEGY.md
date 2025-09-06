# Rollback Strategy Documentation

This document defines comprehensive rollback strategies for the NdcApp deployment pipeline to ensure quick recovery from deployment issues.

## Overview

The NdcApp supports multiple deployment targets, each requiring specific rollback procedures:

- **Web Preview** (Blazor Server) - Docker-based deployment
- **Android** (APK) - Mobile app store deployment  
- **Windows** (MSIX) - Windows app store deployment
- **iOS** (App Store) - iOS app store deployment

## Quick Rollback Reference

| Platform | Rollback Time | Method | Automation Level |
|----------|---------------|--------|------------------|
| Web Preview | < 5 minutes | Docker container switch | Fully automated |
| Android | 2-24 hours | Play Store rollback | Semi-automated |
| Windows | 2-24 hours | Microsoft Store rollback | Semi-automated |
| iOS | 2-24 hours | App Store rollback | Manual |

## 1. Web Preview Rollback (Docker)

The web preview deployment supports immediate rollback through container management.

### Automated Rollback

```bash
# Quick rollback to previous version
docker-compose -f docker-compose.preview.yml down
docker pull ghcr.io/rsutter98/ndcapp/preview:previous
docker-compose -f docker-compose.preview.yml up -d

# Verify rollback
curl -f http://localhost:8080/health || echo "Rollback verification failed"
```

### Tagged Version Rollback

```bash
# Rollback to specific tagged version
export ROLLBACK_VERSION="v1.2.0"
docker-compose -f docker-compose.preview.yml down
docker pull ghcr.io/rsutter98/ndcapp/preview:$ROLLBACK_VERSION
docker tag ghcr.io/rsutter98/ndcapp/preview:$ROLLBACK_VERSION ghcr.io/rsutter98/ndcapp/preview:latest
docker-compose -f docker-compose.preview.yml up -d
```

### Manual Container Rollback

```bash
# List available image tags
docker images ghcr.io/rsutter98/ndcapp/preview

# Stop current deployment
docker-compose -f docker-compose.preview.yml down

# Start with specific version
docker run -d -p 8080:80 --name ndcapp-preview ghcr.io/rsutter98/ndcapp/preview:SAFE_VERSION

# Update compose file for next deployment
sed -i 's/ghcr.io\/rsutter98\/ndcapp\/preview:latest/ghcr.io\/rsutter98\/ndcapp\/preview:SAFE_VERSION/' docker-compose.preview.yml
```

## 2. Mobile App Rollback (Android/iOS/Windows)

Mobile app rollbacks require coordination with app stores and may take several hours.

### Android (Google Play Store)

#### Immediate Actions (< 30 minutes)
```bash
# Stop current release workflow if running
gh workflow run cancel --repo rsutter98/NdcApp

# Disable app distribution in Play Console
echo "1. Go to Google Play Console"
echo "2. Select NdcApp"  
echo "3. Go to Release management → App releases"
echo "4. Find problematic release and select 'Halt rollout'"
```

#### Full Rollback (2-24 hours)
```bash
# Promote previous release to production
echo "1. Go to Google Play Console → Internal testing"
echo "2. Find last known good version"
echo "3. Promote to Production"
echo "4. Submit for review (may take up to 24 hours)"

# Git tag rollback for future builds
git tag -d v1.3.0  # Remove problematic tag
git push origin :refs/tags/v1.3.0
git tag v1.3.0 LAST_KNOWN_GOOD_COMMIT
git push origin v1.3.0
```

### iOS (App Store)

#### Immediate Actions (< 30 minutes)
```bash
# Remove from sale in App Store Connect
echo "1. Go to App Store Connect"
echo "2. Select NdcApp"
echo "3. Go to App Store → App Information"  
echo "4. Change availability to 'Remove from sale'"
```

#### Full Rollback (2-7 days)
```bash
# Submit previous version for review
echo "1. Go to App Store Connect → TestFlight"
echo "2. Find last known good build"
echo "3. Submit for App Store Review"
echo "4. Apple review process: 1-7 days"

# Prepare rollback build
git checkout LAST_KNOWN_GOOD_COMMIT
git tag v1.2.1-rollback
git push origin v1.2.1-rollback
```

### Windows (Microsoft Store)

#### Immediate Actions (< 30 minutes)
```bash
# Stop distribution in Partner Center
echo "1. Go to Microsoft Partner Center"
echo "2. Select NdcApp"
echo "3. Go to App management → App availability"
echo "4. Change to 'Stop acquisition'"
```

#### Full Rollback (2-24 hours)
```bash
# Submit previous package version
echo "1. Go to Partner Center → Packages"
echo "2. Upload last known good MSIX package"
echo "3. Submit for certification"
echo "4. Certification process: 2-24 hours"
```

## 3. Database/State Rollback

If the application includes backend services with database state:

### Preview Environment Database
```bash
# Backup current state
docker exec ndcapp-preview-db pg_dump -U postgres ndcapp > backup_before_rollback.sql

# Restore from backup
docker exec -i ndcapp-preview-db psql -U postgres ndcapp < backup_known_good.sql

# Verify data integrity
docker exec ndcapp-preview-db psql -U postgres -c "SELECT COUNT(*) FROM talks;"
```

### Configuration Rollback
```bash
# Rollback app configuration
git checkout HEAD~1 -- NdcApp.Preview/appsettings.json
git checkout HEAD~1 -- NdcApp/Platforms/*/appsettings.json

# Restart services with old configuration
docker-compose -f docker-compose.preview.yml restart
```

## 4. Automated Rollback Triggers

### Health Check Based Rollback

```yaml
# .github/workflows/auto-rollback.yml
name: Auto Rollback
on:
  schedule:
    - cron: '*/5 * * * *'  # Check every 5 minutes
    
jobs:
  health-check-rollback:
    runs-on: ubuntu-latest
    steps:
    - name: Check application health
      run: |
        if ! curl -f http://preview.ndcapp.local:8080/health; then
          echo "Health check failed, triggering rollback"
          # Trigger rollback workflow
          gh workflow run rollback.yml --repo rsutter98/NdcApp
        fi
```

### Error Rate Based Rollback

```bash
# Monitor error rates and trigger rollback
ERROR_RATE=$(curl -s http://localhost:8080/metrics | grep error_rate | cut -d' ' -f2)
if (( $(echo "$ERROR_RATE > 0.05" | bc -l) )); then
    echo "Error rate too high: $ERROR_RATE. Triggering rollback..."
    ./scripts/rollback-preview.sh
fi
```

## 5. Recovery Procedures

### Post-Rollback Verification

```bash
#!/bin/bash
# verify-rollback.sh

echo "🔍 Verifying rollback success..."

# Check application is running
if curl -f http://localhost:8080/health; then
    echo "✅ Application health check passed"
else
    echo "❌ Application health check failed"
    exit 1
fi

# Verify core functionality
if curl -f http://localhost:8080/api/talks | jq length > /dev/null; then
    echo "✅ Core API functionality verified"
else
    echo "❌ Core API functionality failed"
    exit 1
fi

# Check database connectivity
if docker exec ndcapp-preview-db pg_isready -U postgres; then
    echo "✅ Database connectivity verified"
else
    echo "❌ Database connectivity failed"
    exit 1
fi

echo "🎉 Rollback verification completed successfully"
```

### Communication Templates

#### Internal Team Notification
```
🚨 ROLLBACK EXECUTED - NdcApp

Platform: [Web Preview/Android/iOS/Windows]
Version: Rolled back from v1.3.0 to v1.2.0
Reason: [Brief description of issue]
ETA for fix: [Estimated timeline]
Impact: [User impact description]

Status: ✅ Rollback completed successfully
Next steps: [Investigation and fix plan]
```

#### User Communication
```
📱 App Update Notice

We've temporarily reverted to a previous version of the NdcApp due to a technical issue. 

What this means:
- Your data and saved talks are safe
- Some recent features may be temporarily unavailable
- We're working on a fix and will restore full functionality soon

Expected resolution: [Timeline]
```

## 6. Prevention Strategies

### Pre-deployment Validation
```bash
# Enhanced pre-deployment checks
./scripts/verify-branch-protection.sh
dotnet test --logger trx --results-directory TestResults
docker build -t test-build ./NdcApp.Preview/
docker run --rm test-build dotnet --version
```

### Canary Deployment
```yaml
# Deploy to preview environment first
deploy-preview:
  environment: preview
  if: github.ref == 'refs/heads/develop'
  
deploy-production:
  environment: production  
  if: github.ref == 'refs/heads/main'
  needs: deploy-preview
```

### Automated Testing in Production
```bash
# Post-deployment smoke tests
curl -f http://localhost:8080/health
curl -f http://localhost:8080/api/talks | jq '.length > 0'
curl -f http://localhost:8080/api/categories | jq '.length > 0'
```

## 7. Emergency Contacts

| Role | Contact | Availability |
|------|---------|--------------|
| Lead Developer | @rsutter98 | 24/7 |
| DevOps Engineer | TBD | Business hours |
| Product Owner | TBD | Business hours |

## 8. Rollback Decision Matrix

| Severity | Criteria | Action | Approval Required |
|----------|----------|--------|-------------------|
| Critical | App crash, data loss | Immediate rollback | None |
| High | Core features broken | Rollback within 30 min | Lead Developer |
| Medium | Minor features broken | Rollback within 2 hours | Product Owner |
| Low | UI issues | Fix forward | Team consensus |

## 9. Documentation and Learning

### Post-Incident Review Process
1. **Immediate**: Document what happened
2. **24 hours**: Initial root cause analysis
3. **1 week**: Complete post-mortem with lessons learned
4. **2 weeks**: Update prevention strategies and rollback procedures

### Rollback Testing
- **Monthly**: Test preview environment rollback procedures
- **Quarterly**: Test mobile app rollback procedures (in staging)
- **Annually**: Full disaster recovery drill

---

**Last Updated**: 2024-01-XX  
**Next Review**: 2024-04-XX  
**Document Owner**: Development Team