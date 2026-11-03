using System.Windows;
using System.Windows.Controls;
using kyrsach.Models;
using kyrsach.Windows;

namespace kyrsach.Pages
{
    public partial class Izbranoe : Page
    {
        public Izbranoe()
        {
            InitializeComponent();
            LoadFavorites();
        }

        private void LoadFavorites()
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                // Привязываем список к статической коллекции Favorites
                FavoritesList.ItemsSource = MainWindow.Favorites;
            }
        }

        // Удаление из избранного
        private void RemoveFromFavorites_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Product product)
            {
                MainWindow.Favorites.Remove(product);
            }
        }

        // Возврат в каталог
        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                if (mainWindow.MainFrame.CanGoBack)
                {
                    mainWindow.MainFrame.GoBack();
                }
                else
                {
                    mainWindow.MainFrame.Navigate(new Tovar());
                }
            }
        }
    }
}