﻿using System;
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
    /// Interaction logic for MapWindow.xaml
    /// </summary>
    public partial class MapWindow : Window
    {
        BL.BLApi.Ibl busiAccess;
        enum ObjectType { Drone, Station, Customer}
        class InfoBlock
        {
            public InfoBlock(BL.BO.BOCustomer cust)
            {
                Id = cust.Id;
                ThisObjectType = ObjectType.Customer;
                ColumnPlace = (int)((Math.Round(cust.Location.Longitude, 2) - 35) * 10);
                RowPlace = (int)((Math.Round(cust.Location.Latitude, 2) - 31) * 10);
            
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

            tBoxInfo.Text = "Hover the mouse over a square";

        }

        private void createInfoBlock(InfoBlock _InfoBlock)
        {

            int _rowPlace = _InfoBlock.RowPlace;
            int _columnPlace = _InfoBlock.ColumnPlace;


            System.Windows.Controls.TextBlock t = new System.Windows.Controls.TextBlock();
            t.Name = "tBlock" + _rowPlace.ToString() + _columnPlace.ToString();
            Grid.SetColumn(t, _columnPlace);
            Grid.SetRow(t, _rowPlace);
            t.Text = _InfoBlock.Id.ToString();
            t.Foreground = new SolidColorBrush(Colors.Red);
            t.Background = new SolidColorBrush(Colors.Blue);
            t.MouseEnter += new MouseEventHandler(
                new EventHandler((sender, e) => displayCustomer(sender, e, _InfoBlock)));
            t.MouseLeave += new MouseEventHandler(this.hideInfo);
            t.MouseLeftButtonDown += new MouseButtonEventHandler(
                new EventHandler((sender, e) => openWindow(sender, e, _InfoBlock)));
                
                

            //t.GotMouseCapture += new MouseEventHandler(this.displayInfo);
            //t.LostMouseCapture += new MouseEventHandler(this.hideInfo);
            gridMap.Children.Add(t);
        }



        private void displayCustomer(object sender, System.EventArgs e, MapWindow.InfoBlock _InfoBlock)
        {
            BL.BO.BOCustomer cust = busiAccess.GetBOCustomer(_InfoBlock.Id);

            tBoxInfo.Text = busiAccess.GetOneCustToList(_InfoBlock.Id).ToString()
                + "\n" + "Longitude: " + cust.Location.Longitude.ToString()
                + "\n" + "Latitude: " + cust.Location.Latitude.ToString();
        }
        private void openWindow(object sender, EventArgs e, InfoBlock infoBlock)
        {
            switch (infoBlock.ThisObjectType)
            {
                case ObjectType.Drone:
                    break;
                case ObjectType.Station:
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
    }
}
