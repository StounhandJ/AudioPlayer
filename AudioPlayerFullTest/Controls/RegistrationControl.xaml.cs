using System;
using System.Windows;
using System.Windows.Controls;

namespace AudioPlayerFullTest.Controls
{
    public partial class RegistrationControl : UserControl
    {
        public event Action Login; 
        
        public event Action<string, string> Registration; 
        
        public RegistrationControl()
        {
            InitializeComponent();
        }
        
        public void showError(string text)
        {
            ErrorTextBlock.Text = text;
        }
        
        private void Login_OnClick(object sender, RoutedEventArgs e)
        {
            Login?.Invoke();
        }
        
        private void Registration_OnClick(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox.Text==PasswordReplayTextBox.Text)
            {
                Registration?.Invoke(LoginTextBox.Text, PasswordTextBox.Text);
            }
            else
            {
                showError("Пароли не совпадают");
            }
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            LoginButton.IsEnabled = LoginTextBox.Text != "" && PasswordTextBox.Text != "" && PasswordReplayTextBox.Text != "";
        }
    }
}