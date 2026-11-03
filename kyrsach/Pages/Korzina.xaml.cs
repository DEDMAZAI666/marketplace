using System.Linq;
using System.Windows;
using System.Windows.Controls;
using kyrsach.Models;
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
            // Проверка на null для безопасности
            if (MainWindow.Cart != null)
            {
                CartItemsList.ItemsSource = MainWindow.Cart;
                UpdateTotal();
            }
        }

        private void UpdateTotal()
        {
            if (MainWindow.Cart != null)
            {
                decimal total = MainWindow.Cart.Sum(p => p.Price);
                TotalText.Text = $"Итого: {total:N0} ₽";
            }
        }

        private void RemoveFromCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Product product)
            {
                MainWindow.Cart.Remove(product);
                UpdateTotal();
            }
        }

        // Оформление заказа — переход на страницу оплаты через NavigationService
        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.Cart != null && MainWindow.Cart.Count > 0)
            {
                // Используем NavigationService самой страницы — это самый надежный способ
                if (this.NavigationService != null)
                {
                    this.NavigationService.Navigate(new Oplata());
                }
                else
                {
                    MessageBox.Show("Ошибка навигации: служба навигации недоступна.", "Ошибка");
                }
            }
            else
            {
                MessageBox.Show("Корзина пуста.", "Ошибка");
            }
        }

        // ВОЗВРАТ НАЗАД В КАТАЛОГ
        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null && this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
            else if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new Tovar());
            }
        }
    }
}