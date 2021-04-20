using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private PlayerViewModel _playerViewModel = new PlayerViewModel();
        
        public Player()
        {
            InitializeComponent();
            Initialize();
        }
        
        public Player(Music music)
        {
            InitializeComponent();
            Initialize();
            _playerViewModel.SourceAudio = music.source;
        }
        
        public Player(Playlist Playlist)
        {
            InitializeComponent();
            Initialize();
            SetPlayList(Playlist);
        }

        public void SetPlayList(Playlist Playlist)
        {
            _playerViewModel.PlayList = Playlist;
            _playerViewModel.SourceAudio = _playerViewModel.PlayList.GetNext()?.source;
        }
        
        private void Initialize()
        {
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += new EventHandler(ticktock);
            _timer.Start();
            this.DataContext = _playerViewModel;
        }
        

        void ticktock(object sender, EventArgs e)
        {
            sliderSeek.Value = media.Position.TotalSeconds;
        }

        private void sliderSeek_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mainborder = sender as Border;
            double x = e.GetPosition(mainborder).X;
            double val = 1 - (mainborder.ActualWidth - x) / mainborder.ActualWidth;
            int pos = Convert.ToInt32(val * (sliderSeek.Maximum - sliderSeek.Minimum) + sliderSeek.Minimum);
            media.Position = new TimeSpan(0, 0, 0, pos, 0);
            ticktock(null, null);
        }

        private void sliderVol_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mainborder = sender as Border;
            double x = e.GetPosition(mainborder).X;
            double val = 1 - (mainborder.ActualWidth - x) / mainborder.ActualWidth;
            double pos = Convert.ToDouble(val * (Vol2.Maximum - Vol2.Minimum) + Vol2.Minimum);
            Vol2.Value = pos;
        }

        private void Vol_MouseEnter(object sender, MouseEventArgs e)
        {
            Vol2.Visibility = Visibility.Visible;
            Vol2.IsEnabled = true;
        }

        private void Vol_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Mouse.GetPosition(Vol).X<0 || Mouse.GetPosition(Vol).Y > Vol.ActualHeight || Mouse.GetPosition(Vol).X > Vol.ActualWidth)
            {
                Vol2.Visibility = Visibility.Hidden;
                Vol2.IsEnabled = false;
            }
          
        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            _position = media.NaturalDuration.TimeSpan;
            sliderSeek.Minimum = 0;
            sliderSeek.Maximum = _position.TotalSeconds;
        }
        
        private void Media_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            _playerViewModel.NextMusic();
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
