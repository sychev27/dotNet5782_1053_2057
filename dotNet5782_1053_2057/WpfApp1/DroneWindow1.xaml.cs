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
        IDAL.DO.WeightCategories? weightChoice = null;

        int thisDroneId;

        const string WEIGHTSELECTED = "Weight Selected!";
        const string CHOOSEWEIGHT = "Choose Maximum Weight Category:";
        const string DRONEADDED = "Drone Added Successfully!";

        const string btnUpdateText = "Update this Drone";
        
        //default constructor is to Add a drone
        public DroneWindow(IBL.Ibl _busiAccess, int num)//to add a Drone
        {
            InitializeComponent();
            busiAccess = _busiAccess;
            btnGetDrone.IsEnabled = false;
        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            int _id;
            Int32.TryParse(tBoxIdInput.Text, out _id);
            string _model = tBoxModelInput.Text;
            int _stationId;
            Int32.TryParse(tBoxStationIdInput.Text, out _stationId);
            IDAL.DO.WeightCategories? weight = weightChoice;

            //if(weight == null)
            //    throw excepeiton!!

            //alex write code here. exception


            busiAccess.addDrone(_id, _model, (IDAL.DO.WeightCategories)weight, _stationId);
            tBlock_chooseMaxWeight.Text = CHOOSEWEIGHT;
            tBlock_DroneAdded.Text = DRONEADDED;

        }

        private void btnLightWeight_Click(object sender, RoutedEventArgs e)
        {
            weightChoice = IDAL.DO.WeightCategories.light;
            tBlock_chooseMaxWeight.Text = WEIGHTSELECTED;
            resetWeightBtns();
            btnLightWeight.IsEnabled = false;


        }

        private void btnMediumWeight_Click(object sender, RoutedEventArgs e)
        {
            weightChoice = IDAL.DO.WeightCategories.medium;
            tBlock_chooseMaxWeight.Text = WEIGHTSELECTED;
            resetWeightBtns();
            btnMediumWeight.IsEnabled = false;
        }
        private void btnHeavyWeight_Click(object sender, RoutedEventArgs e)
        {
            weightChoice = IDAL.DO.WeightCategories.heavy;
            tBlock_chooseMaxWeight.Text = WEIGHTSELECTED;
            resetWeightBtns();
            btnHeavyWeight.IsEnabled = false;
        }

        private void resetWeightBtns()
        {
            if(btnLightWeight.IsEnabled == false)
            {
                btnLightWeight.IsEnabled = true; return;
            } 
            else if(btnMediumWeight.IsEnabled == false)
            {
                btnMediumWeight.IsEnabled = true; return;
            }
            else if(btnHeavyWeight.IsEnabled == false)
            {
                btnHeavyWeight.IsEnabled = true; return;
            }
        }

















        //TO UPDATE A DRONE...
        public DroneWindow(IBL.Ibl _busiAccess, char let) //Update Drone CTOR
        {
            //(1) Redesign text boxes and buttons
            InitializeComponent();
            busiAccess = _busiAccess;
            btnAddDrone.IsEnabled = false;
            btnModifyDroneModel.IsEnabled = false;

            btnHeavyWeight.IsEnabled = false;
            btnLightWeight.IsEnabled = false;
            btnMediumWeight.IsEnabled = false;

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

            tBlockDisplayDrone.Text = busiAccess.getBODrone(droneId).ToString();

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
            tBlock_DroneAdded.Text = "Drone modified!";
            tBlockDisplayDrone.Text = busiAccess.getBODrone(id).ToString();
        }

        private void btnSendToCharge_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.chargeDrone(thisDroneId);
            tBlockDisplayDrone.Text = busiAccess.getBODrone(thisDroneId).ToString();
        }

        private void btnFreeDroneFromCharge_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.freeDrone(thisDroneId, 0);
            tBlockDisplayDrone.Text = busiAccess.getBODrone(thisDroneId).ToString();

        }

        private void btnPickupPkg_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.PickupParcel(thisDroneId);
            tBlockDisplayDrone.Text = busiAccess.getBODrone(thisDroneId).ToString();

        }

        private void btnSendDroneToCustomer_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.assignParcel(thisDroneId);
            tBlockDisplayDrone.Text = busiAccess.getBODrone(thisDroneId).ToString();

        }

        private void btnDeliverPkg_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.deliverParcel(thisDroneId);
            tBlockDisplayDrone.Text = busiAccess.getBODrone(thisDroneId).ToString();
        }


        //private void tBoxModelInput_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    tBoxModelInput.Text = "";

        //}
    }
}
