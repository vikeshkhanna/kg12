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
        private string workout;

        public MainViewModel()
        {
            this.workoutDB = App.WorkoutDB;
            this.Items = new ObservableCollection<WorkingExercise>();
            this.AllDBExercises = new List<DBExercise>();

            // Load all exercises
            List<Exercise> allExercises = (from Exercise e in App.WorkoutDB.Exercises
                              select e).ToList();

            foreach (Exercise exercise in allExercises)
            {
                DBExercise ex = new DBExercise()
                {
                    ExerciseName = exercise.Name,
                    WorkoutImage = "Media/" + Utils.GetImage(exercise, 0)
                };

                this.AllDBExercises.Add(ex);
            }
            
            this.AllDBExercises = this.AllDBExercises.OrderBy(e => e.ExerciseName).ToList();
            this.Exercises = new ObservableCollection<DBExercise>(this.AllDBExercises);
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<WorkingExercise> Items { get; private set; }
        public ObservableCollection<DBExercise> Exercises { get; private set; }
        public List<DBExercise> AllDBExercises;
       
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

        public string Workout
        {
            get
            {
                return this.workout;
            }
            set
            {
                if (value != this.workout)
                {
                    this.workout = value;
                    NotifyPropertyChanged("Workout");
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
            this.Items.Clear();
            TimeSpan difference = DateTime.Now - App.ProgramStartDate;
            int dayNum = difference.Days + 1;

            Day day = (from Day d in this.workoutDB.Days
                              where d.Num == dayNum
                              select d).Single();

            List<DayExercise> dayExercises = (from DayExercise de in this.workoutDB.DayExercises
                                              where de.DayId == day.DayId
                                              select de).ToList();

            this.Workout = day.Workout;

            foreach(DayExercise dayExercise in dayExercises)
            {
                Exercise exercise = this.workoutDB.Exercises.Single(ex => ex.ExerciseId == dayExercise.ExerciseId);
                
                this.Items.Add(new WorkingExercise() {
                    WorkoutImage1 = "Media/" + Utils.GetImage(exercise, 0),
                    WorkoutImage2 = "Media/" + Utils.GetImage(exercise, 1), 
                    ExerciseName = exercise.Name, ExerciseDescription = dayExercise.Description});
                // Sample data; replace with real data
            }
            
            this.IsDataLoaded = true;
        }
        public bool IsFollowingProgram
        {
            get
            {
                return App.IsProgramOn;
            }
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