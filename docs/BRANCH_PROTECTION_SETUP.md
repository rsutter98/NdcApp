# Branch Protection Setup Guide

This document explains how to configure branch protection rules to ensure all CI/CD pipelines must pass before PRs can be merged.

> **🎯 Goal**: PRs can only be merged when all pipelines are successful. Otherwise, merging is blocked.
>
> **📋 Status**: ✅ CI/CD pipelines implemented | ⚠️ Repository settings configuration required

## Required Repository Settings

### 1. Branch Protection Rules

To enforce that all pipelines must be green before merging, configure the following branch protection rules in GitHub:

**Repository Settings → Branches → Add rule**

#### For `main` branch:
- [x] **Restrict pushes that create files larger than 100MB**
- [x] **Require a pull request before merging**
  - [x] Require approvals: `1`
  - [x] Dismiss stale PR approvals when new commits are pushed
  - [x] Require review from code owners (if CODEOWNERS file exists)
- [x] **Require status checks to pass before merging**
  - [x] Require branches to be up to date before merging
  - **Required status checks:**
    - `Build and Test` (from CI workflow)
    - `PR Status Check` (from PR Validation workflow) 
    - `Build Preview App` (from Preview workflow)
- [x] **Require conversation resolution before merging**
- [x] **Restrict pushes that create files larger than 100MB**
- [x] **Do not allow bypassing the above settings**

#### For `develop` branch:
- Same settings as `main` branch

### 2. Workflow Status Checks

The following workflows provide status checks that should be required:

| Workflow | Job Name | Purpose |
|----------|----------|---------|
| `CI` | `Build and Test` | Validates code builds and all tests pass |
| `PR Validation` | `PR Status Check` | Comprehensive PR validation including Docker build |
| `Preview Deployment` | `Build Preview App` | Ensures preview app can be built |

### 3. Manual Setup Steps

1. **Go to Repository Settings**
   - Navigate to your repository on GitHub
   - Click on "Settings" tab
   - Select "Branches" from the sidebar

2. **Add Branch Protection Rule**
   - Click "Add rule"
   - Enter branch name pattern: `main`
   - Configure protection settings as listed above
   - Repeat for `develop` branch

3. **Configure Required Status Checks**
   - After creating your first PR, the status checks will appear in the branch protection settings
   - Add the required status checks listed above

## Workflow Integration

### Current Workflow Status

| Workflow | Trigger | Status Check Name | Purpose |
|----------|---------|-------------------|---------|
| `ci.yml` | Push/PR to main/develop | `Build and Test` | Core CI validation |
| `pr-validation.yml` | PR events | `PR Status Check` | Comprehensive PR validation |
| `preview.yml` | Push/PR to main/develop | `Build Preview App` | Preview deployment validation |
| `release.yml` | Tags/manual | N/A | Release builds |

### Benefits

With proper branch protection rules configured:

- ✅ **No broken code in main/develop**: All code must build and pass tests
- ✅ **Comprehensive validation**: Docker builds and other integrations are verified  
- ✅ **Required reviews**: Code changes must be reviewed before merging
- ✅ **Up-to-date branches**: PRs must be current with target branch
- ✅ **Conversation resolution**: All PR discussions must be resolved

## Troubleshooting

### Permission Issues Fixed

The previous 403 permission error in preview.yml has been resolved by:
- Removing the auto-commit step that tried to push to protected branches
- Status updates are now handled without requiring repository write permissions

### Pipeline Failures

If pipelines fail:
1. Check the specific workflow logs in the Actions tab
2. Review the failure summary provided by each workflow
3. Fix the underlying issue (build errors, test failures, etc.)
4. Push fixes to trigger re-validation

### Emergency Bypass

In emergency situations, repository administrators can:
- Temporarily disable branch protection
- Use admin privileges to merge without status checks
- **Warning**: This should only be used in critical situations

## Verification

To verify the setup is working:

1. **Run verification script**:
   ```bash
   ./scripts/verify-branch-protection.sh
   ```

2. **Manual verification**:
   - Create a test PR with intentionally broken code
   - Verify that status checks fail and block the merge
   - Fix the code and verify that status checks pass
   - Confirm that merge is only possible after all checks are green

3. **Expected results**:
   - ✅ Merge button should be disabled when checks are failing
   - ✅ "All checks have passed" message when all pipelines succeed
   - ✅ Required review approval before merge is possible

## Additional Security

Consider enabling additional protections:
- **Require signed commits** for enhanced security
- **Include administrators** in branch protection rules
- **Restrict who can dismiss PR reviews** to core maintainers
- **Require linear history** to maintain clean git history

## Related Documentation

- **🔄 Rollback Strategy**: [docs/operations/ROLLBACK_STRATEGY.md](operations/ROLLBACK_STRATEGY.md)
- **🚀 Deployment Guide**: [DEPLOYMENT.md](../DEPLOYMENT.md)
- **📋 Setup Script**: [scripts/setup-branch-protection.md](../scripts/setup-branch-protection.md)
- **✅ Verification Script**: [scripts/verify-branch-protection.sh](../scripts/verify-branch-protection.sh)