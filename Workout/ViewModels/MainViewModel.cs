﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using WorkoutHelper;
using System.Linq;

namespace Workout
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private WorkoutContext workoutDB;

        public MainViewModel()
        {
            this.workoutDB = new WorkoutContext("Data Source = 'appdata:/workout.sdf'; File Mode = read only;");
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            List<Day> dayEntries = (from Day day in this.workoutDB.Days
                              where day.Num == 1
                              select day).ToList();

            foreach (Day dayEntry in dayEntries)
            {
                Exercise exercise = this.workoutDB.Exercises.Single(ex => ex.ExerciseId == dayEntry.ExerciseId);
                this.Items.Add(new ItemViewModel() { LineOne = exercise.Name, LineTwo = dayEntry.Description, LineThree = "Lorem Ipsum" });
                // Sample data; replace with real data
            }
     
            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}