using System;
using System.Windows;
using System.Windows.Controls;

namespace AudioPlayerFullTest.Controls
{
    public partial class LoginControl : UserControl
    {
        public event Action<string, string> Login; 
        
        public event Action Registration; 
        
        public event Action Skip;
        
        public LoginControl()
        {
            InitializeComponent();
        }

        public void showError(string text)
        {
            ErrorTextBlock.Text = text;
        }

        private void Login_OnClick(object sender, RoutedEventArgs e)
        {
            Login?.Invoke(LoginTextBox.Text, PasswordTextBox.Text);
        }
        
        private void Registration_OnClick(object sender, RoutedEventArgs e)
        {
            Registration?.Invoke();
        }
        
        private void Skip_OnClick(object sender, RoutedEventArgs e)
        {
            Skip?.Invoke();
        }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            LoginButton.IsEnabled = LoginTextBox.Text != "" && PasswordTextBox.Text != "" && LoginTextBox.Text.Length>5 && PasswordTextBox.Text.Length>5 ;
        }
    }
}