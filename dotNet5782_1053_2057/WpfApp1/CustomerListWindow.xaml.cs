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
        }

        private void btnAddCustomer1_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow().ShowDialog(); 
        }

        private void Selector1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Selector2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnCloseList_Click(object sender, RoutedEventArgs e)
        {
            Close();

        }

        private void btnRefreshList_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(busiAccess).ShowDialog();
        }
    }
}
 // delete @