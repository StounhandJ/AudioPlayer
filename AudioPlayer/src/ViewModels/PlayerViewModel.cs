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

        private Uri _sourceAudio { get; set; }
        public Uri SourceAudio 
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
        
        private Uri _noImageAudio = new Uri("../img/noavatar.jpg", UriKind.Relative);
        private Uri _imageAudio = new Uri("../img/noavatar.jpg", UriKind.Relative);
        public Uri ImageAudio
        {
            get
            {
                return _imageAudio;
            }
            set 
            {
                if (_imageAudio == value) return;

                _imageAudio = value;
                RaisePropertyChanged(()=>ImageAudio);
            }
        }
        
        private string _noNameAudio = "Неизвестно";
        private string _nameAudio = "Неизвестно";
        public string NameAudio
        {
            get
            {
                return _nameAudio;
            }
            set 
            {
                if (_nameAudio == value) return;

                _nameAudio = value;
                RaisePropertyChanged(()=>NameAudio);
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
        
        private Image _playerControlImage = new Image { Source = new BitmapImage(new Uri("../img/pause.png", UriKind.Relative))};
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
                return PlayList.IsNextMusic() ? 0.85 : 0.4;
            }
        }
        
        public double PlayerLastOpacity
        {
            get
            {
                return PlayList.IsLastMusic() ? 0.85 : 0.4;
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
                    RandomPlayList();
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
                        StopPlay();
                    }
                    else
                    {
                        RunPlay();
                    }
                });
            }
        }

        public void StopPlay()
        {
            PlayerControlImage = new Image { Source= new BitmapImage(new Uri("../img/play.png", UriKind.Relative)) };
            MediaLoadedBehavior = MediaState.Pause;
        }
        
        public void RunPlay()
        {
            PlayerControlImage = new Image { Source = new BitmapImage(new Uri("../img/pause.png", UriKind.Relative)) };
            MediaLoadedBehavior = MediaState.Play;
        }

        public void RandomPlayList(bool button = true)
        {
            if (_random && button)
            {
                _playList.StandartPlayList();
            }
            else
            {
                _playList.RandomPlayList();
            }

            SetMusic(_playList.GetNext());
            _random = !_random;
            RaisePropertyChanged(()=>PlayerRandomForeground);
        }

        public void SetMusic(Music? music)
        {
            SourceAudio = music?.source ?? new Uri("");
            string fileName = SourceAudio.Segments.Length > 0 && SourceAudio.IsFile ? SourceAudio.Segments[SourceAudio.Segments.Length-1].Split('.')[0] : null;
            ImageAudio = music?.sourceImg ?? _noImageAudio;
            NameAudio = music?.name ?? (fileName??_noNameAudio);
        }
        
        public void NextMusicAuto()
        {
            if (_replay)
            {
                SetMusic(_playList.GetNow());
            }
            else if (_playList != null && _playList.IsNextMusic())
            {
                SetMusic(_playList.GetNext());
            }
        }

        public void NextMusic()
        {
            if (_playList != null && _playList.IsNextMusic())
            {
                SetMusic(_playList.GetNext());
            }
        }

        public void LastMusic()
        {
            if (_playList != null && _playList.IsLastMusic())
            {
                SetMusic(_playList.Getlast());
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