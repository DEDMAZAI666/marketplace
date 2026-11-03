using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using kyrsach.Models;
using kyrsach.Windows;

namespace kyrsach.Pages
{
    public partial class Oplata : Page
    {
        public Oplata()
        {
            InitializeComponent();

            if (MainWindow.Cart != null)
            {
                decimal total = MainWindow.Cart.Sum(p => p.Price);
                TotalText.Text = $"Сумма к оплате: {total:N0} ₽";
            }
        }

        private void Payment_Checked(object sender, RoutedEventArgs e)
        {
            if (CardPanel == null || SberPanel == null || SBPPanel == null) return;

            CardPanel.Visibility = CardRadio.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            SberPanel.Visibility = SberRadio.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;
            SBPPanel.Visibility = SBPRadio.IsChecked == true ? Visibility.Visible : Visibility.Collapsed;

            if (SelectedMethodText != null)
            {
                if (CardRadio.IsChecked == true) SelectedMethodText.Text = "Выбрано: банковская карта";
                else if (SberRadio.IsChecked == true) SelectedMethodText.Text = "Выбрано: SberPay";
                else if (SBPRadio.IsChecked == true) SelectedMethodText.Text = "Выбрано: СБП";
            }

            ResetErrors();
        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Cart == null || MainWindow.Cart.Count == 0)
            {
                MessageBox.Show("Корзина пуста.", "Ошибка");
                return;
            }

            ResetErrors();
            bool isValid = true;

            // --- ВАЛИДАЦИЯ ---
            if (CardRadio.IsChecked == true)
            {
                string cardNumber = Regex.Replace(CardNumberTextBox.Text, @"\D", "");
                if (cardNumber.Length < 16)
                {
                    CardNumberTextBox.Style = (Style)FindResource("ErrorTextBox");
                    CardNumberError.Visibility = Visibility.Visible;
                    isValid = false;
                }

                if (!Regex.IsMatch(CardExpiryTextBox.Text, @"^(0[1-9]|1[0-2])\/\d{2}$"))
                {
                    CardExpiryTextBox.Style = (Style)FindResource("ErrorTextBox");
                    CardExpiryError.Visibility = Visibility.Visible;
                    isValid = false;
                }

                if (CardCvvBox.Password.Length < 3)
                {
                    CardCvvBox.Style = (Style)FindResource("ErrorPasswordBox");
                    CardCvvError.Visibility = Visibility.Visible;
                    isValid = false;
                }
            }
            else if (SberRadio.IsChecked == true)
            {
                string phone = Regex.Replace(SberPhoneTextBox.Text, @"\D", "");
                if (phone.Length < 11)
                {
                    SberPhoneTextBox.Style = (Style)FindResource("ErrorTextBox");
                    SberPhoneError.Visibility = Visibility.Visible;
                    isValid = false;
                }
            }
            else if (SBPRadio.IsChecked == true)
            {
                string phone = Regex.Replace(SBPPhoneTextBox.Text, @"\D", "");
                if (phone.Length < 11)
                {
                    SBPPhoneTextBox.Style = (Style)FindResource("ErrorTextBox");
                    SBPPhoneError.Visibility = Visibility.Visible;
                    isValid = false;
                }
            }

            if (!isValid)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля корректно.", "Внимание");
                return;
            }

            // --- УСПЕШНАЯ ОПЛАТА ---
            MessageBox.Show("Оплата прошла успешно! Спасибо за покупку.", "Успех");

            // 1. СОХРАНЯЕМ ЗАКАЗ В DATASTORE
            int userId = DataStore.CurrentUser?.Id ?? 1;
            decimal total = MainWindow.Cart.Sum(p => p.Price);
            DataStore.CreateOrder(userId, total, MainWindow.Cart.ToList());

            // 2. ОЧИЩАЕМ КОРЗИНУ
            MainWindow.Cart.Clear();

            // 3. ВОЗВРАЩАЕМСЯ НА ГЛАВНУЮ
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.GoHome();
            }
        }

        // --- СБРОС ОШИБОК ПРИ ВВОДЕ ---
        private void CardNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CardNumberTextBox.Style = null;
            CardNumberError.Visibility = Visibility.Collapsed;
        }

        private void CardExpiryTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CardExpiryTextBox.Style = null;
            CardExpiryError.Visibility = Visibility.Collapsed;
        }

        private void CardCvvBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            CardCvvBox.Style = null;
            CardCvvError.Visibility = Visibility.Collapsed;
        }

        private void SberPhoneTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SberPhoneTextBox.Style = null;
            SberPhoneError.Visibility = Visibility.Collapsed;
        }

        private void SBPPhoneTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SBPPhoneTextBox.Style = null;
            SBPPhoneError.Visibility = Visibility.Collapsed;
        }

        private void ResetErrors()
        {
            CardNumberTextBox.Style = null;
            CardNumberError.Visibility = Visibility.Collapsed;

            CardExpiryTextBox.Style = null;
            CardExpiryError.Visibility = Visibility.Collapsed;

            CardCvvBox.Style = null;
            CardCvvError.Visibility = Visibility.Collapsed;

            SberPhoneTextBox.Style = null;
            SberPhoneError.Visibility = Visibility.Collapsed;

            SBPPhoneTextBox.Style = null;
            SBPPhoneError.Visibility = Visibility.Collapsed;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null && this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}