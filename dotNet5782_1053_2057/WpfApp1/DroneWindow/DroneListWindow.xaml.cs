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
using System.ComponentModel;
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
        public ICollectionView droneCollectionView { get; set; }

        //this field holds the different windows of each drone,
        //to ensure that we do not open 2 windows of the same drone
        //btList<DroneWindow> possibleWindows = new List<DroneWindow>(); 
        ObservableCollection<BL.BO.BODrone> droneList;


        public DroneListWindow(BL.BLApi.Ibl busiAccess1) 
        {
            busiAccess = busiAccess1;
            //droneList = busiAccess.GetBODroneList() as ObservableCollection<BL.BO.BODrone>;
            InitializeComponent();
            refreshList();

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
                HelpfulMethods.ErrorMsg("Drone is deleted"); 
            }
            refreshList();  
        }

        private void refreshList()
        {
            droneList = getBODronesAsObservable();
            DataContext = droneList; 
            //DataContext = busiAccess.GetBODroneList(true); 
            droneCollectionView = (CollectionView)CollectionViewSource.
                GetDefaultView(DataContext);
            DronesListView.ItemsSource = droneCollectionView;
            //droneCollectionView.Refresh();
            if ((bool)!chkBoxGetErased.IsChecked)
                droneCollectionView.Filter = filterOutErased;
            else
                droneCollectionView.Filter = null;

            droneCollectionView.SortDescriptions.Add(new SortDescription
                (nameof(BL.BO.BOParcelToList.Id), ListSortDirection.Ascending));
        }
        private bool filterOutErased(object obj)
        {
            if (obj is BL.BO.BODrone item)
            {
                return item.Exists;
            }
            else return false;
        }


        private void chkBoxGetErased_Checked(object sender, RoutedEventArgs e)
        {
            refreshList();
        }
        private void chkBoxGetErased_UnChecked(object sender, RoutedEventArgs e)
        {
            refreshList();
        }

        private ObservableCollection<BL.BO.BODrone> getBODronesAsObservable()
        {
            ObservableCollection<BL.BO.BODrone> res = new ObservableCollection<BL.BO.BODrone>();
            foreach (var item in busiAccess.GetBODroneList(true))
            {
                res.Add(item);
            }
            return res;
        }
        //end of window
    }
}
