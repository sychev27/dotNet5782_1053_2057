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
    public partial class MainWindow : Window
    {

        BL.BLApi.Ibl busiAccess = BL.BLApi.FactoryBL.GetBL();


        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(busiAccess, 0).ShowDialog();
        }

        private void btnOpenDroneList_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(busiAccess).ShowDialog();
        }

        //private void btnUpdateDrone_Click(object sender, RoutedEventArgs e)
        //{
        //    new DroneWindow(busiAccess, '0').Show();
        //}
    }
}
