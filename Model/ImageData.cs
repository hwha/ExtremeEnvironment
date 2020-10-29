using ExtremeEnviroment.Module.ImageInspector;
using ExtremeEnviroment.Module.DataList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ExtremeEnviroment.Model
{
    public class ImageData
    {
        public TreeViewItem ImageTreeViewItem { get; set; }
        public BitmapImage Image { get; set; }
        public string ImageName { get; set; }
        public Dictionary<string, string> ImageProps { get; set; }
        public ObservableCollection<InspectorItem> InspectorItems { get; set; }
        public DataListItem DataListItem { get; set; }
    }
}
