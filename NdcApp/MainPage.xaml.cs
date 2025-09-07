using Microsoft.Maui.Storage;
using NdcApp.Models;
using NdcApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NdcApp;

public partial class MainPage : ContentPage
{
    private const string SELECTED_TALKS_PREFERENCE_KEY = "SelectedTalks";

    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnConferencePlanClicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync("///ConferencePlanPage");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Navigation Error: {ex}");
            System.Diagnostics.Trace.WriteLine($"Navigation Error: {ex}");
            Console.WriteLine($"Navigation Error: {ex}");
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ShowNextSelectedTalk();
    }

    private void ShowNextSelectedTalk()
    {
        // Load selected talks from Preferences
        var selectedTalksRaw = Preferences.Default.Get(SELECTED_TALKS_PREFERENCE_KEY, "");
        if (string.IsNullOrEmpty(selectedTalksRaw))
        {
            NextTalkLabel.Text = "No talk selected.";
            return;
        }
        var selectedTalks = selectedTalksRaw.Split('|')
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
            })
            .ToList();
        // Find next talk by time
        var now = DateTime.Now.TimeOfDay;
        var nextTalk = selectedTalks
            .OrderBy(t => t.Day)
            .ThenBy(t => t.StartTime)
            .FirstOrDefault(t => t.StartTime > now);
        if (nextTalk != null)
        {
            NextTalkLabel.Text = $"Next Talk: {nextTalk.Title} ({nextTalk.StartTime:hh\\:mm})";
        }
        else
        {
            NextTalkLabel.Text = "No upcoming selected talk.";
        }
    }
}
