using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using AudioPlayer.Converts;
using AudioPlayer.ViewModels;

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

        public delegate void MusicEvent(object sender, MusicEventArgs e);

        public event MusicEvent MusicStart;
        public event MusicEvent MusicEnded;

        public SolidColorBrush StyleBackground
        {
            get
            {
                return _playerViewModel.StyleBackground;
            }
            set
            {
                if (_playerViewModel.StyleBackground == value) return;

                _playerViewModel.StyleBackground = value;
            }
        }
        public Player()
        {
            InitializeComponent();
            Initialize();
        }
        
        public Player(Music music)
        {
            InitializeComponent();
            Initialize();
            _playerViewModel.SetMusic(music);
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
            _playerViewModel.SetMusic(_playerViewModel.PlayList.GetNext());
        }
        
        public void SetMusic(Music music)
        {
            _playerViewModel.PlayList = new Playlist(new List<Music>{music});
            _playerViewModel.SetMusic(_playerViewModel.PlayList.GetNext());
        }

        public void Stop()
        {
            _playerViewModel.StopPlay();
        }
        
        public void Play()
        {
            _playerViewModel.RunPlay();
        }

        public void SetVolume(int volume)
        {
            Vol2.Value = volume >= 0 && volume <= 1 ? volume : Vol2.Value;
        }
        
        public void NextMusic()
        {
            _playerViewModel.NextMusic();
        }

        public void LastMusic()
        {
            _playerViewModel.LastMusic();
        }
        
        public void StartRandom()
        {
            _playerViewModel.ChangeRandomPlayList(true);
        }
        
        public void StopRandom()
        {
            _playerViewModel.ChangeRandomPlayList(false);
        }

        public void StartReplay()
        {
            _playerViewModel.ChangeReplay(true);
        }
        
        public void StopReplay()
        {
            _playerViewModel.ChangeReplay(false);
        }
        
        public int getNowMusicIndex()
        {
            return _playerViewModel.PlayList.getIndex();
        }
        
        public Music getNowMusic()
        {
            return _playerViewModel.PlayList.GetNow();
        }
        
        public void AddMusic_PlayList(Music music)
        {
            if (_playerViewModel.PlayList!=null)
            {
                _playerViewModel.PlayList.Add(music);
            }
            else
            {
                _playerViewModel.PlayList = new Playlist(new List<Music>{music});
            }
            _playerViewModel.SetMusic(_playerViewModel.PlayList.GetNext());
        }
        
        public void RemoveMusic_PlayList(Music music)
        {
            _playerViewModel.PlayList?.Del(music);
        }
        
        public void PickNumberMusic_PlayList(int index)
        {
            if (_playerViewModel.PlayList!=null && _playerViewModel.PlayList.SetIndex(index))
            {
                _playerViewModel.SetMusic(_playerViewModel.PlayList.GetNext());
            }
        }
        
        public void PickMusic_PlayList(Music music)
        {
            if (_playerViewModel.PlayList!=null && _playerViewModel.PlayList.SetMusic(music))
            {
                _playerViewModel.SetMusic(_playerViewModel.PlayList.GetNext());
            }
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
            StubVol.IsHitTestVisible = true;
            Vol2.IsEnabled = true;
        }

        private void Vol_MouseLeave(object sender, MouseEventArgs e)
        {
            double x = Mouse.GetPosition(StackPanelVol).X;
            double y = Mouse.GetPosition(StackPanelVol).Y;

            bool LeaveYStackPanelVol = y > StackPanelVol.ActualHeight || y < 0;

            bool IsMouseLeft_VolRectangle = x < StackPanelVol.ActualWidth / 2;
            bool LeaveVolRectangle = (IsMouseLeft_VolRectangle && Mouse.GetPosition(VolRectangle).X<0) || (!IsMouseLeft_VolRectangle && Mouse.GetPosition(VolRectangle).X>VolRectangle.ActualWidth);
            if (LeaveYStackPanelVol || LeaveVolRectangle)
            {
                Vol2.Visibility = Visibility.Hidden;
                VolRectangle.Visibility = Visibility.Hidden;
                StubVol.IsHitTestVisible = false;
                Vol2.IsEnabled = false;
            }
          
        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            _position = media.NaturalDuration.TimeSpan;
            sliderSeek.Minimum = 0;
            sliderSeek.Maximum = _position.TotalSeconds;
            MusicEnded?.Invoke(this,new MusicEventArgs(_playerViewModel.PlayList.getIndex(),_playerViewModel.PlayList.GetNow()));
        }
        
        private void Media_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            MusicEnded?.Invoke(this,new MusicEventArgs(_playerViewModel.PlayList.getIndex(),_playerViewModel.PlayList.GetNow()));
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

    public class MusicEventArgs
    {
        public int indexMusic{get;}
        public Music music {get;}
 
        public MusicEventArgs(int indexMusicI, Music musicI)
        {
            indexMusic = indexMusicI;
            music = musicI;
        }
    }
}
