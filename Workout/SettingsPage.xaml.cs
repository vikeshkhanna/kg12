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
using System.Collections.ObjectModel;
using WorkoutHelper;

namespace Workout
{
    public partial class SettingsPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        private ToggleSwitchContent toggleSwitchData;
        private ObservableCollection<ListPickerContent> days;

        public ObservableCollection<ListPickerContent> Days
        {
            get
            {
                return this.days;
            }
        }

        public ToggleSwitchContent ToggleSwitchData
        {
            get
            {
                if (this.toggleSwitchData == null)
                {
                    this.toggleSwitchData = new ToggleSwitchContent();
                }

                return this.toggleSwitchData;
            }
        }

        public SettingsPage()
        {
            InitializeComponent();
            this.DataContext = this;

            List<Day> daysInDB = (from Day d in App.WorkoutDB.Days
                                    select d).ToList();
            
            this.days = new ObservableCollection<ListPickerContent>();

            foreach (Day d in daysInDB)
            {
                int week = (int)Math.Ceiling(d.Num / 7.0);
                string workout = String.Format("W {0}, D {1} - {2}", Convert.ToString(week), Convert.ToString(d.Num), d.Workout);
                this.Days.Add(new ListPickerContent() { Workout = workout});
            }

            NotifyPropertyChanged("Days");
        }

        public bool IsFollowingProgram
        {
            get
            {
                return App.IsProgramOn;
            }
            set
            {
                if (App.IsProgramOn != value)
                {
                    App.IsProgramOn = value;
                    this.ToggleSwitchData.Refresh();  // just a place holder to fire NotifyPropertyChanged
                    NotifyPropertyChanged("IsFollowingProgram");
                 }
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        private void saveButton_Click(object sender, EventArgs e)
        {

        }

        private void cancelButton_Click(object sender, EventArgs e)
        { 
        
        }
    }
    
    public class ListPickerContent
    {
        private string workout;

        public string Workout
        {
            get
            {
                return this.workout;
            }
            set
            {
                this.workout = value;
            }
        }
    }

    public class ToggleSwitchContent:INotifyPropertyChanged
    {
        public bool IsFollowingProgram
        {
            get
            {
                return App.IsProgramOn;
            }
        }

        internal void Refresh()
        {
            NotifyPropertyChanged("IsFollowingProgram");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        internal void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}