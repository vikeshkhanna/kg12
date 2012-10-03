using System;
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
        private string workout;
        private ImageBrush backgroundImageBrush;
        private ObservableCollection<WorkingExercise> items;
        private ObservableCollection<DBExercise> exercises;

        public MainViewModel()
        {
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<DBExercise> Exercises 
        { 
            get
            {
                if(this.exercises == null)
                {
                         this.exercises= new ObservableCollection<DBExercise>(App.AllDBExercises); //Shallow copy intended. We don't want this list to change cache data
                }

                return this.exercises;
            }
            private set
            {
                if (this.exercises != value)
                {
                    this.exercises = value;
                    NotifyPropertyChanged("Exercises");
                }
            }
        }

        public ObservableCollection<WorkingExercise> Items
        {
            get
            {
                if (this.items == null)
                {
                    this.items = new ObservableCollection<WorkingExercise>();
                }

                return this.items;
            }
            set
            {
                if (this.items != value)
                {
                    this.items = value;
                    NotifyPropertyChanged("Items");
                }
            }
        }

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

        public ImageBrush BackgroundImage
        {
            get
            {
                if (this.backgroundImageBrush == null)
                {
                    int MAX_IMAGES = 3;
                    Random random = new Random();
                    int imgNum = random.Next(1, MAX_IMAGES + 1);
                    this.backgroundImageBrush = new ImageBrush();
                    this.backgroundImageBrush.ImageSource = new BitmapImage(new Uri("/Images/bg" + imgNum +".jpg", UriKind.Relative));
                }

                return this.backgroundImageBrush;
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
            Day day = Utils.GetCurrentDay();
            List<DayExercise> dayExercises = App.AllDayExercises.Where(de => de.DayId == day.DayId).ToList();

            this.Workout = day.Workout;

            foreach(DayExercise dayExercise in dayExercises)
            {
                Exercise exercise = App.AllExercises.Single(ex => ex.ExerciseId == dayExercise.ExerciseId);
                string fullExerciseName = Utils.GetFullyQualifiedExerciseName(dayExercise.ExerciseSetType, exercise.Name);

                this.Items.Add(new WorkingExercise() {
                    WorkoutImage1 = "Media/" + Utils.GetImage(exercise, 0),
                    WorkoutImage2 = "Media/" + Utils.GetImage(exercise, 1), 
                    ExerciseName = exercise.Name, FullyQualifiedExerciseName=fullExerciseName,
                    ExerciseDescription = dayExercise.Description});
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

        public string CurrentDay
        {
            get
            {
                Day day = Utils.GetCurrentDay();
                return String.Format("Week {0}, Day {1}", Convert.ToString(Utils.GetWeekForDay(day.Num)), Convert.ToString(day.Num)); 
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