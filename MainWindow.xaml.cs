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

namespace ClipboardRing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Hooker hooker = new Hooker();

        public MainWindow()
        {
            InitializeComponent();
            Strings.DataContext = hooker;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            hooker.Subscribe();
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            hooker.Unsubscribe();
        }
    }
}
