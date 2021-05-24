using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using AudioPlayer;
using AudioPlayer.Class;
using AudioPlayerFullTest.Structs;
using Microsoft.Win32;

namespace AudioPlayerFullTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private ObservableCollection<PlayListCollection> PlayListCollections;
        
        private string path_MusicDirectory = Directory.GetCurrentDirectory()+"\\musics";

        private PlayListCollection PlayingPlayList;
        
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

            this.SelectedPlayList = playList;
            this.SelectingPlauList(playList);
            
            this.CustomPlayer.MuteVolume(true);
            
            PlayListCollection playList2 = new PlayListCollection();
            playList2.name = "Второй";
            playList2.musics = new ObservableCollection<MusicNotifyChanged>();
            playList2.musics.Add(new MusicNotifyChanged{musics = new Music{name = "Тест 11", source = new Uri(@"C:\Users\rmari\RiderProjects\AudioPlayer\AudioPlayerFullTest\testMusic\BTS-Butter.mp3")}});
            playList2.musics.Add(new MusicNotifyChanged{musics = new Music{name = "Тест 12", source = new Uri(@"C:\Users\rmari\RiderProjects\AudioPlayer\AudioPlayerFullTest\testMusic\Konfuz-Ратата.mp3")}});
            this.PlayListCollections.Add(playList2);
        }

        private void PlayListPanel_OnSelectedMusic(MusicNotifyChanged obj)
        {
            if (this.SelectedPlayList.Equals(this.PlayingPlayList))
            {
                this.CustomPlayer.StopRandom();
                this.CustomPlayer.PickMusic_PlayList(obj.musics);
            }
            else
            {
                this.SelectingPlauList(this.SelectedPlayList);
                this.CustomPlayer.StopRandom();
                this.CustomPlayer.PickMusic_PlayList(obj.musics);
            }
            
        }

        private void CustomPlayer_OnMusicStart(object sender, MusicEventArgs e)
        {
            if (!this.SelectedPlayList.Equals(this.PlayingPlayList)) return;
            
            this.MusicContainer.MusicPlay = this.MusicContainer.playList.musics.First((musicNotify) =>
            {
                return musicNotify.musics.Equals(e.music);
            });
        }

        private void PlayListPanel_OnSelectedPlayList(PlayListCollection obj)
        {
            this.SelectedPlayList = obj;
            this.VisibilitySelectedPlayList();
        }

        private void SelectingPlauList(PlayListCollection playList)
        {
            if (this.PlayListCollections.IndexOf(playList)==-1) return;
            
            this.PlayingPlayList = playList;
            this.PlayListPanel.playLists = PlayListCollections;
            this.MenuAddPlaylist.playLists = PlayListCollections;
            
            this.CustomPlayer.SetPlayList(new Playlist(playList.musics.ToList().ConvertAll((musicNotify) =>
            {
                return musicNotify.musics;
            })));
            
            VisibilitySelectedPlayList();
        }

        private void VisibilitySelectedPlayList()
        {
            if (this.PlayListCollections.IndexOf(this.SelectedPlayList)==-1) return;
            
            this.MusicContainer.playList = this.SelectedPlayList;
            this.MusicContainer.MusicPlay = this.MusicContainer.playList.musics.FirstOrDefault((musicNotify) =>
            {
                return musicNotify.musics.Equals(CustomPlayer.getNowMusic());
            });
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

        private void ButtonAddMusic_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Music|*.mp3";
            if (dlg.ShowDialog() == true)
            {
                uploadMusic(dlg.FileName);
            }
        }
        
        private void uploadMusic(string path)
        {
            int time = (Int32) (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            Directory.CreateDirectory(path_MusicDirectory+"\\"+time);
                    
            string fileName = path.Split('\\').Last();
            string filePath = path_MusicDirectory + "\\" + time + "\\" + fileName;
            File.Copy(path,filePath);

            int index = this.PlayListCollections.IndexOf(this.SelectedPlayList);
            this.PlayListCollections[index].musics.Add(new MusicNotifyChanged{musics = new Music{name = fileName, source = new Uri(filePath)}});
        }

        private void FileDrop(object sender, DragEventArgs e)
        {
            string[] filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            foreach (string filename in filenames)
            {
                uploadMusic(filename);
            }
        }
        
        private void MusicContainer_DragOver(object sender, DragEventArgs e)
        { 
            bool dropEnabled = true;
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            { 
                string[] filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
                foreach (string filename in filenames)
                {
                    if (System.IO.Path.GetExtension(filename).ToUpperInvariant() != ".mp3")
                    {
                        dropEnabled = false; break;
                    }
                } 
            }
            else
            { 
                dropEnabled = false;
            }

            if (!dropEnabled)
            {
                e.Effects = DragDropEffects.None; 
                e.Handled = true;
            }            
        }
    }
}