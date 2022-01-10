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
    /// Interaction logic for StationListWindow.xaml
    /// </summary>
    
    public partial class StationListWindow : Window
    {
        BL.BLApi.Ibl busiAccess;
        public StationListWindow(BL.BLApi.Ibl _busiAccess)
        {
            InitializeComponent();
            busiAccess = _busiAccess;

            //LstViewStation.ItemsSource = busiAccess.GetStationToList();// as IEnumerable<BL.BO.BOStationToList>;
            DataContext = busiAccess.GetStationToList();
            
            ////StatusSelector1.DataContext = Enum.GetValues(typeof(BL.BO.Enum.DroneStatus));
            // StatusSelector1.ItemsSource = Enum.GetValues(typeof(BL.BO.Enum.DroneStatus));
            ////StatusSelector2.ItemsSource = Enum.GetValues(typeof(BL.BO.Enum.WeightCategories));

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DataContext = busiAccess.GetStationToList();
        }

        private void LstViewStation_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            new StationWindow(busiAccess, (LstViewStation.SelectedItem as BL.BO.BOStationToList).Id).ShowDialog();
        }

        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {
            new StationWindow(busiAccess).ShowDialog();
        }
    }
}
