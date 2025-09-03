using Microsoft.Maui.Controls;
using NdcApp.Core.Models;
using NdcApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NdcApp
{
    public partial class TalkRatingPage : ContentPage
    {
        private Talk _currentTalk;
        private int _selectedRating = 0;
        private readonly ConferencePlanService _conferencePlanService;
        private readonly List<Button> _starButtons;

        public TalkRatingPage(Talk talk, ConferencePlanService conferencePlanService)
        {
            InitializeComponent();
            _currentTalk = talk;
            _conferencePlanService = conferencePlanService;
            
            _starButtons = new List<Button> { Star1, Star2, Star3, Star4, Star5 };
            
            LoadTalkInformation();
            LoadExistingComments();
        }

        private void LoadTalkInformation()
        {
            TalkTitleLabel.Text = _currentTalk.Title;
            TalkSpeakerLabel.Text = $"by {_currentTalk.Speaker}";
            
            if (_currentTalk.RatingCount > 0)
            {
                CurrentRatingLabel.Text = $"Current Rating: ★ {_currentTalk.AverageRating:F1} ({_currentTalk.RatingCount} ratings)";
            }
            else
            {
                CurrentRatingLabel.Text = "Current Rating: No ratings yet";
            }
        }

        private void LoadExistingComments()
        {
            var ratings = _conferencePlanService.GetRatingsForTalk(_currentTalk);
            CommentsCollectionView.ItemsSource = ratings.Where(r => !string.IsNullOrEmpty(r.Comment)).ToList();
        }

        private void OnStarClicked(object sender, EventArgs e)
        {
            if (sender is Button clickedStar)
            {
                var rating = _starButtons.IndexOf(clickedStar) + 1;
                SetRating(rating);
            }
        }

        private void SetRating(int rating)
        {
            _selectedRating = rating;
            
            // Update star colors
            for (int i = 0; i < _starButtons.Count; i++)
            {
                if (i < rating)
                {
                    _starButtons[i].TextColor = Color.FromArgb("#FFB400"); // NdcAccent
                }
                else
                {
                    _starButtons[i].TextColor = Colors.Gray;
                }
            }
            
            // Enable submit button
            SubmitRatingButton.IsEnabled = true;
        }

        private async void OnSubmitRatingClicked(object sender, EventArgs e)
        {
            try
            {
                if (_selectedRating == 0)
                {
                    await DisplayAlert("Error", "Please select a rating first.", "OK");
                    return;
                }

                var comment = CommentEditor.Text?.Trim() ?? string.Empty;
                
                // Submit the rating
                _conferencePlanService.RateTalk(_currentTalk, _selectedRating, comment);
                
                // Update the talk's rating display
                _conferencePlanService.UpdateAllTalkRatings(new List<Talk> { _currentTalk });
                
                // Show success message
                await DisplayAlert("Success", "Your rating has been submitted!", "OK");
                
                // Refresh the page
                LoadTalkInformation();
                LoadExistingComments();
                
                // Reset the form
                ResetForm();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to submit rating: {ex.Message}", "OK");
            }
        }

        private void ResetForm()
        {
            _selectedRating = 0;
            CommentEditor.Text = string.Empty;
            SubmitRatingButton.IsEnabled = false;
            
            // Reset star colors
            foreach (var star in _starButtons)
            {
                star.TextColor = Colors.Gray;
            }
        }
    }
}