using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using kyrsach.Windows; // Для доступа к MainWindow и Product

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

        // Обработчик кнопки "В корзину"
        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Product product)
            {
                MainWindow.Cart.Add(product);
                MessageBox.Show($"Товар '{product.Name}' добавлен в корзину!", "Успех");
            }
        }

        // Обработчик кнопки "В избранное" (сердечко)
        private void AddToFavorites_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Product product)
            {
                if (!MainWindow.Favorites.Contains(product))
                {
                    // Добавляем и красим в красный
                    MainWindow.Favorites.Add(product);
                    btn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E81123"));
                }
                else
                {
                    // Удаляем и красим в серый
                    MainWindow.Favorites.Remove(product);
                    btn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCCCCC"));
                }
            }
        }

        // Событие загрузки кнопки (срабатывает при появлении карточки на экране)
        // Проверяет, есть ли товар в избранном, и восстанавливает цвет сердечка
        private void FavoriteButton_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Product product)
            {
                if (MainWindow.Favorites.Contains(product))
                {
                    btn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E81123")); // Красный
                }
                else
                {
                    btn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCCCCC")); // Серый
                }
            }
        }
    }
}