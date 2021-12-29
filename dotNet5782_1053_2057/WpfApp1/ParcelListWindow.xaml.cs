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
            refreshList();
        }

        private void refreshList(bool getDeleted = false)
        {
            ParcelListView.ItemsSource = null;
            ParcelListView.ItemsSource = busiAccess.GetParcelToList();

        }

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(busiAccess).ShowDialog();
        }

        private void PurcelListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BL.BO.BOParcelToList parcel = ParcelListView.SelectedItem as BL.BO.BOParcelToList;
            int id = parcel.Id;
            BL.BO.BOParcel parc = busiAccess.GetBOParcel(id);
            new ParcelWindow(busiAccess,parc).ShowDialog();
        }

        private void btnCloseList_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
