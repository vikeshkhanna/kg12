using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;
using WorkoutHelper;
using System.Linq;
using Microsoft.Phone.Tasks;

namespace Workout
{
    public class Utils
    {
        public static string GetImage(Exercise exercise, int index)
        {
            return exercise.Name.ToLower().Replace(" ", "_") + "_" + Convert.ToString(index) + ".jpg";
        }

        public static Day GetCurrentDay()
        {
            TimeSpan difference = DateTime.Now - App.ProgramStartDate;
            int dayNum = difference.Days + 1;

            Day day = (from Day d in App.WorkoutDB.Days
                       where d.Num == dayNum
                       select d).Single();

            return day;
        }

        public static string GetExerciseURL(string name)
        {
            return "http://www.bodybuilding.com/exercises/main/popup/name/" + name.ToLower().Replace(" ", "-").Replace("(", "").Replace(")", "");
        }

        public static int GetWeekForDay(int num)
        {
            return (int)Math.Ceiling(num / 7.0);
        }

        public static void LaunchMediaPlayer(Uri uri)
        {
            MediaPlayerLauncher mediaPlayerLauncher = new MediaPlayerLauncher();

            mediaPlayerLauncher.Media = uri;
            mediaPlayerLauncher.Location = MediaLocationType.Data;
            mediaPlayerLauncher.Controls = MediaPlaybackControls.Pause | MediaPlaybackControls.Stop;
            mediaPlayerLauncher.Orientation = MediaPlayerOrientation.Landscape;

            mediaPlayerLauncher.Show();
        }

        public static bool IsMediaURL(string url)
        {
            if (url.EndsWith(".mp4"))
            {
                return true;
            }

            return false;
        }

        public static string GetTodayLink()
        {
            Day day = GetCurrentDay();
            return String.Format("http://www.bodybuilding.com/fun/kris-gethin-12-week-daily-trainer-week-{0}-day-{1}.html", 
                Convert.ToString(Utils.GetWeekForDay(day.Num)), Convert.ToString(day.Num));
        }
    }

}
