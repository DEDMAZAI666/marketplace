using System;
using System.Windows;
using kyrsach.Models;

namespace kyrsach.Windows
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string phone = txtPhone.Text.Trim();
            string password = txtPassword.Password;
            string address = txtAddress.Text.Trim();

            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Заполните логин и пароль!", "Ошибка");
                return;
            }

            // Проверяем, есть ли уже такой пользователь в БД
            if (DataStore.FindUser(phone, password) != null)
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Ошибка");
                return;
            }

            var newUser = new User
            {
                Phone = phone,
                Password = password,
                Address = string.IsNullOrWhiteSpace(address) ? "Адрес не указан" : address,
                RegDate = DateTime.Now,
                Role = "User"
            };

            // Сохраняем в БД
            DataStore.AddUser(newUser);

            // Автоматически входим под новым пользователем
            var savedUser = DataStore.FindUser(phone, password);
            DataStore.CurrentUser = savedUser;

            MessageBox.Show("Регистрация успешна!", "Успех");
            DialogResult = true; // Закрываем окно с успехом
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}