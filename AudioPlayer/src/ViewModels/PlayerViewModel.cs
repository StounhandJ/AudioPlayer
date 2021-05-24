using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AudioPlayer.Class;

// using DevExpress.Mvvm;

namespace AudioPlayer.ViewModels
{
    public sealed class PlayerViewModel : INotifyPropertyChanged
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
                this.OnPropertyChanged("ImageAudio");
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
                this.OnPropertyChanged("NameAudio");
            }
        }
        
        private SolidColorBrush _styleBackground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#fc0"));
        public SolidColorBrush StyleBackground
        {
            get
            {
                return _styleBackground;
            }
            set
            {
                if (_styleBackground == value) return;

                _styleBackground = value;
                this.OnPropertyChanged("StyleBackground");
            }
        }
        
        public SolidColorBrush PlayerReplayForeground 
        {
            get
            {
                return _replay?StyleBackground:(SolidColorBrush)(new BrushConverter().ConvertFrom("#000000"));
            }
        }
        
        public SolidColorBrush PlayerRandomForeground 
        {
            get
            {
                return _random?StyleBackground:(SolidColorBrush)(new BrushConverter().ConvertFrom("#000000"));
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
                this.OnPropertyChanged("MediaLoadedBehavior");
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
                this.OnPropertyChanged("PlayerControlImage");
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
                return new DelegateCommand((p) =>
                {
                    NextMusic();
                });
            }
        }
        
        public ICommand PlayerLast_Click
        {
            get
            {
                return new DelegateCommand((p)=>
                {
                    LastMusic();
                });
            }
        }
        
        public ICommand PlayerReplay_Click
        {
            get
            {
                return new DelegateCommand((p) =>
                {
                    ChangeReplay(!_replay);
                });
            }
        }

        public ICommand PlayerRandom_Click
        {
            get
            {
                return new DelegateCommand((p) =>
                {
                    ChangeRandomPlayList(!_random);
                });
            }
        }
        
        public ICommand PlayerControl_Click
        {
            get
            {
                return new DelegateCommand((p) =>
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

        public void ChangeReplay(bool replay)
        {
            _replay = replay;
            this.OnPropertyChanged("PlayerReplayForeground");
        }

        public void ChangeRandomPlayList(bool random)
        {
            if (random==_random) return;
            
            if (random)
            {
                _playList.RandomPlayList();
            }
            else
            {
                _playList.StandartPlayList();
            }

            SetMusic(_playList.Next());
            _random = !_random;
            this.OnPropertyChanged("PlayerRandomForeground");
        }

        public void SetMusic(Music? music)
        {
            SourceAudio = new Uri("null.mp3", UriKind.Relative);
            SourceAudio = music?.source ?? new Uri("null.mp3", UriKind.Relative);
            string fileName = SourceAudio.Segments.Length > 0 && SourceAudio.IsFile ? SourceAudio.Segments[SourceAudio.Segments.Length-1].Split('.')[0] : null;
            ImageAudio = music?.sourceImg ?? _noImageAudio;
            NameAudio = music?.name ?? (fileName??_noNameAudio);
        }
        
        public void NextMusicAuto()
        {
            if (_replay)
            {
                SourceAudio = null;
                SetMusic(_playList.getNow());
            }
            else if (_playList != null && _playList.IsNextMusic())
            {
                SetMusic(_playList.Next());
            }
        }

        public void NextMusic()
        {
            if (_playList != null && _playList.IsNextMusic())
            {
                SetMusic(_playList.Next());
            }
        }

        public void LastMusic()
        {
            if (_playList != null && _playList.IsLastMusic())
            {
                SetMusic(_playList.Last());
            }
        }
        private void update()
        {
            this.OnPropertyChanged("PlayerLastEnabled");
            this.OnPropertyChanged("PlayerNextEnabled");
            this.OnPropertyChanged("PlayerNextOpacity");
            this.OnPropertyChanged("PlayerLastOpacity");
            this.OnPropertyChanged("MediaLoadedBehavior");
            this.OnPropertyChanged("PlayerReplayForeground");
            this.OnPropertyChanged("SourceAudio");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    
    public class DelegateCommand : ICommand
    {
        private Predicate<object> _canExecute;
        private Action<object> _execute;

        public DelegateCommand(Action<object> execute)
        {
            this._canExecute = (p) => { return true;};
            this._execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}