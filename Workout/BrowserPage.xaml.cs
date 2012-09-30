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
    public partial class BrowserPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        private string heading;

        public BrowserPage()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string Heading
        {
            get
            {
                return this.heading;
            }
            set
            {
                if (this.heading != value)
                {
                    this.heading = value;
                    NotifyPropertyChanged("Heading");
                }
            }
        }

        #region InotifyPropertyChanged
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

    }
}