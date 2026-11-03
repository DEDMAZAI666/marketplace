using System.Linq;
using System.Windows;
using System.Windows.Controls;
using kyrsach.Models;

namespace kyrsach.Pages
{
    public partial class History : Page
    {
        public History()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders()
        {
            if (DataStore.CurrentUser == null)
            {
                MessageBox.Show("Войдите в аккаунт, чтобы увидеть историю заказов.", "Внимание");
                return;
            }

            // Берем заказы ТОЛЬКО текущего пользователя
            var userOrders = DataStore.GetUserOrders(DataStore.CurrentUser.Id);

            // Делаем нумерацию строго с 1 (Заказ №1, Заказ №2...)
            int displayIndex = 1;
            foreach (var order in userOrders)
            {
                order.Id = displayIndex++;
            }

            OrdersList.ItemsSource = userOrders;

            if (userOrders.Count == 0)
            {
                MessageBox.Show("У вас пока нет заказов.", "Информация");
            }
        }
    }
}