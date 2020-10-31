using ExtremeEnviroment.Module.ImageList;
using ExtremeEnviroment.Module.ImageView;
using ExtremeEnviroment.Module.ImagePropView;
using ExtremeEnviroment.Module.ImageInspector;
using ExtremeEnviroment.Module.DataList;
using ExtremeEnviroment.Module.ChartView;
using ExtremeEnviroment.Module.Menu;
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
using ExtremeEnviroment.Model;

namespace ExtremeEnviroment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly private String _projectName;

        readonly private ProjectData _projectData;

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
        public MainWindow(String projectName, ProjectData projectData)
        {
            InitializeComponent();
            DataContext = this;
            _projectName = projectName;
            _projectData = projectData;
            _mainWindow = this;
            this.LoadProjectData();
        }

        private void LoadProjectData()
        {
            if (_projectData != null)
            {
                this.ImageList.LoadProjectDataList(this._projectData.images);
            }
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

        internal ChartViewControl GetChartViewControl()
        {
            return this.ChartView;
        }

        internal MenuControl GetMenuControl()
        {
            return this.Menu;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (this.Menu.hasModifiedContents())
            {
                if (MessageBox.Show("변경된 내용이 있습니다. 저장하시겠습니까?", "Alert", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    this.Menu.SaveProjectData();
                }

            }

            System.Windows.Application.Current.Shutdown();
        }

    }
}
