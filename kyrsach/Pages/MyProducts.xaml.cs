using System.Linq;
using System.Windows;
using System.Windows.Controls;
using kyrsach.Models;
using kyrsach.Windows;

namespace kyrsach.Pages
{
    public partial class MyProducts : Page
    {
        public MyProducts()
        {
            InitializeComponent();
            LoadMyProducts();
        }

        private void LoadMyProducts()
        {
            if (DataStore.CurrentUser == null) return;

            // Берем все товары, но оставляем только те, где OwnerId == ID текущего пользователя
            var allProducts = DataStore.GetProducts(null);
            var myProducts = allProducts.Where(p => p.OwnerId == DataStore.CurrentUser.Id).ToList();

            MyProductsList.ItemsSource = myProducts;

            if (myProducts.Count == 0)
            {
                MessageBox.Show("Вы пока не добавили ни одного товара.", "Информация");
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Product product)
            {
                // Окно редактирования (точно такое же, как в модерации)
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
                    LoadMyProducts();
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
                    LoadMyProducts();
                    (Application.Current.MainWindow as MainWindow)?.RefreshCatalog();
                    MessageBox.Show("Товар удален.", "Успех");
                }
            }
        }
    }
}