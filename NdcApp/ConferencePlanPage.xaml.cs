using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using NdcApp.Models;
using NdcApp.Services;
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

        public ConferencePlanPage()
        {
            InitializeComponent();
            LoadTalks();
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
            TalksCollectionView.ItemsSource = allTalks;
        }

        private void LoadSelectedTalks()
        {
            var selectedTalksRaw = Preferences.Default.Get(SELECTED_TALKS_PREFERENCE_KEY, "");
            selectedTalks.Clear();
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
                }
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
            // Group talks by day and time slot
            var grouped = allTalks
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

        private void OnSelectTalk(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is TalkDisplayItem item)
            {
                var talk = item.Talk;
                var key = $"{talk.Day}|{talk.StartTime}";
                selectedTalks[key] = talk;
                // Save selected talks persistently
                Preferences.Default.Set(SELECTED_TALKS_PREFERENCE_KEY, string.Join("|", selectedTalks.Values.Select(t => $"{t.Day},{t.StartTime},{t.EndTime},{t.Room},{t.Title},{t.Speaker},{t.Category}")));
                RefreshTalksView();
                DisplayAlert("Selected", $"Selected: {talk.Title}", "OK");
            }
        }

        private void OnDeselectTalk(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is TalkDisplayItem item)
            {
                var talk = item.Talk;
                var key = $"{talk.Day}|{talk.StartTime}";
                if (selectedTalks.ContainsKey(key))
                {
                    selectedTalks.Remove(key);
                    Preferences.Default.Set(SELECTED_TALKS_PREFERENCE_KEY, string.Join("|", selectedTalks.Values.Select(t => $"{t.Day},{t.StartTime},{t.EndTime},{t.Room},{t.Title},{t.Speaker},{t.Category}")));
                    ShowSelectedTalksOnly();
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadSelectedTalks();
            ShowSelectedTalksOnly();
        }

        private void ShowSelectedTalksOnly()
        {
            var displayList = selectedTalks.Values
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
                var sorted = allTalks
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
                var sorted = selectedTalks.Values
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
                var sorted = allTalks
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
                var sorted = selectedTalks.Values
                    .OrderBy(t => t.Category)
                    .Select(t => new TalkDisplayItem { Talk = t, IsSelected = true })
                    .ToList();
                TalksCollectionView.ItemsSource = sorted;
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
    }
}
