using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace ExtremeEnviroment.Model
{
    public class ImageData
    {
        public BitmapImage Image { get; set; }
        public string ImageName { get; set; }
        public Dictionary<string, string> ImageProps { get; set; }

    }
}
