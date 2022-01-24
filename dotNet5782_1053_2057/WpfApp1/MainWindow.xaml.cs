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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

        BL.BLApi.Ibl busiAccess;          // <-- allows us to access business logic layer..
        DroneListWindow droneListWindow;  // <-- used to prevent user from opening other windows while
                                          // the droneList window is open
        public bool IsClosing = false;         //<-- used to communicate to DroneListwWindow that this window is closing...

        public MainWindow(BL.BLApi.Ibl _busiAccess) //CTOR
        {
            busiAccess = _busiAccess;
            InitializeComponent();
        }
        //BUTTONS:
        private void btnOpenDroneList_Click(object sender, RoutedEventArgs e)
        {
            if (droneListWindow != null) //if droneListWindow is already open...
            {
                droneListWindow.Show();
                droneListWindow.Focus();
                if (droneListWindow.WindowState == WindowState.Minimized)
                    droneListWindow.WindowState = WindowState.Normal;
            }
            else
            {
                droneListWindow = new DroneListWindow(busiAccess, this);
                droneListWindow.Show();
            }
        }
        private void btnCustomerLists_Click(object sender, RoutedEventArgs e)
        {
            if(droneListWindow != null)
                HelpfulMethods.ErrorMsg("Cannot open this window while the drone list winow is open");
            else
                new CustomerListWindow(busiAccess).ShowDialog();
        }
        private void btnParcelLists_Click(object sender, RoutedEventArgs e)
        {
            if (droneListWindow != null)
                HelpfulMethods.ErrorMsg("Cannot open this window while the drone list winow is open");
            else
                new ParcelListWindow(busiAccess).ShowDialog();
        }
        private void btnStationLists_Click(object sender, RoutedEventArgs e)
        {
            if (droneListWindow != null)
                HelpfulMethods.ErrorMsg("Cannot open this window while the drone list winow is open");
            else
                new StationListWindow(busiAccess).ShowDialog();
        }
        private void btnOpenMap_Click(object sender, RoutedEventArgs e)
        {
            new MapWindow(busiAccess).ShowDialog();
        }
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow(busiAccess).Show();
            Close();
        }
        //HELPING FUNCTIONS:
        public void ReleasePtrToDroneListWindow() //called by droneList window when it closes..
        {
            droneListWindow = null;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //ensure that window is not left open..
            if (droneListWindow != null)
            {
                IsClosing = true;
                droneListWindow.Close();
                droneListWindow = null;
            } 
            
        }
        //END OF WINDOW
    }
}
