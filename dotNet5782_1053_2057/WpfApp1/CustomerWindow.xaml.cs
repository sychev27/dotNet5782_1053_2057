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
            tBlockLatitInfo.Visibility = Visibility.Hidden;
            tBlockLatitude.Visibility = Visibility.Hidden;
            tBlockLongiInfo.Visibility = Visibility.Hidden;
            tBlockLongitude.Visibility = Visibility.Hidden;
            btnModifyCustomer.IsEnabled = false;
            btnModifyCustomer.Visibility = Visibility.Hidden;
        }

        public CustomerWindow(BL.BLApi.Ibl _busiAccess)
        {
            InitializeComponent();
            busiAccess = _busiAccess;

            tBoxCusIdInput.IsReadOnly = true;
            tBoxCusIdInput.BorderBrush = Brushes.Transparent;


            btnAddCustomer.IsEnabled = false;
            btnAddCustomer.Visibility = Visibility.Hidden;
            displayBOCustomer(busiAccess.GetBOCustomer(1));
        }

        private void displayBOCustomer(BL.BO.BOCustomer bocustumer)
        {
            thisCustomerId = bocustumer.Id;


            tBoxCusIdInput.Text = bocustumer.Id.ToString();
            tBoxNameInput.Text = bocustumer.Name;
            tBoxPhoneInput.Text = bocustumer.Phone;
            tBlockLongiInfo.Text = bocustumer.Location.Longitude.ToString();
            tBlockLatitInfo.Text = bocustumer.Location.Latitude.ToString();

            //  tBlockCurrentLocationInfo.Text = busiAccess.GetDroneLocationString(bodrone.Id);

            //working on a function in BL..

        }

        private void btnAddCustomer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel1_Click(object sender, RoutedEventArgs e)
        {
            Close();

        }

        private void btnModifyCustomer_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
