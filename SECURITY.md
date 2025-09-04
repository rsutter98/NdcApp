# Security Policy

## Supported Versions

We provide security updates for the following versions:

| Version | Supported          |
| ------- | ------------------ |
| 1.0.x   | :white_check_mark: |

## Reporting a Vulnerability

If you discover a security vulnerability in NdcApp, please follow these steps:

1. **Do not open a public issue** - Security vulnerabilities should be reported privately
2. **Email the maintainers** with details of the vulnerability
3. **Include the following information:**
   - Description of the vulnerability
   - Steps to reproduce
   - Potential impact
   - Suggested fix (if any)

## Security Considerations

### Build Pipeline Security
- All builds run in isolated GitHub Actions runners
- Dependencies are restored from official NuGet sources
- No secrets are exposed in build logs
- Artifacts are generated without embedded credentials

### Code Signing
For production releases, code signing should be configured:
- **Android**: Configure signing keys in GitHub Secrets
- **Windows**: Use certificate signing for MSIX packages
- **iOS**: Apple Developer certificates required

### Dependency Management
- Regular dependency updates through Dependabot
- Security scanning of third-party packages
- Minimal external dependencies

## Deployment Security
- HTTPS-only distribution
- Integrity verification of released artifacts
- Secure storage of signing certificates

## Contact
For security-related concerns, please contact the repository maintainers.