﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
        DispatcherTimer _timerTwo = new DispatcherTimer();
        private PlayerViewModel _playerViewModel = new PlayerViewModel();
        private TimeConverter _timeConverter = new TimeConverter();
        
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
            double x = e.GetPosition(sliderSeek).X;
            double val = 1 - (sliderSeek.ActualWidth - x) / sliderSeek.ActualWidth;
            int pos = Convert.ToInt32(val * (sliderSeek.Maximum - sliderSeek.Minimum) + sliderSeek.Minimum);
            media.Position = new TimeSpan(0, 0, 0, pos, 0);
            ticktock(null, null);
        }

        private void sliderVol_MouseDown(object sender, MouseButtonEventArgs e)
        {
            double x = e.GetPosition(Vol2).X;
            double val = 1 - (Vol2.ActualWidth - x) / Vol2.ActualWidth;
            double pos = Convert.ToDouble(val * (Vol2.Maximum - Vol2.Minimum) + Vol2.Minimum);
            Vol2.Value = pos;
        }

        private void Vol_MouseEnter(object sender, MouseEventArgs e)
        {
            Vol2.Visibility = Visibility.Visible;
            VolRectangle.Visibility = Visibility.Visible;
            Vol2.IsEnabled = true;
        }

        private void Vol_MouseLeave(object sender, MouseEventArgs e)
        {
            if (Mouse.GetPosition(Vol).X<0 || Mouse.GetPosition(Vol).Y > Vol.ActualHeight || Mouse.GetPosition(Vol).X > Vol.ActualWidth)
            {
                Vol2.Visibility = Visibility.Hidden;
                VolRectangle.Visibility = Visibility.Hidden;
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
            _playerViewModel.NextMusicAuto();
        }

        private double lastVol;
        private void Vol_OnClick(object sender, RoutedEventArgs e)
        {
            if (Vol2.Value>0)
            {
                lastVol = Vol2.Value;
                Vol2.Value = 0;
            }
            else if (Vol2.Value==0)
            {
                Vol2.Value = lastVol;
            }
        }

        private void moveTopTime(object sender, EventArgs e)
        {
            double x = Mouse.GetPosition(this).X;
            double val = 1 - (sliderSeek.ActualWidth - x) / sliderSeek.ActualWidth;
            int pos = Convert.ToInt32(val * (sliderSeek.Maximum - sliderSeek.Minimum) + sliderSeek.Minimum);
            TopTimeTextBlock.Text = _timeConverter.TimeFormat(pos);
            Canvas.SetLeft(TopTime,Mouse.GetPosition(this).X-TopTime.ActualWidth/2);
        }
        private void SliderSeek_OnMouseEnter(object sender, MouseEventArgs e)
        {
            sliderSeekTIME.Visibility = Visibility.Visible;
            TopTime.Visibility = Visibility.Visible;
            _timerTwo.Interval = TimeSpan.FromMilliseconds(10);
            _timerTwo.Tick += new EventHandler(moveTopTime);
            _timerTwo.Start();
        }

        private void SliderSeek_OnMouseLeave(object sender, MouseEventArgs e)
        {
            sliderSeekTIME.Visibility = Visibility.Hidden;
            TopTime.Visibility = Visibility.Hidden;
            _timerTwo.Stop();
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
