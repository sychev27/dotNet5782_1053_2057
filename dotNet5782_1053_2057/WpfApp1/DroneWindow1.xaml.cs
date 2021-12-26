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
        BL.BLApi.Ibl busiAccess;
        int thisDroneId;
        bool modelTBoxChanged = false;
        

        //default constructor is to Add a drone
        public DroneWindow(BL.BLApi.Ibl _busiAccess, int num)//to add a Drone
        {
            InitializeComponent();
            busiAccess = _busiAccess;

            cmbWeightChoice.ItemsSource = Enum.GetValues(typeof(BL.BO.Enum.WeightCategories));

            //(1) Disable irrelevant buttons
            //btnGetDrone.IsEnabled = false;
            btnModifyDroneModel.IsEnabled = false;
            btnAssignDroneToParcel.IsEnabled = false;
            btnFreeDroneFromCharge.IsEnabled = false;
            btnPickupPkg.IsEnabled = false;
            btnSendToCharge.IsEnabled = false;
            btnDeliverPkg.IsEnabled = false;
            btnEraseDrone.IsEnabled = false;

            //(2) Hide irrelevant buttons
            //btnGetDrone.Visibility = Visibility.Hidden;
            btnModifyDroneModel.Visibility = Visibility.Hidden;
            btnAssignDroneToParcel.Visibility = Visibility.Hidden;
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
            tBlockBattery.Visibility = Visibility.Hidden;
            tBlockBatteryInfo.Visibility = Visibility.Hidden;

        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            //reset text color
            MainWindow.ChangeTextColor(Colors.Black, tBlock_chooseDroneId, tBlock_chooseMaxWeight,
                tBlock_chooseModel, tBlock_chooseStation);
            
            //(1) Receive Data
            int _id;
            bool idSuccess = Int32.TryParse(tBoxIdInput.Text, out _id);
            string _model = tBoxModelInput.Text;
            int _stationId;
            bool stationIdSuccess = Int32.TryParse(tBoxStationIdInput.Text, out _stationId);
            DalXml.DO.WeightCategories? weight = (DalXml.DO.WeightCategories)cmbWeightChoice.SelectedIndex;


            //(2) Check that Data is Valid
            bool validData = true;
            //check Id
            if (tBoxIdInput.Text == null || !idSuccess || _id <= 0)
            {
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
                try
                {
                    busiAccess.AddDrone(_id, _model, (DalXml.DO.WeightCategories)weight, _stationId);
                    MessageBox.Show("Drone Added Successfully", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                    Close();
                }
                catch (BL.BLApi.EXAlreadyExistsPrintException exception)
                {
                    //if Drone's Id already exists
                    MessageBox.Show(exception.printException(), "Error Message", 
                        MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
                catch (BL.BLApi.EXNotFoundPrintException ex)
                {
                    //if Station not found.. (must Add Drone at existing Station...)
                    MessageBox.Show(ex.ToString(), "Error Message", 
                        MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }
            else
                return;
           
            
        }

      













        //TO UPDATE A DRONE...
        
        public DroneWindow(BL.BLApi.Ibl _busiAccess, BL.BO.BODrone _bodrone) //CTOR called by DroneListWindow
        {
            InitializeComponent();
            busiAccess = _busiAccess;

            //edit buttons and text boxes for Update Window:
            tBoxIdInput.IsReadOnly = true;
            tBoxIdInput.BorderBrush = Brushes.Transparent;
            tBoxStationIdInput.IsReadOnly = true;
            tBoxStationIdInput.BorderBrush = Brushes.Transparent;
            cmbWeightChoice.ItemsSource = Enum.GetValues(typeof(BL.BO.Enum.WeightCategories));

            btnAddDrone.IsEnabled = false;
            btnAddDrone.Visibility = Visibility.Hidden;

            displayBODrone(_bodrone);
          

        }




        private void displayBODrone(BL.BO.BODrone bodrone)
        {
            thisDroneId = bodrone.Id;


            tBoxIdInput.Text = bodrone.Id.ToString();
            tBoxModelInput.Text = bodrone.Model;
            if (busiAccess.GetStationIdOfBODrone(bodrone.Id) != -1)
                tBoxStationIdInput.Text = (busiAccess.GetStationIdOfBODrone(bodrone.Id)).ToString();
            else
                tBoxStationIdInput.Text = "Drone is not charging at a Station";

            cmbWeightChoice.SelectedIndex = (int)bodrone.MaxWeight;
            cmbWeightChoice.IsReadOnly = true;
            cmbWeightChoice.IsEnabled = false;

            tBlockStatusInfo.Text = bodrone.DroneStatus.ToString();
            if (bodrone.ParcelInTransfer.Id == -1 || bodrone.ParcelInTransfer == null)
                tBlockDeliveryInfo.Text = "Not yet carrying Parcel";
            else 
                tBlockDeliveryInfo.Text = bodrone.ParcelInTransfer.ToString();
            tBlockLongInfo.Text = bodrone.Location.Longitude.ToString();
            tBlockLatinfo.Text = bodrone.Location.Latitude.ToString();

            tBlockCurrentLocationInfo.Text = busiAccess.GetDroneLocationString(bodrone.Id);

            tBlockBatteryInfo.Text = bodrone.Battery.ToString();
            //working on a function in BL..

        }

        
        















        private void btnModifyDroneModel_Click(object sender, RoutedEventArgs e)
        {
            int id;
            Int32.TryParse(tBoxIdInput.Text, out id);
            busiAccess.ModifyDrone(id, tBoxModelInput.Text);
            MessageBox.Show("Drone Model Changed", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            Close();


        }

        private void btnSendToCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                busiAccess.ChargeDrone(thisDroneId);
            }
            catch (BL.BLApi.EXDroneUnavailableException ex)
            {
                errorMsg(ex.ToString());
            }
            displayBODrone(busiAccess.GetBODrone(thisDroneId));
        }

        private void btnFreeDroneFromCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                busiAccess.FreeDrone(thisDroneId, DateTime.Now);
            }
            catch (BL.BLApi.EXMiscException ex) //if drone is not charging
            {
                errorMsg(ex.ToString());
            }
             displayBODrone(busiAccess.GetBODrone(thisDroneId));
        }

        private void btnPickupPkg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                busiAccess.PickupParcel(thisDroneId);
            }
            catch (BL.BLApi.EXDroneNotAssignedParcel ex)
            {
                errorMsg(ex.ToString());
            }
            catch (BL.BLApi.EXParcelAlreadyCollected ex)
            {
                errorMsg(ex.ToString());
            }
            displayBODrone(busiAccess.GetBODrone(thisDroneId));
        }

        private void btnAssignDroneToParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                busiAccess.AssignParcel(thisDroneId);
            }
            catch (BL.BLApi.EXNoAppropriateParcel ex)
            {
                errorMsg(ex.ToString());
            }
            catch (BL.BLApi.EXDroneUnavailableException ex)
            {
                errorMsg(ex.ToString());
            }
            displayBODrone(busiAccess.GetBODrone(thisDroneId));
        }

        private void btnDeliverPkg_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                busiAccess.DeliverParcel(thisDroneId);
            }
            catch (BL.BLApi.EXDroneNotAssignedParcel ex)
            {
                errorMsg(ex.ToString());
            }
            catch (BL.BLApi.EXParcelNotCollected ex)
            {
                errorMsg(ex.ToString());
            }
            displayBODrone(busiAccess.GetBODrone(thisDroneId));
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }



        private void errorMsg(string msg)
        {
            MessageBox.Show(msg, "Error",
                   MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
        }

        private void btnEraseDrone_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.EraseDrone(thisDroneId);
            MessageBox.Show("Drone " +tBoxIdInput.Text + " Erased", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            Close();
        }
    }
}
