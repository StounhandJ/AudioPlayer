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

        private Playlist PlayList { get; set; }
        
        private MusicViewModel _MusicViewModel = new MusicViewModel();
        
        public Player()
        {
            InitializeComponent();
            Initialize();
        }
        
        public Player(Music music)
        {
            InitializeComponent();
            _MusicViewModel.SourceAudio = music.source;
            Initialize();
        }
        
        public Player(Playlist Playlist)
        {
            InitializeComponent();
            PlayList = Playlist;
            _MusicViewModel.SourceAudio = PlayList.GetNext()?.source;
            Initialize();
        }

        private void Initialize()
        {
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += new EventHandler(ticktock);
            _timer.Start();
            this.DataContext = _MusicViewModel;
        }

        public void SetPlayList(Playlist Playlist)
        {
            PlayList = Playlist;
            _MusicViewModel.SourceAudio = PlayList.GetNext()?.source;
            _timer.Interval = TimeSpan.FromMilliseconds(1000);
            _timer.Tick += new EventHandler(ticktock);
            _timer.Start();
        }
        

        void ticktock(object sender, EventArgs e)
        {
            sliderSeek.Value = media.Position.TotalSeconds;
        }

        private void PlayerControl_Click(object sender, RoutedEventArgs e)
        {
            if(media.LoadedBehavior == MediaState.Play)
            {
                PlayerControl.Content = new Image { Source= new BitmapImage(new Uri("D:\\AllProject\\Ch\\AudioPlayer\\AudioPlayer\\play.png")) };
                media.LoadedBehavior = MediaState.Pause;
            }
            else
            {
                PlayerControl.Content = new Image { Source = new BitmapImage(new Uri("D:\\AllProject\\Ch\\AudioPlayer\\AudioPlayer\\pause.png")) };
                media.LoadedBehavior = MediaState.Play;
            }
        }
        
        private void PlayerLast_Click(object sender, RoutedEventArgs e)
        {
            _MusicViewModel.SourceAudio = PlayList.Getlast()?.source;
            PlayerNext.IsEnabled = PlayList.IsNextMusic();
            PlayerNext.Opacity = PlayList.IsNextMusic() ? 1 : 0.5;
            PlayerLast.IsEnabled = PlayList.IsLastMusic();
            PlayerLast.Opacity = PlayList.IsLastMusic() ? 1 : 0.5;
        }
        
        private void PlayerNext_Click(object sender, RoutedEventArgs e)
        {
            _MusicViewModel.SourceAudio = PlayList.GetNext()?.source;
            PlayerNext.IsEnabled = PlayList.IsNextMusic();
            PlayerNext.Opacity = PlayList.IsNextMusic() ? 1 : 0.5;
            PlayerLast.IsEnabled = PlayList.IsLastMusic();
            PlayerLast.Opacity = PlayList.IsLastMusic() ? 1 : 0.5;
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
            var t = media;
            Vol2.Visibility = Visibility.Visible;
            Vol2.IsEnabled = true;
            var test = Vol.IsMouseOver; 
            var test2 = StackPanelVol.IsMouseOver;
        }

        private void Vol_MouseLeave(object sender, MouseEventArgs e)
        {
            var test = Mouse.GetPosition(Vol).Y;
            if (Mouse.GetPosition(Vol).X<0 || Mouse.GetPosition(Vol).Y > Vol.ActualHeight || Mouse.GetPosition(Vol).X > Vol.ActualWidth)
            {
                Vol2.Visibility = Visibility.Hidden;
                Vol2.IsEnabled = false;
            }
          
        }

        private void Media_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            if (PlayList != null && PlayList.IsNextMusic())
            {
                _MusicViewModel.SourceAudio = PlayList.GetNext()?.source;
                PlayerNext.IsEnabled = PlayList.IsNextMusic();
                PlayerNext.Opacity = PlayList.IsNextMusic() ? 1 : 0.5;
                PlayerLast.IsEnabled = PlayList.IsLastMusic();
                PlayerLast.Opacity = PlayList.IsLastMusic() ? 1 : 0.5;
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
