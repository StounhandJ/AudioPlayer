using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AudioPlayer.Converts
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TimeFormat((double) value);
        }
     
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        public string TimeFormat(double value)
        {
            var time = (int)Math.Round(value);
            string seconds = (time % 60).ToString();
            seconds = seconds.Length < 2 ? "0" + seconds : seconds;
            return $"{time / 60}:{seconds}";
        }
    }
}