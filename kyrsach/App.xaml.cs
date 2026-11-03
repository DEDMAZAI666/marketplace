using System.Windows;

namespace kyrsach
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Проверяем подключение к БД перед запуском
            if (!DataStore.TestConnection())
            {
                MessageBox.Show(
                    "Не удалось подключиться к базе данных!\n\n" +
                    "Проверьте:\n" +
                    "1. Запущен ли сервер MySQL.\n" +
                    "2. Правильно ли указан пароль в DataStore.cs (12345).\n" +
                    "3. Создана ли база данных mango_db.",
                    "Ошибка подключения",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Shutdown(); // Закрываем приложение
            }
        }
    }
}