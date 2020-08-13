using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace ExtremeEnviroment.Module.MapView
{
    /// <summary>
    /// Interaction logic for MapViewControl.xaml
    /// </summary>
    public partial class MapViewControl : UserControl
    {
        private GMapControl mapControl;
        public MapViewControl()
        {
            InitializeComponent();
            InitControl();
        }

        private void InitControl()
        {
            GoogleMapProvider.Instance.ApiKey = "";

            this.mapControl = new GMapControl();

            // Configuration
            mapControl.MapProvider = GMapProviders.GoogleMap;
            mapControl.Position = new PointLatLng(37.5665, 126.9780);
            mapControl.Zoom = 18;
            mapControl.MaxZoom = 24;
            mapControl.MinZoom = 2;
            mapControl.ShowCenter = false;
            mapControl.DragButton = MouseButton.Left;

            this.groupBox.Content = mapControl;
        }

        public void create_marker(int latitude, int longtitude)
        {
            PointLatLng point = mapControl.FromLocalToLatLng(latitude, longtitude);
            GMapMarker marker = new GMapMarker(point)
            {
                Shape = new Ellipse
                {
                    Width = 10,
                    Height = 10,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1.5
                }
            };
            this.mapControl.Markers.Add(marker);
        }
    }
}
