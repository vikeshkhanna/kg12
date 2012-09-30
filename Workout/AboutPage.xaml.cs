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
using Microsoft.Phone.Tasks;

namespace Workout
{
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        private void GestureListener_Tap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            WebBrowserTask wbt = new WebBrowserTask();

            if(e.OriginalSource == this.twitterButton)
            {
                wbt.Uri = new Uri("http://twitter.com/vikeshkhanna");
            }
            else if (e.OriginalSource == this.facebookButton)
            {
                wbt.Uri = new Uri("http://facebook.com.vikeshkhanna");
            }
            else if(e.OriginalSource == this.quoraButton)
            {
                wbt.Uri = new Uri("http://quora.com/Vikesh-Khanna");
            }
            else if (e.OriginalSource == this.bbButton)
            {
                wbt.Uri = new Uri("http://bodybuilding.com");
            }

            wbt.Show();
        }
    }
}