using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InteractivePrototype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Intro_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void swipeComplete_Click(object sender, RoutedEventArgs e)
        {
            intro.Visibility = System.Windows.Visibility.Hidden;
            menu.Visibility = System.Windows.Visibility.Visible;
        }

        private void Menu_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
