# API Reference

Complete reference for all public types and services in `NdcApp.Core`.

## Models

### `Talk`

Represents a conference session.

**Namespace:** `NdcApp.Core.Models`

| Property | Type | Description |
|---|---|---|
| `Day` | `string` | Day of the week (e.g., `"Monday"`) |
| `StartTime` | `TimeSpan` | Session start time |
| `EndTime` | `TimeSpan` | Session end time |
| `Room` | `string` | Room where the session takes place |
| `Title` | `string` | Talk title |
| `Speaker` | `string` | Speaker name |
| `Category` | `string` | Topic area (e.g., `"Backend"`, `"Frontend"`) |
| `AverageRating` | `double` | Aggregated star rating (0.0–5.0) |
| `RatingCount` | `int` | Total number of ratings received |
| `Id` | `string` (read-only) | Composite key: `"Day\|StartTime\|Title\|Speaker"` |

---

### `TalkRating`

A single user rating for a talk.

**Namespace:** `NdcApp.Core.Models`

| Property | Type | Description |
|---|---|---|
| `TalkId` | `string` | Matches `Talk.Id` |
| `Rating` | `int` | Star value (1–5) |
| `Comment` | `string` | Optional free-text comment |
| `RatingDate` | `DateTime` | When the rating was given (default: `DateTime.Now`) |
| `UserId` | `string` | Identifies the rater (default: `"default"`) |

---

### `TalkRecommendation`

A recommended talk with an explanation.

**Namespace:** `NdcApp.Core.Models`

| Property | Type | Description |
|---|---|---|
| `Talk` | `Talk` | The recommended talk |
| `Score` | `double` | Recommendation strength (0.0–1.0) |
| `Reason` | `string` | Human-readable rationale (e.g., `"Highly rated (4.8★), Popular choice"`) |

---

### `NotificationRequest`

A scheduled notification for an upcoming talk.

**Namespace:** `NdcApp.Core.Models`

| Property | Type | Description |
|---|---|---|
| `Id` | `string` | Unique notification identifier |
| `Title` | `string` | Notification title shown to the user |
| `Message` | `string` | Notification body text |
| `ScheduledDateTime` | `DateTime` | When to deliver the notification |
| `RelatedTalk` | `Talk` | The talk this notification is for |
| `Type` | `NotificationType` | One of `TalkReminder`, `TalkStarting`, or `ScheduleUpdate` |

---

### `NotificationType` (enum)

| Value | Description |
|---|---|
| `TalkReminder` | Reminder for a talk scheduled in advance |
| `TalkStarting` | Notification that a talk starts imminently |
| `ScheduleUpdate` | Alert about a schedule change |

---

## Services

### `ITalkService`

Loads and parses talk data from CSV sources.

**Namespace:** `NdcApp.Core.Services`  
**Implementation:** `TalkService`

#### Methods

```csharp
List<Talk> LoadTalks(string csvPath)
```
Reads and parses a CSV file at `csvPath`. Skips the header row and ignores malformed lines.

- **Throws** `FileNotFoundException` if the file does not exist.

```csharp
List<Talk> ParseTalksFromString(string csvContent)
```
Parses CSV content from a string. Useful for embedded resources or network payloads.

**CSV column order:** `Day, StartTime, EndTime, Room, Title, Speaker, Category`

---

### `ITalkFilterService`

Filters talks using a search string.

**Namespace:** `NdcApp.Core.Services`  
**Implementation:** `TalkFilterService`

#### Methods

```csharp
List<Talk> FilterTalks(List<Talk> talks, string searchText)
```
Returns talks whose `Title`, `Speaker`, `Category`, or `Room` contains `searchText` (case-insensitive). Returns all talks when `searchText` is null or whitespace.

---

### `ITalkRatingService`

Stores and queries talk ratings; generates recommendations.

**Namespace:** `NdcApp.Core.Services`  
**Implementation:** `TalkRatingService`

#### Methods

```csharp
void RateTalk(string talkId, int rating, string comment = "")
```
Adds or replaces the current user's rating for `talkId`. `rating` must be 1–5.

