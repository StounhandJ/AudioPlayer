using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AudioPlayer;
using AudioPlayer.Class;
using AudioPlayerFullTest.Structs;
using Microsoft.Win32;

namespace AudioPlayerFullTest
{
    public partial class MainPage : Page
    {
        public ObservableCollection<PlayListCollection> PlayListCollections;
        
        private string path_MusicDirectory = Directory.GetCurrentDirectory()+"\\musics";

        private PlayListCollection PlayingPlayList;
        
        private PlayListCollection SelectedPlayList;

        public event Action<ObservableCollection<PlayListCollection>> savePlayList;
        public MainPage()
        {
            InitializeComponent();
            this.PlayListCollections = new ObservableCollection<PlayListCollection>();
            PlayListCollection playList = new PlayListCollection();
            string dir = Directory.GetCurrentDirectory() + "\\..\\..\\";
            playList.name = "Основной";
            playList.musics = new ObservableCollection<MusicNotifyChanged>();
            playList.musics.Add(new MusicNotifyChanged{musics = new Music{name = "Тест 1", source = new Uri(dir+"testMusic\\BTS-Butter.mp3")}});
            playList.musics.Add(new MusicNotifyChanged{musics = new Music{name = "Тест 2", source = new Uri(dir+"testMusic\\Konfuz-Ратата.mp3")}});
            playList.musics.Add(new MusicNotifyChanged{musics = new Music{name = "Тест 3", source = new Uri(dir+"testMusic\\РукиВверх-Нокаут.mp3")}});
            this.PlayListCollections.Add(playList);

            this.SelectedPlayList = playList;
            this.SelectingPlayList(playList);
            
            this.CustomPlayer.MuteVolume(true);
            
            PlayListCollection playList2 = new PlayListCollection();
            playList2.name = "Второй";
            playList2.musics = new ObservableCollection<MusicNotifyChanged>();
            playList2.musics.Add(new MusicNotifyChanged{musics = new Music{name = "Тест 11", source = new Uri(dir+"testMusic\\BTS-Butter.mp3")}});
            playList2.musics.Add(new MusicNotifyChanged{musics = new Music{name = "Тест 12", source = new Uri(dir+"testMusic\\Konfuz-Ратата.mp3")}});
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
                this.SelectingPlayList(this.SelectedPlayList);
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
            savePlayList?.Invoke(this.PlayListCollections);
        }

        private void PlayListPanel_OnSelectedPlayList(PlayListCollection obj)
        {
            this.SelectedPlayList = obj;
            this.VisibilitySelectedPlayList();
        }

        private void SelectingPlayList(PlayListCollection playList)
        {
            if (this.PlayListCollections.IndexOf(playList)==-1) return;
            
            this.PlayingPlayList = playList;
            this.PlayListPanel.playLists = PlayListCollections;
            this.MenuAddPlaylist.playLists = PlayListCollections;
            
            this.CustomPlayer.SetPlayList(new Playlist(playList.musics.ToList().ConvertAll((musicNotify) =>
            {
                return musicNotify.musics;
            })));
            
            this.VisibilitySelectedPlayList();
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
        
        private MusicNotifyChanged selectMusicChange;
        private void MusicContainer_OnChangeClick(MusicNotifyChanged music)
        {
            this.selectMusicChange = music;
            this.MenuEditMusic.Visibility = Visibility.Visible;
            this.MenuEditMusic.IsEnabled = true;
            this.MainGrid.IsEnabled = false;
            this.MainGrid.Opacity = 0.5;
            MenuEditMusic.music = music;
        }
        
        private void MusicContainer_OnDeleteClick(MusicNotifyChanged music)
        {
            if (this.SelectedPlayList.musics.Count>1)
            {
                int index = this.PlayListCollections.IndexOf(this.SelectedPlayList);
                this.PlayListCollections[index].musics.Remove(music);
                this.SelectingPlayList(this.SelectedPlayList);
            }
        }
        
        private void PlayListPanel_OnChangeClick(PlayListCollection playList)
        {
            this.MenuEditPlayList.Visibility = Visibility.Visible;
            this.MenuEditPlayList.IsEnabled = true;
            this.MainGrid.IsEnabled = false;
            this.MainGrid.Opacity = 0.5;

            this.MenuEditPlayList.PlayList = this.SelectedPlayList;
        }
        
        private void PlayListPanel_OnDeleteClick(PlayListCollection playList)
        {
            if (this.PlayListCollections.Count>1)
            {
                int index = this.PlayListCollections.IndexOf(playList);
                this.PlayListCollections.Remove(playList);
                this.SelectedPlayList = this.PlayListCollections[index != 0 ? index - 1 : index];
                this.VisibilitySelectedPlayList();
                this.SelectingPlayList(this.SelectedPlayList);
            }
        }
        
        private void MenuEditMusic_OnChangeMusic(string name)
        {
            this.MenuEditMusic.Visibility = Visibility.Hidden;
            this.MenuEditMusic.IsEnabled = false;
            this.MainGrid.IsEnabled = true;
            this.MainGrid.Opacity = 1;
            
            int index = this.PlayListCollections.IndexOf(this.SelectedPlayList);
            int indexMusic = this.PlayListCollections[index].musics.IndexOf(this.selectMusicChange);
            Music music = this.PlayListCollections[index].musics[indexMusic].musics;
            this.PlayListCollections[index].musics[indexMusic].musics = new Music{name = name, sourceImg = music.sourceImg, source = music.source};

            this.SelectingPlayList(this.PlayingPlayList);
        }

        private void MenuEditPlayList_OnChangeMusic(string name)
        {
            this.MenuEditPlayList.Visibility = Visibility.Hidden;
            this.MenuEditPlayList.IsEnabled = false;
            this.MainGrid.IsEnabled = true;
            this.MainGrid.Opacity = 1;
            
            int index = this.PlayListCollections.IndexOf(this.SelectedPlayList);
            this.PlayListCollections[index] = new PlayListCollection{name = name, musics = this.SelectedPlayList.musics};
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