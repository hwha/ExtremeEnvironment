using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using ExtremeEnviroment.Module.ImageInspector;
using ExtremeEnviroment.Module.DataList;
using System.Collections.ObjectModel;

namespace ExtremeEnviroment.Model
{
    public class ProjectData
    {
        public List<ProjectImage> images { get; set;  }
    }

    public class ProjectImage
    {
        public int index { get; set; }

        public TreeViewItem ImageTreeViewItem { get; set; }
        public BitmapImage Image { get; set; }
        public string ImageName { get; set; }
        public Dictionary<string, string> ImageProps { get; set; }
        public ObservableCollection<InspectorItem> InspectorItems { get; set; }
        public DataListItem DataListItem { get; set; }
    }
}
