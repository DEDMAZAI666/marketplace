using System.Linq;
using System.Windows;
using System.Windows.Controls;
using kyrsach.Windows;

namespace kyrsach.Pages
{
    public partial class Korzina : Page
    {
        public Korzina()
        {
            InitializeComponent();
            LoadCartItems();
        }

        private void LoadCartItems()
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                // Обращаемся к статической коллекции MainWindow
                CartItemsList.ItemsSource = MainWindow.Cart;
                UpdateTotal();
            }
        }

        private void UpdateTotal()
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                decimal total = MainWindow.Cart.Sum(p => p.Price);
                TotalText.Text = $"Итого: {total:N0} ₽";
            }
        }

        // Удаление товара из корзины
        private void RemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Product product)
            {
                MainWindow.Cart.Remove(product);
                UpdateTotal();
            }
        }

        // Оформление заказа
        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Cart.Count > 0)
            {
                MessageBox.Show("Заказ успешно оформлен! Спасибо за покупку.", "Успех");
                MainWindow.Cart.Clear();
                UpdateTotal();
            }
            else
            {
                MessageBox.Show("Корзина пуста.", "Ошибка");
            }
        }

        // ВОЗВРАТ НАЗАД В КАТАЛОГ
        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                // Если есть история навигации в Frame — идем назад
                if (mainWindow.MainFrame.CanGoBack)
                {
                    mainWindow.MainFrame.GoBack();
                }
                // Иначе принудительно открываем страницу каталога
                else
                {
                    mainWindow.MainFrame.Navigate(new Tovar());
                }
            }
        }
    }
}