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
        bool modelTBoxChanged = false;
        

        //default constructor is to Add a drone
        public DroneWindow(IBL.Ibl _busiAccess, int num)//to add a Drone
        {
            InitializeComponent();
            busiAccess = _busiAccess;

            cmbWeightChoice.ItemsSource = Enum.GetValues(typeof(IBL.BO.Enum.WeightCategories));

            //(1) Disable irrelevant buttons
            //btnGetDrone.IsEnabled = false;
            btnModifyDroneModel.IsEnabled = false;
            btnSendDroneToCustomer.IsEnabled = false;
            btnFreeDroneFromCharge.IsEnabled = false;
            btnPickupPkg.IsEnabled = false;
            btnSendToCharge.IsEnabled = false;
            btnDeliverPkg.IsEnabled = false;

            //(2) Hide irrelevant buttons
            //btnGetDrone.Visibility = Visibility.Hidden;
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
            //reset text color
            changeTBlockColor(Colors.Black, tBlock_chooseDroneId, tBlock_chooseMaxWeight,
                tBlock_chooseModel, tBlock_chooseStation);
            
            //(1) Receive Data
            int _id;
            bool idSuccess = Int32.TryParse(tBoxIdInput.Text, out _id);
            string _model = tBoxModelInput.Text;
            int _stationId;
            bool stationIdSuccess = Int32.TryParse(tBoxStationIdInput.Text, out _stationId);
            IDAL.DO.WeightCategories? weight = (IDAL.DO.WeightCategories)cmbWeightChoice.SelectedIndex;


            //(2) Check that Data is Valid
            bool validData = true;
            //check Id
            if (tBoxIdInput.Text == null || !idSuccess || _id <= 0)
            {
                tBlock_chooseDroneId.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }
            else if (busiAccess.droneIdExists(_id))
            {
                tBlock_chooseDroneId.Text = "Drone Id is not unique!";
                tBlock_chooseDroneId.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }


            if(tBoxModelInput.Text == null || tBoxModelInput.Text == "")
            {
                tBlock_chooseModel.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }

            if (tBoxStationIdInput.Text == null || !stationIdSuccess || _stationId <= 0)
            {
                tBlock_chooseStation.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }
            if(weight == null || (int)weight == -1)  //check weight categories
            {
                tBlock_chooseMaxWeight.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }



            //(3) Add Drone..
            if (validData)
            {
                busiAccess.addDrone(_id, _model, (IDAL.DO.WeightCategories)weight, _stationId);
                //try
                //{
                //    busiAccess.addDrone(_id, _model, (IDAL.DO.WeightCategories)weight, _stationId);
                //}
                //catch (IDAL.DO.EXItemNotFoundException)
                //{
                 
                //    throw;
                //} 
            }
            else
                return;
            
            MessageBox.Show("Drone Added Successfully","SUCCESS",MessageBoxButton.OK, MessageBoxImage.Information,MessageBoxResult.OK);
            Close();
        }

      













        //TO UPDATE A DRONE...
        
        public DroneWindow(IBL.Ibl _busiAccess, IBL.BO.BODrone _bodrone) //CTOR called by DroneListWindow
        {
            InitializeComponent();
            busiAccess = _busiAccess;

            //edit buttons and text boxes for Update Window:
            tBoxIdInput.IsReadOnly = true;
            tBoxIdInput.BorderBrush = Brushes.Transparent;
            tBoxStationIdInput.IsReadOnly = true;
            tBoxStationIdInput.BorderBrush = Brushes.Transparent;
            cmbWeightChoice.ItemsSource = Enum.GetValues(typeof(IBL.BO.Enum.WeightCategories));

            btnAddDrone.IsEnabled = false;
            btnAddDrone.Visibility = Visibility.Hidden;

            displayBODrone(_bodrone);
          

        }




        private void displayBODrone(IBL.BO.BODrone bodrone)
        {
            thisDroneId = bodrone.Id;


            tBoxIdInput.Text = bodrone.Id.ToString();
            tBoxModelInput.Text = bodrone.Model;
            if (busiAccess.getStationIdOfBODrone(bodrone.Id) != -1)
                tBoxStationIdInput.Text = (busiAccess.getStationIdOfBODrone(bodrone.Id)).ToString();
            else
                tBoxStationIdInput.Text = "Drone is not charging at a Station";

            cmbWeightChoice.SelectedIndex = (int)bodrone.MaxWeight;
            cmbWeightChoice.IsReadOnly = true;
            cmbWeightChoice.IsEnabled = false;

            tBlockStatusInfo.Text = bodrone.DroneStatus.ToString();
            if (bodrone.ParcelInTransfer.Id == -1)
                tBlockDeliveryInfo.Text = "Not yet carrying Parcel";
            else 
                tBlockDeliveryInfo.Text = bodrone.ParcelInTransfer.ToString();
            tBlockLongInfo.Text = bodrone.Location.Longitude.ToString();
            tBlockLatinfo.Text = bodrone.Location.Latitude.ToString();

            tBlockCurrentLocation.Text = "working on this....";
            //working on a function in BL..

        }

        private void changeTBlockColor(Color color, params TextBlock[] listTBlock)
        {
            foreach (var item in listTBlock)
            {
                item.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        
        //private bool modelTextBoxChanged()
        //{

        //}

















        private void btnModifyDroneModel_Click(object sender, RoutedEventArgs e)
        {
            int id;
            Int32.TryParse(tBoxIdInput.Text, out id);
            busiAccess.modifyDrone(id, tBoxModelInput.Text);
           
        }

        private void btnSendToCharge_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.chargeDrone(thisDroneId);
            displayBODrone(busiAccess.getBODrone(thisDroneId));
        }

        private void btnFreeDroneFromCharge_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.freeDrone(thisDroneId, 0);
            displayBODrone(busiAccess.getBODrone(thisDroneId));
        }

        private void btnPickupPkg_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.PickupParcel(thisDroneId);
            displayBODrone(busiAccess.getBODrone(thisDroneId));
        }

        private void btnSendDroneToCustomer_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.assignParcel(thisDroneId);
            displayBODrone(busiAccess.getBODrone(thisDroneId));
        }

        private void btnDeliverPkg_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.deliverParcel(thisDroneId);
            displayBODrone(busiAccess.getBODrone(thisDroneId));
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }








        //public DroneWindow(IBL.Ibl _busiAccess, char let) //Update Drone CTOR, 
        //    //called from Main Window Only!
        //{
        //    //(1) Disable irrelevant buttons
        //    InitializeComponent();
        //    busiAccess = _busiAccess;

        //    btnAddDrone.IsEnabled = false;
        //    btnAddDrone.Visibility = Visibility.Hidden;

        //    tBoxModelInput.IsEnabled = true;
        //    tBoxStationIdInput.IsEnabled = false;


        //    btnModifyDroneModel.IsEnabled = false;
        //    btnSendDroneToCustomer.IsEnabled = false;
        //    btnFreeDroneFromCharge.IsEnabled = false;
        //    btnPickupPkg.IsEnabled = false;
        //    btnSendToCharge.IsEnabled = false;
        //    btnDeliverPkg.IsEnabled = false;

        //    //(2) Display this drone


        //}


        //private void btnGetDrone_Click(object sender, RoutedEventArgs e)
        //{
        //    int droneId;
        //    Int32.TryParse(tBoxIdInput.Text, out droneId);
        //    //alex write code here, if ID wasnt typed correctly. exception
        //    //and if boDrone doesnt exists
        //    //create an error msg

        //    //AFTER THROWING EXCEPTIONS:

        //    tBlock_chooseDroneId.Text = "Drone ID: ";
        //    tBoxIdInput.Text = droneId.ToString();
        //    tBoxIdInput.IsEnabled = false;
        //    thisDroneId = droneId;

        //    if (busiAccess.getStationIdOfBODrone(droneId) != -1)
        //        tBoxStationIdInput.Text = (busiAccess.getStationIdOfBODrone(droneId)).ToString();
        //    else
        //        tBoxStationIdInput.Text = "Drone is not charging at a Station";

        //    tBlock_chooseModel.Text = "Model";
        //    tBoxModelInput.Text = busiAccess.getBODroneModel(droneId);
        //    tBoxModelInput.IsEnabled = true;

        //    tBlock_chooseMaxWeight.Text = "Max Weight: \n" + busiAccess.getBoDroneMaxWeight(droneId);


        //    //enable modifying buttons:
        //    btnModifyDroneModel.IsEnabled = true;
        //    btnFreeDroneFromCharge.IsEnabled = true;
        //    btnSendDroneToCustomer.IsEnabled = true;
        //    btnPickupPkg.IsEnabled = true;
        //    btnSendToCharge.IsEnabled = true;
        //    btnDeliverPkg.IsEnabled = true;


        //}

    }
}
