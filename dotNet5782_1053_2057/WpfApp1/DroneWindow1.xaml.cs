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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL.Ibl busiAccess;
        IDAL.DO.WeightCategories? weightChoice = null;

        const string WEIGHTSELECTED = "Weight Selected!";
        const string CHOOSEWEIGHT = "Choose Maximum Weight Category:";
        const string DRONEADDED = "Drone Added Successfully!";

        public DroneWindow(IBL.Ibl _busiAccess, int num)//to add a Drone
        {
            InitializeComponent();
            busiAccess = _busiAccess;
        }

        public DroneWindow(IBL.Ibl _busiAccess, char let) //to update Drone
        {
            InitializeComponent();
            busiAccess = _busiAccess;
            btnAddDrone.IsEnabled = false;
            btnHeavyWeight.IsEnabled = false;
            btnLightWeight.IsEnabled = false;
            btnMediumWeight.IsEnabled = false;

            tBlock_chooseMaxWeight.Text = "Max Weight:";
            


        }

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            int _id;
            Int32.TryParse(tBoxIdInput.Text, out _id);
            string _model = tBoxModelInput.Text;
            int _stationId;
            Int32.TryParse(tBoxStationIdInput.Text, out _stationId);
            IDAL.DO.WeightCategories? weight = weightChoice;

            //if(weight == null)
            //    throw excepeiton!!

            busiAccess.addDrone(_id, _model, (IDAL.DO.WeightCategories)weight, _stationId);
            tBlock_chooseMaxWeight.Text = CHOOSEWEIGHT;
            tBlock_DroneAdded.Text = DRONEADDED;

        }

        private void btnLightWeight_Click(object sender, RoutedEventArgs e)
        {
            weightChoice = IDAL.DO.WeightCategories.light;
            tBlock_chooseMaxWeight.Text = WEIGHTSELECTED;
        }

        private void btnMediumWeight_Click(object sender, RoutedEventArgs e)
        {
            weightChoice = IDAL.DO.WeightCategories.medium;
            tBlock_chooseMaxWeight.Text = WEIGHTSELECTED;
        }
        private void btnHeavyWeight_Click(object sender, RoutedEventArgs e)
        {
            weightChoice = IDAL.DO.WeightCategories.heavy;
            tBlock_chooseMaxWeight.Text = WEIGHTSELECTED;
        }





    }
}
