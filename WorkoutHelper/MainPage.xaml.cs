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


namespace WorkoutHelper
{
    public partial class MainPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            XDocument xdoc = XDocument.Load("days.xml");

            foreach (XElement element in xdoc.Descendants())
            { 
                
            }

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
        private int exerciseId;
        private int num;
        private string description;
        
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
        public Table<Exercise> Exercise;
    }
    
}