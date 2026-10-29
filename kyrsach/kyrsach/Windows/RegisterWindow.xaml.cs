using kyrsach.Windows;
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

namespace kyrsach
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterAction_Click(object sender, RoutedEventArgs e)
        {
            string login = RegLoginBox.Text;
            string pass1 = RegPasswordBox.Password;
            string pass2 = RegPasswordBox2.Password;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(pass1))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (pass1 != pass2)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }

            // --- БУДУЩЕМ БУДЕТ СОХРАНЕНИЕ В БАЗУ ДАННЫХ ---
            MessageBox.Show("Аккаунт успешно создан! Войдите в систему.");

            // 1.Открываем окно логина заново
            Login loginWindow = new Login();
            loginWindow.Show();

            // 2.Закрываем текущее окно регистрации
            this.Close();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Если ни одного окна не осталось (логин и регистрация закрыты) - выходим
            if (Application.Current.Windows.Count == 0)
            {
                Application.Current.Shutdown();
            }
        }
    }
}

