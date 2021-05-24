using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using AudioPlayer.Class;
using AudioPlayer.Converts;
using AudioPlayer.ViewModels;

namespace AudioPlayer
{
    /// <summary>
    /// Логика взаимодействия для Player.xaml
    /// </summary>
    public partial class Player : UserControl
    {
        private TimeSpan _position;
        private DispatcherTimer _timer = new DispatcherTimer();
        private DispatcherTimer _timerTwo = new DispatcherTimer();
        private PlayerViewModel _playerViewModel = new PlayerViewModel();
        private TimeConverter _timeConverter = new TimeConverter();

        public delegate void MusicEvent(object sender, MusicEventArgs e);
        
        public delegate void PlayListEvent(object sender, Playlist e);

        /// <summary>
        /// Event when the track starts playing
        /// </summary>
        public event MusicEvent MusicStart;
        
        /// <summary>
        /// Event at the end of track playback
        /// </summary>
        public event MusicEvent MusicEnded;
        
        /// <summary>
        /// Event at the end of track playback
        /// </summary>
        public event PlayListEvent PlayListEnded;

        /// <summary>
        /// Main color the Audio player
        /// </summary>
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

        /// <summary>
        /// Sets a playlist and plays it back
        /// </summary>
        public void SetPlayList(Playlist Playlist)
        {
            _playerViewModel.PlayList = Playlist;
            _playerViewModel.SetMusic(_playerViewModel.PlayList.Next());
        }
        
        /// <summary>
        /// Set a Music and play it back
        /// </summary>
        public void SetMusic(Music music)
        {
            _playerViewModel.PlayList = new Playlist(new List<Music>{music});
            _playerViewModel.SetMusic(_playerViewModel.PlayList.Next());
        }

        /// <summary>
        /// Stop play music
        /// </summary>
        public void Stop()
        {
            _playerViewModel.StopPlay();
        }
        
        /// <summary>
        /// Run play music
        /// </summary>
        public void Play()
        {
            _playerViewModel.RunPlay();
        }

        /// <summary>
        /// Set volume music
        /// </summary>
        /// <param name="volume">from 0 to 1</param>
        public void SetVolume(int volume)
        {
            VolumeSlider.Value = volume >= 0 && volume <= 1 ? volume : VolumeSlider.Value;
        }
        
        private double lastVol = -1;
        /// <summary>
        /// Mute or unmute
        /// </summary>
        public void MuteVolume(bool IsMute)
        {
            if (IsMute)
            {
                lastVol = VolumeSlider.Value;
                this._playerViewModel.Volume = 0;
            }
            else
            {
                this._playerViewModel.Volume = lastVol;
                lastVol = -1;
            }
        }
        
        /// <summary>
        /// Is mute install
        /// </summary>
        public bool IsMuteVolume()
        {
            return lastVol!=-1;
        }
        
        /// <summary>
        /// Play next music
        /// </summary>
        public void NextMusic()
        {
            _playerViewModel.NextMusic();
        }

        /// <summary>
        /// Play last music
        /// </summary>
        public void LastMusic()
        {
            _playerViewModel.LastMusic();
        }
        
        /// <summary>
        /// Enables shuffle playlist
        /// </summary>
        public void StartRandom()
        {
            _playerViewModel.ChangeRandomPlayList(true);
        }
        
        /// <summary>
        /// Disables shuffle playlist
        /// </summary>
        public void StopRandom()
        {
            _playerViewModel.ChangeRandomPlayList(false);
        }

        /// <summary>
        /// Enables music replay
        /// </summary>
        public void StartReplay()
        {
            _playerViewModel.ChangeReplay(true);
        }
        
        /// <summary>
        /// Disables music replay
        /// </summary>
        public void StopReplay()
        {
            _playerViewModel.ChangeReplay(false);
        }
        
        /// <summary>
        /// Get number play music
        /// </summary>
        public int getNowMusicIndex()
        {
            return _playerViewModel.PlayList.getIndex();
        }
        
        /// <summary>
        /// Get number play musicc
        /// </summary>
        public Music getNowMusic()
        {
            return _playerViewModel.PlayList.getNow();
        }
        
        /// <summary>
        /// Add music to a playlist
        /// </summary>
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
            _playerViewModel.SetMusic(_playerViewModel.PlayList.Next());
        }
        
        /// <summary>
        /// Delete music from a playlist
        /// </summary>
        public void RemoveMusic_PlayList(Music music)
        {
            _playerViewModel.PlayList?.Del(music);
        }
        
        /// <summary>
        /// Play music under the number (index from scratch)
        /// </summary>
        public void PickNumberMusic_PlayList(int index)
        {
            if (_playerViewModel.PlayList!=null && _playerViewModel.PlayList.SetIndex(index))
            {
                _playerViewModel.SetMusic(_playerViewModel.PlayList.Next());
            }
        }
        
        /// <summary>
        /// Play music if in playlist
        /// </summary>
        public void PickMusic_PlayList(Music music)
        {
            if (_playerViewModel.PlayList!=null && _playerViewModel.PlayList.SetMusic(music))
            {
                _playerViewModel.SetMusic(_playerViewModel.PlayList.Next());
            }
        }
        
        private void Initialize()
        {
            _timer.Interval = TimeSpan.FromMilliseconds(400);
            _timer.Tick += new EventHandler(ticktock);
            _timer.Start();
            this.DataContext = _playerViewModel;
        }
        

        private void ticktock(object sender, EventArgs e)
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
            double x = e.GetPosition(VolumeSlider).X;
            double val = 1 - (VolumeSlider.ActualWidth - x) / VolumeSlider.ActualWidth;
            double pos = Convert.ToDouble(val * (VolumeSlider.Maximum - VolumeSlider.Minimum) + VolumeSlider.Minimum);
            VolumeSlider.Value = pos;
        }

        private void Vol_MouseEnter(object sender, MouseEventArgs e)
        {
            VolumeSlider.Visibility = Visibility.Visible;
            VolRectangle.Visibility = Visibility.Visible;
            StubVol.IsHitTestVisible = true;
            VolumeSlider.IsEnabled = true;
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
                VolumeSlider.Visibility = Visibility.Hidden;
                VolRectangle.Visibility = Visibility.Hidden;
                StubVol.IsHitTestVisible = false;
                VolumeSlider.IsEnabled = false;
            }
          
        }

        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            _position = media.NaturalDuration.TimeSpan;
            sliderSeek.Minimum = 0;
            sliderSeek.Maximum = _position.TotalSeconds;
            MusicStart?.Invoke(this,new MusicEventArgs(_playerViewModel.PlayList.getIndex(),_playerViewModel.PlayList.getNow()));
        }
        
        private void Media_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            MusicEnded?.Invoke(this,new MusicEventArgs(_playerViewModel.PlayList.getIndex(),_playerViewModel.PlayList.getNow()));
            if (_playerViewModel.PlayList.IsNextMusic())
            {
                _playerViewModel.NextMusicAuto();
            }
            else
            {
                PlayListEnded?.Invoke(this, _playerViewModel.PlayList);
            }
        }
        
        private void Vol_OnClick(object sender, RoutedEventArgs e)
        {
            this.MuteVolume(!this.IsMuteVolume());
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
