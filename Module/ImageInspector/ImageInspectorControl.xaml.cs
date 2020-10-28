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
using System.Security.Cryptography;
using ExtremeEnviroment.Model;
using System.Collections;
using System.Collections.ObjectModel;

namespace ExtremeEnviroment.Module.ImageInspector
{
    /// <summary>
    /// ImageInspectorControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageInspectorControl : UserControl
    {
        public ObservableCollection<InspectorItem> currentInspectorItems;
        public ObservableCollection<InspectorItem> CurrentInspectorItems {
            get { return this.currentInspectorItems; }
        }

        public InspectorItem SelectedItem;
        public ImageInspectorControl()
        {
            InitializeComponent();
            currentInspectorItems = new ObservableCollection<InspectorItem>();
        }

        public Boolean CheckedArea()
        {
            return this.areaRadio.IsChecked == true;
        }

        public Boolean CheckedPixel()
        {
            return this.pixelRadio.IsChecked == true;
        }

        public void AddRow(int index, Dictionary<string, int> row)
        {
            this.AddRow(index, new InspectorItem(row));
        }

        public void AddRow(int index, int numPixel, string pixel)
        {
            string[] split = pixel.Split(",");
            this.AddRow(index, new InspectorItem
            {
                INSPECTOR = "영역",
                NUM_PIXEL = numPixel,
                AVG_TEMP = split[0],
                MAX_TEMP = split[1],
                MIN_TEMP = split[2]
            });
        }

        public void AddRow(int index, InspectorItem item)
        {
            item.INDEX = index;
            item.NUM = DgInspector.Items.Count + 1;
            currentInspectorItems.Add(item);
            this.RefreshDataGrid();

            MainWindow mainWindow = ExtremeEnviroment.MainWindow._mainWindow;
         
            ImageData imageData = mainWindow.CurrentImageData;
            imageData.InspectorItems = (ObservableCollection<InspectorItem>)DgInspector.ItemsSource;
        }

        public void SetIspectItemList(ObservableCollection<InspectorItem> inspectorItems)
        {
            this.currentInspectorItems = inspectorItems;
            this.RefreshDataGrid();
        }

        private void RefreshDataGrid()
        {
            this.DgInspector.ItemsSource = null;
            this.DgInspector.ItemsSource = this.currentInspectorItems;
        }

        private void DgInspectorMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                DataGrid dataGrid = (DataGrid) sender;
                if (dataGrid != null && dataGrid.SelectedItems != null && dataGrid.SelectedItems.Count == 1)
                {
                    InspectorItem selectedItem = (InspectorItem)dataGrid.SelectedItem;
                    ExtremeEnviroment.MainWindow._mainWindow.ImageViewer.DrawRectangle(selectedItem.X, selectedItem.Y, selectedItem.Width, selectedItem.Height);
                }
            }
        }
    }

    public class InspectorItem
    {
        public InspectorItem()
        {
        }
        public InspectorItem(Dictionary<string, int> keyValuePairs)
        {
            this.FromDictionary(keyValuePairs);
        }
        public int INDEX { get; set; }
        public int NUM { get; set; }
        public string INSPECTOR { get; set; }
        public int NUM_PIXEL { get; set; }
        public string AVG_TEMP { get; set; }
        public string MAX_TEMP { get; set; }
        public string MIN_TEMP { get; set; }
        // not visible
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }


        public void FromDictionary(Dictionary<string, int> keyValuePairs)
        {
            this.NUM_PIXEL = keyValuePairs.GetValueOrDefault("NUM_PIXEL");
            this.AVG_TEMP = keyValuePairs.GetValueOrDefault("AVG_TEMP").ToString();
            this.MAX_TEMP = keyValuePairs.GetValueOrDefault("MAX_TEMP").ToString();
            this.MIN_TEMP = keyValuePairs.GetValueOrDefault("MIN_TEMP").ToString();
            this.X = keyValuePairs.GetValueOrDefault("X");
            this.Y = keyValuePairs.GetValueOrDefault("Y");
            this.Width = keyValuePairs.GetValueOrDefault("Width");
            this.Height = keyValuePairs.GetValueOrDefault("Height");
        }
    }
}
