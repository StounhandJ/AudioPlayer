using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using AudioPlayer;
using AudioPlayer.Class;
using AudioPlayerFullTest.Structs;

namespace AudioPlayerFullTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ObservableCollection<PlayListCollection> PlayListCollections;

        private PlayListCollection SelectedPlayList;
        public MainWindow()
        {
            InitializeComponent();
            this.PlayListCollections = new ObservableCollection<PlayListCollection>();
            PlayListCollection playList = new PlayListCollection();
            playList.name = "Основной";
            playList.musics = new ObservableCollection<MusicNotifyChanged>();
            playList.musics.Add(new MusicNotifyChanged{musics = new Music{name = "Тест 1", source = new Uri(@"C:\Users\rmari\RiderProjects\AudioPlayer\AudioPlayerFullTest\testMusic\BTS-Butter.mp3")}});
            playList.musics.Add(new MusicNotifyChanged{musics = new Music{name = "Тест 2", source = new Uri(@"C:\Users\rmari\RiderProjects\AudioPlayer\AudioPlayerFullTest\testMusic\Konfuz-Ратата.mp3")}});
            playList.musics.Add(new MusicNotifyChanged{musics = new Music{name = "Тест 3", source = new Uri(@"C:\Users\rmari\RiderProjects\AudioPlayer\AudioPlayerFullTest\testMusic\РукиВверх-Нокаут.mp3")}});
            this.PlayListCollections.Add(playList);
            
            this.SelectingPlauList(playList);
            
            this.CustomPlayer.SetVolume(0);
            
            PlayListCollection playList2 = new PlayListCollection();
            playList2.name = "Второй";
            playList2.musics = new ObservableCollection<MusicNotifyChanged>();
            playList2.musics.Add(new MusicNotifyChanged{musics = new Music{name = "Тест 11", source = new Uri(@"C:\Users\rmari\RiderProjects\AudioPlayer\AudioPlayerFullTest\testMusic\BTS-Butter.mp3")}});
            playList2.musics.Add(new MusicNotifyChanged{musics = new Music{name = "Тест 12", source = new Uri(@"C:\Users\rmari\RiderProjects\AudioPlayer\AudioPlayerFullTest\testMusic\Konfuz-Ратата.mp3")}});
            this.PlayListCollections.Add(playList2);
        }

        private void PlayListPanel_OnSelectedMusic(MusicNotifyChanged obj)
        {
            this.CustomPlayer.StopRandom();
            this.CustomPlayer.PickMusic_PlayList(obj.musics);
        }

        private void CustomPlayer_OnMusicStart(object sender, MusicEventArgs e)
        {
            this.MusicContainer.MusicPlay = this.MusicContainer.playList.musics.First((musicNotify) =>
            {
                return musicNotify.musics.Equals(e.music);
            });
        }

        private void PlayListPanel_OnSelectedPlayList(PlayListCollection obj)
        {
            if (!this.SelectedPlayList.Equals(obj))
            {
                this.SelectingPlauList(obj);
            }
        }

        private void SelectingPlauList(PlayListCollection playList)
        {
            if (this.PlayListCollections.IndexOf(playList)==-1) return;
            
            this.SelectedPlayList = playList;
            this.MusicContainer.playList = playList;
            this.PlayListPanel.playLists = PlayListCollections;
            this.MenuAddPlaylist.playLists = PlayListCollections;
            
            this.CustomPlayer.SetPlayList(new Playlist(playList.musics.ToList().ConvertAll((musicNotify) =>
            {
                return musicNotify.musics;
            })));
        }

        private MusicNotifyChanged selectMusicAdd;
        private void MusicContainer_OnContextMenuClick(MusicNotifyChanged selectMusicAdd)
        {
            this.selectMusicAdd = selectMusicAdd;
            this.MenuAddPlaylist.Visibility = Visibility.Visible;
            this.MenuAddPlaylist.IsEnabled = true;
            this.MainGrid.IsEnabled = false;
            this.MainGrid.Opacity = 0.5;
        }

        private void MenuAddPlaylist_OnClose()
        {
            this.MenuAddPlaylist.Visibility = Visibility.Hidden;
            this.MenuAddPlaylist.IsEnabled = false;
            this.MainGrid.IsEnabled = true;
            this.MainGrid.Opacity = 1;
        }
        
        private void MenuAddPlaylist_OnCreatePlayList(string name)
        {
            int count = this.PlayListCollections.Count(pl => pl.name == name);
            if (count!=0) return;
            
            this.PlayListCollections.Add(new PlayListCollection{name = name, musics = new ObservableCollection<MusicNotifyChanged>()});
            this.MenuAddPlaylist_OnSelectedPlayList(this.PlayListCollections.Last());
        }

        private void MenuAddPlaylist_OnSelectedPlayList(PlayListCollection playList)
        {
            int index = this.PlayListCollections.IndexOf(playList);
            if (index==-1 || this.PlayListCollections[index].musics.IndexOf(selectMusicAdd)!=-1) return;
            
            this.PlayListCollections[index].musics.Add(selectMusicAdd);
        }
    }
}