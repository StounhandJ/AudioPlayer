using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using AudioPlayerFullTest.Structs;

namespace AudioPlayerFullTest.Controls
{
    public partial class PlayListPanel : UserControl
    {
        public event Action<PlayListCollection> SelectedPlayList;
        
        public event Action<PlayListCollection> ChangeClick;
        
        public event Action<PlayListCollection> DeleteClick;
        
        private ObservableCollection<PlayListCollection> _playLists;
        public ObservableCollection<PlayListCollection> playLists
        {
            get
            {
                return _playLists;
            }
            set
            {
                ListViewPlayList.ItemsSource = value;
                _playLists = value;
            }
        }
        public PlayListPanel()
        {
            InitializeComponent();
        }

        private void ListViewPlayList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedPlayList?.Invoke(this.ListViewPlayList.SelectedItem is PlayListCollection ? (PlayListCollection) this.ListViewPlayList.SelectedItem : default);
        }

        private void MenuItemChange_OnClick(object sender, RoutedEventArgs e)
        {
            ChangeClick?.Invoke(this.ListViewPlayList.SelectedItem is PlayListCollection ? (PlayListCollection) this.ListViewPlayList.SelectedItem : default);
        }

        private void MenuItemDelete_OnClick(object sender, RoutedEventArgs e)
        {
            DeleteClick?.Invoke(this.ListViewPlayList.SelectedItem is PlayListCollection ? (PlayListCollection) this.ListViewPlayList.SelectedItem : default);
        }
    }
}