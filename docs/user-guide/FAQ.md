# Frequently Asked Questions (FAQ)

Common questions and troubleshooting for NdcApp users.

## 📱 General Usage

### Q: How do I get started with NdcApp?
**A:** Start with our [Quick Start Guide](QUICK_START.md)! It takes just 5 minutes to get up and running.

### Q: What conferences does NdcApp support?
**A:** NdcApp is designed for NDC conferences but can work with any conference that provides data in CSV format. The sample data is from NDC Copenhagen.

### Q: Do I need an internet connection?
**A:** You need internet for initial setup and data loading. Once loaded, most features work offline, though you'll need internet for data updates and notifications.

### Q: Can I use NdcApp for multiple conferences?
**A:** Currently, NdcApp focuses on one conference at a time. Multi-conference support is planned for future releases.

## 🔍 Features and Functionality

### Q: How do I search for specific talks?
**A:** Use the search bar at the top of the Conference Plan page. You can search by:
- Talk titles (e.g., "AI", "Machine Learning")
- Speaker names (e.g., "Jodie Burchell")
- Categories (e.g., "Backend", "Frontend")
- Room names (e.g., "Room 1", "Main Hall")

### Q: How do I select talks for my schedule?
**A:** There are two ways:
1. **Button method**: Tap "Select Talk" under any talk
2. **Swipe method**: Swipe right on a talk card for quick selection

### Q: Can I rate talks before attending them?
**A:** Yes! Use the 5-star rating system. Your ratings help improve recommendations and can be updated at any time.

### Q: How do I see only my selected talks?
**A:** Use the filter buttons at the top: "All Talks" shows everything, "My Talks" shows only your selections.

### Q: What sorting options are available?
**A:** You can sort talks by:
- **Time** (chronological, default)
- **Speaker** (alphabetical)
- **Category** (grouped by topic)
- **Rating** (highest rated first)

## 📲 Notifications and Reminders

### Q: How do I enable notifications for my talks?
**A:** 
1. Tap the 🔔 notification icon in the Conference Plan
2. Enable notifications for your platform
3. Choose timing (15 and 5 minutes before talks)

### Q: Why am I not receiving notifications?
**A:** Check:
- App notification permissions in device settings
- "Do Not Disturb" mode settings
- Selected talks (only selected talks trigger notifications)
- Network connection for notification scheduling

### Q: Can I customize notification timing?
**A:** Currently notifications are set for 15 and 5 minutes before talks. Customizable timing is planned for future releases.

## 💾 Data and Synchronization

### Q: Are my selections saved automatically?
**A:** Yes! Your talk selections, ratings, and preferences are automatically saved locally on your device.

### Q: What happens if I lose internet connection?
**A:** Your selected talks and ratings remain available offline. You won't be able to refresh data or receive new notifications until connection is restored.

### Q: Can I export my schedule?
**A:** Currently there's no export feature, but it's on the roadmap. Your data is stored locally and persists across app restarts.

### Q: How do I update conference data?
**A:** Use "pull-to-refresh" by pulling down on the talk list to reload the latest data.

## 🛠️ Technical Issues

### Q: The app won't start on my device
**A:** Try these steps:
1. Check system requirements in [Installation Guide](INSTALLATION.md)
2. Restart your device
3. Reinstall the app
4. Check for OS updates

### Q: Talks are not loading or showing
**A:** 
1. Check your internet connection
2. Use pull-to-refresh in the talk list
3. Restart the app
4. Verify the CSV data source is accessible

### Q: App is running slowly
**A:**
1. Close other apps to free up memory
2. Restart your device
3. Check available storage space
4. Update to latest app version

### Q: My ratings aren't saving
**A:**
1. Ensure app is fully loaded before rating
2. Check for sufficient storage space
3. Try pull-to-refresh to sync changes
4. Restart app if issues persist

### Q: Search isn't finding talks I know exist
**A:**
- Check spelling and try partial terms
- Search is case-insensitive but requires exact word matches
- Try searching different fields (title, speaker, category)
- Use pull-to-refresh to ensure latest data

## 📱 Platform-Specific Issues

### Windows Issues

**Q: Installation is blocked by Windows**
**A:** Enable Developer Mode or allow app sideloading in Windows Settings.

**Q: App appears blurry on high-DPI displays**
**A:** Right-click app → Properties → Compatibility → Override high DPI scaling.

### Android Issues  

**Q: Can't install APK file**
**A:** Enable "Install unknown apps" for your browser/file manager in Android security settings.

**Q: App crashes on startup**
**A:** Clear app data: Settings → Apps → NDC Conference Planner → Storage → Clear Data.

### iOS Issues

**Q: TestFlight installation fails**
**A:** Ensure iOS 15.0+ and TestFlight app is up to date.

**Q: App is grayed out after installation**
**A:** Go to Settings → General → VPN & Device Management and trust the developer.

## 🔄 Updates and Versions

### Q: How do I update to the latest version?
**A:** 
- **Windows/Android**: Download and install new version from releases
- **iOS**: Update through TestFlight
- Your settings and selections are preserved during updates

### Q: What's new in the latest version?
**A:** Check the [releases page](https://github.com/rsutter98/NdcApp/releases) for detailed changelog and new features.

### Q: Can I downgrade to a previous version?
**A:** Yes, download the desired version from releases and install. Note that data compatibility isn't guaranteed with older versions.

## 📞 Getting More Help

### Q: My question isn't answered here
**A:** Try these resources:
1. **User Guide**: [Complete documentation](README.md)
2. **GitHub Issues**: [Search existing issues](https://github.com/rsutter98/NdcApp/issues)
3. **Discussions**: [Community Q&A](https://github.com/rsutter98/NdcApp/discussions)
4. **Bug Reports**: [Report new issues](https://github.com/rsutter98/NdcApp/issues/new?template=bug_report.md)

### Q: How can I request a new feature?
**A:** [Create a feature request](https://github.com/rsutter98/NdcApp/issues/new?template=feature_request.md) on GitHub with your idea and use case.

### Q: Can I contribute to the project?
**A:** Absolutely! See our [Developer Guide](../developer-guide/README.md) and [Contributing Guidelines](../developer-guide/CONTRIBUTING.md).

## 📋 Feature Requests and Roadmap

### Q: What features are planned for future releases?
**A:** Check our [Roadmap](../../ROADMAP.md) for detailed development plans and upcoming features.

### Q: Will there be a web version?
**A:** There's already a [preview web version](../../PREVIEW.md) for demonstration purposes. A full web app is under consideration.

### Q: Any plans for Apple Watch or Android Wear support?
**A:** Smartwatch companions are on the roadmap for future releases.

---

**Still need help?** Don't hesitate to [create an issue](https://github.com/rsutter98/NdcApp/issues/new) or join the [discussions](https://github.com/rsutter98/NdcApp/discussions)!