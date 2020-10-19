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

        public void DrawMarker(double latitude, double longtitude)
        {
            // this.mapControl.Markers.Clear();
            PointLatLng point = new PointLatLng(latitude, longtitude);
            GMapMarker marker = new GMapMarker(point)
            {
                Shape = new Image
                {
                    Width = 10,
                    Height = 10,
                    Source = new BitmapImage(new Uri("pack://application:,,,/Assets/marker.png"))
                }
            };
            this.mapControl.Markers.Add(marker);
        }

        public void Clear()
        {
            this.mapControl.Markers.Clear();
        }

        public void Refresh()
        {
            this.Clear();
            MainWindow mainWindow = ExtremeEnviroment.MainWindow._mainWindow;
            List<Model.ImageData> imageDataList = mainWindow.ImageList.GetImageDataList();
            imageDataList.ForEach(imageData =>
            {
                Dictionary<string, string> imageProps = imageData.ImageProps;
                if (imageProps.ContainsKey("Latitude") && imageProps.ContainsKey("Longitude"))
                {
                    this.DrawMarker(double.Parse(imageProps.GetValueOrDefault("Latitude"))
                        , double.Parse(imageProps.GetValueOrDefault("Longitude")));
                }
            });
        }
    }
}
