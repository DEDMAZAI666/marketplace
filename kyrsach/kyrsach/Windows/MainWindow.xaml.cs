using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime;
using System.Windows;
using System.Windows.Controls;
using kyrsach.Pages;

namespace kyrsach.Windows
{
    public class Product
    {
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
    }

    public partial class MainWindow : Window
    {
        // Общие коллекции
        public static ObservableCollection<Product> AllProducts { get; set; } = new ObservableCollection<Product>();
        public static ObservableCollection<Product> Cart { get; set; } = new ObservableCollection<Product>();
        public static ObservableCollection<Product> Favorites { get; set; } = new ObservableCollection<Product>();

        private string _currentCategory = "Все";
        private double _maxPrice = 100000;
        private string _searchQuery = "";

        private Tovar _tovarPage;

        public MainWindow()
        {
            InitializeComponent();
            LoadTestData();

            // Создаем страницу Tovar и загружаем в Frame
            _tovarPage = new Tovar();
            MainFrame.Navigate(_tovarPage);

            // Применяем фильтры
            ApplyFilter();
        }

        private void LoadTestData()
        {
            AllProducts.Add(new Product { Name = "Смартфон", Category = "Электроника", Price = 25000, ImageUrl = "https://via.placeholder.com/200" });
            AllProducts.Add(new Product { Name = "Ноутбук", Category = "Электроника", Price = 60000, ImageUrl = "https://via.placeholder.com/200" });
            AllProducts.Add(new Product { Name = "Футболка", Category = "Одежда", Price = 1500, ImageUrl = "https://via.placeholder.com/200" });
            AllProducts.Add(new Product { Name = "Диван", Category = "Дом и сад", Price = 45000, ImageUrl = "https://via.placeholder.com/200" });
            AllProducts.Add(new Product { Name = "Кукла", Category = "Детские товары", Price = 2500, ImageUrl = "https://via.placeholder.com/200" });
            AllProducts.Add(new Product { Name = "Шины", Category = "Автотовары", Price = 8000, ImageUrl = "https://via.placeholder.com/200" });
        }

        // ==========================================
        // ФИЛЬТРАЦИЯ + ПОИСК
        // ==========================================
        private void ApplyFilter()
        {
            var filtered = AllProducts.Where(p =>
                (_currentCategory == "Все" || p.Category == _currentCategory) &&
                (double)p.Price <= _maxPrice &&
                (string.IsNullOrEmpty(_searchQuery) || p.Name.ToLower().Contains(_searchQuery.ToLower()))
            ).ToList();

            if (_tovarPage != null)
            {
                // Если открыта не страница Tovar (например, корзина или избранное) — возвращаемся на неё
                if (MainFrame.Content != _tovarPage)
                {
                    MainFrame.Navigate(_tovarPage);
                }

                _tovarPage.UpdateProducts(filtered);
            }
        }

        // Поиск (TextChanged)
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Скрываем/показываем placeholder
            if (SearchPlaceholder != null)
            {
                SearchPlaceholder.Visibility = string.IsNullOrWhiteSpace(SearchTextBox.Text)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }

            _searchQuery = SearchTextBox.Text;
            ApplyFilter();
        }

