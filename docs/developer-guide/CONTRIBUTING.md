# Contributing to NdcApp

Thank you for your interest in contributing to NdcApp! This guide will help you get started.

## 🎯 How to Contribute

We welcome contributions in many forms:
- **🐛 Bug Reports**: Help us identify and fix issues
- **💡 Feature Requests**: Suggest new functionality
- **📝 Documentation**: Improve guides and documentation
- **💻 Code**: Fix bugs, add features, improve performance
- **🧪 Testing**: Write tests, test on different platforms
- **🎨 Design**: UI/UX improvements and suggestions

## 🚀 Getting Started

### 1. Set Up Development Environment
Follow our [Developer Guide](README.md) to set up your development environment.

### 2. Find Something to Work On
- **Good First Issues**: Look for issues labeled `good-first-issue`
- **Help Wanted**: Check issues labeled `help-wanted`
- **Documentation**: Issues labeled `documentation`
- **Bug Reports**: Issues labeled `bug`

### 3. Fork and Clone
```bash
# Fork the repository on GitHub
# Then clone your fork
git clone https://github.com/YOUR-USERNAME/NdcApp.git
cd NdcApp
```

## 🔄 Contribution Workflow

### 1. Create a Branch
```bash
# Create and switch to a new branch
git checkout -b feature/your-feature-name
# or
git checkout -b fix/issue-description
```

### 2. Make Your Changes
- Write clean, readable code
- Follow existing code style and patterns
- Add tests for new functionality
- Update documentation as needed

### 3. Test Your Changes
```bash
# Run all tests
dotnet test

# Test on multiple platforms if possible
dotnet run --project NdcApp/NdcApp.csproj -f net8.0-windows10.0.19041.0
dotnet run --project NdcApp/NdcApp.csproj -f net8.0-android
```

### 4. Commit Your Changes
```bash
# Add your changes
git add .

# Commit with a descriptive message
git commit -m "feat: add talk filtering by category

- Add category filter dropdown to ConferencePlanPage
- Update TalkService to support category filtering
- Add tests for category filtering functionality
- Update user documentation"
```

### 5. Push and Create Pull Request
```bash
# Push to your fork
git push origin feature/your-feature-name

# Create a pull request on GitHub
```

## 📋 Pull Request Guidelines

### Before Submitting
- [ ] **All tests pass** (92+ tests must remain passing)
- [ ] **New code has tests** (maintain high test coverage)
- [ ] **Documentation updated** (for user-facing changes)
- [ ] **Code follows style** (consistent with existing code)
- [ ] **Single focused change** (one feature/fix per PR)

### Pull Request Description
Include:
- **What**: Brief description of the change
- **Why**: Reason for the change
- **How**: Technical approach taken
- **Testing**: How you tested the change
- **Breaking Changes**: Any compatibility issues

Example:
```markdown
## What
Add category filtering to conference plan page

## Why
Users requested ability to filter talks by category to make it easier to find relevant sessions

## How
- Added category dropdown to ConferencePlanPage.xaml
- Extended TalkService with FilterByCategory method
- Updated ConferencePlanService to support category filtering

## Testing
- Added unit tests for FilterByCategory functionality
- Tested on Windows and Android platforms
- Verified with sample NDC data

## Breaking Changes
None - this is a new feature with no API changes
```

## 🎨 Code Style Guidelines

### C# Code Style
- Use **PascalCase** for public members
- Use **camelCase** for private fields and parameters
- Use **nullable reference types** consistently
- Include **XML documentation** for public APIs
- Keep methods **focused and testable**

### XAML Guidelines
- Use **x:DataType** for compile-time binding
- Follow **MVVM pattern** consistently
- Use **data binding** over code-behind
- Maintain **NDC design consistency**

### Test Guidelines
- Use **descriptive test names**: `Should_ReturnFilteredTalks_When_CategoryProvided`
- Follow **AAA pattern**: Arrange, Act, Assert
- Test **edge cases and error conditions**
- Mock **external dependencies**

