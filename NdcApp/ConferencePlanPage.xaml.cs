using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using NdcApp.Core.Models;
using NdcApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NdcApp
{
    public partial class ConferencePlanPage : ContentPage
    {
        private const string SELECTED_TALKS_PREFERENCE_KEY = "SelectedTalks";
        
        private List<Talk> allTalks = new();
        private Dictionary<string, Talk> selectedTalks = new(); // key: "Day|StartTime"
        private bool showAll = true;
        private string searchText = string.Empty;
        private readonly TalkNotificationService? _notificationService;
        private readonly ConferencePlanService _conferencePlanService;

        public ConferencePlanPage()
        {
            InitializeComponent();
            
            // Get services from DI
            _notificationService = ServiceHelper.GetService<TalkNotificationService>();
            _conferencePlanService = ServiceHelper.GetService<ConferencePlanService>() ?? new ConferencePlanService();
            
            LoadTalks();
            _ = InitializeNotificationsAsync();
        }

        private async Task InitializeNotificationsAsync()
        {
            if (_notificationService != null)
            {
                var hasPermission = await _notificationService.RequestNotificationPermissionAsync();
                if (!hasPermission)
                {
                    await DisplayAlert("Notifications", 
                        "Notification permission denied. You won't receive reminders for your selected talks.", 
                        "OK");
                }
            }
        }

        private void LoadTalks()
        {
            // Always load CSV from project Resources/Raw folder
            string csvFileName = "ndc.csv";
            string resourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Raw", csvFileName);
            if (!System.IO.File.Exists(resourcePath))
            {
                // Fallback: Try loading from project root (for debugging)
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var parentDir = Directory.GetParent(baseDir);
                var grandParentDir = parentDir?.Parent;
                var greatGrandParentDir = grandParentDir?.Parent;
                if (greatGrandParentDir != null)
                {
                    resourcePath = Path.Combine(greatGrandParentDir.FullName, "Resources", "Raw", csvFileName);
                }
            }
            if (!System.IO.File.Exists(resourcePath))
            {
                DisplayAlert("Error", $"CSV file not found: {resourcePath}", "OK");
                allTalks = new List<Talk>();
                TalksCollectionView.ItemsSource = allTalks;
                return;
            }
            allTalks = TalkService.LoadTalks(resourcePath);
            
            // Load sample rating data for demonstration
            SampleRatingDataService.LoadSampleRatingData(_conferencePlanService.GetRatingService(), allTalks);
            
            TalksCollectionView.ItemsSource = allTalks;
        }

        private async Task LoadSelectedTalks()
        {
            var selectedTalksRaw = Preferences.Default.Get(SELECTED_TALKS_PREFERENCE_KEY, "");
            selectedTalks.Clear();
            _conferencePlanService.ClearSelectedTalks();
            
            if (!string.IsNullOrEmpty(selectedTalksRaw))
            {
                var selected = selectedTalksRaw.Split('|')
                    .Select(line => line.Split(','))
                    .Where(parts => parts.Length == 7)
                    .Select(parts => new Talk {
                        Day = parts[0],
                        StartTime = TimeSpan.Parse(parts[1]),
                        EndTime = TimeSpan.Parse(parts[2]),
                        Room = parts[3],
                        Title = parts[4],
                        Speaker = parts[5],
                        Category = parts[6]
                    });
                    
                foreach (var talk in selected)
                {
                    var key = $"{talk.Day}|{talk.StartTime}";
                    selectedTalks[key] = talk;
                    _conferencePlanService.SelectTalk(talk);
                }
                
                // Schedule notifications for loaded talks
                await ScheduleNotificationsAsync();
            }
        }

        private void OnShowAllClicked(object sender, EventArgs e)
        {
            showAll = true;
            RefreshTalksView();
        }

        private void OnShowSelectedClicked(object sender, EventArgs e)
        {
            showAll = false;
            ShowSelectedTalksOnly();
        }

        public void RefreshTalksView()
        {
            if (!showAll)
            {
                ShowSelectedTalksOnly();
                return;
            }
            
            // Apply search filter first
            var filteredTalks = FilterTalks(allTalks);
            
            // Group talks by day and time slot
            var grouped = filteredTalks
                .GroupBy(t => new { t.Day, t.StartTime })
                .OrderBy(g => g.Key.Day)
                .ThenBy(g => g.Key.StartTime)
                .ToList();

            var displayList = new List<TalkDisplayItem>();
            foreach (var group in grouped)
            {
                foreach (var talk in group)
                {
                    if (talk == null) continue; // Defensive: skip null talks
                    var key = $"{talk.Day}|{talk.StartTime}";
                    bool isSelected = selectedTalks.ContainsKey(key) && selectedTalks[key].Title == talk.Title;
                    displayList.Add(new TalkDisplayItem
                    {
                        Talk = talk ?? new Talk(), // Defensive: never null
                        IsSelected = isSelected
                    });
                }
            }
            TalksCollectionView.ItemsSource = displayList;
        }

        private async void OnSelectTalk(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is TalkDisplayItem item)
            {
                var talk = item.Talk;
                var key = $"{talk.Day}|{talk.StartTime}";
                
                // Update both local storage and core service
                selectedTalks[key] = talk;
                _conferencePlanService.SelectTalk(talk);
                
                // Save selected talks persistently
                Preferences.Default.Set(SELECTED_TALKS_PREFERENCE_KEY, string.Join("|", selectedTalks.Values.Select(t => $"{t.Day},{t.StartTime},{t.EndTime},{t.Room},{t.Title},{t.Speaker},{t.Category}")));
                
                // Schedule notifications for all selected talks
                await ScheduleNotificationsAsync();
                
                RefreshTalksView();
                await DisplayAlert("Selected", $"Selected: {talk.Title}\nNotifications scheduled for reminders.", "OK");
            }
        }

        private async void OnDeselectTalk(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is TalkDisplayItem item)
            {
                var talk = item.Talk;
                var key = $"{talk.Day}|{talk.StartTime}";
                if (selectedTalks.ContainsKey(key))
                {
                    // Update both local storage and core service
                    selectedTalks.Remove(key);
                    _conferencePlanService.DeselectTalk(talk);
                    
                    Preferences.Default.Set(SELECTED_TALKS_PREFERENCE_KEY, string.Join("|", selectedTalks.Values.Select(t => $"{t.Day},{t.StartTime},{t.EndTime},{t.Room},{t.Title},{t.Speaker},{t.Category}")));
                    
                    // Reschedule notifications (this will cancel old ones and create new ones)
                    await ScheduleNotificationsAsync();
                    
                    ShowSelectedTalksOnly();
                }
            }
        }

        private async void OnSwipeSelectTalk(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem && swipeItem.CommandParameter is TalkDisplayItem item)
            {
                var talk = item.Talk;
                var key = $"{talk.Day}|{talk.StartTime}";
                
                // Update both local storage and core service
                selectedTalks[key] = talk;
                _conferencePlanService.SelectTalk(talk);
                
                // Save selected talks persistently
                Preferences.Default.Set(SELECTED_TALKS_PREFERENCE_KEY, string.Join("|", selectedTalks.Values.Select(t => $"{t.Day},{t.StartTime},{t.EndTime},{t.Room},{t.Title},{t.Speaker},{t.Category}")));
                
                // Schedule notifications for all selected talks
                await ScheduleNotificationsAsync();
                
                RefreshTalksView();
                await DisplayAlert("Selected", $"Selected: {talk.Title}\nNotifications scheduled for reminders.", "OK");
            }
        }

        private async void OnSwipeDeselectTalk(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem && swipeItem.CommandParameter is TalkDisplayItem item)
            {
                var talk = item.Talk;
                var key = $"{talk.Day}|{talk.StartTime}";
                if (selectedTalks.ContainsKey(key))
                {
                    // Update both local storage and core service
                    selectedTalks.Remove(key);
                    _conferencePlanService.DeselectTalk(talk);
                    
                    Preferences.Default.Set(SELECTED_TALKS_PREFERENCE_KEY, string.Join("|", selectedTalks.Values.Select(t => $"{t.Day},{t.StartTime},{t.EndTime},{t.Room},{t.Title},{t.Speaker},{t.Category}")));
                    
                    // Reschedule notifications
                    await ScheduleNotificationsAsync();
                    
                    RefreshTalksView();
                    await DisplayAlert("Deselected", $"Deselected: {talk.Title}", "OK");
                }
            }
        }

        private async void OnSwipeRateTalk(object sender, EventArgs e)
        {
            if (sender is SwipeItem swipeItem && swipeItem.CommandParameter is TalkDisplayItem item)
            {
                var ratingPage = new TalkRatingPage(item.Talk, _conferencePlanService);
                await Navigation.PushAsync(ratingPage);
            }
        }

        private async Task ScheduleNotificationsAsync()
        {
            if (_notificationService != null)
            {
                try
                {
                    await _notificationService.ScheduleNotificationsForSelectedTalksAsync();
                }
                catch (Exception)
                {
                    // Log error but don't interrupt user experience
                    await DisplayAlert("Notification Error", 
                        "Failed to schedule notifications. Your talks are still selected.", 
                        "OK");
                }
            }
        }

        private async void OnRefresh(object sender, EventArgs e)
        {
            // Reload talks from CSV
            LoadTalks();
            // Reload selected talks from preferences
            await LoadSelectedTalks();
            // Refresh the current view
            if (showAll)
                RefreshTalksView();
            else
                ShowSelectedTalksOnly();
            
            // Stop the refresh indicator
            RefreshViewControl.IsRefreshing = false;
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            searchText = e.NewTextValue ?? string.Empty;
            if (showAll)
                RefreshTalksView();
            else
                ShowSelectedTalksOnly();
        }

        private List<Talk> FilterTalks(List<Talk> talks)
        {
            return TalkFilterService.FilterTalks(talks, searchText);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadSelectedTalks();
            ShowSelectedTalksOnly();
        }

        private void ShowSelectedTalksOnly()
        {
            var selectedTalksList = selectedTalks.Values.ToList();
            var filteredSelected = FilterTalks(selectedTalksList);
            
            var displayList = filteredSelected
                .OrderBy(t => t.Day)
                .ThenBy(t => t.StartTime)
                .Select(t => new TalkDisplayItem { Talk = t, IsSelected = true })
                .ToList();
            TalksCollectionView.ItemsSource = displayList;
        }

        private void SortBySpeaker()
        {
            if (showAll)
            {
                var filteredTalks = FilterTalks(allTalks);
                var sorted = filteredTalks
                    .OrderBy(t => t.Speaker)
                    .Select(t => new TalkDisplayItem {
                        Talk = t,
                        IsSelected = selectedTalks.ContainsKey($"{t.Day}|{t.StartTime}") && selectedTalks[$"{t.Day}|{t.StartTime}"].Title == t.Title
                    })
                    .ToList();
                TalksCollectionView.ItemsSource = sorted;
            }
            else
            {
                var selectedTalksList = selectedTalks.Values.ToList();
                var filteredSelected = FilterTalks(selectedTalksList);
                var sorted = filteredSelected
                    .OrderBy(t => t.Speaker)
                    .Select(t => new TalkDisplayItem { Talk = t, IsSelected = true })
                    .ToList();
                TalksCollectionView.ItemsSource = sorted;
            }
        }

        private void SortByCategory()
        {
            if (showAll)
            {
                var filteredTalks = FilterTalks(allTalks);
                var sorted = filteredTalks
                    .OrderBy(t => t.Category)
                    .Select(t => new TalkDisplayItem {
                        Talk = t,
                        IsSelected = selectedTalks.ContainsKey($"{t.Day}|{t.StartTime}") && selectedTalks[$"{t.Day}|{t.StartTime}"].Title == t.Title
                    })
                    .ToList();
                TalksCollectionView.ItemsSource = sorted;
            }
            else
            {
                var selectedTalksList = selectedTalks.Values.ToList();
                var filteredSelected = FilterTalks(selectedTalksList);
                var sorted = filteredSelected
                    .OrderBy(t => t.Category)
                    .Select(t => new TalkDisplayItem { Talk = t, IsSelected = true })
                    .ToList();
                TalksCollectionView.ItemsSource = sorted;
            }
        }

        private void SortByRating()
        {
            if (showAll)
            {
                var filteredTalks = FilterTalks(allTalks);
                var sorted = filteredTalks
                    .OrderByDescending(t => t.AverageRating)
                    .ThenByDescending(t => t.RatingCount)
                    .Select(t => new TalkDisplayItem {
                        Talk = t,
                        IsSelected = selectedTalks.ContainsKey($"{t.Day}|{t.StartTime}") && selectedTalks[$"{t.Day}|{t.StartTime}"].Title == t.Title
                    })
                    .ToList();
                TalksCollectionView.ItemsSource = sorted;
            }
            else
            {
                var selectedTalksList = selectedTalks.Values.ToList();
                var filteredSelected = FilterTalks(selectedTalksList);
                var sorted = filteredSelected
                    .OrderByDescending(t => t.AverageRating)
                    .ThenByDescending(t => t.RatingCount)
                    .Select(t => new TalkDisplayItem { Talk = t, IsSelected = true })
                    .ToList();
                TalksCollectionView.ItemsSource = sorted;
            }
        }

        private async void OnNotificationsClicked(object sender, EventArgs e)
        {
            if (_notificationService == null)
            {
                await DisplayAlert("Notifications", "Notification service is not available.", "OK");
                return;
            }

            try
            {
                var upcomingNotifications = await _notificationService.GetUpcomingNotificationsAsync();
                var notificationList = upcomingNotifications.ToList();

                if (!notificationList.Any())
                {
                    await DisplayAlert("Notifications", "No upcoming notifications scheduled.", "OK");
                    return;
                }

                var message = "Upcoming notifications:\n\n";
                foreach (var notification in notificationList.Take(5)) // Show first 5
                {
                    message += $"🔔 {notification.Title}\n";
                    message += $"   {notification.Message}\n";
                    message += $"   ⏰ {notification.ScheduledDateTime:MMM dd, HH:mm}\n\n";
                }

                if (notificationList.Count > 5)
                {
                    message += $"... and {notificationList.Count - 5} more notifications";
                }

                await DisplayAlert("Upcoming Notifications", message, "OK");
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Failed to load notifications.", "OK");
            }
        }

        private void OnSortPickerChanged(object sender, EventArgs e)
        {
            if (SortPicker.SelectedIndex == 1) // Presenter
            {
                SortBySpeaker();
            }
            else if (SortPicker.SelectedIndex == 2) // Category
            {
                SortByCategory();
            }
            else if (SortPicker.SelectedIndex == 3) // Rating
            {
                SortByRating();
            }
            else // Standard
            {
                if (showAll)
                    RefreshTalksView();
                else
                    ShowSelectedTalksOnly();
            }
        }
    }

    public class TalkDisplayItem
    {
        public Talk Talk { get; set; } = new Talk();
        public bool IsSelected { get; set; }
        
        // Computed properties for display
        public string RatingDisplay => Talk.RatingCount > 0 
            ? $"★ {Talk.AverageRating:F1} ({Talk.RatingCount})" 
            : "No ratings";
        
        public bool HasRating => Talk.RatingCount > 0;
    }
}
