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
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        BL.BLApi.Ibl busiAccess;
        int thisCustomerId;
        bool modelTBoxChanged = false;
        public CustomerWindow()
        {
            InitializeComponent();
            //edit buttons and text boxes for Update Window:
            btnModifyCustomer.IsEnabled = false;
            btnModifyCustomer.Visibility = Visibility.Hidden;
            lstParcelList.Visibility = Visibility.Hidden;
        }

        public CustomerWindow(BL.BLApi.Ibl _busiAccess,BL.BO.BOCustomer customer)
        {
            InitializeComponent();
            busiAccess = _busiAccess;

            tBoxCusIdInput.IsReadOnly = true;
            tBoxCusIdInput.BorderBrush = Brushes.Transparent;


            btnAddCustomer.IsEnabled = false;
            btnAddCustomer.Visibility = Visibility.Hidden;
            displayBOCustomer(customer);
        }

        private void displayBOCustomer(BL.BO.BOCustomer bocustumer)
        {
            thisCustomerId = bocustumer.Id;


            tBoxCusIdInput.Text = bocustumer.Id.ToString();
            tBoxNameInput.Text = bocustumer.Name;
            tBoxPhoneInput.Text = bocustumer.Phone;
            tBoxLongiInfo.Text = bocustumer.Location.Longitude.ToString();
            tBoxLatitInfo.Text = bocustumer.Location.Latitude.ToString();
            //lstParcelList.ItemsSource = bocustumer.

            //  tBlockCurrentLocationInfo.Text = busiAccess.GetDroneLocationString(bodrone.Id);

            //working on a function in BL..

        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            //reset text color
            changeTBlockColor(Colors.Black, tBlock_chooseCustomerId, tBlock_chooseName,
            tBlock_choosePhone, tBlockLongitude, tBlockLatitude);

            //(1) Receive Data
            int _id;
            int _phoneCheck;
            double _longitude;
            double _latitude;
            bool idSuccess = Int32.TryParse(tBoxCusIdInput.Text, out _id);
            bool phoneSuccess = Int32.TryParse(tBoxCusIdInput.Text, out _phoneCheck);
            bool longSuccess = double.TryParse(tBoxLongiInfo.Text, out _longitude);
            bool latSuccess = double.TryParse(tBoxLatitInfo.Text, out _latitude);
            string _name = tBoxNameInput.Text;
            string _phone = tBoxPhoneInput.Text;


            //(2) Check that Data is Valid
            bool validData = true;
            //check Id
            if (tBoxCusIdInput.Text == null || !idSuccess || _id <= 0)
            {
                tBlock_chooseCustomerId.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }


            if (tBoxNameInput.Text == null || tBoxNameInput.Text == "")
            {
                tBlock_chooseName.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }

            if (tBoxPhoneInput.Text == null || !phoneSuccess || _phoneCheck <= 0)
            {
                tBlock_choosePhone.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }

            if (tBoxLongiInfo.Text == null || !longSuccess || _longitude < 35 || _longitude > 36)
            {
                tBlockLongitude.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }

            if (tBoxLatitInfo.Text == null || !latSuccess || _latitude < 31 || _latitude > 32)
            {
                tBlockLatitude.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }

            //(3) Add Drone..
            if (validData)
            {
                try
                {
                    busiAccess.AddCustomer(_id,_name,_phone, _longitude, _latitude);
                    MessageBox.Show("Customer Added Successfully", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
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

        private void btnCancel1_Click(object sender, RoutedEventArgs e)
        {
            Close();

        }

        private void btnModifyCustomer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void changeTBlockColor(Color color, params TextBlock[] listTBlock)
        {
            foreach (var item in listTBlock)
            {
                item.Foreground = new SolidColorBrush(color);
            }
        }
    }
}