## 📚 Documentation Guidelines

### User Documentation
- Write in **clear, simple language**
- Include **step-by-step instructions**
- Add **screenshots** where helpful
- Consider **different skill levels**

### Developer Documentation
- Explain **technical decisions**
- Include **code examples**
- Document **APIs and interfaces**
- Keep **architectural docs** updated

### Documentation Updates Required For:
- New user-facing features
- API changes
- Installation/setup changes
- Configuration changes
- Breaking changes

## 🧪 Testing Requirements

### Test Categories
All changes should include appropriate tests:

- **Unit Tests**: Business logic in NdcApp.Core
- **Integration Tests**: End-to-end scenarios
- **UI Tests**: User interface behavior (when possible)
- **Platform Tests**: Platform-specific functionality

### Test Coverage
- Maintain **92+ passing tests**
- New features need **corresponding tests**
- Bug fixes need **regression tests**
- Edge cases should be **tested**

### Running Tests
```bash
# Run all tests
dotnet test

# Run specific test category
dotnet test --filter "Category=Unit"

# Run with coverage (if tools available)
dotnet test --collect:"XPlat Code Coverage"
```

## 🚀 Types of Contributions

### 🐛 Bug Fixes
1. **Reproduce the bug** first
2. **Write a failing test** that demonstrates the issue
3. **Fix the bug** with minimal changes
4. **Verify the test passes** and no regressions
5. **Update documentation** if needed

### ✨ New Features
1. **Discuss the feature** in an issue first
2. **Design the API/UI** before coding
3. **Implement incrementally** with tests
4. **Update documentation** and examples
5. **Consider backward compatibility**

### 📝 Documentation
1. **Identify gaps** in current documentation
2. **Write clear, helpful content**
3. **Test instructions** by following them
4. **Get feedback** from other contributors
5. **Keep it up to date**

### 🎨 UI/UX Improvements
1. **Understand the current design** system
2. **Maintain NDC branding** consistency
3. **Consider accessibility** requirements
4. **Test on multiple platforms**
5. **Get design feedback** before major changes

## 🤝 Community Guidelines

### Be Respectful
- Use **welcoming and inclusive** language
- Respect **different viewpoints** and experiences
- Accept **constructive feedback** gracefully
- Focus on **what's best for the community**

### Collaborate Effectively
- **Ask questions** if you're unsure
- **Share knowledge** and help others
- **Review others' contributions** thoughtfully
- **Be patient** with response times

### Communication
- Use **GitHub issues** for bug reports and feature requests
- Use **GitHub discussions** for questions and ideas
- Use **pull request comments** for code-specific discussions
- Use **clear, descriptive titles** for issues and PRs

## 🎖️ Recognition

We appreciate all contributions! Contributors will be:
- **Listed in release notes** for significant contributions
- **Mentioned in README** acknowledgments
- **Invited to team discussions** for regular contributors
- **Given credit** in commit messages and documentation

## 📞 Getting Help

### Need Help Contributing?
- **Read the documentation**: Start with [Developer Guide](README.md)
- **Ask in discussions**: [GitHub Discussions](https://github.com/rsutter98/NdcApp/discussions)
- **Check existing issues**: [GitHub Issues](https://github.com/rsutter98/NdcApp/issues)
- **Contact maintainers**: Create an issue with `question` label

### Resources
- **Git Help**: [GitHub Git Handbook](https://guides.github.com/introduction/git-handbook/)
- **.NET MAUI Docs**: [Microsoft MAUI Documentation](https://docs.microsoft.com/dotnet/maui/)
- **C# Guidelines**: [C# Coding Conventions](https://docs.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions)

---

**Thank you for contributing to NdcApp!** 🎉

Your contributions help make conference planning better for everyone. Don't hesitate to ask questions or seek help - we're here to support you!