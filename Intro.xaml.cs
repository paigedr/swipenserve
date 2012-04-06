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

namespace Controls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Intro : UserControl
    {
        public Intro()
        {
            InitializeComponent();
        }

        private void swipeComplete_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
