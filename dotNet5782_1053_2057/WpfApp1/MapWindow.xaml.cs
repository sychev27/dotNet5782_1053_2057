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
        static String ImgDroneWithParcel = "C:\\Users\\dyyb1\\OneDrive\\Documentos\\AA windows project\\dotNet5782_1053_2057\\WpfApp1\\Pictures\\drone_with_parcel.jpg";
        static String ImgDroneWithoutParcel = "C:\\Users\\dyyb1\\OneDrive\\Documentos\\AA windows project\\dotNet5782_1053_2057\\WpfApp1\\Pictures\\drone_without_parcel2.PNG";
        static String ImgStation = "C:\\Users\\dyyb1\\OneDrive\\Documentos\\AA windows project\\dotNet5782_1053_2057\\WpfApp1\\Pictures\\station.jpg";
        static String ImgHouse = "C:\\Users\\dyyb1\\OneDrive\\Documentos\\AA windows project\\dotNet5782_1053_2057\\WpfApp1\\Pictures\\house.png";
        List<TextBlock> listTextBlocks = new List<TextBlock>();
        List<Image> listImages = new List<Image>();

        readonly System.Windows.Media.Color customerColor = Colors.Blue;
        readonly System.Windows.Media.Color stationColor = Colors.Orange;
        readonly System.Windows.Media.Color droneColor = Colors.Red;
        readonly System.Windows.Media.Color textColor = Colors.Black;
        readonly int IMAGESIZE = 1; //gridspan and rowspan of image
        readonly string emptyTextForInfoWindow = "Hover the mouse over a square or image";

        MainWindow parent; //pointer which allows us to communicate with main 

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
                numParcels = _numParcelsAtCustomer;
            }
            public InfoBlock(BL.BO.BOStation st)
            {
                Id = st.Id;
                ThisObjectType = ObjectType.Station;
                ColumnPlace = getColumnPlace(st.Location);
                RowPlace = getRowPlace(st.Location);
            }
            public InfoBlock(BL.BO.BODrone drone)
            {
                Id = drone.Id;
                ThisObjectType = ObjectType.Drone;
                ColumnPlace = getColumnPlace(drone.Location);
                RowPlace = getRowPlace(drone.Location);
                numParcels = (drone.ParcelInTransfer.Id == 0 || drone.ParcelInTransfer.Id == -1) ?
                    0 : 1;
            }
           //FIELDS:
            public int Id { get; set; }
            public ObjectType ThisObjectType { get; set; }
            public int RowPlace { get; set; }
            public int ColumnPlace { get; set; }
            public int? numParcels { get; set; } //used for drone and Customer, not station...
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

        public MapWindow(BL.BLApi.Ibl _busiAccess) //CTOR
        {
            InitializeComponent();
            busiAccess = _busiAccess;
            tBoxInfo.Text = emptyTextForInfoWindow;
            refreshMap();
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
        private void fillMapWithTextBlocks()
        {
            foreach (var item in busiAccess.GetAllBOCustomers())
            {
                listTextBlocks.Add(createTextBlock(new InfoBlock(item,
                    busiAccess.GetNumParcelsWaitingAtCustomer(item))));
            }
            foreach (var item in busiAccess.GetStations())
            {
                listTextBlocks.Add(createTextBlock(new InfoBlock(item)));
            }
            foreach (var item in busiAccess.GetBODroneList())
            {
                listTextBlocks.Add(createTextBlock(new InfoBlock(item)));
            }
        }
        private void fillMapWithImages()
        {
            foreach (var item in busiAccess.GetAllBOCustomers())
            {
                listImages.Add(createImage(new InfoBlock(item,
                    busiAccess.GetNumParcelsWaitingAtCustomer(item))));
            }
            foreach (var item in busiAccess.GetStations())
            {
                listImages.Add(createImage(new InfoBlock(item)));
            }
            foreach (var item in busiAccess.GetBODroneList())
            {
                listImages.Add(createImage(new InfoBlock(item)));
            }
        }
        private TextBlock createTextBlock(InfoBlock _InfoBlock) //Creates and sets TextBlock on Grid
        {
            TextBlock newTextBlock = new TextBlock();
            newTextBlock.Name = "tBlockR" + _InfoBlock.RowPlace.ToString() + "C" + _InfoBlock.ColumnPlace.ToString();
            Grid.SetColumn(newTextBlock, _InfoBlock.ColumnPlace);
            Grid.SetRow(newTextBlock, _InfoBlock.RowPlace);
            newTextBlock.FontSize = 20;
            newTextBlock.FontWeight = FontWeights.Bold;
            newTextBlock.Foreground = new SolidColorBrush(textColor);
            newTextBlock.FontWeight = FontWeight.FromOpenTypeWeight(300);
            
            newTextBlock.MouseLeftButtonDown += new MouseButtonEventHandler(
                new EventHandler((sender, e) => openWindow(sender, e, _InfoBlock)));
            newTextBlock.MouseLeave += new MouseEventHandler(this.hideInfo);
            switch (_InfoBlock.ThisObjectType)
            {
                case ObjectType.Station:
                    {
                        newTextBlock.Background = new SolidColorBrush(stationColor);
                        newTextBlock.MouseEnter += new MouseEventHandler(
                            new EventHandler((sender, e) => displayStation(sender, e, _InfoBlock)));
                        newTextBlock.Text = _InfoBlock.Id.ToString();
                    }
                    break;
                case ObjectType.Customer:
                    {
                        newTextBlock.Background = new SolidColorBrush(customerColor);
                        newTextBlock.MouseEnter += new MouseEventHandler(
                            new EventHandler((sender, e) => displayCustomer(sender, e, _InfoBlock)));
                        newTextBlock.Text = _InfoBlock.numParcels.ToString();
                    }
                    break;
                case ObjectType.Drone:
                    {
                        newTextBlock.Background = new SolidColorBrush(droneColor);
                        newTextBlock.MouseEnter += new MouseEventHandler(
                            new EventHandler((sender, e) => displayDrone(sender, e, _InfoBlock)));
                        newTextBlock.Text = _InfoBlock.numParcels.ToString();
                    }
                    break;
                default:
                    break;
            }

            gridMap.Children.Add(newTextBlock);
            return newTextBlock;
        }
        private Image createImage(InfoBlock _InfoBlock)
        {
            Image newImage = new Image();
            Grid.SetColumn(newImage, _InfoBlock.ColumnPlace);
            Grid.SetColumnSpan(newImage, IMAGESIZE);
            Grid.SetRow(newImage, _InfoBlock.RowPlace);
            Grid.SetRowSpan(newImage, IMAGESIZE);
            ImageSource _imageSource;
            switch(_InfoBlock.ThisObjectType)
            {
                case ObjectType.Drone:
                    {
                        _imageSource = (_InfoBlock.numParcels == 0) ?
                                        new BitmapImage(new Uri(ImgDroneWithoutParcel))
                                        : new BitmapImage(new Uri(ImgDroneWithParcel));
                        newImage.MouseEnter += new MouseEventHandler(
                            new EventHandler((sender, e) => displayDrone(sender, e, _InfoBlock)));
                    }
                    break;
                case ObjectType.Station:
                    {
                        _imageSource = new BitmapImage(new Uri(ImgStation));
                        newImage.MouseEnter += new MouseEventHandler(
                            new EventHandler((sender, e) => displayStation(sender, e, _InfoBlock)));
                    }
                    break;
                case ObjectType.Customer:
                    {
                        _imageSource = new BitmapImage(new Uri(ImgHouse));
                        newImage.MouseEnter += new MouseEventHandler(
                            new EventHandler((sender, e) => displayCustomer(sender, e, _InfoBlock)));
                    }
                    break;
                default:
                    _imageSource = null;
                    break;
            }
            newImage.Source = _imageSource;
            newImage.MouseLeftButtonDown += new MouseButtonEventHandler(
                new EventHandler((sender, e) => openWindow(sender, e, _InfoBlock)));
            newImage.MouseLeave += new MouseEventHandler(this.hideInfo);
            gridMap.Children.Add(newImage);
            return newImage;
        }
        private void displayCustomer(object sender, System.EventArgs e, MapWindow.InfoBlock _InfoBlock)
        {
            var item = busiAccess.GetBOCustomer(_InfoBlock.Id);

            tBoxInfo.Text = busiAccess.GetOneCustToList(_InfoBlock.Id).ToString()
                + "\n" + "Long: " + Math.Round(item.Location.Longitude, 3).ToString()
                + "\n" + "Lat: " + Math.Round(item.Location.Latitude, 3).ToString();
        }
        private void displayStation(object sender, System.EventArgs e, MapWindow.InfoBlock _InfoBlock)
        {
            var item = busiAccess.GetBOStation(_InfoBlock.Id);

            tBoxInfo.Text = item.ToString()
                + "\n" + "Long: " + Math.Round(item.Location.Longitude, 3).ToString()
                + "\n" + "Lat: " + Math.Round(item.Location.Latitude, 3).ToString(); 
        }
        private void displayDrone(object sender, System.EventArgs e, MapWindow.InfoBlock _InfoBlock)
        {
            var item = busiAccess.GetBODrone(_InfoBlock.Id);

            tBoxInfo.Text = item.ToString();
        }
        private void openWindow(object sender, EventArgs e, InfoBlock infoBlock)
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
        }
        private void hideInfo(object sender, System.EventArgs e)
        {
            tBoxInfo.Text = emptyTextForInfoWindow;
        }
        private void refreshMap()
        {
            clearMap();
            if((bool)chkboxTextMode.IsChecked)
                fillMapWithTextBlocks();
            else
                fillMapWithImages();
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

        





        //END OF MAP WINDOW
    }
}
