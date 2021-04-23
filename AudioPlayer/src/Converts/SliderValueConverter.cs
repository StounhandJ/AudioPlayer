using System;
using System.Windows.Data;

namespace AudioPlayer.Converts
{
    public class SliderValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double val = (double)values[0];
            double min = (double)values[1];
            double max = (double)values[2];
            double sliderWidth = (double)values[3];
            return sliderWidth * (val - min) / (max - min);
        }



        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}