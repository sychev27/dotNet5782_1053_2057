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
        public DroneWindow(IBL.Ibl _busiAccess)//to add a Drone
        {
            InitializeComponent();
            busiAccess = _busiAccess;
        }

        

        private void btnAddDrone_Click(object sender, RoutedEventArgs e)
        {
            int _id;
            Int32.TryParse(IdInput.Text, out _id);
            string _model = ModelInput.Text;
            int _stationId;
            Int32.TryParse(StationIdInput.Text, out _stationId);
            IDAL.DO.WeightCategories? weight = weightChoice;

            //if(weight == null)
            //    throw excepeiton!!
            

            busiAccess.addDrone(_id, _model, (IDAL.DO.WeightCategories)weight, _stationId);
        }

        private void btnLightWeight_Click(object sender, RoutedEventArgs e)
        {
            weightChoice = IDAL.DO.WeightCategories.light;
        }

        private void btnMediumWeight_Click(object sender, RoutedEventArgs e)
        {
            weightChoice = IDAL.DO.WeightCategories.medium;
        }
        private void btnHeavyWeight_Click(object sender, RoutedEventArgs e)
        {
            weightChoice = IDAL.DO.WeightCategories.heavy;
        }


    }
}
