using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AudioPlayer
{
    /// <summary>
    /// Логика взаимодействия для Player.xaml
    /// </summary>
    public partial class Player : UserControl
    {
        TimeSpan _position;
        DispatcherTimer _timer = new DispatcherTimer();
        public string SourceAudio { get; set; }
        public Player()
        {
            InitializeComponent();
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += new EventHandler(ticktock);
            _timer.Start();
            this.DataContext = this;
        }

        void ticktock(object sender, EventArgs e)
        {
            sliderSeek.Value = media.Position.TotalSeconds;
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            media.LoadedBehavior = MediaState.Play;
        }

        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            media.LoadedBehavior = MediaState.Pause;
        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            _position = media.NaturalDuration.TimeSpan;
            sliderSeek.Minimum = 0;
            sliderSeek.Maximum = _position.TotalSeconds;
        }

        private void sliderSeek_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var mainborder = sender as Border;
            double x = e.GetPosition(mainborder).X;
            double val = 1 - (mainborder.ActualWidth - x) / mainborder.ActualWidth;
            int pos = Convert.ToInt32(val * (sliderSeek.Maximum - sliderSeek.Minimum) + sliderSeek.Minimum);
            media.Position = new TimeSpan(0, 0, 0, pos, 0);
        }

        private void sliderVal_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var mainborder = sender as Border;
            double x = e.GetPosition(mainborder).X;
            double val = 1 - (mainborder.ActualWidth - x) / mainborder.ActualWidth;
            double pos = Convert.ToDouble(val * (Vol2.Maximum - Vol2.Minimum) + Vol2.Minimum);
            Vol2.Value = pos;
        }

        private void Slider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            int pos = Convert.ToInt32(sliderSeek.Value);
            media.Position = new TimeSpan(0, 0, 0, pos, 0);
        }

        private void Vol_MouseEnter(object sender, MouseEventArgs e)
        {
            Vol2.Visibility = Visibility.Visible;
            Vol2.IsEnabled = true;
            var test = Vol.IsMouseOver; 
            var test2 = StackPanelVol.IsMouseOver;
        }

        private void Vol_MouseLeave(object sender, MouseEventArgs e)
        {
            var test = Mouse.GetPosition(Vol).Y;
            if (Mouse.GetPosition(Vol).X<0 || Mouse.GetPosition(Vol).Y > Vol.ActualHeight)
            {
                Vol2.Visibility = Visibility.Hidden;
                Vol2.IsEnabled = false;
            }
          
        }
    }

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
