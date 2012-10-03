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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media.Imaging;
using WorkoutHelper;
using System.IO.IsolatedStorage;
using System.Collections.ObjectModel;

namespace Workout
{
    public partial class App : Application
    {
        private static MainViewModel viewModel = null;
        private static WorkoutContext workoutDB;
        private static bool isProgramOn;
        private static DateTime programStartDate;
        private static IsolatedStorageSettings userSettings = IsolatedStorageSettings.ApplicationSettings;

        public static List<DayExercise> AllDayExercises {get; private set;}
        public static List<Exercise> AllExercises { get; private set; }
        public static ObservableCollection<DBExercise> AllDBExercises { get; private set; }
        
        /// <summary>
        /// A static ViewModel used by the views to bind against.
        /// </summary>
        /// <returns>The MainViewModel object.</returns>
        public static MainViewModel ViewModel
        {
            get
            {
                // Delay creation of the view model until necessary
                if (viewModel == null)
                    viewModel = new MainViewModel();

                return viewModel;
            }
        }

        public static WorkoutContext WorkoutDB
        {
            get
            {
                return App.workoutDB;
            }
            private set
            {
                App.workoutDB = value;
            }
        }

        public static bool IsProgramOn
        {
            get
            {
                return App.isProgramOn;
            }
            set
            {
                App.isProgramOn = value;
            }
        }

        public static DateTime ProgramStartDate
        {
            get
            {
                return App.programStartDate;
            }
            set
            {
                App.programStartDate = value;
            }
        }

        public static IsolatedStorageSettings UserSettings
        {
            get
            {
                return App.userSettings;
            }
            private set
            {
                App.userSettings = value;
            }
        }

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            //Instantiate DB context
            App.WorkoutDB = new WorkoutContext("Data Source = 'appdata:/workout.sdf'; File Mode = read only;");

            //Retrieve settings
            try
            {
                App.IsProgramOn = (bool)App.UserSettings["IsProgramOn"];
                App.ProgramStartDate = (DateTime)App.UserSettings["ProgramStartDate"];
            }
            catch (KeyNotFoundException ex)
            {
                App.IsProgramOn = false;
                App.ProgramStartDate = DateTime.Now;
                Console.WriteLine("<kg12> Exception : " + ex.Message);
            }

            //Cache Reference DB
            App.AllDayExercises = (from DayExercise de in App.WorkoutDB.DayExercises
                                    select de).ToList();
            App.AllExercises = (from Exercise e in App.WorkoutDB.Exercises
                                select e).ToList();


            App.AllDBExercises = new ObservableCollection<DBExercise>();

            // Load all DB exercises
            foreach (Exercise exercise in App.AllExercises)
            {
                DBExercise ex = new DBExercise()
                {
                    ExerciseName = exercise.Name,
                    WorkoutImage = "Media/" + Utils.GetImage(exercise, 0)
                };

                App.AllDBExercises.Add(ex);
            }

            App.AllDBExercises = new ObservableCollection<DBExercise>(App.AllDBExercises.OrderBy(e => e.ExerciseName));
        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            // Ensure that application state is restored appropriately
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            // Ensure that required application state is persisted here.
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }

    /// <summary>
    /// Converts True to "Yes", False to "No"
    /// </summary>
    public class BooleanToYesNoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (((bool)value == true) ? "Yes" : "No");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (((bool)value == true) ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ReverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (((bool)value == false) ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}