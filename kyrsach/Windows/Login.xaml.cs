using System;
using System.Windows;
using kyrsach.Models;

namespace kyrsach.Windows
{
    public partial class Login : Window
    {
        public string UserPhone { get; private set; }
        public string UserAddress { get; private set; }
        public DateTime UserRegDate { get; private set; }

        public Login()
        {
            InitializeComponent();
        }

        private void Войти(object sender, RoutedEventArgs e)
        {
            string phone = LoginTextBox.Text.Trim();
            string password = PasswordTextBox.Password;

            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Введите логин и пароль!", "Ошибка");
                return;
            }

            var user = DataStore.FindUser(phone, password);

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка");
                return;
            }

            DataStore.CurrentUser = user;
            UserPhone = user.Phone;
            UserAddress = user.Address;
            UserRegDate = user.RegDate;

            DialogResult = true;
            Close();
        }

        private void Регистрация(object sender, RoutedEventArgs e)
        {
            var regWindow = new RegisterWindow { Owner = this };

            if (regWindow.ShowDialog() == true)
            {
                var user = DataStore.CurrentUser;
                UserPhone = user.Phone;
                UserAddress = user.Address;
                UserRegDate = user.RegDate;

                DialogResult = true;
                Close();
            }
        }
    }
}