- **Throws** `ArgumentException` for out-of-range ratings.

```csharp
double GetAverageRating(string talkId)
```
Returns the mean rating for a talk, or `0.0` if no ratings exist.

```csharp
int GetRatingCount(string talkId)
```
Returns the total number of ratings for a talk.

```csharp
List<TalkRating> GetRatingsForTalk(string talkId)
```
Returns all ratings for a talk, ordered by `RatingDate` descending.

```csharp
void UpdateTalkRatings(List<Talk> talks)
```
Writes `AverageRating` and `RatingCount` onto each `Talk` in the list.

```csharp
List<TalkRecommendation> GetRecommendations(List<Talk> talks, int maxRecommendations = 5)
```
Returns up to `maxRecommendations` talks ordered by recommendation score. Score factors:

| Factor | Weight | Description |
|---|---|---|
| Average rating | 40 % | Normalised 1–5 star value |
| Popularity | 20 % | Ratings count relative to the most-rated talk |
| Category trend | 20 % | Bonus for popular categories |
| Recent activity | 20 % | Bonus for ratings in the past 30 days |

---

### `IConferencePlanService`

Manages the user's personal conference schedule.

**Namespace:** `NdcApp.Core.Services`  
**Implementation:** `ConferencePlanService`

#### Talk Selection

```csharp
void SelectTalk(Talk? talk)
```
Adds a talk to the schedule. If a talk in the same time slot already exists it is replaced.

```csharp
void DeselectTalk(Talk? talk)
```
Removes a talk from the schedule. Null is a no-op.

```csharp
bool IsTalkSelected(Talk? talk)
```
Returns `true` when the talk is in the current schedule.

```csharp
List<Talk> GetSelectedTalks()
```
Returns all selected talks as a list.

```csharp
void ClearSelectedTalks()
```
Removes all talks from the schedule.

#### Persistence

```csharp
string SerializeSelectedTalks()
```
Serializes selected talks to a pipe-delimited string suitable for `Preferences` storage.

Format per talk: `Day,StartTime,EndTime,Room,Title,Speaker,Category,AverageRating,RatingCount`

```csharp
void DeserializeSelectedTalks(string? serializedTalks)
```
Restores the schedule from a previously serialized string. Passing null or empty clears the schedule.

- **Throws** on invalid format after logging an error.

#### Sorting

```csharp
List<Talk> SortTalksStandard(List<Talk> talks)      // Chronological (day then time)
List<Talk> SortTalksBySpeaker(List<Talk> talks)     // Alphabetical by speaker
List<Talk> SortTalksByCategory(List<Talk> talks)    // Alphabetical by category
List<Talk> SortTalksByRating(List<Talk> talks)      // Highest average rating first
```

#### Recommendations & Ratings

```csharp
List<TalkRecommendation> GetRecommendations(List<Talk> talks, int maxRecommendations = 5)
void RateTalk(Talk? talk, int rating, string comment = "")
List<TalkRating> GetRatingsForTalk(Talk? talk)
void UpdateAllTalkRatings(List<Talk> talks)
```
Delegates to the injected `ITalkRatingService`. See `ITalkRatingService` above for details.

#### Upcoming Talk

```csharp
Talk? GetNextSelectedTalk()
```
Returns the next selected talk with a `StartTime` greater than `DateTime.Now.TimeOfDay`, sorted chronologically. Returns `null` when no future talks are selected.

---

### `INotificationService`

Schedules and cancels system notifications.

