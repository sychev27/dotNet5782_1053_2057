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
    /// Interaction logic for DroneList.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        BL.Ibl busiAccess;
        public DroneListWindow(BL.Ibl busiAccess1)
        {
            InitializeComponent();
            busiAccess = busiAccess1;

            DronesListView.ItemsSource = busiAccess.GetBODroneList();
            StatusSelector1.ItemsSource = Enum.GetValues(typeof(BL.BO.Enum.DroneStatus));
            StatusSelector2.ItemsSource = Enum.GetValues(typeof(BL.BO.Enum.WeightCategories));

        }

        private void StatusSelector1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = StatusSelector1.SelectedIndex;
            DronesListView.ItemsSource = busiAccess.GetSpecificDroneListStatus(index);
        }

        private void StatusSelector2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = StatusSelector2.SelectedIndex;
            DronesListView.ItemsSource = busiAccess.GetSpecificDroneListWeight(index);
        }

       private void btnAddDrone1_Click(object sender, RoutedEventArgs e)
       {
            new DroneWindow(busiAccess, 0).ShowDialog();
            //refresh the info...
            DronesListView.ItemsSource = null;
            DronesListView.ItemsSource = busiAccess.GetBODroneList();


        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BL.BO.BODrone drone = DronesListView.SelectedItem as BL.BO.BODrone;
            new DroneWindow(busiAccess, drone).Show();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            DronesListView.ItemsSource = null;
            DronesListView.ItemsSource = busiAccess.GetBODroneList();
        }
    }
}
