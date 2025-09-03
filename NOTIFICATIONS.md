# Notification System

## Overview

The NDC App now includes a comprehensive notification system that automatically reminds users about their selected talks.

## Features

### 🔔 Push Notifications for Upcoming Talks
- Automatic notifications scheduled when you select talks
- Multiple reminder intervals: 15 minutes, 5 minutes, and 1 minute before each talk
- Smart scheduling that only creates notifications for future talks

### ⏰ Reminder System
- Integrated with the existing talk selection system
- Notifications are automatically rescheduled when you select/deselect talks
- View upcoming notifications with the 🔔 button in the app

### 📱 Cross-Platform Support
- Uses local notifications that work on all MAUI platforms
- Requests notification permissions on first use
- Graceful fallback if permissions are denied

## How It Works

1. **Select a Talk**: When you select a talk using the "Select" button or swipe gesture, the app automatically schedules notifications
2. **Automatic Reminders**: You'll receive 3 notifications for each selected talk:
   - 15 minutes before: "Talk starts in 15 minutes"
   - 5 minutes before: "Talk starts in 5 minutes" 
   - 1 minute before: "Talk starts in 1 minute!"
3. **View Notifications**: Tap the 🔔 button to see all upcoming notifications
4. **Smart Updates**: When you deselect a talk, its notifications are automatically cancelled

## Technical Implementation

### Core Components

- **`NotificationRequest`**: Model representing a scheduled notification
- **`INotificationService`**: Interface for platform-specific notification implementations
- **`TalkNotificationService`**: Core service that manages talk-related notifications
- **`LocalNotificationService`**: MAUI implementation using Plugin.LocalNotification

### Architecture

```
UI Layer (ConferencePlanPage)
    ↓ 
Core Service (TalkNotificationService)
    ↓
Platform Service (LocalNotificationService)
    ↓
Native Notifications (Plugin.LocalNotification)
```

### Integration Points

- Integrated with existing `ConferencePlanService` for talk selection
- Uses Dependency Injection for service registration
- Preserves existing UI flow and user experience

## Configuration

The notification system is automatically configured in `MauiProgram.cs`:

```csharp
builder.Services.AddSingleton<ConferencePlanService>();
builder.Services.AddSingleton<INotificationService, LocalNotificationService>();
builder.Services.AddSingleton<TalkNotificationService>();
```

## Future Enhancements

- **Calendar Integration**: Export selected talks to device calendar
- **Custom Reminder Times**: Allow users to set custom notification intervals
- **Rich Notifications**: Add talk images and action buttons
- **Background Sync**: Update notifications when talk details change

## Testing

The notification system includes comprehensive unit tests covering:
- Permission handling
- Notification scheduling for selected talks
- Cancellation of notifications
- Edge cases (past talks, upcoming talks)

Run tests with: `dotnet test`