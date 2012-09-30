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

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
             base.OnNavigatedTo(e);
             string h, url;

             if (NavigationContext.QueryString.TryGetValue("heading", out h))
             {
                 this.Heading = h;
             }

             if (NavigationContext.QueryString.TryGetValue("url", out url))
             {
                 this.webBrowserControl.Navigate(new Uri(url));
             }
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

        private void webBrowserControl_Navigating(object sender, NavigatingEventArgs e)
        {
            if (Utils.IsMediaURL(e.Uri.AbsoluteUri))
            {
                Utils.LaunchMediaPlayer(e.Uri);
            }
        }
    }
}