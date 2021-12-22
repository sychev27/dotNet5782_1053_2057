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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        BL.BLApi.Ibl busiAccess = BL.BLApi.FactoryBL.GetBL();
        public LoginWindow()
        {
            InitializeComponent();

        }

        private void btnOpenMain_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(busiAccess).Show();
            Close();
        }
    }
}
