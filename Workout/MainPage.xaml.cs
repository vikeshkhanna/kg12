using System;
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
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
            
            Day day = Utils.GetCurrentDay();
            string url = String.Format("<html><head><style>body{{background:black; width:100%; height:100%}}</style></head><body><div style='width:100%;height:100%' class=\"BBCOMVideoEmbed\" data-dimensions=\"1024x768\" data-video-key=\"{0}\" data-autoplay=\"false\" data-thumbnail-url=\"{1}\"><script type=\"text/javascript\" src=\"http://assets.bodybuilding.com/videos/javascript/min/external-video-embed.js\"></script></div></body></html>",
                day.VideoKey, day.ThumbnailUrl);

            this.webBrowserControl.NavigateToString(url);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            App.ViewModel.LoadData();
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void webBrowserControl_Navigating(object sender, NavigatingEventArgs e)
        {
            MediaPlayerLauncher mediaPlayerLauncher = new MediaPlayerLauncher();

            mediaPlayerLauncher.Media = e.Uri;
            mediaPlayerLauncher.Location = MediaLocationType.Data;
            mediaPlayerLauncher.Controls = MediaPlaybackControls.Pause | MediaPlaybackControls.Stop;
            mediaPlayerLauncher.Orientation = MediaPlayerOrientation.Landscape;

            mediaPlayerLauncher.Show();
        }

        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Focus();
                return;    
            }

            App.ViewModel.Exercises.Clear();

            foreach (DBExercise exercise in App.ViewModel.AllDBExercises)
            {
                if (exercise.ExerciseName.ToLower().StartsWith(this.SearchTextBox.Text.ToLower()))
                {
                    App.ViewModel.Exercises.Add(exercise);
                }
            }
        }

        private void ExercisesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}