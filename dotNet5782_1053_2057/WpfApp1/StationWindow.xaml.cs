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
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        BL.BLApi.Ibl busiAccess;
        public StationWindow(BL.BLApi.Ibl _busiAccess)
        {
            InitializeComponent();
            busiAccess = _busiAccess;
        }

        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {
            //reset text color
            MainWindow.ChangeTextColor(Colors.Black, tBlockId, tBlockChargeSlots, 
                tBlockLong, tBlockLatitude, tBlockName);

            //(1) Receive Data
            int _id;
            bool idSuccess = Int32.TryParse(tBoxIdInput.Text, out _id);

            int _name;
            bool nameSuccess = Int32.TryParse(tBoxNameInput.Text, out _name);
            //NAME TZARICH IYUN!


            int numChargeSlots;
            bool chargeSlotsSuccess = Int32.TryParse(tBoxChargeSlotsInput.Text, out numChargeSlots);

            double _longitude;
            double _latitude;
            bool longSuccess = double.TryParse(tBoxLongInput.Text, out _longitude);
            bool latSuccess = double.TryParse(tBoxLatInput.Text, out _latitude);


            //(2) Check that Data is Valid
            bool validData = true;
            //check Id
            if (tBoxIdInput.Text == null || !idSuccess || _id <= 0)
            {
                tBlockId.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }

            //check Id
            if (tBoxNameInput.Text == null || !nameSuccess)
            {
                tBlockName.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }

            if (tBoxChargeSlotsInput.Text == null || !chargeSlotsSuccess || numChargeSlots <= 0)
            {
                tBlockChargeSlots.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }
            if (tBoxLongInput.Text == null || !longSuccess || _longitude < 35 || _longitude > 36)
            {
                tBlockLong.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }
            if (tBoxLatInput.Text == null || !latSuccess || _latitude< 31 || _latitude > 32)
            {
                tBlockLatitude.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }


            //(3) Add Drone..
            if (validData)
            {
                try
                {
                    busiAccess.AddStation(_id, _name, _longitude, _latitude, numChargeSlots);
                    MessageBox.Show("Station Added Successfully", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                    Close();
                }
                catch (BL.BLApi.EXStationAlreadyExists exception)
                {
                    MainWindow.ErrorMsg(exception.ToString());
                }
               
            }
            else
                return;

        }














        //END OF WINDOW
    }
}
