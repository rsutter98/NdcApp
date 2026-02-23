# Developer Guide

Welcome to the NdcApp development guide! This document helps you get started contributing to the project.

## 🎯 Overview

NdcApp is a .NET MAUI cross-platform conference planning application. The architecture emphasizes:
- **Testability**: Business logic separated into NdcApp.Core
- **Cross-platform compatibility**: Windows, Android, iOS support
- **Modern .NET**: Built with .NET 8.0 and MAUI
- **Clean architecture**: Clear separation of concerns

## 🛠️ Development Setup

### Prerequisites
- **Visual Studio 2022** (recommended) or **VS Code**
- **.NET 8.0 SDK** - [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
- **MAUI Workloads** - Install via `dotnet workload install maui`

### Platform-Specific Requirements

#### Windows Development
- Windows 11 (recommended) or Windows 10 version 1903+
- Visual Studio 2022 with MAUI workload
- Windows App SDK for MSIX packaging

#### Android Development  
- Android SDK API 21+ (Android 5.0+)
- Android emulator or physical device
- USB debugging enabled (for physical devices)

#### iOS Development (macOS only)
- Xcode 14+ with iOS 15+ SDK
- iOS Simulator or physical iOS device
- Apple Developer account (for device testing)

## 🚀 Getting Started

### 1. Clone and Build
```bash
# Clone the repository
git clone https://github.com/rsutter98/NdcApp.git
cd NdcApp

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests to verify setup
dotnet test
```

### 2. Run the Application
```bash
# Run on specific platform
dotnet run --project NdcApp/NdcApp.csproj -f net8.0-windows10.0.19041.0
dotnet run --project NdcApp/NdcApp.csproj -f net8.0-android
```

### 3. Development Workflow
```bash
# Run tests continuously during development
dotnet watch test --project NdcApp.Tests

# Hot reload for UI changes (MAUI apps)
dotnet run --project NdcApp/NdcApp.csproj
```

## 🏗️ Architecture Overview

### Project Structure
```
NdcApp/
├── NdcApp/                 # MAUI UI Application
│   ├── MainPage.xaml       # Main landing page
│   ├── ConferencePlanPage.xaml # Talk management UI
│   ├── Converters/         # XAML value converters
│   └── Resources/          # App resources, icons, CSV data
├── NdcApp.Core/           # Business Logic Library
│   ├── Models/            # Data models (Talk, etc.)
│   ├── Services/          # Business services
│   └── Converters/        # UI helper converters
├── NdcApp.Preview/        # Blazor Web Preview
├── NdcApp.Tests/          # Test Suite (92 tests)
└── docs/                  # Documentation
```

### Key Components

#### Models (`NdcApp.Core/Models/`)
- **Talk.cs**: Conference session data model
- Domain objects with validation and business rules

#### Services (`NdcApp.Core/Services/`)
- **TalkService.cs**: CSV loading and parsing
- **TalkFilterService.cs**: Case-insensitive full-text talk search
- **TalkRatingService.cs**: In-memory ratings and recommendation engine
- **ConferencePlanService.cs**: Talk selection, sorting, and persistence
- **TalkNotificationService.cs**: Multi-reminder notification scheduling
- **ErrorHandlingService.cs**: Exception-to-user-message mapping
- **LoggerService.cs**: `ILoggerService` wrapping `Microsoft.Extensions.Logging`
- Pure business logic, framework-independent

#### UI Layer (`NdcApp/`)
- **MainPage.xaml**: Landing page with next talk display
- **ConferencePlanPage.xaml**: Main talk management interface
- MVVM pattern with data binding

## 🧪 Testing Strategy

### Test Organization
The test suite covers all business logic comprehensively:

- **TalkServiceTests.cs** (8 tests) - CSV parsing and loading
- **TalkFilterServiceTests.cs** (9 tests) - Full-text search filtering
- **TalkRatingServiceTests.cs** (10 tests) - Ratings and recommendations
- **ConferencePlanServiceTests.cs** (21 tests) - Core business logic
- **ConferencePlanServiceRatingTests.cs** (7 tests) - Rating delegation
- **TalkNotificationServiceTests.cs** (5 tests) - Notification scheduling
- **ErrorHandlingServiceTests.cs** (7 tests) - Error handling
- **IntegrationTests.cs** (5 tests) - End-to-end scenarios
- **ActualDataTests.cs** (2 tests) - Real data validation
- **PerformanceTests.cs** (4 tests) - Performance baselines

### Running Tests
```bash
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "ClassName=TalkServiceTests"

# Run with detailed output
dotnet test --verbosity detailed

# Watch for changes
dotnet watch test
```

### Writing Tests
- Test business logic in `NdcApp.Core` (testable without UI)
- Use descriptive test names: `Should_ReturnSelectedTalks_When_FilteredByMyTalks`
- Follow AAA pattern: Arrange, Act, Assert
- Mock external dependencies
- Test edge cases and error conditions

## 🎨 UI Development

### XAML Guidelines
- Use `x:DataType` for compile-time binding validation
- Leverage data binding over code-behind
- Keep UI logic in converters when possible
- Follow MVVM pattern consistently

### Styling
- Maintain NDC corporate design (blue/yellow theme)
- Use consistent spacing and typography
- Ensure responsive design across screen sizes
- Test on multiple platforms and devices

## 📱 Platform-Specific Considerations

### Windows
- MSIX packaging configuration in project file
- Windows 11 design principles
- Native file system access for CSV import

### Android  
- Material Design principles
- APK optimization settings
- Android-specific permissions

### iOS
- Human Interface Guidelines compliance
- App Store submission requirements
- iOS-specific capabilities

## 🔧 Configuration Management

### Build Configurations
- **Debug**: Single platform (net8.0) for faster development
- **Release**: Multi-platform targeting with `BuildingForRelease=true`

### Environment Variables
- `BuildingForRelease=true`: Enables multi-platform targeting
- Configure in CI/CD for release builds

## 📦 Deployment

### Local Development Builds
```bash
# Debug build (single platform)
dotnet build

# Release build (all platforms)
dotnet build -c Release -p:BuildingForRelease=true
```

### Platform-Specific Deployment
```bash
# Android APK
dotnet publish -f net8.0-android -c Release

# Windows MSIX
dotnet publish -f net8.0-windows10.0.19041.0 -c Release

# iOS (macOS only)
dotnet build -f net8.0-ios -c Release
```

## 🤝 Contributing Guidelines

### Code Style
- Use C# naming conventions (PascalCase for public members)
- Include XML documentation for public APIs
- Keep methods focused and testable
- Use nullable reference types consistently

### Git Workflow
1. Create feature branch from `main`
2. Write tests for new functionality
3. Ensure all tests pass
4. Update documentation as needed
5. Submit pull request with clear description

### Pull Request Requirements
- [ ] All tests passing (92+ tests)
- [ ] New features have corresponding tests
- [ ] Documentation updated for user-facing changes
- [ ] Code follows established patterns
- [ ] No breaking changes without discussion

## 🔗 Useful Resources

### Documentation
- [MAUI Documentation](https://docs.microsoft.com/dotnet/maui/)
- [.NET 8 Documentation](https://docs.microsoft.com/dotnet/core/)
- [Testing in .NET](https://docs.microsoft.com/dotnet/core/testing/)

### Tools
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- [Hot Reload](https://docs.microsoft.com/visualstudio/debugger/hot-reload)
- [MAUI Essentials](https://docs.microsoft.com/dotnet/maui/essentials/)

### Project Links
- [Main Repository](https://github.com/rsutter98/NdcApp)
- [Issues & Bug Reports](https://github.com/rsutter98/NdcApp/issues)
- [Build Pipeline](https://github.com/rsutter98/NdcApp/actions)

---

**Ready to contribute?** Check out the [Contributing Guide](CONTRIBUTING.md) for specific contribution workflow and requirements.