**Namespace:** `NdcApp.Core.Services`  
**Platform implementation:** `NdcApp.Services.LocalNotificationService` (uses [Plugin.LocalNotification](https://github.com/thudugala/Plugin.LocalNotification))

#### Methods

```csharp
Task<bool> RequestPermissionAsync()
```
Prompts the user for notification permission. Returns `true` if granted.

```csharp
Task ScheduleNotificationAsync(NotificationRequest notification)
```
Schedules a notification for delivery at `notification.ScheduledDateTime`.

```csharp
Task CancelNotificationAsync(string notificationId)
```
Cancels a single scheduled notification by its `Id`.

```csharp
Task CancelAllNotificationsAsync()
```
Cancels all scheduled notifications.

```csharp
Task<IEnumerable<NotificationRequest>> GetScheduledNotificationsAsync()
```
Returns all future (not yet delivered) scheduled notifications.

---

### `TalkNotificationService`

Orchestrates notification scheduling for all selected talks.

**Namespace:** `NdcApp.Core.Services`

> **Note:** This is a concrete class, not an interface. It composes `INotificationService` and `ConferencePlanService`.

#### Methods

```csharp
Task<bool> RequestNotificationPermissionAsync()
```
Forwards the permission request to `INotificationService`.

```csharp
Task ScheduleNotificationsForSelectedTalksAsync()
```
Cancels all existing notifications, then schedules reminders for every selected talk at **15 minutes**, **5 minutes**, and **1 minute** before start. Only future notifications are created.

```csharp
Task CancelAllNotificationsAsync()
```
Cancels all outstanding talk notifications.

```csharp
Task<IEnumerable<NotificationRequest>> GetUpcomingNotificationsAsync()
```
Returns all pending notifications.

```csharp
List<Talk> GetTalksStartingSoon(int withinMinutes = 30)
```
Returns selected talks whose `StartTime` falls within the next `withinMinutes` minutes.

---

### `IErrorHandlingService`

Centralises exception handling and user-facing error messages.

**Namespace:** `NdcApp.Core.Services`  
**Implementation:** `ErrorHandlingService`

#### Methods

```csharp
Task<bool> HandleErrorAsync(Exception exception, string userMessage = "")
```
Logs the exception and returns `true` to indicate it was handled. Generates a user-friendly message when `userMessage` is empty.

```csharp
string GetUserFriendlyMessage(Exception exception)
```
Maps exception types to plain-English messages:

| Exception | Message |
|---|---|
| `FileNotFoundException` | File could not be found — try refreshing. |
| `UnauthorizedAccessException` | Access denied — check permissions. |
| `HttpRequestException` | Network error — check connectivity. |
| `TimeoutException` | Operation timed out — try again. |
| `ArgumentNullException` / `ArgumentException` | Invalid input — check your data. |
| `IOException` / `DirectoryNotFoundException` | File operation failed — try again. |
| Others | Unexpected error — contact support. |

```csharp
void LogError(Exception exception, string context = "")
```
Writes the exception with optional context to `ILoggerService`.

---

### `ILoggerService`

Thin wrapper over `Microsoft.Extensions.Logging.ILogger`.

**Namespace:** `NdcApp.Core.Services`  
**Implementation:** `LoggerService`

#### Methods

```csharp
void LogDebug(string message)
void LogInfo(string message)
void LogWarning(string message)
void LogError(string message, Exception? exception = null)
void LogError(Exception exception, string message)
```

Logging levels map directly to `ILogger` severity levels: `Debug`, `Information`, `Warning`, and `Error`.

---

## Dependency Injection

Register all services in `MauiProgram.CreateMauiApp()`:

```csharp
// Core services (singletons for in-memory state)
builder.Services.AddSingleton<ITalkRatingService, TalkRatingService>();
builder.Services.AddSingleton<IConferencePlanService, ConferencePlanService>();
builder.Services.AddSingleton<ConferencePlanService>();         // concrete registration required by TalkNotificationService
builder.Services.AddSingleton<ITalkService, TalkService>();
builder.Services.AddSingleton<ITalkFilterService, TalkFilterService>();
builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddSingleton<IErrorHandlingService, ErrorHandlingService>();

// Platform-specific
builder.Services.AddSingleton<INotificationService, LocalNotificationService>();
builder.Services.AddSingleton<TalkNotificationService>();
builder.Services.AddSingleton<GlobalExceptionHandler>();
```

---

## See Also

- [Architecture](ARCHITECTURE.md) — Component diagram and design decisions
- [Testing](TESTING.md) — How the API is covered by tests
- [Contributing](CONTRIBUTING.md) — Adding new services and models
