﻿#pragma checksum "C:\Users\khann_000\Documents\Visual Studio 2010\Projects\KrisGethin12Week\Workout\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "8F2B9388FB143EF50FFF9D90E3D4ED5F"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Workout {
    
    
    public partial class MainPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Microsoft.Phone.Controls.Panorama panoramaControl;
        
        internal System.Windows.Controls.ListBox todayExerciseListBox;
        
        internal System.Windows.Controls.TextBox SearchTextBox;
        
        internal System.Windows.Controls.ListBox ExercisesListBox;
        
        internal Microsoft.Phone.Controls.WebBrowser webBrowserControl;
        
        internal Microsoft.Phone.Shell.ApplicationBarMenuItem openTodayAppButton;
        
        internal Microsoft.Phone.Shell.ApplicationBarMenuItem aboutAppButton;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Workout;component/MainPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.panoramaControl = ((Microsoft.Phone.Controls.Panorama)(this.FindName("panoramaControl")));
            this.todayExerciseListBox = ((System.Windows.Controls.ListBox)(this.FindName("todayExerciseListBox")));
            this.SearchTextBox = ((System.Windows.Controls.TextBox)(this.FindName("SearchTextBox")));
            this.ExercisesListBox = ((System.Windows.Controls.ListBox)(this.FindName("ExercisesListBox")));
            this.webBrowserControl = ((Microsoft.Phone.Controls.WebBrowser)(this.FindName("webBrowserControl")));
            this.openTodayAppButton = ((Microsoft.Phone.Shell.ApplicationBarMenuItem)(this.FindName("openTodayAppButton")));
            this.aboutAppButton = ((Microsoft.Phone.Shell.ApplicationBarMenuItem)(this.FindName("aboutAppButton")));
        }
    }
}

