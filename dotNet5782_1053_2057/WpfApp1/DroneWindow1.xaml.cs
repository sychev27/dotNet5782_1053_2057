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
            tBlockBattery.Visibility = Visibility.Hidden;
            tBlockBatteryInfo.Visibility = Visibility.Hidden;

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
                catch (BL.BO.EXAlreadyPrintException exception)
                {
                    MessageBox.Show(exception.printException(), "Error Message", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
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
            if (bodrone.ParcelInTransfer.Id == -1)
                tBlockDeliveryInfo.Text = "Not yet carrying Parcel";
            else 
                tBlockDeliveryInfo.Text = bodrone.ParcelInTransfer.ToString();
            tBlockLongInfo.Text = bodrone.Location.Longitude.ToString();
            tBlockLatinfo.Text = bodrone.Location.Latitude.ToString();

            tBlockCurrentLocationInfo.Text = busiAccess.GetDroneLocationString(bodrone.Id);

            tBlockBatteryInfo.Text = bodrone.Battery.ToString();
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
            busiAccess.ModifyDrone(id, tBoxModelInput.Text);
            MessageBox.Show("Model Changed", "Drone model changed", 
                MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            Close();

        }

        private void btnSendToCharge_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.ChargeDrone(thisDroneId);
            displayBODrone(busiAccess.GetBODrone(thisDroneId));
        }

        private void btnFreeDroneFromCharge_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.FreeDrone(thisDroneId, 0);
            displayBODrone(busiAccess.GetBODrone(thisDroneId));
        }

        private void btnPickupPkg_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.PickupParcel(thisDroneId);
            displayBODrone(busiAccess.GetBODrone(thisDroneId));
        }

        private void btnSendDroneToCustomer_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.AssignParcel(thisDroneId);
            displayBODrone(busiAccess.GetBODrone(thisDroneId));
        }

        private void btnDeliverPkg_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.DeliverParcel(thisDroneId);
            displayBODrone(busiAccess.GetBODrone(thisDroneId));
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
