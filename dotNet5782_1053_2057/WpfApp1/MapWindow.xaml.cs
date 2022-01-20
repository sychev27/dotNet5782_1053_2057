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
        static String ImgDroneWithoutParcel = "C:\\Users\\dyyb1\\OneDrive\\Documentos\\AA windows project\\dotNet5782_1053_2057\\WpfApp1\\Pictures\\drone_without_parcel.PNG";


        class InfoBlock
        {
            public InfoBlock(BL.BO.BOCustomer cust)
            {
                Id = cust.Id;
                ThisObjectType = ObjectType.Customer;
                ColumnPlace = (int)((Math.Round(cust.Location.Longitude, 2) - 35) * 10);
                RowPlace = (int)((Math.Round(cust.Location.Latitude, 2) - 31) * 10);
            
            }
            public InfoBlock(BL.BO.BOStation st)
            {
                Id = st.Id;
                ThisObjectType = ObjectType.Station;
                ColumnPlace = (int)((Math.Round(st.Location.Longitude, 2) - 35) * 10);
                RowPlace = (int)((Math.Round(st.Location.Latitude, 2) - 31) * 10);
            }
            public int Id { get; set; }
            public ObjectType ThisObjectType { get; set; }
            public int RowPlace { get; set; }
            public int ColumnPlace { get; set; }

         }

        public MapWindow(BL.BLApi.Ibl _busiAccess)
        {
            InitializeComponent();
            busiAccess = _busiAccess;

            foreach (var item in busiAccess.GetAllBOCustomers())
            {
                createInfoBlock(new InfoBlock(item));
            }
            foreach (var item in busiAccess.GetStations())
            {
                createInfoBlock(new InfoBlock(item));
            }

            tBoxInfo.Text = "Hover the mouse over a square";
                       

            Image ImageDroneWithParc = new Image();
            
            //ImageDroneWithParc.Stretch = (Stretch.Fill);
            Grid.SetColumn(ImageDroneWithParc, 1);
            Grid.SetColumnSpan(ImageDroneWithParc, 2);
            Grid.SetRow(ImageDroneWithParc, 2);
            Grid.SetRowSpan(ImageDroneWithParc, 3);
            ImageSource droneWithParcel = new BitmapImage(new Uri(ImgDroneWithoutParcel));
            ImageDroneWithParc.Source = droneWithParcel;
            //ImageDroneWithParc.MouseLeftButtonDown = ;
            gridMap.Children.Add(ImageDroneWithParc);
            
        }

        private void createInfoBlock(InfoBlock _InfoBlock)
        {

            int _rowPlace = _InfoBlock.RowPlace;
            int _columnPlace = _InfoBlock.ColumnPlace;


            System.Windows.Controls.TextBlock t = new System.Windows.Controls.TextBlock();
            t.Name = "tBlockR" + _rowPlace.ToString() + "C" + _columnPlace.ToString();
            Grid.SetColumn(t, _columnPlace);
            Grid.SetRow(t, _rowPlace);
            t.Text = _InfoBlock.Id.ToString();
            t.Foreground = new SolidColorBrush(Colors.Red);
            //t.FontStyle;
            t.FontWeight = FontWeight.FromOpenTypeWeight(500);
            
            t.MouseLeftButtonDown += new MouseButtonEventHandler(
                new EventHandler((sender, e) => openWindow(sender, e, _InfoBlock)));


            switch (_InfoBlock.ThisObjectType)
            {
                case ObjectType.Drone:
                    break;
                case ObjectType.Station:
                    {
                        t.Background = new SolidColorBrush(Colors.Orange);
                        t.MouseEnter += new MouseEventHandler(
                            new EventHandler((sender, e) => displayStation(sender, e, _InfoBlock)));
                        t.MouseLeave += new MouseEventHandler(this.hideInfo);
                    }
                    break;
                case ObjectType.Customer:
                    {
                        t.Background = new SolidColorBrush(Colors.Blue);
                        t.MouseEnter += new MouseEventHandler(
                            new EventHandler((sender, e) => displayCustomer(sender, e, _InfoBlock)));
                        t.MouseLeave += new MouseEventHandler(this.hideInfo);
                    }
                    break;
                default:
                    break;
            }




            //t.GotMouseCapture += new MouseEventHandler(this.displayInfo);
            //t.LostMouseCapture += new MouseEventHandler(this.hideInfo);
            gridMap.Children.Add(t);
        }



        private void displayCustomer(object sender, System.EventArgs e, MapWindow.InfoBlock _InfoBlock)
        {
            var item = busiAccess.GetBOCustomer(_InfoBlock.Id);

            tBoxInfo.Text = busiAccess.GetOneCustToList(_InfoBlock.Id).ToString()
                + "\n" + "Longitude: " + item.Location.Longitude.ToString()
                + "\n" + "Latitude: " + item.Location.Latitude.ToString();
        }
        private void displayStation(object sender, System.EventArgs e, MapWindow.InfoBlock _InfoBlock)
        {
            var item = busiAccess.GetBOStation(_InfoBlock.Id);

            tBoxInfo.Text = item.ToString();
            //    + "\n" + "Longitude: " + item.Location.Longitude.ToString()
            //    + "\n" + "Latitude: " + item.Location.Latitude.ToString();
        }
        private void openWindow(object sender, EventArgs e, InfoBlock infoBlock)
        {
            switch (infoBlock.ThisObjectType)
            {
                case ObjectType.Drone:
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
            tBoxInfo.Text = "Hover the mouse over a square";
        }

        private void btnReturnToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }




        //END OF MAP WINDOW
    }
}
