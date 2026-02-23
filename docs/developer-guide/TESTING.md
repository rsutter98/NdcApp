# NdcApp Test Suite

This test suite provides comprehensive coverage for the NdcApp conference planning application.

## Overview

The NdcApp is a .NET MAUI application for managing conference schedules. Users can:
- Load conference talks from CSV files
- Select talks to attend
- Sort talks by speaker, category, or chronologically
- Persist their selections
- View their next upcoming talk

## Test Coverage

### Core Library (`NdcApp.Core`)
The core business logic has been extracted into a separate class library for testability:

- **Models/Talk.cs**: Conference talk data model
- **Services/TalkService.cs**: CSV loading and parsing
- **Services/ConferencePlanService.cs**: Talk selection and management
- **Services/TalkFilterService.cs**: Full-text talk search
- **Services/TalkRatingService.cs**: Ratings and recommendation engine
- **Services/TalkNotificationService.cs**: Notification scheduling
- **Services/ErrorHandlingService.cs**: Exception handling and user messages
- **Services/LoggerService.cs**: Logging abstraction

### Test Files

1. **TalkServiceTests.cs** (8 tests)
   - Tests CSV file loading and parsing
   - Covers error handling for missing files and invalid data
   - Tests both file-based and string-based parsing methods

2. **TalkFilterServiceTests.cs** (9 tests)
   - Tests full-text search across title, speaker, category, and room
   - Case-insensitive matching, empty/null input handling

3. **TalkRatingServiceTests.cs** (10 tests)
   - Tests rating submission, average calculation, and rating count
   - Tests recommendation score computation

4. **ConferencePlanServiceTests.cs** (21 tests)
   - Tests talk selection and deselection
   - Tests persistence through serialization/deserialization
   - Tests all sorting methods (speaker, category, chronological, rating)
   - Tests time conflict handling (same time slot selection)
   - Tests next talk calculation

5. **ConferencePlanServiceRatingTests.cs** (7 tests)
   - Tests rating delegation from `ConferencePlanService` to `ITalkRatingService`
   - Tests `UpdateAllTalkRatings` and recommendation retrieval

6. **TalkNotificationServiceTests.cs** (5 tests)
   - Tests notification scheduling for selected talks
   - Tests permission request flow and cancellation

7. **ErrorHandlingServiceTests.cs** (7 tests)
   - Tests user-friendly message mapping for each exception type
   - Tests `HandleErrorAsync` and `LogError`

8. **LoggerServiceTests.cs** (2 tests)
   - Tests log level delegation to `ILogger`

9. **NotificationIntegrationTests.cs** (3 tests)
   - End-to-end notification scheduling integration

10. **IntegrationTests.cs** (5 tests)
    - End-to-end workflow testing
    - Complete conference scenario simulation
    - Cross-component integration

11. **ActualDataTests.cs** (2 tests)
    - Tests with real CSV data structure
    - Validates against known NDC conference data format

12. **RealDataNotificationTests.cs** (2 tests)
    - Notification scheduling using production-like talk data

13. **PerformanceTests.cs** (4 tests)
    - Timing assertions for CSV loading and filtering operations

14. **PerformanceBenchmarkTests.cs** (4 tests)
    - Benchmark-style tests for sorting and recommendation computation

15. **UnitTest1.cs** (3 tests)
    - Smoke tests for basic service construction

## Running Tests

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity detailed

# Run specific test class
dotnet test --filter "ClassName=TalkServiceTests"

# Generate coverage report (if coverage tools are installed)
dotnet test --collect:"XPlat Code Coverage"
```

## Test Statistics

- **Total Tests**: 92
- **Test Classes**: 15
- **All tests passing**: ✅
- **Code Coverage**: Comprehensive coverage of all business logic

## Key Testing Scenarios

### 1. CSV Data Loading
- Valid CSV parsing
- Error handling for invalid/missing files
- Header skipping and data validation

### 2. Talk Selection Management
- Single talk selection
- Time conflict resolution (same time slot)
- Selection persistence across app restarts
- Bulk operations

### 3. Sorting Functionality
- Chronological sorting (Monday → Sunday, then by time)
- Alphabetical sorting by speaker
- Category-based sorting
- Maintaining selection state during sorting

### 4. Data Persistence
- Serialization of selected talks
- Deserialization and restoration
- Round-trip data integrity

### 5. UI Support
- Boolean converters for UI state
- Text converters for button labels
- Color converters for visual feedback

### 6. Edge Cases
- Null input handling
- Invalid data graceful handling
- Empty collections
- Malformed CSV data

## Architecture Benefits

The test suite demonstrates:

1. **Separation of Concerns**: Business logic separated from UI for testability
2. **Comprehensive Coverage**: All major functionality paths tested
3. **Error Resilience**: Graceful handling of invalid inputs
4. **Maintainability**: Clear test structure and naming conventions
5. **Integration Testing**: End-to-end scenarios validate complete workflows

## Future Enhancements

Potential areas for additional testing:
- Performance testing with large CSV files
- Concurrent access testing
- UI automation tests (requires MAUI test framework)
- Load testing for multiple simultaneous users