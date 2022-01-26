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
using System.Threading;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MapWindow.xaml
    /// </summary>
    public partial class MapWindow : Window
    {
        BL.BLApi.Ibl busiAccess;
        enum ObjectType { Drone, Station, Customer}
        //David's computer's file paths:
        static String ImgDroneWithParcel = "C:\\Users\\dyyb1\\OneDrive\\Documentos\\AA windows project\\dotNet5782_1053_2057\\WpfApp1\\Pictures\\drone_with_parcel.jpg";
        static String ImgDroneWithoutParcel = "C:\\Users\\dyyb1\\OneDrive\\Documentos\\AA windows project\\dotNet5782_1053_2057\\WpfApp1\\Pictures\\drone_without_parcel2.PNG";
        static String ImgStation = "C:\\Users\\dyyb1\\OneDrive\\Documentos\\AA windows project\\dotNet5782_1053_2057\\WpfApp1\\Pictures\\station.jpg";
        static String ImgHouse = "C:\\Users\\dyyb1\\OneDrive\\Documentos\\AA windows project\\dotNet5782_1053_2057\\WpfApp1\\Pictures\\house.png";

        //Alex's computer's file paths:




        List<TextBlock> listTextBlocks = new List<TextBlock>();
        List<Image> listImages = new List<Image>();

        readonly System.Windows.Media.Color customerColor = Colors.Blue;
        readonly System.Windows.Media.Color stationColor = Colors.Green;
        readonly System.Windows.Media.Color droneColor = Colors.Red;
        readonly System.Windows.Media.Color textColor = Colors.Black;
        readonly int IMAGESIZEforDrones = 1; //gridspan and rowspan of image
        readonly int IMAGESIZEforCustStations = 2; //gridspan and rowspan of image
        readonly string emptyTextForInfoWindow = "Hover the mouse over a square or image";

        //for simulator:
        readonly BackgroundWorker worker = new BackgroundWorker();
        bool simulatorOn = false;
        readonly string textForSimulatorBtnStart = "Turn On Simulator";
        readonly int DELAY_BTW_REFRESH = 500; // __ miliseconds
        /// <summaryOfInfoBlock>
        /// Each block is synchronized with an id number, tagged with a type of object,
        /// and given appropriate Column and Row places
        /// </summary>
        class InfoBlock
        {
            /// <summaryOfNumGridSpots>
            /// number of squares available on map,  only set once...
            /// MUST BE A MULTIPLE OF 10...grid must be square..
            /// </summary>
            public readonly int numGridSpots = 20; 
           //CTORS: (3 in total)
            public InfoBlock(BL.BO.BOCustomer cust, int _numParcelsAtCustomer)
            {
                Id = cust.Id;
                ThisObjectType = ObjectType.Customer;
                ColumnPlace = getColumnPlace(cust.Location);
                RowPlace = getRowPlace(cust.Location);
                numParcelsOrDronesCharging = _numParcelsAtCustomer;
                name = cust.Name;
            }
            public InfoBlock(BL.BO.BOStation st, int _numDronesCharging)
            {
                Id = st.Id;
                ThisObjectType = ObjectType.Station;
                ColumnPlace = getColumnPlace(st.Location);
                RowPlace = getRowPlace(st.Location);
                numParcelsOrDronesCharging = _numDronesCharging;
                name = "Station: " + st.Id.ToString();
            }
            public InfoBlock(BL.BO.BODrone drone)
            {
                Id = drone.Id;
                ThisObjectType = ObjectType.Drone;
                ColumnPlace = getColumnPlace(drone.Location);
                RowPlace = getRowPlace(drone.Location);
                numParcelsOrDronesCharging = (drone.ParcelInTransfer.Id == 0 || drone.ParcelInTransfer.Id == -1
                    || drone.ParcelInTransfer.Collected == false) ? //if drone has not yet picked up parcel...
                    /*set to Zero*/ 0 : /*else set to 1*/  1;
                name = "Drone " + drone.Id.ToString();
            }
           //FIELDS:
            public int Id { get; set; }
            public ObjectType ThisObjectType { get; set; }
            public int RowPlace { get; set; }
            public int ColumnPlace { get; set; }
            public int? numParcelsOrDronesCharging { get; set; } //used differently for drone, customer, and station...
            public string name { get; set; }
            //METHODS:
            private int getColumnPlace(BL.BO.BOLocation loc)
            {
                return (int)((Math.Round(loc.Longitude, 2) - 35) * 10);
            }
            private int getRowPlace(BL.BO.BOLocation loc)
            {
                return (int)((Math.Round(loc.Latitude, 2) - 31) * 10);
            }
        }

        //CTOR of MapWindow:
        public MapWindow(BL.BLApi.Ibl _busiAccess) 
        {
            InitializeComponent();
            busiAccess = _busiAccess;
            tBoxInfoWindow.Text = emptyTextForInfoWindow;
            refreshMap();

            WindowState = WindowState.Maximized;

            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.WorkerSupportsCancellation = true;
        }
        //BUTTONS AND OTHER USER INTERFACE:
        private void btnReturnToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            refreshMap();
        }
        private void chkboxTextMode_Checked(object sender, RoutedEventArgs e)
        {
            refreshMap();
        }
        private void chkboxTextMode_Unchecked(object sender, RoutedEventArgs e)
        {
            refreshMap();
        }
        //HELPING FUNCTIONS
        private void fillMapWithTextBlocks() //drones are behind stations, in front of customers
        {
            foreach (var item in busiAccess.GetAllBOCustomers())
            {
                if (item.Exists) 
                    listTextBlocks.Add(createTextBlock(new InfoBlock(item,
                    busiAccess.GetNumParcelsWaitingAtCustomer(item))));
            }
            foreach (var item in busiAccess.GetBODroneList())
            {
                if (item.Exists)
                    listTextBlocks.Add(createTextBlock(new InfoBlock(item)));
            }
            foreach (var item in busiAccess.GetStations())
            {
                if (item.Exists) 
                    listTextBlocks.Add(createTextBlock(new InfoBlock(item,
                    busiAccess.GetOneStationToList(item.Id).ChargeSlotsTaken)));
            }
        }
        private void fillMapWithImages() //drones are in front of other items
        {
            foreach (var item in busiAccess.GetAllBOCustomers())
            {
                if (item.Exists) 
                    listImages.Add(createImage(new InfoBlock(item,
                    busiAccess.GetNumParcelsWaitingAtCustomer(item))));
            }
            foreach (var item in busiAccess.GetStations())
            {
                if (item.Exists) 
                    listImages.Add(createImage(new InfoBlock(item,
                    busiAccess.GetOneStationToList(item.Id).ChargeSlotsTaken)));
            }
            foreach (var item in busiAccess.GetBODroneList())
            {
                if (item.Exists) 
                    listImages.Add(createImage(new InfoBlock(item)));
            }
        }
        private TextBlock createTextBlock(InfoBlock _InfoBlock) //Creates and sets TextBlock on Grid
        {
            TextBlock newTextBlock = new TextBlock();
            newTextBlock.Name = "tBlockR" + _InfoBlock.RowPlace.ToString() + "C" + _InfoBlock.ColumnPlace.ToString();
            Grid.SetColumn(newTextBlock, _InfoBlock.ColumnPlace);
            Grid.SetRow(newTextBlock, _InfoBlock.RowPlace);
            newTextBlock.FontSize = 11;
            newTextBlock.FontWeight = FontWeights.Bold;
            newTextBlock.Foreground = new SolidColorBrush(textColor);
            newTextBlock.FontWeight = FontWeight.FromOpenTypeWeight(700);
            
            newTextBlock.MouseLeftButtonDown += new MouseButtonEventHandler(
                new EventHandler((sender, e) => openWindowOfInfoBlock(sender, e, _InfoBlock)));
            newTextBlock.MouseLeave += new MouseEventHandler(this.hideTextInInfoWindow);
            switch (_InfoBlock.ThisObjectType)
            {
                case ObjectType.Station:
                    {
                        newTextBlock.Background = new SolidColorBrush(stationColor);
                        newTextBlock.MouseEnter += new MouseEventHandler(
                            new EventHandler((sender, e) => displayStationInInfoWindow(sender, e, _InfoBlock)));
                        newTextBlock.Text = _InfoBlock.numParcelsOrDronesCharging.ToString();
                    }
                    break;
                case ObjectType.Customer:
                    {
                        newTextBlock.Background = new SolidColorBrush(customerColor);
                        newTextBlock.MouseEnter += new MouseEventHandler(
                            new EventHandler((sender, e) => displayCustomerInInfoWindow(sender, e, _InfoBlock)));
                        newTextBlock.Text = _InfoBlock.numParcelsOrDronesCharging.ToString();
                    }
                    break;
                case ObjectType.Drone:
                    {
                        newTextBlock.Background = new SolidColorBrush(droneColor);
                        newTextBlock.MouseEnter += new MouseEventHandler(
                            new EventHandler((sender, e) => displayDroneInInfoWindow(sender, e, _InfoBlock)));
                        newTextBlock.Text = _InfoBlock.numParcelsOrDronesCharging.ToString();
                        newTextBlock.Margin = new Thickness(5, 0, 5, 0);
                    }
                    break;
                default:
                    break;
            }
            newTextBlock.Text += "\n" + _InfoBlock.name;
            gridMap.Children.Add(newTextBlock);
            return newTextBlock;
        }
        private Image createImage(InfoBlock _InfoBlock)
        {
            Image newImage = new Image();
            Grid.SetColumn(newImage, _InfoBlock.ColumnPlace);
            Grid.SetRow(newImage, _InfoBlock.RowPlace);
            ImageSource _imageSource;
            switch(_InfoBlock.ThisObjectType)
            {
                case ObjectType.Drone:
                    {
                        _imageSource = (_InfoBlock.numParcelsOrDronesCharging == 0) ?
                                        new BitmapImage(new Uri(ImgDroneWithoutParcel))
                                        : new BitmapImage(new Uri(ImgDroneWithParcel));
                        newImage.MouseEnter += new MouseEventHandler(
                            new EventHandler((sender, e) => displayDroneInInfoWindow(sender, e, _InfoBlock)));
                        Grid.SetColumnSpan(newImage, IMAGESIZEforDrones);
                        Grid.SetRowSpan(newImage, IMAGESIZEforDrones);
                    }
                    break;
                case ObjectType.Station:
                    {
                        _imageSource = new BitmapImage(new Uri(ImgStation));
                        newImage.MouseEnter += new MouseEventHandler(
                            new EventHandler((sender, e) => displayStationInInfoWindow(sender, e, _InfoBlock)));
                        Grid.SetColumnSpan(newImage, IMAGESIZEforCustStations);
                        Grid.SetRowSpan(newImage, IMAGESIZEforCustStations);
                    }
                    break;
                case ObjectType.Customer:
                    {
                        _imageSource = new BitmapImage(new Uri(ImgHouse));
                        newImage.MouseEnter += new MouseEventHandler(
                            new EventHandler((sender, e) => displayCustomerInInfoWindow(sender, e, _InfoBlock)));
                        Grid.SetColumnSpan(newImage, IMAGESIZEforCustStations);
                        Grid.SetRowSpan(newImage, IMAGESIZEforCustStations);
                    }
                    break;
                default:
                    _imageSource = null;
                    break;
            }
            newImage.Source = _imageSource;
            newImage.MouseLeftButtonDown += new MouseButtonEventHandler(
                new EventHandler((sender, e) => openWindowOfInfoBlock(sender, e, _InfoBlock)));
            newImage.MouseLeave += new MouseEventHandler(this.hideTextInInfoWindow);
            gridMap.Children.Add(newImage);
            return newImage;
        }
        private void displayCustomerInInfoWindow(object sender, System.EventArgs e, MapWindow.InfoBlock _InfoBlock)
        {
            var item = busiAccess.GetBOCustomer(_InfoBlock.Id);

            tBoxInfoWindow.Text = busiAccess.GetOneCustToList(_InfoBlock.Id).ToString()
                + "\n" + "Long: " + Math.Round(item.Location.Longitude, 3).ToString()
                + "\n" + "Lat: " + Math.Round(item.Location.Latitude, 3).ToString();
        }
        private void displayStationInInfoWindow(object sender, System.EventArgs e, MapWindow.InfoBlock _InfoBlock)
        {
            var item = busiAccess.GetBOStation(_InfoBlock.Id);

            tBoxInfoWindow.Text = item.ToString()
                + "\n" + "Long: " + Math.Round(item.Location.Longitude, 3).ToString()
                + "\n" + "Lat: " + Math.Round(item.Location.Latitude, 3).ToString(); 
        }
        private void displayDroneInInfoWindow(object sender, System.EventArgs e, MapWindow.InfoBlock _InfoBlock)
        {
            var item = busiAccess.GetBODrone(_InfoBlock.Id);

            tBoxInfoWindow.Text = item.ToString();
        }
        private void openWindowOfInfoBlock(object sender, EventArgs e, InfoBlock infoBlock)
        {
            switch (infoBlock.ThisObjectType)
            {
                case ObjectType.Drone:
                    new DroneWindow(busiAccess, 
                        busiAccess.GetBODrone(infoBlock.Id)).ShowDialog();
                    break;
                case ObjectType.Station:
                    new StationWindow(busiAccess,
                        (infoBlock.Id)).ShowDialog();
                    break;
                case ObjectType.Customer:
                    new CustomerWindow(busiAccess, 
                busiAccess.GetBOCustomer(infoBlock.Id)).ShowDialog();
                    break;
                default:
                    break;
            }
            refreshMap();
        }
        private void hideTextInInfoWindow(object sender, System.EventArgs e)
        {
            tBoxInfoWindow.Text = emptyTextForInfoWindow;
        }
        private void refreshMap()
        {
            Dispatcher.Invoke(() =>
           {
               clearMap();
               if ((bool)chkboxTextMode.IsChecked)
                   fillMapWithTextBlocks();
               else
                   fillMapWithImages();
           });
        }
        private void clearMap()
        {
            foreach (var item in listTextBlocks)
            {
                gridMap.Children.Remove(item);
            }
            foreach (var item in listImages)
            {
                gridMap.Children.Remove(item);
            }
        }

        //FOR SIMULATOR:
        private void worker_DoWork(object sender, DoWorkEventArgs e) //displays Map, Sleeps
        {
            while(simulatorOn)
            {
                refreshMap();
                Thread.Sleep(DELAY_BTW_REFRESH);
            }
        }
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //HelpfulMethods.SuccessMsg("Simulator Stopped Successfully");
        }
        private void beginSimulator()
        {
            foreach (var item in busiAccess.GetBODroneList()) //begin all Simulators in BL
            {
                if(item.Exists)
                {
                    busiAccess.BeginSimulatorForDrone(item.Id);
                }
            }
            worker.RunWorkerAsync(); //begin simular in PL
        } 
        private void btnSimulator_Click(object sender, RoutedEventArgs e)
        {
            if (!simulatorOn) //turn on simulator...
            {
                simulatorOn = true;
               Thread newThread = new Thread(beginSimulator);
               newThread.Start();
               btnSimulator.Content = "Turn off Simulator";
            }
            else    //turn off simulator
            {
                stopSimulator();
            }
        }
        private void stopSimulator()
        {
            simulatorOn = false;
            worker.CancelAsync();
            btnSimulator.Content = textForSimulatorBtnStart;
            foreach (var item in busiAccess.GetBODroneList()) //stop simulators in BL
            {
                if (item.Exists)
                {
                    busiAccess.StopSimulatorForDrone(item.Id);
                }
            }
            HelpfulMethods.SuccessMsg("All Drone Simulators canceled");
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if(simulatorOn)
                stopSimulator();
        }




        //END OF MAP WINDOW
    }
}
