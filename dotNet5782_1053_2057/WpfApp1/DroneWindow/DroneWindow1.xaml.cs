using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel; 
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        BL.BLApi.Ibl busiAccess;
        //DroneStringViewModel currentDroneViewModel;
        int thisDroneId; //field to allow window's function to retrieve bodrone from BL 
        private readonly Object lockThisThread = new Object();     //used to lock threads
        private readonly BackgroundWorker workerForPLSimulator = new BackgroundWorker();
        bool simulatorOn = false;
        //bool keepDroneCharging = false;

        readonly int DELAY_BTW_STEPS = 500; //wait __ miliseconds between steps of 


       
        
        public DroneWindow(BL.BLApi.Ibl _busiAccess)//default CTOR - to Add a drone
        {
            InitializeComponent();
            busiAccess = _busiAccess;
            cmbWeightChoice.ItemsSource = Enum.GetValues(typeof(BL.BO.Enum.WeightCategories));

            //(1) Disable and Hide irrelevant buttons
            HelpfulMethods.ChangeVisibilty(Visibility.Hidden, btnModifyDroneModel, btnAssignDroneToParcel, btnFreeDroneFromCharge,
                btnPickupPkg, btnSendToCharge, btnDeliverPkg, btnEraseDrone, btnSimulator);

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
            HelpfulMethods.ChangeTextColor(Colors.Black, tBlock_chooseDroneId, tBlock_chooseMaxWeight,
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
                catch (BL.BLApi.EXDroneAlreadyExists exception)
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
            
            //updates DroneViewModel to display details
            thisDroneId = _bodrone.Id; 
            displayBODrone(thisDroneId);
            

            //edit buttons and text boxes for Update Window:
            tBoxIdInput.IsReadOnly = true;
            tBoxIdInput.BorderBrush = Brushes.Transparent;
            tBoxStationIdInput.IsReadOnly = true;
            tBoxStationIdInput.BorderBrush = Brushes.Transparent;
            cmbWeightChoice.ItemsSource = Enum.GetValues(typeof(BL.BO.Enum.WeightCategories));
            cmbWeightChoice.IsReadOnly = true;
            cmbWeightChoice.IsEnabled = false;
            btnAddDrone.IsEnabled = false;
            btnAddDrone.Visibility = Visibility.Hidden;

            //initialize BackGroundWorker for Simulator
            workerForPLSimulator.DoWork += worker_DoWork;
            workerForPLSimulator.RunWorkerCompleted += worker_RunWorkerCompleted;
            workerForPLSimulator.ProgressChanged += worker_ProgressChanged;
            workerForPLSimulator.WorkerReportsProgress = true;
            workerForPLSimulator.WorkerSupportsCancellation = true;

        }
        private void displayBODrone(int _droneId) //updates this drone model
        {
                BL.BO.BODrone bodrone = busiAccess.GetBODrone(_droneId);
                DataContext = createDroneViewModel(bodrone);
            if (bodrone.DroneStatus == BL.BO.Enum.DroneStatus.Charging)
                HelpfulMethods.ChangeTextColor(Colors.Green, tBlockBatteryInfo);
            else
                HelpfulMethods.ChangeTextColor(Colors.Black, tBlockBatteryInfo);
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
                //keepDroneCharging = true;
                //Thread newChargingThread = new Thread(chargeDroneThreadFunc);
                //newChargingThread.Start();
            }
            catch (BL.BLApi.EXDroneUnavailableException ex)
            {
                errorMsg(ex.ToString());
            }
            displayBODrone(thisDroneId);
        }
       
        private void btnFreeDroneFromCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                busiAccess.FreeDrone(thisDroneId, DateTime.Now);
                //keepDroneCharging = false;
            }
            catch (BL.BLApi.EXMiscException ex) //if drone is not charging
            {
                errorMsg(ex.ToString());
            }
             displayBODrone(thisDroneId);
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
            displayBODrone(thisDroneId);
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
            displayBODrone(thisDroneId);
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
            displayBODrone(thisDroneId);
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
            try
            {
                busiAccess.EraseDrone(thisDroneId);
            }
            catch (BL.BLApi.EXCantDltDroneWParc ex)
            {
                HelpfulMethods.ErrorMsg(ex.ToString());
                return;
            }

            MessageBox.Show("Drone " + tBoxIdInput.Text + " Erased", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            Close();
        }
















        //SIMULATOR FUNCTIONS
        private void btnSimulator_Click(object sender, RoutedEventArgs e)
        {
            if (simulatorOn == false)
            {
                //TURN ON SIMULATOR
                simulatorOn = true;
                Thread newSimulatorThread = new Thread(beginSimulator);
                newSimulatorThread.Start();
                btnSimulator.Content = "End Simulator";
                HelpfulMethods.ChangeVisibilty(Visibility.Hidden, btnFreeDroneFromCharge,
                    btnSendToCharge, btnAssignDroneToParcel, btnPickupPkg,
                    btnDeliverPkg, btnEraseDrone);
            }

            else // if(simulatorOn == true)
            {
                workerForPLSimulator.CancelAsync();
                simulatorOn = false;
                busiAccess.StopSimulator();
                btnSimulator.Content = "Begin Simulator";
                HelpfulMethods.ChangeVisibilty(Visibility.Visible, btnFreeDroneFromCharge,
                    btnSendToCharge, btnAssignDroneToParcel, btnPickupPkg,
                    btnDeliverPkg, btnEraseDrone);
            }
            
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //this.Dispatcher.Invoke(() =>
            //{
                busiAccess.BeginSimulator(thisDroneId);
                while (simulatorOn == true)
                {
                    Thread.Sleep(DELAY_BTW_STEPS);
                    //busiAccess.UpdateSimulator();
                    //int percentage = 5;// busiAccess.GetDroneJourneyPercentage(droneId);
                    workerForPLSimulator.ReportProgress(1/*percentage++*/);
                    
                }

               
            //});

           
        }
        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Dispatcher.Invoke(() => //Invoke function ensures that
                                         //only one thread access this code block
            {
                displayBODrone(thisDroneId);
            });
        }
        private void worker_RunWorkerCompleted(object sender,   RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Simulator ended successfully", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            workerForPLSimulator.Dispose();
        }
        private void beginSimulator()
        {
            simulatorOn = true;
            //https://stackoverflow.com/questions/5483565/how-to-use-wpf-background-worker
            //https://wpf-tutorial.com/misc/multi-threading-with-the-backgroundworker/

            workerForPLSimulator.RunWorkerAsync();             //<- begin backgroundWorker...
            

        }

        private WpfApp1.DroneStringViewModel createDroneViewModel(BL.BO.BODrone origDrone)
        {
            WpfApp1.DroneStringViewModel newDrone = new WpfApp1.DroneStringViewModel();
            newDrone.Battery = origDrone.Battery.ToString();
            newDrone.DroneStatus = origDrone.DroneStatus.ToString();
            newDrone.Exists = origDrone.Exists;
            newDrone.Id = origDrone.Id.ToString();
            newDrone.Longitude = origDrone.Location.Longitude.ToString();
            newDrone.Latitude = origDrone.Location.Latitude.ToString();
            newDrone.MaxWeight = origDrone.MaxWeight.ToString();
            newDrone.Model = origDrone.Model;
            newDrone.ParcelInTransfer = (origDrone.ParcelInTransfer.Id == -1 || origDrone.ParcelInTransfer == null) ?
                "Not yet carrying Parcel" : origDrone.ParcelInTransfer.ToString();
            newDrone.LocationString = busiAccess.GetDroneLocationString(origDrone.Id);
            int stationId = busiAccess.GetStationIdOfBODrone(origDrone.Id);
            newDrone.StationId = (stationId != -1) ? stationId.ToString() : "Drone is not charging at a Station";
            return newDrone;
        }


        //END OF DRONE WINDOW CODE.
    }
}
