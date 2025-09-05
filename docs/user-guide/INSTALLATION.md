# Installation Guide

Platform-specific installation instructions for NdcApp.

## 📱 Supported Platforms

NdcApp supports the following platforms:
- **Windows 10/11** (x64)
- **Android 5.0+** (API level 21+)
- **iOS 15.0+** (iPhone and iPad)

## 📥 Download Options

### Official Releases
- **Latest Release**: [GitHub Releases](https://github.com/rsutter98/NdcApp/releases/latest)
- **All Versions**: [All Releases](https://github.com/rsutter98/NdcApp/releases)

### Beta Versions
- **iOS TestFlight**: Contact maintainers for beta access
- **Android Sideload**: Development builds available on releases page

## 🖥️ Windows Installation

### System Requirements
- Windows 10 version 1903 (build 18362) or later
- Windows 11 (recommended)
- 4 GB RAM minimum
- 100 MB free disk space

### Installation Steps
1. **Download** the latest `.msix` file from [releases](https://github.com/rsutter98/NdcApp/releases)
2. **Enable Developer Mode** (if prompted):
   - Open Settings → Update & Security → For developers
   - Select "Developer mode"
3. **Install the Package**:
   - Double-click the `.msix` file
   - Click "Install" when prompted
   - Allow installation from "Unknown publisher" if needed
4. **Launch the App**:
   - Find "NDC Conference Planner" in Start Menu
   - Or search for "NdcApp"

### Windows Troubleshooting
- **Installation blocked**: Enable Developer Mode or sideloading
- **App won't start**: Check Windows version compatibility
- **Missing dependencies**: Install latest Windows updates

## 🤖 Android Installation

### System Requirements
- Android 5.0 (API level 21) or later
- 2 GB RAM minimum
- 50 MB free storage space
- Internet connection for initial data load

### Installation Steps
1. **Enable Unknown Sources**:
   - Open Settings → Security (or Apps & notifications → Special app access)
   - Enable "Install unknown apps" for your browser/file manager
2. **Download APK**:
   - Download the latest `.apk` file from [releases](https://github.com/rsutter98/NdcApp/releases)
   - Save to Downloads folder
3. **Install APK**:
   - Open file manager and navigate to Downloads
   - Tap the `.apk` file
   - Confirm installation when prompted
4. **Launch App**:
   - Find "NDC Conference Planner" in app drawer
   - Or look for the NDC-themed icon

### Android Troubleshooting
- **Installation blocked**: Check "Unknown sources" setting
- **App crashes**: Clear app data and restart device
- **Notifications not working**: Check notification permissions in Settings

## 🍎 iOS Installation

### System Requirements
- iOS 15.0 or later
- iPhone 8 or newer
- iPad Air 2 or newer
- 50 MB free storage space

### TestFlight Installation (Recommended)
1. **Install TestFlight**:
   - Download TestFlight from App Store (if not already installed)
2. **Get Invitation**:
   - Contact project maintainers for TestFlight invitation
   - Or check GitHub discussions for public beta links
3. **Install via TestFlight**:
   - Open invitation link on your device
   - Tap "Install" in TestFlight
   - Launch app from TestFlight or home screen

### Direct Installation (Developers)
1. **Download IPA**:
   - Download `.ipa` file from releases
2. **Install via Xcode**:
   - Connect device to Mac
   - Open Xcode → Window → Devices and Simulators
   - Drag `.ipa` file to your device
3. **Trust Developer**:
   - Go to Settings → General → VPN & Device Management
   - Trust the developer certificate

### iOS Troubleshooting
- **Installation fails**: Check iOS version compatibility
- **App won't start**: Restart device and try again
- **Certificate issues**: Re-trust developer in device settings

## 🔄 Updating the App

### Automatic Updates
- **Windows**: Updates through Microsoft Store (if available)
- **Android**: Manual download and install new APK
- **iOS**: Updates through TestFlight

### Manual Updates
1. Download latest version from releases
2. Install over existing version (settings preserved)
3. Restart app to load new features

## 📊 Verification

### After Installation
1. **Launch the app** and verify it starts properly
2. **Check main page** shows current time and NDC branding
3. **Test navigation** to "Conference Plan" page
4. **Verify data loading** (sample CSV should load automatically)

### Common First-Run Issues
- **No data visible**: Check internet connection and restart app
- **UI elements missing**: Verify platform compatibility
- **Performance issues**: Restart device and ensure adequate RAM

## 🆘 Need Help?

If you encounter issues during installation:

1. **Check System Requirements** - Ensure your device meets minimum requirements
2. **Review Troubleshooting** - See platform-specific troubleshooting above
3. **Check GitHub Issues** - [Search existing issues](https://github.com/rsutter98/NdcApp/issues)
4. **Create New Issue** - [Report installation problems](https://github.com/rsutter98/NdcApp/issues/new?template=bug_report.md)

### Useful Information for Bug Reports
- Device model and OS version
- Installation method attempted
- Error messages (screenshots helpful)
- Steps to reproduce the issue

---

**✅ Successfully installed?** Continue with the [Quick Start Guide](QUICK_START.md) to begin using NdcApp!