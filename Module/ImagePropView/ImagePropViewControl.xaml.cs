using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using ExtremeEnviroment.Module.ImageList;


namespace ExtremeEnviroment.Module.ImagePropView
{
    /// <summary>
    /// ImagePropViewControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImagePropViewControl : UserControl
    {
        public ImagePropViewControl()
        {
            InitializeComponent();
        }

        private void DgImageProp_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        public void SetImageProps(Dictionary<String, String> metadataMap)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("PROP", typeof(string));
            dataTable.Columns.Add("VALUE", typeof(string));

            foreach (KeyValuePair<String, String> entry in metadataMap) 
            {
                dataTable.Rows.Add(new object[] { entry.Key, entry.Value });
            }



            DgImageProp.ItemsSource = dataTable.DefaultView;
            
        }

        private void DgImageProp_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            MainWindow mainWindow = ExtremeEnviroment.MainWindow._mainWindow;

            TextBlock props = DgImageProp.Columns[0].GetCellContent(DgImageProp.Items[e.Row.GetIndex()]) as TextBlock;
            TextBox value = e.EditingElement as TextBox;

            if (props != null && value != null)
            {
                ImageListControl imageListControl = mainWindow.GetImageListControl();

                Dictionary<string, string> metadataMap = new Dictionary<string, string>();
                metadataMap.Add(props.Text, value.Text);
                imageListControl.UpdateTreeItem(metadataMap);
            }
        }

        private void DgImageProp_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.Row.Item != null) 
            {
            }
        }
    }
}
