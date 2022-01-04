using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using System.Windows.Input;
using System.ComponentModel;
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
        public ICollectionView parcelListCollection { get; set; }
        public ParcelListWindow(BL.BLApi.Ibl busiAccess1)
        {
            InitializeComponent();
            busiAccess = busiAccess1;
            //DataContext = busiAccess.GetParcelToList();
            parcelListCollection = (CollectionView)CollectionViewSource.GetDefaultView(busiAccess.GetParcelToList());
            ParcelListView.ItemsSource = parcelListCollection;

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
            try
            {
                BL.BO.BOParcel parc = busiAccess.GetBOParcel(id);
                new ParcelWindow(busiAccess, parc).ShowDialog();
            }
            catch (BL.BLApi.EXParcelNotFound ex)
            {
                MainWindow.ErrorMsg(ex.ToString());
            }
            
        }

        private void btnCloseList_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void chkboxShowErased_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkboxByPriority_Unchecked(object sender, RoutedEventArgs e)
        {
            refreshGrouping();
        }
        private void chkboxByPriority_Checked(object sender, RoutedEventArgs e)
        {
            refreshGrouping();
            //if (chkboxSortPriority.IsChecked == null) return;
            if ((bool)chkboxSortPriority.IsChecked)
                parcelListCollection.GroupDescriptions.
                    Add(new PropertyGroupDescription(nameof(BL.BO.BOParcelToList.Priority)));
           
        }


        private void refreshGrouping()
        {
            var itemToRemove = parcelListCollection.GroupDescriptions.OfType<PropertyGroupDescription>()
     .FirstOrDefault(groupPropDescrip => groupPropDescrip.PropertyName == nameof(BL.BO.BOParcelToList.Priority));

            if (itemToRemove != null)
            {
                parcelListCollection.GroupDescriptions.Remove(itemToRemove);
            }
        }

    }
}
