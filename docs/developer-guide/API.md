# API Reference

Technical reference for all public service interfaces in NdcApp.

## 📦 Namespaces

- `NdcApp.Core.Models` — Data transfer objects and domain models
- `NdcApp.Core.Services` — Business-logic service interfaces and implementations
- `NdcApp.Services` — Platform-specific service implementations (MAUI layer)

---

## 🗂️ Models

### `Talk`

Represents a single conference session.

| Property | Type | Description |
|---|---|---|
| `Day` | `string` | Day of the week (e.g., `"Monday"`) |
| `StartTime` | `TimeSpan` | Session start time |
| `EndTime` | `TimeSpan` | Session end time |
| `Room` | `string` | Room name or number |
| `Title` | `string` | Talk title |
| `Speaker` | `string` | Presenter name |
| `Category` | `string` | Topic category |
| `AverageRating` | `double` | Mean rating across all submitted ratings |
| `RatingCount` | `int` | Total number of ratings received |
| `Id` | `string` | Computed unique identifier (`Day\|StartTime\|Title\|Speaker`) |

### `TalkRating`

Represents a single user rating for a talk.

| Property | Type | Description |
|---|---|---|
| `TalkId` | `string` | The `Talk.Id` this rating belongs to |
| `Rating` | `int` | Score, typically 1–5 |
| `Comment` | `string` | Optional free-text comment |

### `TalkRecommendation`

A recommended talk returned by the recommendation engine.

| Property | Type | Description |
|---|---|---|
| `Talk` | `Talk` | The recommended talk |
| `Score` | `double` | Recommendation confidence score |
| `Reason` | `string` | Human-readable reason for the recommendation |

### `NotificationRequest`

Payload for scheduling a local notification.

| Property | Type | Description |
|---|---|---|
| `NotificationId` | `string` | Unique identifier for the notification |
| `Title` | `string` | Notification title |
| `Description` | `string` | Notification body text |
| `ScheduleTime` | `DateTime` | When to deliver the notification |

---

## 🛠️ Service Interfaces

### `IConferencePlanService`

Manages the user's personal conference schedule.

```csharp
void SelectTalk(Talk? talk)
void DeselectTalk(Talk? talk)
bool IsTalkSelected(Talk? talk)
List<Talk> GetSelectedTalks()
void ClearSelectedTalks()
string SerializeSelectedTalks()
void DeserializeSelectedTalks(string? serializedTalks)
Talk? GetNextSelectedTalk()
List<Talk> SortTalksBySpeaker(List<Talk> talks)
List<Talk> SortTalksByCategory(List<Talk> talks)
List<Talk> SortTalksByRating(List<Talk> talks)
List<Talk> SortTalksStandard(List<Talk> talks)
List<TalkRecommendation> GetRecommendations(List<Talk> talks, int maxRecommendations = 5)
void RateTalk(Talk? talk, int rating, string comment = "")
List<TalkRating> GetRatingsForTalk(Talk? talk)
void UpdateAllTalkRatings(List<Talk> talks)
```

**Key behaviors:**
- `SelectTalk` replaces any existing selection in the same time slot (one talk per slot enforced).
- `GetNextSelectedTalk` returns the nearest future talk based on the current system clock.
- `SerializeSelectedTalks` / `DeserializeSelectedTalks` handle persistence via `Preferences`.

---

### `ITalkService`

Loads and parses talk data from CSV sources.

```csharp
List<Talk> LoadTalks(string csvPath)
List<Talk> ParseTalksFromString(string csvContent)
```

The bundled data file is `NdcApp/Resources/Raw/ndc.csv`. The CSV format is:

```
Day,StartTime,EndTime,Room,Title,Speaker,Category
```

---

### `ITalkFilterService`

Searches talks across multiple fields.

```csharp
List<Talk> FilterTalks(List<Talk> talks, string searchText)
```

Searches `Title`, `Speaker`, `Category`, and `Room` (case-insensitive, substring match).

---

### `ITalkRatingService`

Stores and retrieves per-talk ratings independently of the plan service.

```csharp
void RateTalk(string talkId, int rating, string comment = "")
double GetAverageRating(string talkId)
int GetRatingCount(string talkId)
List<TalkRating> GetRatingsForTalk(string talkId)
List<TalkRecommendation> GetRecommendations(List<Talk> talks, int maxRecommendations = 5)
```

Ratings are keyed by `Talk.Id` and persisted across sessions.

---

### `INotificationService`

Schedules and cancels local device notifications.

```csharp
Task<bool> RequestPermissionAsync()
Task ScheduleNotificationAsync(NotificationRequest notification)
Task CancelNotificationAsync(string notificationId)
Task CancelAllNotificationsAsync()
Task<IEnumerable<NotificationRequest>> GetScheduledNotificationsAsync()
```

The MAUI implementation uses [Plugin.LocalNotification](https://github.com/thudugala/Plugin.LocalNotification). Call `RequestPermissionAsync()` before scheduling.

---

### `ILoggerService`

Structured application logging.

```csharp
void LogDebug(string message)
void LogInfo(string message)
void LogWarning(string message)
void LogError(string message, Exception? exception = null)
void LogError(Exception exception, string message)
```

In `DEBUG` builds the minimum log level is `Debug`; in `Release` builds it is `Information`.

---

### `IErrorHandlingService`

Centralised error handling with user-friendly messaging.

```csharp
Task<bool> HandleErrorAsync(Exception exception, string userMessage = "")
string GetUserFriendlyMessage(Exception exception)
void LogError(Exception exception, string context = "")
```

`HandleErrorAsync` logs the exception and optionally presents an alert to the user. Returns `true` on successful handling.

---

## 🔌 Platform Services (MAUI layer)

### `LocalNotificationService`

Implements `INotificationService` using Plugin.LocalNotification. Registered as a singleton in `MauiProgram`.

### `TalkNotificationService`

Orchestrates talk-reminder notifications: schedules or cancels notifications whenever the user's plan changes.

### `GlobalExceptionHandler`

Registers top-level handlers for unhandled exceptions on both the UI thread (`AppDomain.UnhandledException`) and background threads. Initialised at app startup via `MauiProgram`.

---

## 🔗 See Also

- [Architecture Overview](ARCHITECTURE.md)
- [Developer Guide](README.md)
- [Testing Guide](TESTING.md)
