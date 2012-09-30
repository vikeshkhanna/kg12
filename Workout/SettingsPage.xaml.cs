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
        private bool isFollowingProgram;

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
            this.IsFollowingProgram = App.IsProgramOn;

            List<Day> daysInDB = (from Day d in App.WorkoutDB.Days
                                    select d).ToList();
            
            this.days = new ObservableCollection<ListPickerContent>();
           
            foreach (Day d in daysInDB)
            {
                int week = Utils.GetWeekForDay(d.Num);
                string workout = String.Format("W {0}, D {1} - {2}", Convert.ToString(week), Convert.ToString(d.Num), d.Workout);
                this.Days.Add(new ListPickerContent() { Workout = workout});
            }

            NotifyPropertyChanged("Days");
            
            // Must always be after notify changed
            if (App.IsProgramOn)
            {
                TimeSpan difference = DateTime.Now - App.ProgramStartDate;
                this.ListPickerControl.SelectedIndex = difference.Days;
            }
        }

        public bool IsFollowingProgram
        {
            get
            {
                return isFollowingProgram;
            }
            set
            {
                if (isFollowingProgram != value)
                {
                    isFollowingProgram = value;
                    this.toggleSwitchData.IsFollowingProgram = value;
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
            App.ProgramStartDate = DateTime.Now.Subtract(new TimeSpan(this.ListPickerControl.SelectedIndex,0,0,0));
            App.IsProgramOn = this.IsFollowingProgram;

            if (!App.UserSettings.Contains("IsProgramOn"))
            {
                App.UserSettings.Add("IsProgramOn", this.IsFollowingProgram);
            }
            else
            {
                App.UserSettings["IsProgramOn"] = this.IsFollowingProgram;
            }

            if (!App.UserSettings.Contains("ProgramStartDate"))
            {
                
                App.UserSettings.Add("ProgramStartDate", App.ProgramStartDate);
            }
            else
            {
                App.UserSettings["ProgramStartDate"] = App.ProgramStartDate;
            }

            App.UserSettings.Save();
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
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
        private bool isFollowingProgram;

        public bool IsFollowingProgram
        {
            get
            {
                return this.isFollowingProgram;
            }
            set
            {
                if (this.isFollowingProgram != value)
                {
                    this.isFollowingProgram = value;
                    NotifyPropertyChanged("IsFollowingProgram");
                }
            }
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