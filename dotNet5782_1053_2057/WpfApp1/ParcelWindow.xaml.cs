﻿using System;
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
        
        public ParcelWindow(BL.BLApi.Ibl _busiAccess)
        //To Add a Parcel 
        {
            InitializeComponent();
            busiAccess = _busiAccess;
            cmbWeightCategory.ItemsSource = Enum.GetValues(typeof(BL.BO.Enum.WeightCategories));
            cmbPriority.ItemsSource = Enum.GetValues(typeof(BL.BO.Enum.Priorities));

            btnModifyParcel.IsEnabled = false;
            btnModifyParcel.Visibility = Visibility.Hidden;
            tBlock_chooseParcelId.Visibility = Visibility.Hidden;
            tBlockTimeOfAssignment.Visibility = Visibility.Hidden;
            tBlockTimeOfCollection.Visibility = Visibility.Hidden;
            tBlockTimeOfCreation.Visibility = Visibility.Hidden;
            tBlockTimeOfDelivery.Visibility = Visibility.Hidden;
            tBoxParcIdInput.Visibility = Visibility.Hidden;
            tBoxTimeOfAssignment.Visibility = Visibility.Hidden;
            tBoxTimeOfCollection.Visibility = Visibility.Hidden;
            tBoxTimeOfCreation.Visibility = Visibility.Hidden;
            tBoxTimeOfDelivery.Visibility = Visibility.Hidden;

            btnEraseParce.IsEnabled = false;
        }

        public ParcelWindow(BL.BLApi.Ibl _busiAccess, BL.BO.BOParcel parcel)
        //To Update a Parcel (called from Parcel List)
        {
            InitializeComponent();
            busiAccess = _busiAccess;
            cmbWeightCategory.ItemsSource = Enum.GetValues(typeof(BL.BO.Enum.WeightCategories));
            cmbPriority.ItemsSource = Enum.GetValues(typeof(BL.BO.Enum.Priorities));

            btnAddParcel.IsEnabled = false;
            btnAddParcel.Visibility = Visibility.Hidden;
            
            displayBOParcel(busiAccess, parcel);

        }

        private void displayBOParcel(BL.BLApi.Ibl _busiAccess, BL.BO.BOParcel boparcel)
        {
            tBoxParcIdInput.Text = boparcel.Id.ToString();
            tBoxSenderId.Text = boparcel.Sender.Id.ToString();
            tBlockNameOfSender.Text = busiAccess.GetBOCustomer(boparcel.Sender.Id).Name;
            tBoxReceiverId.Text = boparcel.Receiver.Id.ToString();
            tBlockNameOfReceiver.Text = busiAccess.GetBOCustomer(boparcel.Receiver.Id).Name;
            
            cmbWeightCategory.SelectedIndex = (int)boparcel.WeightCategory;
            cmbWeightCategory.IsReadOnly = true;
            cmbWeightCategory.IsEnabled = false;

            cmbPriority.SelectedIndex = (int)boparcel.Priority;
            
            if(boparcel.TimeOfDelivery != null)
            {
                cmbPriority.IsReadOnly = true;
                cmbPriority.IsEnabled = false;
                btnModifyParcel.IsEnabled = false;
            }

            tBoxTimeOfCreation.Text = boparcel.TimeOfCreation.ToString();
            tBoxTimeOfAssignment.Text = boparcel.TimeOfAssignment.ToString();

            try
            {
                tBoxDroneIdOutput.Text = 
                    busiAccess.GetDroneIdOfParcel(boparcel.Id).ToString();
            }
            catch (BL.BLApi.EXDroneNotFound)
            {
                tBoxDroneIdOutput.Text = "Not yet assigned to a drone";
            }

            tBoxTimeOfCollection.Text = boparcel.TimeOfCollection.ToString();
            tBoxTimeOfDelivery.Text = boparcel.TimeOfDelivery.ToString();

        }

        private void btnAddParcel_Click(object sender, RoutedEventArgs e)
        {
            //reset text color
            MainWindow.ChangeTextColor(Colors.Black, tBlock_chooseParcelId, tBlock_chooseSenderId,
            tBlockWeightCategory, tBlockPriority);

            //(1) Receive Data
            
            int senderId;
            int receiverId;
            bool senderIdSuccess = Int32.TryParse(tBoxSenderId.Text, out senderId);
            bool receiverIdSuccess = Int32.TryParse(tBoxReceiverId.Text, out receiverId);
            DalXml.DO.WeightCategories? weight = (DalXml.DO.WeightCategories)cmbWeightCategory.SelectedIndex;
            DalXml.DO.Priorities? priority = (DalXml.DO.Priorities)cmbPriority.SelectedIndex;

            //(2) Check that Data is Valid
            bool validData = true;
            
            //check senderId
            if (tBoxSenderId.Text == null || !senderIdSuccess || senderId <= 0)
            {
                tBlock_chooseSenderId.Foreground = new SolidColorBrush(Colors.Red);
                validData = false;
            }
            //check receiver Id
            if (tBoxReceiverId.Text == null || !receiverIdSuccess || receiverId <= 0
                || receiverId == senderId)
            {
                tBlock_chooseReceiverId.Foreground = new SolidColorBrush(Colors.Red);
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
                    busiAccess.AddParcel(senderId, receiverId, weight,priority);
                    MessageBox.Show("Parcel Added Successfully", "SUCCESS", 
                        MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                    Close();
                }
                catch (BL.BLApi.EXCustomerAlreadyExists exception)
                {
                    //if Parcels's Id already exists
                    MainWindow.ErrorMsg(exception.ToString());
                }
            }
            else
                return;

           


        }

        private void btnCancel1_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnModifyParcel_Click(object sender, RoutedEventArgs e)
        {
            busiAccess.ModifyParcel(Int32.Parse(tBoxParcIdInput.Text),
                (BL.BO.Enum.Priorities)cmbPriority.SelectedItem);
            MessageBox.Show("Priority modified successfully", "SUCCESS", 
                MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            Close();   
        }

        private void btnEraseparc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                busiAccess.EraseParcel(Int32.Parse(tBoxParcIdInput.Text));
                MessageBox.Show("Parcel Erased Successfully", "SUCCESS",
                        MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                Close();
            }
            catch (BL.BLApi.EXCantDltParAlrdyAssgndToDrone ex)
            {
                MainWindow.ErrorMsg(ex.ToString());
            }
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
            Int32.TryParse(tBoxReceiverId.Text, out _id);
            BL.BO.BOCustomer custumer = busiAccess.GetBOCustomer(_id);
            new CustomerWindow(busiAccess, custumer).ShowDialog();
        }

        private void cmbWeightCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbPriority_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void tBoxReceiverId_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tBoxDroneIdOutput_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int droneId;
            bool droneIdSuccess = Int32.TryParse(tBoxDroneIdOutput.Text, out droneId);
            if (droneIdSuccess)
                new DroneWindow(busiAccess, busiAccess.GetBODrone(droneId)).ShowDialog();
            else
                return;
        }
    }
}