        private void Электроника(object sender, RoutedEventArgs e) { _currentCategory = "Электроника"; ApplyFilter(); }
        private void Одежда(object sender, RoutedEventArgs e) { _currentCategory = "Одежда"; ApplyFilter(); }
        private void Дом(object sender, RoutedEventArgs e) { _currentCategory = "Дом и сад"; ApplyFilter(); }
        private void Детские(object sender, RoutedEventArgs e) { _currentCategory = "Детские товары"; ApplyFilter(); }
        private void Игры(object sender, RoutedEventArgs e) { _currentCategory = "Игры и консоли"; ApplyFilter(); }
        private void Авто(object sender, RoutedEventArgs e) { _currentCategory = "Автотовары"; ApplyFilter(); }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _maxPrice = e.NewValue;
            ApplyFilter();
        }

        // ==========================================
        // СБРОС ФИЛЬТРОВ
        // ==========================================
        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            _currentCategory = "Все";
            _maxPrice = 100000;
            _searchQuery = "";

            if (PriceSlider != null)
                PriceSlider.Value = 100000;

            if (SearchTextBox != null)
                SearchTextBox.Text = "";

            ApplyFilter();
        }

        // ==========================================
        // ДОБАВЛЕНИЕ ТОВАРА
        // ==========================================
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            Window inputWindow = new Window
            {
                Title = "Новый товар",
                Width = 350,
                Height = 420,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize,
                WindowStyle = WindowStyle.ToolWindow,
                Background = System.Windows.Media.Brushes.White
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(20) };

            panel.Children.Add(new TextBlock { Text = "Название:", Margin = new Thickness(0, 0, 0, 5) });
            TextBox txtName = new TextBox { Height = 30, Margin = new Thickness(0, 0, 0, 15), Padding = new Thickness(5, 0, 0, 0), VerticalContentAlignment = VerticalAlignment.Center };
            panel.Children.Add(txtName);

            panel.Children.Add(new TextBlock { Text = "Цена:", Margin = new Thickness(0, 0, 0, 5) });
            TextBox txtPrice = new TextBox { Height = 30, Margin = new Thickness(0, 0, 0, 15), Padding = new Thickness(5, 0, 0, 0), VerticalContentAlignment = VerticalAlignment.Center };
            panel.Children.Add(txtPrice);

            panel.Children.Add(new TextBlock { Text = "Ссылка на картинку:", Margin = new Thickness(0, 0, 0, 5) });
            TextBox txtImage = new TextBox { Height = 30, Margin = new Thickness(0, 0, 0, 15), Text = "https://via.placeholder.com/200", Padding = new Thickness(5, 0, 0, 0), VerticalContentAlignment = VerticalAlignment.Center };
            panel.Children.Add(txtImage);

            panel.Children.Add(new TextBlock { Text = "Категория:", Margin = new Thickness(0, 0, 0, 5) });
            ComboBox cmbCategory = new ComboBox { Height = 30, Margin = new Thickness(0, 0, 0, 20) };
            cmbCategory.Items.Add("Электроника"); cmbCategory.Items.Add("Одежда"); cmbCategory.Items.Add("Дом и сад");
            cmbCategory.Items.Add("Детские товары"); cmbCategory.Items.Add("Игры и консоли"); cmbCategory.Items.Add("Автотовары");
            cmbCategory.SelectedIndex = 0;
            panel.Children.Add(cmbCategory);

            Button btnSave = new Button { Content = "Сохранить", Height = 40, Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#0078D7"), Foreground = System.Windows.Media.Brushes.White, BorderThickness = new Thickness(0), Cursor = System.Windows.Input.Cursors.Hand };

            btnSave.Click += (s, args) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) || !decimal.TryParse(txtPrice.Text, out decimal price))
                {
                    MessageBox.Show("Заполните название и корректную цену!", "Ошибка");
                    return;
                }

                AllProducts.Add(new Product
                {
                    Name = txtName.Text,
                    Price = price,
                    Category = cmbCategory.SelectedItem.ToString(),
                    ImageUrl = string.IsNullOrWhiteSpace(txtImage.Text) ? "https://via.placeholder.com/200" : txtImage.Text
                });

                ApplyFilter();
                inputWindow.Close();
            };

            panel.Children.Add(btnSave);
            inputWindow.Content = panel;
            inputWindow.ShowDialog();
        }

        // ==========================================
        // ПЕРЕХОДЫ НА СТРАНИЦЫ
        // ==========================================
        private void ShowCart_Click(object sender, RoutedEventArgs e)
        {
            // Открываем страницу корзины
            MainFrame.Navigate(new kyrsach.Pages.Korzina());
        }

        private void ShowFavorites_Click(object sender, RoutedEventArgs e)
        {
            // Открываем страницу избранного
            MainFrame.Navigate(new kyrsach.Pages.Izbranoe());
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new Login();
            loginWindow.Owner = this;
            this.Hide();    

            if (loginWindow.ShowDialog() == true)
            {
                this.Show();
                MessageBox.Show("Вы успешно вошли!", "Успех");
               
            }
            else
            {
                this.Show();
                Log.Visibility = Visibility.Hidden;
                Text1.Visibility = Visibility.Hidden;
                Text2.Visibility = Visibility.Hidden;
            }
        }

        // ==========================================
        // УПРАВЛЕНИЕ ОКНОМ
        // ==========================================
        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void MaximizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();

    }
}