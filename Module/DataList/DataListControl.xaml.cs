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
using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using ExtremeEnviroment.Module.ImageInspector;
using ExtremeEnviroment.Model;

namespace ExtremeEnviroment.Module.DataList
{
    /// <summary>
    /// DataListControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DataListControl : UserControl
    {
        public ObservableCollection<DataListItem> currentDataListItems;
        public ObservableCollection<DataListItem> CurrentDataListItems
        {
            get { return this.currentDataListItems; }
        }

        public DataListControl()
        {
            InitializeComponent();
            currentDataListItems = new ObservableCollection<DataListItem>();
        }

        public void SetDataListItems(List<ImageData> imageDataList)
        {
            this.currentDataListItems = new ObservableCollection<DataListItem>();

            foreach (ImageData imageData in imageDataList)
            {
                if (imageData.InspectorItems != null && imageData.InspectorItems.Count > 0)
                {
                    imageData.DataListItem = this.UpdateDataList(imageData.DataListItem.FILE_NAME, imageData.InspectorItems);
                }

                currentDataListItems.Add(imageData.DataListItem);

            }
            this.RefreshDataGrid();
        }

        public DataListItem UpdateDataList(string fileName, ObservableCollection<InspectorItem> inspectorItems)
        {
            int NumPixel = inspectorItems.Sum(data => data.NUM_PIXEL);
            double AvgTemp = inspectorItems.Average(data => Convert.ToDouble(data.AVG_TEMP));
            double MaxTemp = inspectorItems.Max(data => Convert.ToDouble(data.MAX_TEMP));
            double MinTemp = inspectorItems.Min(data => Convert.ToDouble(data.MIN_TEMP));
            double StdDev = 0;
            return this.UpdateDataList(fileName, NumPixel, AvgTemp.ToString(), MaxTemp.ToString(), MinTemp.ToString(), StdDev.ToString());
        }

        public DataListItem UpdateDataList(string fileName, int numPixel, string avgTemp, string maxTemp, string minTemp, string stdDev)
        {
            return new DataListItem
            {
                FILE_NAME = fileName,
                NUM_PIXEL = numPixel,
                AVG_TEMP = avgTemp,
                MAX_TEMP = maxTemp,
                MIN_TEMP = minTemp,
                STD_DEV = stdDev
            };
        }

        private void RefreshDataGrid()
        {
            this.DgDataList.ItemsSource = null;
            this.DgDataList.ItemsSource = this.currentDataListItems;
        }

    }

    public class DataListItem
    {
        public DataListItem()
        {
        }
        public DataListItem(Dictionary<string, int> keyValuePairs)
        {
            this.FromDictionary(keyValuePairs);
        }
        public string FILE_NAME { get; set; }
        public int NUM_PIXEL { get; set; }
        public string AVG_TEMP { get; set; }
        public string MAX_TEMP { get; set; }
        public string MIN_TEMP { get; set; }
        public string STD_DEV { get; set; }

        public void FromDictionary(Dictionary<string, int> keyValuePairs)
        {
            this.FILE_NAME = keyValuePairs.GetValueOrDefault("FILE_NAME").ToString();
            this.NUM_PIXEL = keyValuePairs.GetValueOrDefault("NUM_PIXEL");
            this.AVG_TEMP = keyValuePairs.GetValueOrDefault("AVG_TEMP").ToString();
            this.MAX_TEMP = keyValuePairs.GetValueOrDefault("MAX_TEMP").ToString();
            this.MIN_TEMP = keyValuePairs.GetValueOrDefault("MIN_TEMP").ToString();
            this.STD_DEV = keyValuePairs.GetValueOrDefault("STD_DEV").ToString();
        }
    }
}
