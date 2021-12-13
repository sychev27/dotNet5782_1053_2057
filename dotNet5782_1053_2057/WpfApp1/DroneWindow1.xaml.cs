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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL.Ibl busiAccess;

        int thisDroneId;

        const string btnUpdateText = "Update this Drone";
        
        //default constructor is to Add a drone
        public DroneWindow(IBL.Ibl _busiAccess, int num)//to add a Drone
        {
            InitializeComponent();
            busiAccess = _busiAccess;

            cmbWeightChoice.ItemsSource = Enum.GetValues(typeof(IBL.BO.Enum.WeightCategories));

            //(1) Disable irrelevant buttons
            btnGetDrone.IsEnabled = false;
            btnModifyDroneModel.IsEnabled = false;
            btnSendDroneToCustomer.IsEnabled = false;
            btnFreeDroneFromCharge.IsEnabled = false;
            btnPickupPkg.IsEnabled = false;
            btnSendToCharge.IsEnabled = false;
            btnDeliverPkg.IsEnabled = false;

            //(2) Hide irrelevant buttons
            btnGetDrone.Visibility = Visibility.Hidden;
            btnModifyDroneModel.Visibility = Visibility.Hidden;
            btnSendDroneToCustomer.Visibility = Visibility.Hidden;
            btnFreeDroneFromCharge.Visibility = Visibility.Hidden;
            btnPickupPkg.Visibility = Visibility.Hidden;
            btnSendToCharge.Visibility = Visibility.Hidden;
            btnDeliverPkg.Visibility = Visibility.Hidden;

            //(3) Hide irrelevnat TextBlocks
            tBlockStatus.Visibility = Visibility.Hidden;
            tBlockStatusInfo.Visibility = Visibility.Hidden;
            tBlockDelivery.Visibility = Visibility.Hidden;
            tBlockDeliveryInfo.Visibility = Visibility.Hidden;
            tBlockCurrentLocation.Visibility = Visibility.Hidden;
            tBlockCurrentLocationInfo.Visibility = Visibility.Hidden;
            tBlockLongitude.Visibility = Visibility.Hidden;
            tBlockLongInfo.Visibility = Visibility.Hidden;
            tBlockLatitude.Visibility = Visibility.Hidden;
            tBlockLatinfo.Visibility = Visibility.Hidden;

        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            int _id;
            Int32.TryParse(tBoxIdInput.Text, out _id);
            string _model = tBoxModelInput.Text;
            int _stationId;
            Int32.TryParse(tBoxStationIdInput.Text, out _stationId);
            IDAL.DO.WeightCategories? weight = (IDAL.DO.WeightCategories)cmbWeightChoice.SelectedIndex;

            //if(weight == null)
            //    throw excepeiton!!

            //alex write code here. exception


            busiAccess.addDrone(_id, _model, (IDAL.DO.WeightCategories)weight, _stationId);
            
            
            MessageBox.Show("Drone Added Successfully","SUCCESS",MessageBoxButton.OK, MessageBoxImage.Information,MessageBoxResult.OK);
            Close();
        }

      
















        //TO UPDATE A DRONE...
        public DroneWindow(IBL.Ibl _busiAccess, char let) //Update Drone CTOR
        {
            //(1) Redesign text boxes and buttons
            InitializeComponent();
            busiAccess = _busiAccess;
            btnAddDrone.IsEnabled = false;
            btnModifyDroneModel.IsEnabled = false;


            tBlock_chooseMaxWeight.Text = "Max Weight: ";
            tBlock_chooseStation.Text = "Station Id:";

            tBoxModelInput.IsEnabled = false;
            tBoxStationIdInput.IsEnabled = false;


            btnModifyDroneModel.IsEnabled = false;
            btnSendDroneToCustomer.IsEnabled = false;
            btnFreeDroneFromCharge.IsEnabled = false;
            btnPickupPkg.IsEnabled = false;
            btnSendToCharge.IsEnabled = false;
            btnDeliverPkg.IsEnabled = false;


        }
        
        public DroneWindow(IBL.Ibl _busiAccess, IBL.BO.BODrone _drone) //CTOR called by DroneListWindow
        {
            //int droneId;
            //Int32.TryParse(tBoxIdInput.Text, out droneId);
            //alex write code here, if ID wasnt typed correctly. exception
            //and if boDrone doesnt exists
            //create an error msg

            //AFTER THROWING EXCEPTIONS:
            InitializeComponent();
            busiAccess = _busiAccess;

            tBlock_chooseDroneId.Text = "Drone ID: ";
            tBoxIdInput.Text = _drone.Id.ToString();
            tBoxIdInput.IsEnabled = false;
            thisDroneId = _drone.Id;

            if (busiAccess.getStationIdOfBODrone(_drone.Id) != -1)
                tBoxStationIdInput.Text = (busiAccess.getStationIdOfBODrone(_drone.Id)).ToString();
            else
                tBoxStationIdInput.Text = "Drone is not charging at a Station";

            tBlock_chooseModel.Text = "Model";
            tBoxModelInput.Text = busiAccess.getBODroneModel(_drone.Id);
            tBoxModelInput.IsEnabled = true;

            tBlock_chooseMaxWeight.Text = "Max Weight: \n" + busiAccess.getBoDroneMaxWeight(_drone.Id);

           
            //enable modifying buttons:
            btnModifyDroneModel.IsEnabled = true;
            btnFreeDroneFromCharge.IsEnabled = true;
            btnSendDroneToCustomer.IsEnabled = true;
            btnPickupPkg.IsEnabled = true;
            btnSendToCharge.IsEnabled = true;
            btnDeliverPkg.IsEnabled = true;


        }

        private void btnGetDrone_Click(object sender, RoutedEventArgs e)
        {
            int droneId;
            Int32.TryParse(tBoxIdInput.Text, out droneId);
            //alex write code here, if ID wasnt typed correctly. exception
            //and if boDrone doesnt exists
            //create an error msg

            //AFTER THROWING EXCEPTIONS:

            tBlock_chooseDroneId.Text = "Drone ID: ";
            tBoxIdInput.Text = droneId.ToString();
            tBoxIdInput.IsEnabled = false;
            thisDroneId = droneId;

            if (busiAccess.getStationIdOfBODrone(droneId) != -1)
                tBoxStationIdInput.Text = (busiAccess.getStationIdOfBODrone(droneId)).ToString();
            else
                tBoxStationIdInput.Text = "Drone is not charging at a Station";

            tBlock_chooseModel.Text = "Model";
            tBoxModelInput.Text = busiAccess.getBODroneModel(droneId);
            tBoxModelInput.IsEnabled = true;

            tBlock_chooseMaxWeight.Text = "Max Weight: \n" + busiAccess.getBoDroneMaxWeight(droneId);

           
            //enable modifying buttons:
            btnModifyDroneModel.IsEnabled = true;
            btnFreeDroneFromCharge.IsEnabled = true;
            btnSendDroneToCustomer.IsEnabled = true;
            btnPickupPkg.IsEnabled = true;
            btnSendToCharge.IsEnabled = true;
            btnDeliverPkg.IsEnabled = true;


        }

        private void btnModifyDroneModel_Click(object sender, RoutedEventArgs e)
        {
            int id;
            Int32.TryParse(tBoxIdInput.Text, out id);
            busiAccess.modifyDrone(id, tBoxModelInput.Text);
           
        }

        private void btnSendToCharge_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.chargeDrone(thisDroneId);
        }

        private void btnFreeDroneFromCharge_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.freeDrone(thisDroneId, 0);

        }

        private void btnPickupPkg_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.PickupParcel(thisDroneId);

        }

        private void btnSendDroneToCustomer_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.assignParcel(thisDroneId);

        }

        private void btnDeliverPkg_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.deliverParcel(thisDroneId);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


    }
}
