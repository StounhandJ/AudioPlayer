using System;
using System.Windows;
using System.Windows.Controls;
using AudioPlayerFullTest.Structs;

namespace AudioPlayerFullTest.Controls
{
    public partial class MenuEditMusic : UserControl
    {
        public event Action<string> ChangeMusic;
        
        private MusicNotifyChanged _music;
        public MusicNotifyChanged music
        {
            get
            {
                return _music;
            }
            set
            {
                NameMusic.Text = value.musics.name;
                _music = value;
            }
        }
        public MenuEditMusic()
        {
            InitializeComponent();
        }

        private void Change_OnClick(object sender, RoutedEventArgs e)
        {
            ChangeMusic?.Invoke(NameMusic.Text);
        }

        private void NameMusic_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeButton.IsEnabled = (sender as TextBox).Text != "";
        }
    }
}