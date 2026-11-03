using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using kyrsach.Models;
using kyrsach.Pages;

namespace kyrsach.Windows
{
    public partial class MainWindow : Window
    {
        public static ObservableCollection<Product> AllProducts { get; set; } = new ObservableCollection<Product>();
        public static ObservableCollection<Product> Cart { get; set; } = new ObservableCollection<Product>();
        public static ObservableCollection<Product> Favorites { get; set; } = new ObservableCollection<Product>();

        public static bool IsUserLoggedIn => DataStore.CurrentUser != null;
        public static bool IsAdmin => DataStore.CurrentUser?.IsAdmin ?? false;

        private string _currentCategory = "Все";
        private double _maxPrice = 100000;
        private string _searchQuery = "";

        private Tovar _tovarPage;

        public MainWindow()
        {
            InitializeComponent();

            _tovarPage = new Tovar();
            MainFrame.Navigate(_tovarPage);

            Cart.CollectionChanged += Cart_CollectionChanged;
            UpdateCartBadge();

            LoadProductsFromStore();
            UpdateAdminStatus();
            ApplyFilter();
        }

        private bool CheckAuth()
        {
            if (!IsUserLoggedIn)
            {
                MessageBox.Show("Сначала войдите в аккаунт.", "Доступ запрещен", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        public void LoadProductsFromStore()
        {
            AllProducts.Clear();
            var approved = DataStore.GetProducts("Approved");
            foreach (var p in approved)
                AllProducts.Add(p);
        }

        public void UpdateAdminStatus()
        {
            if (AdminModerationButton != null)
                AdminModerationButton.Visibility = IsAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        private void Cart_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => UpdateCartBadge();

        private void UpdateCartBadge()
        {
            if (CartBadge == null || CartBadgeText == null) return;
            int count = Cart.Count;
            CartBadgeText.Text = count.ToString();
            CartBadge.Visibility = count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        public void GoHome()
        {
            _currentCategory = "Все";
            _maxPrice = 100000;
            _searchQuery = "";

            if (PriceSlider != null) PriceSlider.Value = 100000;
            if (SearchTextBox != null) SearchTextBox.Text = "";
            if (SearchPlaceholder != null) SearchPlaceholder.Visibility = Visibility.Visible;

            ApplyFilter();
            MainFrame.Navigate(_tovarPage);
        }

        public void RefreshCatalog()
        {
            LoadProductsFromStore();
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            var filtered = AllProducts.Where(p =>
                p.Status == ProductStatus.Approved &&
                (_currentCategory == "Все" || p.Category == _currentCategory) &&
                (double)p.Price <= _maxPrice &&
                (string.IsNullOrEmpty(_searchQuery) || p.Name.ToLower().Contains(_searchQuery.ToLower()))
            ).ToList();

            if (_tovarPage != null)
            {
                if (MainFrame.Content != _tovarPage) MainFrame.Navigate(_tovarPage);
                _tovarPage.UpdateProducts(filtered);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchPlaceholder != null)
                SearchPlaceholder.Visibility = string.IsNullOrWhiteSpace(SearchTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
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

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            _currentCategory = "Все";
            _maxPrice = 100000;
            _searchQuery = "";
            if (PriceSlider != null) PriceSlider.Value = 100000;
            if (SearchTextBox != null) SearchTextBox.Text = "";
            ApplyFilter();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAuth()) return;

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
            TextBox txtName = new TextBox { Height = 30, Margin = new Thickness(0, 0, 0, 15), VerticalContentAlignment = VerticalAlignment.Center };
            panel.Children.Add(txtName);

            panel.Children.Add(new TextBlock { Text = "Цена:", Margin = new Thickness(0, 0, 0, 5) });
            TextBox txtPrice = new TextBox { Height = 30, Margin = new Thickness(0, 0, 0, 15), VerticalContentAlignment = VerticalAlignment.Center };
            panel.Children.Add(txtPrice);

            panel.Children.Add(new TextBlock { Text = "Ссылка на картинку:", Margin = new Thickness(0, 0, 0, 5) });
            TextBox txtImage = new TextBox { Height = 30, Margin = new Thickness(0, 0, 0, 15), Text = "https://via.placeholder.com/200", VerticalContentAlignment = VerticalAlignment.Center };
            panel.Children.Add(txtImage);

            panel.Children.Add(new TextBlock { Text = "Категория:", Margin = new Thickness(0, 0, 0, 5) });
            ComboBox cmbCategory = new ComboBox { Height = 30, Margin = new Thickness(0, 0, 0, 20) };
            cmbCategory.Items.Add("Электроника");
            cmbCategory.Items.Add("Одежда");
            cmbCategory.Items.Add("Дом и сад");
            cmbCategory.Items.Add("Игры и консоли");
            cmbCategory.Items.Add("Автотовары");
            cmbCategory.Items.Add("Детские товары");
            cmbCategory.SelectedIndex = 0;
            panel.Children.Add(cmbCategory);

            Button btnSave = new Button
            {
                Content = "Сохранить",
                Height = 40,
                Background = System.Windows.Media.Brushes.DodgerBlue,
                Foreground = System.Windows.Media.Brushes.White,
                BorderThickness = new Thickness(0),
                Cursor = System.Windows.Input.Cursors.Hand
            };

            btnSave.Click += (s, args) =>
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) || !decimal.TryParse(txtPrice.Text, out decimal price))
                {
                    MessageBox.Show("Заполните название и корректную цену!", "Ошибка");
                    return;
                }

                var product = new Product
                {
                    Name = txtName.Text,
                    Price = price,
                    Category = cmbCategory.SelectedItem.ToString(),
                    ImageUrl = string.IsNullOrWhiteSpace(txtImage.Text) ? "https://via.placeholder.com/200" : txtImage.Text,
                    Status = IsAdmin ? ProductStatus.Approved : ProductStatus.Pending,
                    OwnerId = DataStore.CurrentUser?.Id ?? 0
                };

                DataStore.AddProduct(product);
                LoadProductsFromStore();
                ApplyFilter();
                inputWindow.Close();

                MessageBox.Show(IsAdmin ? "Товар добавлен в каталог!" : "Товар отправлен на модерацию!", "Успех");
            };

            panel.Children.Add(btnSave);
            inputWindow.Content = panel;
            inputWindow.ShowDialog();
        }

        private void ShowCart_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAuth()) return;
            MainFrame.Navigate(new Korzina());
        }

