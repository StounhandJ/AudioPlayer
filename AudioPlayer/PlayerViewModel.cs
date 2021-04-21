using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Mvvm;

namespace AudioPlayer
{
    public class PlayerViewModel : ViewModelBase
    {
        private bool _replay = false;
        
        private bool _random = false;
        private Playlist _playList { get; set; }
        public Playlist PlayList
        {
            get
            {
                return _playList;
            }
            set
            {
                if (_playList == value) return;

                _playList = value;
                update();
            }
        }

        private string _sourceAudio { get; set; }
        public string SourceAudio 
        {
            get
            {
                return _sourceAudio;
            }
            set 
            {
                if (_sourceAudio == value) return;

                _sourceAudio = value;
                update();
            }
        }
        
        public SolidColorBrush PlayerReplayForeground 
        {
            get
            {
                return (SolidColorBrush)(new BrushConverter().ConvertFrom(_replay?"#fc0":"#000000"));
            }
        }
        
        public SolidColorBrush PlayerRandomForeground 
        {
            get
            {
                return (SolidColorBrush)(new BrushConverter().ConvertFrom(_random?"#fc0":"#000000"));
            }
        }

        private MediaState _mediaLoadedBehavior = MediaState.Play;
        public MediaState MediaLoadedBehavior 
        {
            get
            {
                return _mediaLoadedBehavior;
            }
            set 
            {
                if (_mediaLoadedBehavior == value) return;

                _mediaLoadedBehavior = value;
                RaisePropertyChanged(()=>MediaLoadedBehavior);
            }
        }
        
        private Image _playerControlImage = new Image { Source = new BitmapImage(new Uri("img/pause.png", UriKind.Relative))};
        public Image PlayerControlImage
        {
            get
            {
                return _playerControlImage;
            }
            set 
            {
                if (_playerControlImage == value) return;

                _playerControlImage = value;
                RaisePropertyChanged(()=>PlayerControlImage);
            }
        }
        

        public bool PlayerNextEnabled
        {
            get
            {
                return _playList.IsNextMusic();
            }
        }
        
        public bool PlayerLastEnabled
        {
            get
            {
                return _playList.IsLastMusic();
            }
        }
        
        public double PlayerNextOpacity
        {
            get
            {
                return PlayList.IsNextMusic() ? 1 : 0.5;
            }
        }
        
        public double PlayerLastOpacity
        {
            get
            {
                return PlayList.IsLastMusic() ? 1 : 0.5;
            }
        }

        public ICommand PlayerNext_Click
        {
            get
            {
                return new DelegateCommand(NextMusic);
            }
        }
        
        public ICommand PlayerLast_Click
        {
            get
            {
                return new DelegateCommand(LastMusic);
            }
        }
        
        public ICommand PlayerReplay_Click
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    _replay = !_replay;
                    RaisePropertyChanged(()=>PlayerReplayForeground);
                });
            }
        }

        public ICommand PlayerRandom_Click
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (_random)
                    {
                        _playList.StandartPlayList();
                    }
                    else
                    {
                        _playList.RandomPlayList();
                    }

                    SourceAudio = _playList.GetNext()?.source;
                    _random = !_random;
                    RaisePropertyChanged(()=>PlayerRandomForeground);
                });
            }
        }
        
        public ICommand PlayerControl_Click
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if(MediaLoadedBehavior == MediaState.Play)
                    {
                        PlayerControlImage = new Image { Source= new BitmapImage(new Uri("img/play.png", UriKind.Relative)) };
                        MediaLoadedBehavior = MediaState.Pause;
                    }
                    else
                    {
                        PlayerControlImage = new Image { Source = new BitmapImage(new Uri("img/pause.png", UriKind.Relative)) };
                        MediaLoadedBehavior = MediaState.Play;
                    }
                });
            }
        }
        
        public void NextMusicAuto()
        {
            if (_replay)
            {
                SourceAudio = "";
                SourceAudio = _playList.GetNow()?.source;
            }
            else if (_playList != null && _playList.IsNextMusic())
            {
                SourceAudio = _playList.GetNext()?.source;
            }
        }

        public void NextMusic()
        {
            if (_playList != null && _playList.IsNextMusic())
            {
                SourceAudio = _playList.GetNext()?.source;
            }
        }

        public void LastMusic()
        {
            if (_playList != null && _playList.IsLastMusic())
            {
                SourceAudio = _playList.Getlast()?.source;
            }
        }
        private void update()
        {
            RaisePropertyChanged(()=>PlayerLastEnabled);
            RaisePropertyChanged(()=>PlayerNextEnabled);
            RaisePropertyChanged(()=>PlayerNextOpacity);
            RaisePropertyChanged(()=>PlayerLastOpacity);
            RaisePropertyChanged(()=>MediaLoadedBehavior);
            RaisePropertyChanged(()=>PlayerReplayForeground);
            RaisePropertyChanged(()=>SourceAudio);
        }

        public event PropertyChangedEventHandler PropertyChanged;
		
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}