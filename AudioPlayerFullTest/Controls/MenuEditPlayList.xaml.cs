using System;
using System.Windows;
using System.Windows.Controls;
using AudioPlayerFullTest.Structs;

namespace AudioPlayerFullTest.Controls
{
    public partial class MenuEditPlayList : UserControl
    {
        public event Action<string> ChangePlayList;
        
        private PlayListCollection _playList;
        public PlayListCollection PlayList
        {
            get
            {
                return _playList;
            }
            set
            {
                NamePlayList.Text = value.name;
                _playList = value;
            }
        }
        
        public MenuEditPlayList()
        {
            InitializeComponent();
        }
        
        private void Change_OnClick(object sender, RoutedEventArgs e)
        {
            ChangePlayList?.Invoke(NamePlayList.Text);
        }

        private void NamePlayList_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeButton.IsEnabled = (sender as TextBox).Text != "";
        }
    }
}