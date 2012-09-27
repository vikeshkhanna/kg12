﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using WorkoutHelper;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media.Imaging;

namespace Workout
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private string exerciseName;
        private string exerciseDescription;
        private string workoutImage1;
        private string workoutImage2;

        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string ExerciseName
        {
            get
            {
                return this.exerciseName;
            }
            set
            {
                if (value != this.exerciseName)
                {
                    this.exerciseName = value;
                    NotifyPropertyChanged("ExerciseName");
                }
            }
        }

        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string ExerciseDescription
        {
            get
            {
                return this.exerciseDescription;
            }
            set
            {
                if (value != this.exerciseDescription)
                {
                    this.exerciseDescription = value;
                    NotifyPropertyChanged("ExerciseDescription");
                }
            }
        }

        private string _lineThree;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineThree
        {
            get
            {
                return _lineThree;
            }
            set
            {
                if (value != _lineThree)
                {
                    _lineThree = value;
                    NotifyPropertyChanged("LineThree");
                }
            }
        }

        public string WorkoutImage1
        {
            get
            {
                return this.workoutImage1;
            }
            set
            {
                if (value != this.workoutImage1)
                {
                    this.workoutImage1 = value;
                    NotifyPropertyChanged("WorkoutImage1");
                }
            }
        }

        public string WorkoutImage2
        {
            get
            {
                return this.workoutImage2;
            }
            set
            {
                if (value != this.workoutImage2)
                {
                    this.workoutImage2 = value;
                    NotifyPropertyChanged("WorkoutImage2");
                }
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