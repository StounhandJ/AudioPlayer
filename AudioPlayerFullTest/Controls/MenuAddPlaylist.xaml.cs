using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AudioPlayerFullTest.Structs;

namespace AudioPlayerFullTest.Controls
{
    public partial class MenuAddPlaylist : UserControl
    {
        public event Action Close;

        public event Action<string> CreatePlayList; 
        
        public event Action<PlayListCollection> SelectedPlayList; 
        
        private ObservableCollection<PlayListCollection> _playLists;
        public ObservableCollection<PlayListCollection> playLists
        {
            get
            {
                return _playLists;
            }
            set
            {
                ListViewPlayListAdd.ItemsSource = value;
                _playLists = value;
            }
        }
        public MenuAddPlaylist()
        {
            InitializeComponent();
        }

        private void CreatePlayList_OnClick(object sender, RoutedEventArgs e)
        {
            if (NewNamePlayList.Text=="" || this.playLists.Count(x => x.name == NewNamePlayList.Text)>0) return;
            
            CreatePlayList?.Invoke(NewNamePlayList.Text);
            Close?.Invoke();
            NewNamePlayList.Text = "";
        }

        private void ListViewPlayListAdd_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedPlayList?.Invoke(this.ListViewPlayListAdd.SelectedItem is PlayListCollection ? (PlayListCollection) this.ListViewPlayListAdd.SelectedItem : default);
            Close?.Invoke();
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Close?.Invoke();
        }
    }
}