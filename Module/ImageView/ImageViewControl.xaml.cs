using ExtremeEnviroment.Module.ImageInspector;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExtremeEnviroment.Module.ImageView
{
    /// <summary>
    /// Interaction logic for ImageViewControl.xaml
    /// </summary>
    public partial class ImageViewControl : UserControl
    {
        private Point startPoint;
        private Rectangle currentRectangle;
        private readonly List<Rectangle> rectangleList = new List<Rectangle>();

        public ImageViewControl()
        {
            InitializeComponent();
        }
        public void SetImageSource(BitmapImage bitmapImage)
        {
            this.bgImage.Source = bitmapImage;
        }
        private void BgImage_MouseEnter(object sender, MouseEventArgs e)
        {
            this.locationText.Text = "0 x 0";
        }
        private void BgImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Mouse.Capture(this.bgImage);
                this.startPoint = e.GetPosition(this.imageCanvas);
                Rectangle newRectangle = new Rectangle
                {
                    Stroke = Brushes.Red,
                    StrokeThickness = 1
                };
                this.currentRectangle = newRectangle;
                this.rectangleList.Add(newRectangle);

                Canvas.SetLeft(this.currentRectangle, startPoint.X);
                Canvas.SetTop(this.currentRectangle, startPoint.Y);

                imageCanvas.Children.Add(this.currentRectangle);
            }
        }
        private void BgImage_MouseMove(object sender, MouseEventArgs e)
        {
            Point posOnImage = e.GetPosition(this.bgImage);
           
            int posX = (int)posOnImage.X;
            if (posX <= 0)
            {
                posX = 0;
            } else if(posX >= this.bgImage.Width)
            {
                posX = (int)this.bgImage.Width;
            }

            int posY = (int)posOnImage.Y;
            if (posY <= 0)
            {
                posY = 0;
            }
            else if (posY >= this.bgImage.Height)
            {
                posY = (int)this.bgImage.Height;
            }

            if ( 0 < posOnImage.X && posOnImage.X <= this.bgImage.Width
                && posOnImage.Y > 0 && posOnImage.Y <= this.bgImage.Height)
            {
                this.locationText.Text = posX + " x " + posY;
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point pos = e.GetPosition(this.imageCanvas);
                    double x = Math.Min(pos.X, startPoint.X);
                    double y = Math.Min(pos.Y, startPoint.Y);

                    double w = Math.Max(pos.X, startPoint.X) - x;
                    double h = Math.Max(pos.Y, startPoint.Y) - y;

                    this.currentRectangle.Width = w;
                    this.currentRectangle.Height = h;

                    Canvas.SetLeft(this.currentRectangle, x);
                    Canvas.SetTop(this.currentRectangle, y);
                }

            }
        }
        private void BgImage_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int index = this.rectangleList.IndexOf(this.currentRectangle);
            int numPixel = (int) this.currentRectangle.Width * (int) this.currentRectangle.Height;
            MainWindow mainWindow = ExtremeEnviroment.MainWindow._mainWindow;
            mainWindow.ImageInspector.AddRow(index, numPixel);

            imageCanvas.Children.Remove(this.currentRectangle);
            Mouse.Capture(null);
        }
        private void BgImage_MouseLeave(object sender, MouseEventArgs e)
        {
            this.locationText.Text = "";
        }

    }
}
