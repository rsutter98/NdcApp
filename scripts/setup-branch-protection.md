# Branch Protection Setup Script

This guide provides step-by-step instructions to configure branch protection rules that enforce all CI/CD pipelines must pass before PR merging.

## Prerequisites

- Repository admin access
- Existing CI/CD workflows (✅ Already implemented)
- GitHub repository settings access

## Automated Configuration Steps

### 1. GitHub Web Interface Configuration

1. **Navigate to Repository Settings**
   ```
   https://github.com/{OWNER}/{REPO}/settings/branches
   ```

2. **Add Branch Protection Rule for `main`**
   ```
   Branch name pattern: main
   ```

3. **Configure Protection Settings**
   ```yaml
   Protect matching branches:
     ✅ Restrict pushes that create files larger than 100MB
     ✅ Require a pull request before merging
       ✅ Require approvals: 1
       ✅ Dismiss stale PR approvals when new commits are pushed
       ✅ Require review from code owners
     ✅ Require status checks to pass before merging
       ✅ Require branches to be up to date before merging
       Required status checks:
         ✅ Build and Test (ci.yml)
         ✅ PR Status Check (pr-validation.yml)  
         ✅ Build Preview App (preview.yml)
     ✅ Require conversation resolution before merging
     ✅ Do not allow bypassing the above settings
   ```

4. **Repeat for `develop` branch**
   ```
   Branch name pattern: develop
   (Same settings as main)
   ```

### 2. Verification Commands

After configuration, verify with these commands:

```bash
# Test status check enforcement
git checkout -b test-branch-protection
echo "# Test change" >> README.md
git add README.md
git commit -m "Test: Verify branch protection"
git push origin test-branch-protection

# Create PR and verify status checks are required
gh pr create --title "Test Branch Protection" --body "Testing that status checks block merge"
```

### 3. Status Check Names Reference

The following status checks should be configured as required:

| Workflow File | Job Name | Status Check Name |
|---------------|----------|-------------------|
| `.github/workflows/ci.yml` | `build-and-test` | `Build and Test` |
| `.github/workflows/pr-validation.yml` | `pr-status-check` | `PR Status Check` |
| `.github/workflows/preview.yml` | `build-preview` | `Build Preview App` |

### 4. Expected Behavior

After configuration:

- ✅ **PR Creation**: Creates PR successfully
- ✅ **Status Checks**: All workflows trigger automatically  
- ✅ **Merge Blocking**: Merge button disabled until all checks pass
- ✅ **Review Required**: At least 1 approval required
- ✅ **Branch Update**: PR must be up-to-date with target branch

### 5. Troubleshooting

**Status checks not appearing:**
1. Create a test PR to trigger workflows
2. Return to branch protection settings
3. Status check names will appear in the dropdown

**Permission errors:**
- Ensure you have admin access to the repository
- Organization policies may restrict branch protection changes

**Merge still possible despite failing checks:**
- Verify "Do not allow bypassing the above settings" is enabled
- Check that admin privileges are included in restrictions

## API Configuration (Alternative)

For automated setup via GitHub API:

```bash
# Install GitHub CLI
gh auth login

# Configure branch protection for main
gh api repos/{OWNER}/{REPO}/branches/main/protection \
  --method PUT \
  --field required_status_checks='{"strict":true,"contexts":["Build and Test","PR Status Check","Build Preview App"]}' \
  --field enforce_admins=true \
  --field required_pull_request_reviews='{"required_approving_review_count":1,"dismiss_stale_reviews":true}' \
  --field restrictions=null

# Configure branch protection for develop  
gh api repos/{OWNER}/{REPO}/branches/develop/protection \
  --method PUT \
  --field required_status_checks='{"strict":true,"contexts":["Build and Test","PR Status Check","Build Preview App"]}' \
  --field enforce_admins=true \
  --field required_pull_request_reviews='{"required_approving_review_count":1,"dismiss_stale_reviews":true}' \
  --field restrictions=null
```

## Verification Script

```bash
#!/bin/bash
# verify-branch-protection.sh

echo "🔍 Verifying branch protection configuration..."

# Check if required status checks are configured
MAIN_PROTECTION=$(gh api repos/{OWNER}/{REPO}/branches/main/protection 2>/dev/null)
DEVELOP_PROTECTION=$(gh api repos/{OWNER}/{REPO}/branches/develop/protection 2>/dev/null)

if [ "$MAIN_PROTECTION" != "" ]; then
    echo "✅ main branch protection is configured"
else
    echo "❌ main branch protection is NOT configured"
fi

if [ "$DEVELOP_PROTECTION" != "" ]; then
    echo "✅ develop branch protection is configured"  
else
    echo "❌ develop branch protection is NOT configured"
fi

echo ""
echo "🎯 Expected status checks:"
echo "- Build and Test"
echo "- PR Status Check" 
echo "- Build Preview App"
echo ""
echo "📋 Next steps if not configured:"
echo "1. Go to repository Settings → Branches"
echo "2. Add protection rules for main and develop"
echo "3. Configure required status checks as listed above"
```

## Security Considerations

- **Include administrators**: Consider enabling "Include administrators" for stricter enforcement
- **Linear history**: Enable "Require linear history" for cleaner git history
- **Signed commits**: Consider requiring signed commits for enhanced security
- **Code owners**: Create `.github/CODEOWNERS` file for automatic reviewer assignment

## Documentation Links

- **GitHub Branch Protection**: https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/defining-the-mergeability-of-pull-requests/about-protected-branches
- **Status Checks**: https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/defining-the-mergeability-of-pull-requests/about-status-checks
- **GitHub API**: https://docs.github.com/en/rest/branches/branch-protection