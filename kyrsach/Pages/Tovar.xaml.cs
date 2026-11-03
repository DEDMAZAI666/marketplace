using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using kyrsach.Models;
using kyrsach.Windows; 

namespace kyrsach.Pages
{
    public partial class Tovar : Page
    {
        public Tovar()
        {
            InitializeComponent();
        }

        // Метод для обновления списка карточек (вызывается из MainWindow)
        public void UpdateProducts(IEnumerable products)
        {
            ProductsList.ItemsSource = products;
        }

        // Обработчик кнопки «В корзину»
        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.IsUserLoggedIn)
            {
                MessageBox.Show(
                    "Сначала войдите в аккаунт.",
                    "Требуется вход",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            if (sender is Button btn && btn.CommandParameter is Product product)
            {
                MainWindow.Cart.Add(product);

                MessageBox.Show(
                    $"Товар '{product.Name}' добавлен в корзину!",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        // Обработчик кнопки «В избранное»
        private void AddToFavorites_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Product product)
            {
                if (!MainWindow.Favorites.Contains(product))
                {
                    MainWindow.Favorites.Add(product);
                    btn.Foreground = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#E81123"));
                }
                else
                {
                    MainWindow.Favorites.Remove(product);
                    btn.Foreground = new SolidColorBrush(
                        (Color)ColorConverter.ConvertFromString("#CCCCCC"));
                }
            }
        }

        // Восстанавливает цвет сердечка при отображении карточки
        private void FavoriteButton_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Product product)
            {
                string color = MainWindow.Favorites.Contains(product)
                    ? "#E81123"
                    : "#CCCCCC";

                btn.Foreground = new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString(color));
            }
        }
    }
}