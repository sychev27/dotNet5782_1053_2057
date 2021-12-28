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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        BL.BLApi.Ibl busiAccess;
        int thisParcelId;
        bool registerMode;

        public ParcelWindow(BL.BLApi.Ibl _busiAccess, BL.BO.BOParcel parcel)
        //To Update a Customer (called from Customer List)
        {
            InitializeComponent();
            busiAccess = _busiAccess;

            //tBoxCusIdInput.IsReadOnly = true;
            //tBoxCusIdInput.BorderBrush = Brushes.Transparent;
            //tBoxLatitInfo.IsReadOnly = true;
            //tBoxLatitInfo.BorderBrush = Brushes.Transparent;
            //tBoxLongiInfo.IsReadOnly = true;
            //tBoxLongiInfo.BorderBrush = Brushes.Transparent;


            btnAddParcel.IsEnabled = false;
            btnAddParcel.Visibility = Visibility.Hidden;
            //hideCustomerLogInBtns();


            displayBOCustomer(busiAccess, parcel);

            btnEraseParce.IsEnabled = false;
        }

        private void displayBOCustomer(BL.BLApi.Ibl _busiAccess, BL.BO.BOParcel boparcel)
        {
            thisParcelId = boparcel.Id;


            tBoxParcIdInput.Text = boparcel.Id.ToString();
            tBoxSenderName.Text = boparcel.Sender.Name;
            tBoxReceiverName.Text = boparcel.Receiver.Name;
            tBoxWeightCategory.Text = boparcel.WeightCategory.ToString();
            tBoxPriority.Text = boparcel.Priority.ToString();
            tBoxTimeOfCreation.Text = boparcel.TimeOfCreation.ToString();
            tBoxTimeOfAssignment.Text = boparcel.TimeOfAssignment.ToString();
            tBoxTimeOfCollection.Text = boparcel.TimeOfCollection.ToString();
            tBoxTimeOfDelivery.Text = boparcel.TimeOfDelivery.ToString();


            //working on a function in BL..

        }

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel1_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnModifyParcel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEraseparc_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
