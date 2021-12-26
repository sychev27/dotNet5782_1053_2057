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
        public CustomerWindow(BL.BLApi.Ibl _busiAccess)
        {
            InitializeComponent();
            busiAccess = _busiAccess;
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
            tBoxLatitInfo.IsReadOnly = true;
            tBoxLatitInfo.BorderBrush = Brushes.Transparent;
            tBoxLongiInfo.IsReadOnly = true;
            tBoxLongiInfo.BorderBrush = Brushes.Transparent;


            btnAddCustomer.IsEnabled = false;
            btnAddCustomer.Visibility = Visibility.Hidden;
            displayBOCustomer(busiAccess,customer);
        }

        private void displayBOCustomer(BL.BLApi.Ibl _busiAccess,BL.BO.BOCustomer bocustumer)
        {
            thisCustomerId = bocustumer.Id;


            tBoxCusIdInput.Text = bocustumer.Id.ToString();
            tBoxNameInput.Text = bocustumer.Name;
            tBoxPhoneInput.Text = bocustumer.Phone;
            tBoxLongiInfo.Text = bocustumer.Location.Longitude.ToString();
            tBoxLatitInfo.Text = bocustumer.Location.Latitude.ToString();
            lstParcelList.ItemsSource = _busiAccess.GetBOParcelAtCustomerList(bocustumer);

            //working on a function in BL..

        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            //reset text color
            changeTBlockColor(Colors.Black, tBlock_chooseCustomerId, tBlock_chooseName,
            tBlock_choosePhone, tBlockLongitude, tBlockLatitude);

            //(1) Receive Data
            int _id;
            Int64 _phoneCheck;
            double _longitude;
            double _latitude;
            bool idSuccess = Int32.TryParse(tBoxCusIdInput.Text, out _id);
            bool phoneSuccess = Int64.TryParse(tBoxPhoneInput.Text, out _phoneCheck);
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

            //check name
            if (tBoxNameInput.Text == null || tBoxNameInput.Text == "")
            {
                tBlock_chooseName.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }

            //check phone
            if (tBoxPhoneInput.Text == null || !phoneSuccess || _phoneCheck <= 0)
            {
                tBlock_choosePhone.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }

            //check longitude
            if (tBoxLongiInfo.Text == null || !longSuccess || _longitude < 35 || _longitude > 36)
            {
                tBlockLongitude.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }

            //check latitude
            if (tBoxLatitInfo.Text == null || !latSuccess || _latitude < 31 || _latitude > 32)
            {
                tBlockLatitude.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }

            //(3) Add Customer..
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
                    //if Customer's Id already exists
                    MessageBox.Show(exception.printException(), "Error Message",
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
            //reset text color
            changeTBlockColor(Colors.Black, tBlock_choosePhone, tBlock_chooseName);

            //(1) Receive Data
            int _id;
            Int64 _phoneCheck;
            Int32.TryParse(tBoxCusIdInput.Text, out _id);
            bool phoneSuccess = Int64.TryParse(tBoxPhoneInput.Text, out _phoneCheck);
            string _phone = tBoxPhoneInput.Text;
            string _name = tBoxNameInput.Text;

            //(2) Check that Data is Valid
            bool validData = true;

            //check name
            if (tBoxNameInput.Text == null || tBoxNameInput.Text == "")
            {
                tBlock_chooseName.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }

            //check phone
            if (tBoxPhoneInput.Text == null || !phoneSuccess || _phoneCheck <= 0)
            {
                tBlock_choosePhone.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }


            if (validData)
            {
                busiAccess.ModifyCust(_id, _name, _phone);
                MessageBox.Show("Customer Name and Phone Changed", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                Close();
            }
            else
                return;
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
