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
        IBL.Ibl busiAccess;
        
        public DroneListWindow(IBL.Ibl busiAccess1)
        {
            InitializeComponent();
            busiAccess = busiAccess1;
            DronesListView.ItemsSource = busiAccess.getBODroneList();
            StatusSelector1.ItemsSource = Enum.GetValues(typeof(IBL.BO.Enum.DroneStatus));
            StatusSelector2.ItemsSource = Enum.GetValues(typeof(IBL.BO.Enum.WeightCategories));

        }

        private void StatusSelector1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = StatusSelector1.SelectedIndex;
            DronesListView.ItemsSource = busiAccess.getSpecificDroneListStatus(index);
        }

        private void StatusSelector2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = StatusSelector2.SelectedIndex;
            DronesListView.ItemsSource = busiAccess.getSpecificDroneListWeight(index);
        }

       private void btnAddDrone1_Click(object sender, RoutedEventArgs e)
       {
           new DroneWindow(busiAccess,0).Show();
           //DronesListView.ItemsSource = busiAccess.getBODroneList();
        }

    }
}
