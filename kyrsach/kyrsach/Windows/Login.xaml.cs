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
using System.Windows.Shapes;

namespace kyrsach.Windows
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
           
        }
      

        private void Войти(object sender, RoutedEventArgs e)
        {
            // 1. Берем текст, который ввел пользователь
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Password;

            // 2. Проверяем (пока для теста пишем жестко admin/123)
            // Позже мы заменим это на запрос к базе данных
            if (login == "admin" && password == "123")
            {
                // Если пароль верный:
                MessageBox.Show("Добро пожаловать!");

                // Создаем и открываем главное окно магазина
                MainWindow main = new MainWindow();
                main.Show();

                // Закрываем текущее окно авторизации
                this.Close();
            }
            else
            {
                // Если пароль неверный:
                MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Регистрация(object sender, RoutedEventArgs e)
        {
            // 1. Создаем окно регистрации
            RegisterWindow regWindow = new RegisterWindow();

            // 2. Показываем его пользователю
            regWindow.Show();

            // 3. Закрываем текущее окно авторизации
            this.Close();
        }
    }
}
