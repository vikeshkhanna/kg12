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

namespace Workout
{
    public partial class SettingsPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        private ToggleSwitchContent toggleSwitchData;

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