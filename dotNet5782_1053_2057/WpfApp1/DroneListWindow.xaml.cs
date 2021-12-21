using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        BL.BLApi.Ibl busiAccess;
        private ObservableCollection<BL.BO.BODrone> droneList = new ObservableCollection<BL.BO.BODrone>();
        //btnAddCustomer.IsEnabled = false;
        // btnAddCustomer.Visibility = Visibility.Hidden;
        public DroneListWindow(BL.BLApi.Ibl busiAccess1) 
        {
            InitializeComponent();
            busiAccess = busiAccess1;
            droneList = busiAccess.GetBODroneList() as ObservableCollection<BL.BO.BODrone>;
            DataContext = droneList;
            //DronesListView.ItemsSource = busiAccess.GetBODroneList();
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
            int index = StatusSelector1.SelectedIndex;
            DronesListView.ItemsSource = busiAccess.GetSpecificDroneListStatus(index);
            index = StatusSelector2.SelectedIndex;
            DronesListView.ItemsSource = busiAccess.GetSpecificDroneListWeight(index);
            //refreshList();

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BL.BO.BODrone drone = DronesListView.SelectedItem as BL.BO.BODrone;
            new DroneWindow(busiAccess, drone).ShowDialog();
            refreshList();
            
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            refreshList();
         }

        private void refreshList()
        {
            DronesListView.ItemsSource = null;
            DronesListView.ItemsSource = busiAccess.GetBODroneList();

        }

    }
}
