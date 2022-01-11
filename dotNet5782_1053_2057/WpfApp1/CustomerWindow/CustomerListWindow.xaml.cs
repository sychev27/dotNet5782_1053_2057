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
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Globalization;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for CustomerListWindow.xaml
    /// </summary>
    public partial class CustomerListWindow : Window
    {
        BL.BLApi.Ibl busiAccess;

        public ICollectionView custCollectionView { get; set; }
        private string _customerFilter = string.Empty;
        public string CustomerFilter
        {
            get
            {
                return _customerFilter;
            }
            set
            {
                _customerFilter = value;
                OnPropertyChanged(nameof(CustomerFilter));
                custCollectionView.Refresh();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged( string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }




        public CustomerListWindow(BL.BLApi.Ibl busiAccess1)
        {
            InitializeComponent();
            busiAccess = busiAccess1;
            refreshList();

            custCollectionView.SortDescriptions.Add(new SortDescription(
                nameof(BL.BO.BOCustomerToList.Id), ListSortDirection.Ascending));
            custCollectionView.Filter = FilterByName;

        }
        private bool FilterByName(object obj)
        {
            if (obj is BL.BO.BOCustomerToList cust)
            {
                return cust.CustomerName.Contains(CustomerFilter, StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }
        private void btnAddCustomer1_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(busiAccess).ShowDialog();
        }

        

        private void CustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BL.BO.BOCustomerToList customer = CustomerListView.SelectedItem as BL.BO.BOCustomerToList;
            if (customer == null)
                return;
            
            int id = customer.Id;
            BL.BO.BOCustomer cust = busiAccess.GetBOCustomer(id);
            new CustomerWindow(busiAccess, cust).ShowDialog();
            refreshList();

        }

        private void btnCloseList_Click(object sender, RoutedEventArgs e)
        {
            Close();

        }

        private void btnRefreshList_Click(object sender, RoutedEventArgs e)
        {
            refreshList();
        }

        private void refreshList(bool getDeleted = false)
        {

            DataContext = busiAccess.GetCustToList(); // for grouping...
            custCollectionView = (CollectionView)CollectionViewSource.
                GetDefaultView(DataContext);
            CustomerListView.ItemsSource = custCollectionView;

            
        }


        

        private void chkboxNumParcels_Checked(object sender, RoutedEventArgs e)
        {
            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DataContext);
            //PropertyGroupDescription groupDescription = new PropertyGroupDescription("NumParcelsSentNotDelivered");
            //view.GroupDescriptions.Add(groupDescription);
        }
        private void chkboxNumParcels_Unchecked(object sender, RoutedEventArgs e)
        {
            //refreshList();
        }

        private void chkBoxGetErased_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkBoxGetErased_UnChecked(object sender, RoutedEventArgs e)
        {

        }
    }


    //END OF WINDOW
}


