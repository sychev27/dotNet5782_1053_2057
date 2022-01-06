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
        
        //private ObservableCollection<BL.BO.BODrone> droneList = getBODronesAsObservable();
        

        public DroneListWindow(BL.BLApi.Ibl busiAccess1) 
        {
            busiAccess = busiAccess1;
            //droneList = busiAccess.GetBODroneList() as ObservableCollection<BL.BO.BODrone>;
             
            DataContext = busiAccess.GetBODroneList();
            InitializeComponent();



            //DronesListView.ItemsSource = busiAccess.GetBODroneList();
            StatusSelector1.DataContext = Enum.GetValues(typeof(BL.BO.Enum.DroneStatus));
           // StatusSelector1.ItemsSource = Enum.GetValues(typeof(BL.BO.Enum.DroneStatus));
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
            new DroneWindow(busiAccess).ShowDialog();
            //keep previous filters
            //int index = StatusSelector1.SelectedIndex;
            //DronesListView.ItemsSource = busiAccess.GetSpecificDroneListStatus(index);
            //index = StatusSelector2.SelectedIndex;
            //DronesListView.ItemsSource = busiAccess.GetSpecificDroneListWeight(index);

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DronesListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BL.BO.BODrone drone = DronesListView.SelectedItem as BL.BO.BODrone;
            //check if drone is erased:
            if (drone.Exists)
                new DroneWindow(busiAccess, drone).ShowDialog();
            else
            {
                MainWindow.ErrorMsg("Drone is deleted"); 
                //add function to allow user to Restore drone!
            }
            
              
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            refreshList();
         }

        private void refreshList(bool getDeleted = false)
        {
            //DronesListView.ItemsSource = null;
            DronesListView.ItemsSource = busiAccess.GetBODroneList(getDeleted);

        }

        private void chkBoxGetErased_Checked(object sender, RoutedEventArgs e)
        {
            //DataContext = busiAccess.GetBODroneList(true);
            DronesListView.ItemsSource = busiAccess.GetBODroneList(true);

        }
        private void chkBoxGetErased_UnChecked(object sender, RoutedEventArgs e)
        {
            //DataContext = busiAccess.GetBODroneList();
            DataContext = busiAccess.GetBODroneList();
        }

        private ObservableCollection<BL.BO.BODrone> getBODronesAsObservable()
        {
            ObservableCollection<BL.BO.BODrone> res = new ObservableCollection<BL.BO.BODrone>();
            foreach (var item in busiAccess.GetBODroneList())
            {
                res.Add(item);
            }
            return res;
        }





       








        //end of window
    }
}
