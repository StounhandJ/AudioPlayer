using System;
using System.Windows;
using System.Windows.Controls;
using AudioPlayerFullTest.Structs;

namespace AudioPlayerFullTest.Controls
{
    public partial class MusicContainer : UserControl
    {
        public event Action<MusicNotifyChanged> ContextMenuClick;
        
        public event Action<MusicNotifyChanged> SelectedMusic;

        private PlayListCollection _playList;
        public PlayListCollection playList
        {
            get
            {
                return _playList;
            }
            set
            {
                DataListBox.ItemsSource = value.musics;
                _playList = value;
            }
        }

        private MusicNotifyChanged _musicPlay;
        public MusicNotifyChanged MusicPlay
        {
            get
            {
                return _musicPlay;
            }
            set
            {
                this.noPlayAll();
                int index = DataListBox.Items.IndexOf(value);
                (DataListBox.Items.GetItemAt(index) as MusicNotifyChanged)?.play();
                _musicPlay = value;
            }
        }

        public int _musicPlayIndex;
        public int MusicPlayIndex
        {
            get
            {
                return _musicPlayIndex;
            }
            set
            {
                this.noPlayAll();
                (DataListBox.Items.GetItemAt(value) as MusicNotifyChanged)?.play();
                _musicPlayIndex = value;
            }
        }
        public MusicContainer()
        {
            InitializeComponent();
        }
        
        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            ContextMenuClick?.Invoke(this.DataListBox.SelectedItems[0] as MusicNotifyChanged);
        }

        private void ListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataListBox.SelectedIndex > -1)
            {
                MusicNotifyChanged music = (DataListBox.SelectedItem as MusicNotifyChanged);
                this.MusicPlay = music;
                this.MusicPlayIndex = DataListBox.SelectedIndex;
                SelectedMusic?.Invoke(music);
            }
        }

        private void noPlayAll()
        {
            foreach (MusicNotifyChanged music in DataListBox.Items)
            {
                music.noPlay();
            }
        }
    }
}