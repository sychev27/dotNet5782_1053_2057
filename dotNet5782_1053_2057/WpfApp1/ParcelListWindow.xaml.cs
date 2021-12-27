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
    /// Interaction logic for ParcelListWindow.xaml
    /// </summary>
    public partial class ParcelListWindow : Window
    {
        BL.BLApi.Ibl busiAccess;
        public ParcelListWindow(BL.BLApi.Ibl busiAccess1)
        {
            InitializeComponent();
            busiAccess = busiAccess1;
            DataContext = busiAccess.GetParcelToList();
        }

        private void Selector2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Selector1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnRefreshList_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PurcelListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnCloseList_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
