using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace kyrsach.Windows
{
/// <summary>
    /// Логика взаимодействия для MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
public MainWindow()
{
InitializeComponent();
}

private void CloseWindow_Click(object sender, RoutedEventArgs e)
{
Close();
}

private void MaxWindow_Click(object sender, RoutedEventArgs e)
{
if (WindowState == WindowState.Normal)
{
WindowState=WindowState.Maximized;
}
else
{
WindowState = WindowState.Normal;
}
}

private void MinWindow_Click(object sender, RoutedEventArgs e)
{
WindowState=WindowState.Minimized;
}

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void Вход(object sender, RoutedEventArgs e)
        {
            new Login().Show();
            Close();
        }

        private void Избранное(object sender, RoutedEventArgs e)
        {

        }

        private void Корзина(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked1(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked2(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked3(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked4(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked5(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButton_Checked6(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
