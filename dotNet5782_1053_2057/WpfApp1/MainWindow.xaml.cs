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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

        BL.BLApi.Ibl busiAccess;// = BL.BLApi.FactoryBL.GetBL();


        public MainWindow(BL.BLApi.Ibl _busiAccess)
        {
            busiAccess = _busiAccess;
            InitializeComponent();
        }

        public static void ErrorMsg(string msg)
        {
            MessageBox.Show(msg, "Error",
                   MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
        }
        public static void ChangeTextColor(Color color, params TextBlock[] listTBlock)
        {
            foreach (var item in listTBlock)
            {
                item.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        public static void ChangeVisibilty(System.Windows.Visibility vis, params TextBlock[] listTBlock )
        {
            foreach (var item in listTBlock)
            {
                item.Visibility = vis;
            }
        }
        public static void ChangeVisibilty(System.Windows.Visibility vis, params Button[] listButton)
        {
            foreach (var item in listButton)
            {
                item.Visibility = vis;
            }
        }





        private void btnOpenDroneList_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(busiAccess).ShowDialog();
        }

        private void btnCustomerLists_Click(object sender, RoutedEventArgs e)
        {
            new CustomerListWindow(busiAccess).ShowDialog();
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow(busiAccess).Show();
            Close();
        }

        private void btnParcelLists_Click(object sender, RoutedEventArgs e)
        {
            new ParcelListWindow(busiAccess).ShowDialog();
        }

        private void btnStationLists_Click(object sender, RoutedEventArgs e)
        {
            new StationListWindow(busiAccess).Show();
            Close();
        }
    }
}
