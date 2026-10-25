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

namespace kyrsach
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

private void Button_Click(object sender, RoutedEventArgs e)
{

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
}
}
