using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
            FG.SetPlayList(new Playlist(new List<Music>
            {
                new Music{source = "D:\\AllProject\\Ch\\Audio-player\\src\\Artik.mp3"},
                new Music{source = "D:\\AllProject\\Ch\\Audio-player\\src\\Rasa.mp3"},
            }));
        }
    }
}
