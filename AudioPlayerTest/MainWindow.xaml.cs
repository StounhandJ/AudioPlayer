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
            FG.SetPlayList(new Playlist(new List<Music>
            {
                new Music{source = directory+"audioTest/Artik.mp3"},
                new Music{source = directory+"audioTest/Rasa.mp3"},
                new Music{source = directory+"audioTest/MATRANG.mp3"},
                new Music{source = directory+"audioTest/Rakhim.mp3"},
            }));
        }
    }
}
