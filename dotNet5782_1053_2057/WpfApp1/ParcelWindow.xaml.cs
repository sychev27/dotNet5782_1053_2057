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


        public ParcelWindow(BL.BLApi.Ibl _busiAccess)
        //To Add a Parcel 
        {
            InitializeComponent();
            busiAccess = _busiAccess;
            cmbWeightCategory.ItemsSource = Enum.GetValues(typeof(BL.BO.Enum.WeightCategories));
            cmbPriority.ItemsSource = Enum.GetValues(typeof(BL.BO.Enum.Priorities));

            btnModifyParcel.IsEnabled = false;
            btnModifyParcel.Visibility = Visibility.Hidden;
            tBlock_chooseReceiverId.Visibility = Visibility.Hidden;
            tBlockTimeOfAssignment.Visibility = Visibility.Hidden;
            tBlockTimeOfCollection.Visibility = Visibility.Hidden;
            tBlockTimeOfCreation.Visibility = Visibility.Hidden;
            tBlockTimeOfDelivery.Visibility = Visibility.Hidden;
            tBoxReceiverId.Visibility = Visibility.Hidden;
            tBoxTimeOfAssignment.Visibility = Visibility.Hidden;
            tBoxTimeOfCollection.Visibility = Visibility.Hidden;
            tBoxTimeOfCreation.Visibility = Visibility.Hidden;
            tBoxTimeOfDelivery.Visibility = Visibility.Hidden;



            //hideCustomerLogInBtns();




            btnEraseParce.IsEnabled = false;
        }

        public ParcelWindow(BL.BLApi.Ibl _busiAccess, BL.BO.BOParcel parcel)
        //To Update a Parcel (called from Parcel List)
        {
            InitializeComponent();
            busiAccess = _busiAccess;
            cmbWeightCategory.ItemsSource = Enum.GetValues(typeof(BL.BO.Enum.WeightCategories));
            cmbPriority.ItemsSource = Enum.GetValues(typeof(BL.BO.Enum.Priorities));

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
            tBoxSenderId.Text = boparcel.Sender.Id.ToString();
            tBoxReceiverId.Text = boparcel.Receiver.Id.ToString();

            cmbWeightCategory.SelectedIndex = (int)boparcel.WeightCategory;
            cmbWeightCategory.IsReadOnly = true;
            cmbWeightCategory.IsEnabled = false;

            cmbPriority.SelectedIndex = (int)boparcel.Priority;
            cmbPriority.IsReadOnly = true;
            cmbPriority.IsEnabled = false;

            tBoxTimeOfCreation.Text = boparcel.TimeOfCreation.ToString();
            tBoxTimeOfAssignment.Text = boparcel.TimeOfAssignment.ToString();
            tBoxTimeOfCollection.Text = boparcel.TimeOfCollection.ToString();
            tBoxTimeOfDelivery.Text = boparcel.TimeOfDelivery.ToString();


            //working on a function in BL..

        }

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            //reset text color
            MainWindow.ChangeTextColor(Colors.Black, tBlock_chooseParcelId, tBlock_chooseSenderId,
            tBlockWeightCategory, tBlockPriority);

            //(1) Receive Data
            int parcId;
            int senderId;
            bool parcIdSuccess = Int32.TryParse(tBoxParcIdInput.Text, out parcId);
            bool senderIdSuccess = Int32.TryParse(tBoxParcIdInput.Text, out senderId);
            DalXml.DO.WeightCategories? weight = (DalXml.DO.WeightCategories)cmbWeightCategory.SelectedIndex;
            DalXml.DO.Priorities? priority = (DalXml.DO.Priorities)cmbPriority.SelectedIndex;

            //(2) Check that Data is Valid
            bool validData = true;
            //check parcelId
            if (tBoxParcIdInput.Text == null || !parcIdSuccess || parcId <= 0)
            {
                tBlock_chooseParcelId.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }

            //check senderId
            if (tBoxParcIdInput.Text == null || !senderIdSuccess || senderId <= 0)
            {
                tBlock_chooseSenderId.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }

            //check weight categories
            if (weight == null || (int)weight == -1)  
            {
                tBlockWeightCategory.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }

            //check priority 
            if (priority == null || (int)priority == -1)
            {
                tBlockPriority.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }

            //(3) Add Parcel..
            if (validData)
            {
                try
                {
                    busiAccess.AddParcel(senderId, parcId, weight,priority);
                    MessageBox.Show("Parcel Added Successfully", "SUCCESS", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                    Close();
                }
                catch (BL.BLApi.EXCustomerAlreadyExists exception)
                {
                    //if Parcels's Id already exists
                    MessageBox.Show(exception.printException(), "Error Message",
                        MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                }
            }
            else
                return;

           // if (!registerMode) //if window opened from List...
             //   Close();
           // else              // if window opened from LoginWindow
           // {
              //  new AddUserWindow(busiAccess, _id).Show();
               // Close();
          //  }


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

        private void tBoxSenderId_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int _id;
            Int32.TryParse(tBoxSenderId.Text, out _id);
            BL.BO.BOCustomer custumer = busiAccess.GetBOCustomer(_id);
            new CustomerWindow(busiAccess,custumer).ShowDialog();
        }

        private void tBoxReceiverId_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int _id;
            Int32.TryParse(tBoxSenderId.Text, out _id);
            BL.BO.BOCustomer custumer = busiAccess.GetBOCustomer(_id);
            new CustomerWindow(busiAccess, custumer).ShowDialog();
        }

        private void cmbWeightCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbPriority_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
