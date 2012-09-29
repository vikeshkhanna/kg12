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
using System.ComponentModel;

using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Linq;
using System.Text.RegularExpressions;


namespace WorkoutHelper
{
    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        private WorkoutContext workoutDB;
        
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            XDocument xdoc = XDocument.Load("days.xml");
            XElement root = xdoc.Root;

            //Database
            this.workoutDB = new WorkoutContext(WorkoutContext.DBConnectionString);

            // Gather all exercises only
            // For each day
            foreach (XElement element in root.Nodes())
            { 
                int num = Convert.ToInt32((element.Descendants("num").Single() as XElement).Value);

                Day day = new Day();
                day.Workout = (element.Descendants("workout").Single() as XElement).Value.ToString();
                day.Num = num;
                day.VideoKey = ((element.Descendants("video").Single() as XElement).Descendants("video_key").Single() as XElement).Value;
                day.ThumbnailUrl = ((element.Descendants("video").Single() as XElement).Descendants("thumbnail_url").Single() as XElement).Value;

                this.workoutDB.Days.InsertOnSubmit(day);
                this.workoutDB.SubmitChanges();
         
                if (element.Elements().Count() == 4)
                {
                    XElement exerciseRoot = element.Descendants("exercises").Single() as XElement;
                    
                    // For each exercise 
                    foreach (XElement exec in exerciseRoot.Nodes())
                    {
                        Exercise exercise = new Exercise();
                        exercise.Name = (exec.FirstNode as XElement).Value.ToString();
                        
                        try
                        {
                            this.workoutDB.Exercises.First(ex => ex.Name == exercise.Name);
                        }
                        catch (Exception ex)
                        {
                            this.workoutDB.Exercises.InsertOnSubmit(exercise);
                            Console.WriteLine(ex);
                        }
                        
                        this.workoutDB.SubmitChanges();
                    }
                }
            }

           
            List<Exercise> exercisesInDB = (from Exercise exercise in workoutDB.Exercises
                                            select exercise).ToList();

            List<Day> daysInDB = (from Day day in workoutDB.Days 
                                  select day).ToList();
            
            // Days
            foreach (XElement element in root.Nodes())
            {
                int num = Convert.ToInt32((element.Descendants("num").Single() as XElement).Value);

                // Exercises are also present
                if (element.Elements().Count() == 4)
                {
                    XElement exerciseRoot = element.Descendants("exercises").Single() as XElement;
                    
                    foreach (XElement exec in exerciseRoot.Nodes())
                    {
                        DayExercise dayExercise = new DayExercise();
                        
                        string exerciseName = (exec.FirstNode as XElement).Value.ToString();
                        Exercise exercise =  exercisesInDB.Single(ex => ex.Name == exerciseName);
                        Day day = daysInDB.Single(d => d.Num == num);
 
                        dayExercise.ExerciseId = exercise.ExerciseId;
                        dayExercise.DayId = day.DayId;
                        dayExercise.Description = (exec.LastNode as XElement).Value.ToString().Trim(new char[] { '\n', ' ' }); ;
                        Regex rgx = new Regex("\n\\s*");
                        dayExercise.Description = rgx.Replace(dayExercise.Description, "\n");
                        this.workoutDB.DayExercises.InsertOnSubmit(dayExercise);
                    }
                }
            }

            this.workoutDB.SubmitChanges();
            // Call the base method.
            base.OnNavigatedTo(e);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }

    [Table]
    public class Exercise : INotifyPropertyChanged, INotifyPropertyChanging
    { 
        private string name;
        private int exerciseId;


        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int ExerciseId
        {
            get
            {
                return this.exerciseId;
            }
            set
            {
                if (this.exerciseId != value)
                {
                    NotifyPropertyChanging("ExerciseId");
                    this.exerciseId = value;
                    NotifyPropertyChanged("ExerciseId");
                }
            }
        }
        
        [Column]
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (this.name != value)
                {
                    NotifyPropertyChanging("Name");
                    this.name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

    }

    [Table]
    public class Day : INotifyPropertyChanged, INotifyPropertyChanging
    {
        private int dayId;
        private int num;
        private string workout;
        private string videoKey;
        private string thumbnailUrl;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int DayId
        {
            get
            {
                return this.dayId;
            }
            set
            {
                if (this.dayId != value)
                {
                    NotifyPropertyChanging("DayId");
                    this.dayId = value;
                    NotifyPropertyChanged("DayId");
                }
            }
        }

        [Column]
        public int Num
        {
            get
            {
                return this.num;
            }
            set
            {
                if (this.num != value)
                {
                    NotifyPropertyChanging("Num");
                    this.num = value;
                    NotifyPropertyChanged("Num");
                }
            }
        }

        [Column]
        public string Workout
        {
            get
            {
                return this.workout;
            }
            set
            {
                if (this.workout != value)
                {
                    NotifyPropertyChanging("Workout");
                    this.workout = value;
                    NotifyPropertyChanged("Workout");
                }
            }
        }

        [Column]
        public string VideoKey
        {
            get
            {
                return this.videoKey;
            }
            set
            {
                if (this.videoKey != value)
                {
                    NotifyPropertyChanging("VideoKey");
                    this.videoKey = value;
                    NotifyPropertyChanged("VideoKey");
                }
            }
        }


        [Column]
        public string ThumbnailUrl
        {
            get
            {
                return this.thumbnailUrl;
            }
            set
            {
                if (this.videoKey != value)
                {
                    NotifyPropertyChanging("ThumbnailUrl");
                    this.thumbnailUrl = value;
                    NotifyPropertyChanged("ThumnbailUrl");
                }
            }
        }



        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

    }

    [Table]
    public class DayExercise
    {
        private int dayExerciseId;
        private string description;
        private int exerciseId;
        private int dayId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int DayExerciseId
        {
            get
            {
                return this.dayExerciseId;
            }
            set
            {
                if (this.dayExerciseId != value)
                {
                    NotifyPropertyChanging("DayExerciseId");
                    this.dayExerciseId = value;
                    NotifyPropertyChanged("DayExerciseId");
                }
            }
        }

        [Column]
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                if (this.description != value)
                {
                    NotifyPropertyChanging("Description");
                    this.description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }

        [Column]
        public int DayId
        {
            get
            {
                return this.dayId;
            }
            set
            {
                if (this.dayId != value)
                {
                    NotifyPropertyChanging("DayId");
                    this.dayId = value;
                    NotifyPropertyChanged("DayId");
                }
            }
        }



        [Column]
        public int ExerciseId
        {
            get
            {
                return this.exerciseId;
            }
            set
            {
                if (this.exerciseId != value)
                {
                    NotifyPropertyChanging("ExerciseId");
                    this.exerciseId = value;
                    NotifyPropertyChanged("ExerciseId");
                }
            }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify the page that a data context property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify the data context that a data context property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion


    
    }

    public class WorkoutContext : DataContext
    {
        // Specify the connection string as a static, used in main page and app.xaml.
        public static string DBConnectionString = "Data Source=isostore:/workout.sdf";

        // Pass the connection string to the base class.
        public WorkoutContext(string connectionString)
            : base(connectionString)
        { }

        // Specify a single table for the to-do items.
        public Table<Exercise> Exercises;
        public Table<Day> Days;
        public Table<DayExercise> DayExercises;
    }
    
}