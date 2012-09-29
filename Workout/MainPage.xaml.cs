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

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.ViewModel.Exercises.Clear();

            foreach (DBExercise exercise in App.ViewModel.AllDBExercises)
            {
                if (exercise.ExerciseName.ToLower().StartsWith(this.SearchTextBox.Text.ToLower()))
                {
                    App.ViewModel.Exercises.Add(exercise);
                }
            }
        }
    }
}