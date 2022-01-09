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
        bool registerMode; //this is true if the window is opened by a user
        
        public CustomerWindow(BL.BLApi.Ibl _busiAccess, bool register = false) //To Add a Customer
        {
            InitializeComponent();
            busiAccess = _busiAccess;
            //edit buttons and text boxes for Update Window:
            btnModifyCustomer.IsEnabled = false;
            btnModifyCustomer.Visibility = Visibility.Hidden;
            lstParcelList.Visibility = Visibility.Hidden;
            hideCustomerLogInBtns();

            btnEraseCust.IsEnabled = false;

            if (register)
            {
                btnAddCustomer.Content = "Register";
                registerMode = true;
            }
        }

        public CustomerWindow(BL.BLApi.Ibl _busiAccess, BL.BO.BOCustomer customer)
            //To Update a Customer (called from Customer List)
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
            hideCustomerLogInBtns();


            displayBOCustomer(busiAccess, customer);

            btnEraseCust.IsEnabled = false;
        }


        public CustomerWindow(BL.BLApi.Ibl _busiAccess, int custId)  :
            this(_busiAccess, _busiAccess.GetBOCustomer(custId))
            //called from Customer Log-in
        {
            hideCustomerLogInBtns(true);
        }





        private void displayBOCustomer(BL.BLApi.Ibl _busiAccess, BL.BO.BOCustomer bocustumer)
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
            MainWindow.ChangeTextColor(Colors.Black, tBlock_chooseCustomerId, tBlock_chooseName,
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
                    busiAccess.AddCustomer(_id, _name, _phone, _longitude, _latitude);
                    MessageBox.Show("Customer Added Successfully", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                }
                catch (BL.BLApi.EXCustomerAlreadyExists exception)
                {
                    //if Customer's Id already exists
                    MessageBox.Show(exception.printException(), "Error Message",
                        MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }
            else
                return;

            if(!registerMode) //if window opened from List...
                Close();
            else              // if window opened from LoginWindow
            {
                new AddUserWindow(busiAccess, _id).Show();
                Close();
            }

        }

        private void btnCancel1_Click(object sender, RoutedEventArgs e)
        {
            if (registerMode)
                new LoginWindow(busiAccess).Show();
            Close();

        }

        private void btnModifyCustomer_Click(object sender, RoutedEventArgs e)
        {
            //reset text color
            MainWindow.ChangeTextColor(Colors.Black, tBlock_choosePhone, tBlock_chooseName);

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

        
        private void btnEraseCust_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                busiAccess.EraseCustomer(thisCustomerId);
            }
            catch (BL.BLApi.EXCantDltCustWParcInDelivery ex)
            {
                MainWindow.ErrorMsg(ex.ToString());
            }
        }

        private void btnSendParcel_Click(object sender, RoutedEventArgs e)
        {
            //open new ParcelWindow(), send the Sender ID.... 
        }

        private void hideCustomerLogInBtns(bool isCustLogin = false)
        {
            //if (true) show the buttons. else: hide them
            btnLogOut.IsEnabled = isCustLogin;
            btnLogOut.Visibility = (!isCustLogin)? Visibility.Hidden : Visibility.Visible;
            btnSendParcel.IsEnabled = isCustLogin;
            btnSendParcel.Visibility = (!isCustLogin) ? Visibility.Hidden : Visibility.Visible;
            
            //(if isCustomer Login  -> hide these buttons!)
            btnCancel1.IsEnabled = !isCustLogin;
            btnCancel1.Visibility = (isCustLogin) ? Visibility.Hidden : Visibility.Visible;
            btnEraseCust.IsEnabled = !isCustLogin;
            btnEraseCust.Visibility = (isCustLogin) ? Visibility.Hidden : Visibility.Visible;
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow(busiAccess).Show(); 
            Close();
            
        }

        private void lstParcelList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BL.BO.BOParcelAtCustomer parcel = lstParcelList.SelectedItem as BL.BO.BOParcelAtCustomer;
            int id = parcel.Id;
            BL.BO.BOParcel parc = busiAccess.GetBOParcel(id);
            new ParcelWindow(busiAccess, parc).ShowDialog();
        }
    }
}
