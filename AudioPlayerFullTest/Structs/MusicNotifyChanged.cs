using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using AudioPlayer;
using AudioPlayer.Class;
using AudioPlayer.Converts;

namespace AudioPlayerFullTest.Structs
{
    public class MusicNotifyChanged : INotifyPropertyChanged
    {
        private TimeConverter _timeConverter = new TimeConverter();

        private double _opacityI = 1;
        
        public double OpacityI {
            get
            {
                return _opacityI;
            }
            set
            {
                _opacityI = value;
                this.OnPropertyChanged("OpacityI");
            } }
            
        private Visibility _visibility = Visibility.Hidden;
        
        public Visibility VisibilityI {
            get
            {
                return _visibility;
            }
            set
            {
                _visibility = value;
                this.OnPropertyChanged("VisibilityI");
            } }

        private SolidColorBrush _background = new SolidColorBrush(Colors.Transparent);
        public SolidColorBrush Background {
            get
            {
                return _background;
            }
            set
            {
                _background = value;
                this.OnPropertyChanged("Background");
            } }
        public Music musics { get; set; }

        private string _time = "0";
        public string time
        {
            get
            {
                return _timeConverter.TimeFormat(Double.Parse(_time));
            }
            set
            {
                _time = value;
            }
        }

        public MusicNotifyChanged()
        {
            time = "0";
        }

        public void play()
        {
            Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#E0E0E0"));
            VisibilityI = Visibility.Visible;
            OpacityI = 0.6;
        }
        
        public void noPlay()
        {
            Background = Brushes.Transparent;
            VisibilityI = Visibility.Hidden;
            OpacityI = 1;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}