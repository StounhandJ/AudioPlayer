using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
        private AuthPage authPage;

        private MainPage mainPage;

        private Profile? profile;
        public MainWindow()
        {
            InitializeComponent();
            this.authPage = new AuthPage();
            this.authPage.EndAuth += EndAuth;
            this.mainPage = new MainPage();
            this.mainPage.savePlayList += savePlayList;
            MainContent.Content = this.authPage;
        }

        private void EndAuth(Profile? profile)
        {
            this.profile = profile;
            MainContent.Content = this.mainPage;
            if (this.profile.HasValue)
            {
                this.mainPage.AD = false;
                this.mainPage.setPlayListsCollection(this.profile?.playLists);
            }
            else
            {
                this.mainPage.AD = true;
                this.mainPage.setPlayListsCollection(this.authPage.CreateStandartPlayList());
            }
        }
        
        private void savePlayList(ObservableCollection<PlayListCollection> playListCollections)
        {
            if (this.profile.HasValue)
            {
                List<Profile> profiles = ManagementSave.loadProfilesJSON();
                int index = profiles.FindIndex(p=>p.name==this.profile?.name && p.password==this.profile?.password);
                if (index == -1) return;
                profiles[index] = new Profile(this.profile?.name, this.profile?.password, playListCollections);
                ManagementSave.saveProfilesJSON(profiles);
            }
        }
    }
}