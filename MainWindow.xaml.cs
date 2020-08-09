using ExtremeEnviroment.Module.ImageList;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExtremeEnviroment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitLayout();
        }

        private void InitLayout()
        {
            ImageListControl imageListControl = new ImageListControl();
            
            DockPanel.SetDock(imageListControl, Dock.Left);
            mainPanel.Children.Add(imageListControl);
        }
    }
}
