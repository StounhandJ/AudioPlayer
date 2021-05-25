using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AudioPlayerFullTest.Structs;

namespace AudioPlayerFullTest
{
    public partial class AuthPage : Page
    {
        public List<Profile> profiles;

        public event Action<Profile?> EndAuth;
        public AuthPage()
        {
            InitializeComponent();
            this.profiles = ManagementSave.loadProfilesJSON();
        }

        private void LoginControl_OnRegistration()
        {
            this.LoginControl.Visibility = Visibility.Hidden;
            this.LoginControl.IsEnabled = false;

            this.RegistrationControl.Visibility = Visibility.Visible;
            this.RegistrationControl.IsEnabled = true;
        }

        private void RegistrationControl_OnLogin()
        {
            this.LoginControl.Visibility = Visibility.Visible;
            this.LoginControl.IsEnabled = true;

            this.RegistrationControl.Visibility = Visibility.Hidden;
            this.RegistrationControl.IsEnabled = false;
        }

        private void RegistrationControl_OnRegistration(string login, string password)
        {
            int index = this.profiles.FindIndex(p => p.name==login);
            if (index==-1)
            {
                this.profiles.Add(new Profile{name = login, password = password, playLists = new ObservableCollection<PlayListCollection>()});
                this.EndAuth?.Invoke(this.profiles.Last());
                ManagementSave.saveProfilesJSON(this.profiles);
            }
            else
            {
                RegistrationControl.showError("Данный пользователь уже существует");
            }
        }
        
        private void LoginControl_OnLogin(string login, string password)
        {
            int index = this.profiles.FindIndex(p => p.name==login && p.password==password);
            if (index!=-1)
            {
                EndAuth?.Invoke(this.profiles[index]);
            }
            else
            {
                LoginControl.showError("Неправильные данные");
            }
        }

        private void LoginControl_OnSkip()
        {
            EndAuth?.Invoke(null);
        }
    }
}