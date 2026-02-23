# Security

## 🛡️ Supported Versions

| Version | Security updates |
|---|---|
| 1.0.x | ✅ Supported |

---

## 🚨 Reporting a Vulnerability

**Do not open a public GitHub issue** for security vulnerabilities.

Instead:

1. Email the repository maintainers with full details.
2. Include:
   - A clear description of the vulnerability
   - Steps to reproduce
   - Potential impact assessment
   - Suggested fix (if known)
3. Expect an acknowledgement within 72 hours.
4. We will coordinate a fix and disclosure timeline with you.

---

## 🔒 Security Model

NdcApp is a **client-side mobile/desktop application** with no backend server. All data is stored locally on the user's device. The threat surface is therefore limited to:

- **Bundled data** — The conference schedule (`ndc.csv`) is read-only and ships with the app binary.
- **User preferences** — Selected talks and ratings are stored via platform-native `Preferences` storage (protected by the OS sandbox).
- **Local notifications** — Notification content is generated from bundled data only; no network calls are made.

---

## 🏗️ Build Pipeline Security

- All builds run in isolated, ephemeral GitHub Actions runners.
- Dependencies are restored exclusively from the official NuGet feed.
- No secrets are written to build logs.
- Release artifacts are generated without embedded credentials.
- The `update-docs` workflow operates with `read-all` permissions and creates only draft PRs.

---

## 🔑 Code Signing

Production builds should be signed before distribution:

| Platform | Mechanism | Storage |
|---|---|---|
| Android | Keystore file | GitHub Actions secret (`ANDROID_KEYSTORE`) |
| Windows | Code-signing certificate | GitHub Actions secret |
| iOS | Apple Developer certificate + provisioning profile | Xcode / GitHub Actions secret |

Never commit signing keys or certificates to the repository.

---

## 📦 Dependency Management

- **Dependabot** is configured (`.github/dependabot.yml`) to automatically open PRs for outdated NuGet and GitHub Actions dependencies.
- External dependencies are kept to a minimum; the main third-party runtime dependency is `Plugin.LocalNotification`.
- All PRs updating dependencies must pass CI before merging.

---

## 🔐 Distribution Security

- Releases are distributed via GitHub Releases with attached signed artifacts.
- HTTPS is enforced for all downloads from GitHub.
- Artifact integrity can be verified against the SHA-256 checksums published with each release.

---

## 🔗 See Also

- [Deployment](../operations/DEPLOYMENT.md)
- [Release Process](../operations/RELEASE.md)
- [Root SECURITY.md](../../SECURITY.md) — Legacy security policy