        private void ShowFavorites_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAuth()) return;
            MainFrame.Navigate(new Izbranoe());
        }

        private void ShowModeration_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAuth()) return;
            if (!IsAdmin) { MessageBox.Show("Доступ только для администратора!", "Доступ запрещен"); return; }
            MainFrame.Navigate(new Moderaciya());
        }

        private void ShowHistory_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAuth()) return;
            MainFrame.Navigate(new History());
        }

        private void ShowMyProducts_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckAuth()) return;
            MainFrame.Navigate(new MyProducts());
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new Login { Owner = this };
            Hide();

            if (loginWindow.ShowDialog() == true)
            {
                Show();
                UpdateAdminStatus();
                ShowProfile(loginWindow.UserPhone, loginWindow.UserAddress, loginWindow.UserRegDate);
                GoHome();
                MessageBox.Show(IsAdmin ? "Вы вошли как АДМИНИСТРАТОР." : "Вы успешно вошли!", "Успех");
            }
            else
            {
                DataStore.CurrentUser = null;
                UpdateAdminStatus();
                Show();

                ProfilePanel.Visibility = Visibility.Collapsed;
                LogoutButton.Visibility = Visibility.Collapsed;
                Log.Visibility = Visibility.Visible;
                Text1.Visibility = Visibility.Visible;
                Text2.Visibility = Visibility.Visible;

                GoHome();
            }
        }

        public void ShowProfile(string phone, string address, DateTime regDate)
        {
            ProfilePanel.Visibility = Visibility.Visible;
            LogoutButton.Visibility = Visibility.Visible;
            Log.Visibility = Visibility.Collapsed;

            ProfilePhone.Text = string.IsNullOrWhiteSpace(phone) ? "Телефон не указан" : phone;
            ProfileAddress.Text = string.IsNullOrWhiteSpace(address) ? "Адрес не указан" : address;
            ProfileRegDate.Text = regDate.ToString("dd.MM.yyyy HH:mm");

            ProfileRole.Text = IsAdmin ? "Администратор" : "Покупатель";
            ProfileRole.Foreground = IsAdmin ? System.Windows.Media.Brushes.Crimson : System.Windows.Media.Brushes.DodgerBlue;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            DataStore.CurrentUser = null;
            UpdateAdminStatus();

            ProfilePanel.Visibility = Visibility.Collapsed;
            LogoutButton.Visibility = Visibility.Collapsed;
            Log.Visibility = Visibility.Visible;
            Text1.Visibility = Visibility.Visible;
            Text2.Visibility = Visibility.Visible;

            GoHome();
            MessageBox.Show("Вы вышли из аккаунта.", "Информация");
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void MaximizeButton_Click(object sender, RoutedEventArgs e) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
    }
}