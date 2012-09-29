﻿using System;
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
    }
}
