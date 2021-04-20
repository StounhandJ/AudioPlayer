using System.ComponentModel;
using System.Windows.Input;

namespace AudioPlayer
{
    public class MusicViewModel : INotifyPropertyChanged
    {
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
                this.OnPropertyChanged("SourceAudio");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
		
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}