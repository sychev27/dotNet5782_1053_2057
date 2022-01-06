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
    /// Interaction logic for CustomerListWindow.xaml
    /// </summary>
    public partial class CustomerListWindow : Window
    {
        BL.BLApi.Ibl busiAccess;
        public CustomerListWindow(BL.BLApi.Ibl busiAccess1)
        {
            InitializeComponent();
            busiAccess = busiAccess1;
            DataContext = busiAccess.GetCustToList();
            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DataContext);
            //PropertyGroupDescription groupDescription = new PropertyGroupDescription("NumParcelsSentNotDelivered");
            //view.GroupDescriptions.Add(groupDescription);

        }

        private void btnAddCustomer1_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(busiAccess).ShowDialog();
        }

        private void Selector1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Selector2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BL.BO.BOCustomerToList customer = CustomerListView.SelectedItem as BL.BO.BOCustomerToList;
            int id = customer.Id;
            BL.BO.BOCustomer cust = busiAccess.GetBOCustomer(id);
            new CustomerWindow(busiAccess, cust).ShowDialog();

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
            CustomerListView.ItemsSource = null;
            CustomerListView.ItemsSource = busiAccess.GetCustToList();

            //if((bool)chkboxNumParcels.IsChecked)
            //{
            //    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DataContext);
            //    PropertyGroupDescription groupDescription = new PropertyGroupDescription("NumParcelsSentNotDelivered");
            //    view.GroupDescriptions.Add(groupDescription);
            //} 
            //else
            //{
            //    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DataContext);
            //    PropertyGroupDescription groupDescription = new PropertyGroupDescription("NumParcelsSentNotDelivered");
            //    view.GroupDescriptions.Remove(groupDescription);
            //}

        }


        //<TextBox Grid.Row="0" Text="{Binding EmployeesFilter, UpdateSourceTrigger=PropertyChanged}" />


        private void chkboxNumParcels_Checked(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(DataContext);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("NumParcelsSentNotDelivered");
            view.GroupDescriptions.Add(groupDescription);
        }
        private void chkboxNumParcels_Unchecked(object sender, RoutedEventArgs e)
        {
            refreshList();
        }

        //{
        //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //    {
        //        bool param = bool.Parse(value.ToString());
        //        if (param == true)
        //            return Visibility.Visible;
        //        else
        //            return Visibility.Collapsed;
        //    }

        //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //END OF WINDOWS
    }
}

