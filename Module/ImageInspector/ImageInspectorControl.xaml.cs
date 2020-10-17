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

namespace ExtremeEnviroment.Module.ImageInspector
{
    /// <summary>
    /// ImageInspectorControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageInspectorControl : UserControl
    {
        public InspectorItem SelectedItem;
        public ImageInspectorControl()
        {
            InitializeComponent();
        }
        public void AddRow(int index, InspectorItem item)
        {
            ItemCollection inspectorItems = DgInspector.Items;
            item.INDEX = index;
            item.NUM = inspectorItems.Count + 1;
            inspectorItems.Add(item);
        }
        public void AddRow(int index, Dictionary<string, int> row)
        {
            ItemCollection inspectorItems = DgInspector.Items;

            InspectorItem inspectorItem = new InspectorItem(row)
            {
                INDEX = index,
                NUM = inspectorItems.Count + 1
            };

            inspectorItems.Add(inspectorItem);
        }
        public void AddRow(int idx, int numPixel, string pixel)
        {
            ItemCollection inspectorItems = DgInspector.Items;
            string[] split = pixel.Split(",");
            inspectorItems.Add(new InspectorItem {
                INDEX = idx,
                NUM = inspectorItems.Count+1,
                INSPECTOR = "영역",
                NUM_PIXEL = numPixel,
                AVG_TEMP = split[0],
                MAX_TEMP = split[1],
                MIN_TEMP = split[2]
            });
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
