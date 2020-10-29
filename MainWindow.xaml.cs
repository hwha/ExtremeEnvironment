using ExtremeEnviroment.Module.ImageList;
using ExtremeEnviroment.Module.ImageView;
using ExtremeEnviroment.Module.ImagePropView;
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
using ExtremeEnviroment.Module.ImageInspector;
using ExtremeEnviroment.Module.DataList;
using ExtremeEnviroment.Model;

namespace ExtremeEnviroment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly private String _projectName;

        public static MainWindow _mainWindow;

        public string MainWindowTitle
        {
            get { return this._projectName; }
            set { this.Title = _projectName; }
        }

        public MainWindow(String projectName)
        {
            InitializeComponent();
            DataContext = this;
            _projectName = projectName;
            _mainWindow = this;
        }

        internal ImageData CurrentImageData
        {
            get { return this.ImageList.GetCurrentImageData(); }
        }

        internal ImageViewControl GetImageViewControl()
        {
            return this.ImageViewer;
        }

        internal ImagePropViewControl GetImagePropViewControl()
        {
            return this.ImagePropView;
        }

        internal ImageListControl GetImageListControl()
        {
            return this.ImageList;
        }

        internal ImageInspectorControl GetImageInspectorControl()
        {
            return this.ImageInspector;
        }

        internal DataListControl GetDatListControl()
        {
            return this.DataList;
        }
    }
}
