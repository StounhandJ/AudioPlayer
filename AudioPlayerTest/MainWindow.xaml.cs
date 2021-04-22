using System;
using System.Collections.Generic;
using System.Windows;
using AudioPlayer;

namespace AudioPlayerTest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string directory = Environment.CurrentDirectory+"\\..\\..\\";
            Playlist list = new Playlist(new List<Music>
            {
                new Music {source = new Uri(directory + "audioTest/Artik.mp3"), name = "Artik 2.0"},
                new Music {source = new Uri(directory + "audioTest/Rasa.mp3"), sourceImg = new Uri("https://avatars.mds.yandex.net/get-zen_doc/1591494/pub_5d05c6ad97d1910df850692d_5d05daefcf474f0da0398c8f/scale_1200")},
                new Music {source = new Uri(directory + "audioTest/MATRANG.mp3")},
                new Music {source = new Uri(directory + "audioTest/Rakhim.mp3"), name = "Dance", sourceImg = new Uri(directory + "imgTest/slon.jpg")},
            });
            CustomPlayer.SetPlayList(list);
            // CustomPlayer.SetMusic(new Music {source = new Uri(directory + "audioTest/Artik.mp3")});
        }
    }
}
