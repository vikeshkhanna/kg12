﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using WorkoutHelper;
using Microsoft.Phone.Tasks;

namespace Workout
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool isVideoFresh = false;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.ViewModel.LoadData();
            isVideoFresh = false;
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void webBrowserControl_Navigating(object sender, NavigatingEventArgs e)
        {
            if (Utils.IsMediaURL(e.Uri.AbsoluteUri))
            {
                Utils.LaunchMediaPlayer(e.Uri);
            }
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Focus();
                return;    
            }

            App.ViewModel.Exercises.Clear();

            foreach (DBExercise exercise in App.AllDBExercises)
            {
                if (exercise.ExerciseName.ToLower().StartsWith(this.SearchTextBox.Text.ToLower()))
                {
                    App.ViewModel.Exercises.Add(exercise);
                }
            }
        }

        private void ExercisesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems.Count > 0)
                {
                    DBExercise selectedExercise = (e.AddedItems[0] as DBExercise);
                    string heading = selectedExercise.ExerciseName;
                    string url = Utils.GetExerciseURL(selectedExercise.ExerciseName);
                    NavigationService.Navigate(new Uri("/BrowserPage.xaml?heading=" + heading + "&url=" + url, UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("<kg12> Exception " + ex.Message);
            }
        }

        private void todayExerciseListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems.Count > 0)
                {
                    WorkingExercise selectedExercise = (e.AddedItems[0] as WorkingExercise);
                    string heading = selectedExercise.ExerciseName;
                    string url = Utils.GetExerciseURL(selectedExercise.ExerciseName);
                    NavigationService.Navigate(new Uri("/BrowserPage.xaml?heading=" + heading + "&url=" + url, UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("<kg12> Exception " + ex.Message);
            }
        }

        private void aboutAppButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
        }

        private void openTodayAppButton_Click(object sender, EventArgs e)
        {
            string heading = "Today";
            string url = Utils.GetTodayLink();
            NavigationService.Navigate(new Uri("/BrowserPage.xaml?heading=" + heading + "&url=" + url, UriKind.Relative));
        }

        private void panoramaControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && !this.isVideoFresh)
            {
                if ((e.AddedItems[0] as PanoramaItem).Header.ToString() == "video")
                {
                   //Placeholder for plausible performance improvement in video loading
                    Day day = Utils.GetCurrentDay();
                    string url = String.Format("<html><head><style>*{{width:1130; height:722px;}}body{{background:black; overflow:hidden;}}</style></head><body><div style='width:100%;height:100%' class=\"BBCOMVideoEmbed\" data-dimensions=\"570*361\" data-video-key=\"{0}\" data-autoplay=\"false\" data-thumbnail-url=\"{1}\"><script type=\"text/javascript\" src=\"http://assets.bodybuilding.com/videos/javascript/min/external-video-embed.js\"></script></div></body></html>",
                        day.VideoKey, day.ThumbnailUrl);

                    this.isVideoFresh = true;
                    this.webBrowserControl.NavigateToString(url);
                }
            }
        }
    }
}