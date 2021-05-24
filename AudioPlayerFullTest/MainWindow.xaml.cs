using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using AudioPlayer;
using AudioPlayerFullTest.Structs;

namespace AudioPlayerFullTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            PlayListCollection playList = new PlayListCollection();
            playList.musics = new ObservableCollection<MusicNotifyChanged>();
            playList.musics.Add(new MusicNotifyChanged{musics = new Music{name = "Тест 1", source = new Uri(@"C:\Users\rmari\RiderProjects\AudioPlayer\AudioPlayerFullTest\testMusic\BTS-Butter.mp3")}});
            playList.musics.Add(new MusicNotifyChanged{musics = new Music{name = "Тест 2", source = new Uri(@"C:\Users\rmari\RiderProjects\AudioPlayer\AudioPlayerFullTest\testMusic\Konfuz-Ратата.mp3")}});
            playList.musics.Add(new MusicNotifyChanged{musics = new Music{name = "Тест 3", source = new Uri(@"C:\Users\rmari\RiderProjects\AudioPlayer\AudioPlayerFullTest\testMusic\РукиВверх-Нокаут.mp3")}});
            this.PlayListPanel.playList = playList;
            this.CustomPlayer.SetPlayList(new Playlist(playList.musics.ToList().ConvertAll((musicNotify) =>
            {
                return musicNotify.musics;
            })));
            this.CustomPlayer.SetVolume(0);
        }

        private void PlayListPanel_OnSelectedMusic(MusicNotifyChanged obj)
        {
            this.CustomPlayer.StopRandom();
            this.CustomPlayer.PickMusic_PlayList(obj.musics);
        }

        private void CustomPlayer_OnMusicStart(object sender, MusicEventArgs e)
        {
            this.PlayListPanel.MusicPlay = this.PlayListPanel.playList.musics.First((musicNotify) =>
            {
                return musicNotify.musics.Equals(e.music);
            });
        }
    }
}