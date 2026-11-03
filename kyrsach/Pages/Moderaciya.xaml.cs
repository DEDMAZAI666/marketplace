using System.Linq;
using System.Windows;
using System.Windows.Controls;
using kyrsach.Models;
using kyrsach.Windows;

namespace kyrsach.Pages
{
    public partial class Moderaciya : Page
    {
        public Moderaciya()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // 1. Загружаем на модерацию (только для админа)
            if (MainWindow.IsAdmin)
            {
                var pending = DataStore.GetProducts("Pending");
                PendingList.ItemsSource = pending;
            }

            // 2. Загружаем товары для редактирования/удаления
            var allProducts = DataStore.GetProducts(null);

            if (MainWindow.IsAdmin)
            {
                // Админ видит ВСЕ товары
                MyProductsList.ItemsSource = allProducts;
            }
            else
            {
                // Пользователь видит ТОЛЬКО свои товары (где OwnerId = его Id)
                int currentUserId = DataStore.CurrentUser?.Id ?? 0;
                var myProducts = allProducts.Where(p => p.OwnerId == currentUserId).ToList();
                MyProductsList.ItemsSource = myProducts;
            }
        }

        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Product product)
            {
                DataStore.UpdateProductStatus(product.Id, ProductStatus.Approved);
                LoadData();
                (Application.Current.MainWindow as MainWindow)?.RefreshCatalog();
            }
        }

        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Product product)
            {
                DataStore.UpdateProductStatus(product.Id, ProductStatus.Rejected);
                LoadData();
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Product product)
            {
                // Окно редактирования (как в прошлом коде)
                Window editWindow = new Window
                {
                    Title = "Редактировать товар",
                    Width = 350,
                    Height = 420,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = Application.Current.MainWindow,
                    ResizeMode = ResizeMode.NoResize,
                    WindowStyle = WindowStyle.ToolWindow,
                    Background = System.Windows.Media.Brushes.White
                };

                StackPanel panel = new StackPanel { Margin = new Thickness(20) };

                panel.Children.Add(new TextBlock { Text = "Название:", Margin = new Thickness(0, 0, 0, 5) });
                TextBox txtName = new TextBox { Text = product.Name, Height = 30, Margin = new Thickness(0, 0, 0, 15), VerticalContentAlignment = VerticalAlignment.Center };
                panel.Children.Add(txtName);

                panel.Children.Add(new TextBlock { Text = "Цена:", Margin = new Thickness(0, 0, 0, 5) });
                TextBox txtPrice = new TextBox { Text = product.Price.ToString(), Height = 30, Margin = new Thickness(0, 0, 0, 15), VerticalContentAlignment = VerticalAlignment.Center };
                panel.Children.Add(txtPrice);

                panel.Children.Add(new TextBlock { Text = "Ссылка на картинку:", Margin = new Thickness(0, 0, 0, 5) });
                TextBox txtImage = new TextBox { Text = product.ImageUrl, Height = 30, Margin = new Thickness(0, 0, 0, 15), VerticalContentAlignment = VerticalAlignment.Center };
                panel.Children.Add(txtImage);

                panel.Children.Add(new TextBlock { Text = "Категория:", Margin = new Thickness(0, 0, 0, 5) });
                ComboBox cmbCategory = new ComboBox { Height = 30, Margin = new Thickness(0, 0, 0, 20) };
                cmbCategory.Items.Add("Электроника");
                cmbCategory.Items.Add("Одежда");
                cmbCategory.Items.Add("Дом и сад");
                cmbCategory.Items.Add("Игры и консоли");
                cmbCategory.Items.Add("Автотовары");
                cmbCategory.Items.Add("Детские товары");
                cmbCategory.SelectedItem = product.Category;
                if (cmbCategory.SelectedItem == null) cmbCategory.SelectedIndex = 0;
                panel.Children.Add(cmbCategory);

                Button btnSave = new Button
                {
                    Content = "Сохранить изменения",
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

                    product.Name = txtName.Text;
                    product.Price = price;
                    product.Category = cmbCategory.SelectedItem.ToString();
                    product.ImageUrl = string.IsNullOrWhiteSpace(txtImage.Text) ? "https://via.placeholder.com/200" : txtImage.Text;

                    DataStore.UpdateProduct(product);
                    LoadData();
                    (Application.Current.MainWindow as MainWindow)?.RefreshCatalog();

                    editWindow.Close();
                    MessageBox.Show("Товар обновлен!", "Успех");
                };

                panel.Children.Add(btnSave);
                editWindow.Content = panel;
                editWindow.ShowDialog();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Product product)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить товар \"{product.Name}\"?",
                                             "Подтверждение",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    DataStore.DeleteProduct(product.Id);
                    LoadData();
                    (Application.Current.MainWindow as MainWindow)?.RefreshCatalog();
                    MessageBox.Show("Товар удален.", "Успех");
                }
            }
        }
    }
}