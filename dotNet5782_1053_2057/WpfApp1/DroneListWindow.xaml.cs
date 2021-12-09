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
    /// Interaction logic for DroneList.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        IBL.Ibl busiAccess;
        
        public DroneListWindow(IBL.Ibl busiAccess1)
        {
            InitializeComponent();
            busiAccess = busiAccess1;
            DronesListView.ItemsSource = busiAccess.getBODroneList();

            
        }
    }
}
