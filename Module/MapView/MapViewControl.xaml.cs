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
        GMapMarker lastMarker;
        public MapViewControl()
        {
            InitializeComponent();
            InitControl();
        }

        private void InitControl()
        {
            GoogleMapProvider.Instance.ApiKey = "";

            // Configuration
            this.mapControl.MapProvider = GMapProviders.GoogleMap;
            this.mapControl.Position = new PointLatLng(37.5665, 126.9780);
            this.mapControl.Zoom = Convert.ToDouble(10);
            this.mapControl.MaxZoom = 25;
            this.mapControl.MinZoom = 0;
            this.mapControl.ShowCenter = false;
            this.mapControl.DragButton = MouseButton.Left;
            this.mapControl.IgnoreMarkerOnMouseWheel = true;
        }

        public void DrawMarker(double latitude, double longitude)
        {
            this.mapControl.Markers.Clear();
            PointLatLng point = new PointLatLng(latitude, longitude);
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
            this.lastMarker = marker;
        }

        public void ZoomMap(Dictionary<string, string> mapData, int zoomLevel)
        {
            this.ZoomMap(double.Parse(mapData.GetValueOrDefault("Latitude", "37.5665"))
                        , double.Parse(mapData.GetValueOrDefault("Longitude", "126.9780")), zoomLevel);
        }

        public void ZoomMap(double latitude, double longitude, int zoomLevel)
        {
            this.mapControl.Zoom = Convert.ToDouble(zoomLevel);
            this.mapControl.Position = new PointLatLng(latitude, longitude);
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
                    this.DrawMarker(double.Parse(imageProps.GetValueOrDefault("Latitude", "37.5665"))
                        , double.Parse(imageProps.GetValueOrDefault("Longitude", "126.9780")));
                }
            });
        }
    }
}
