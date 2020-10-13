using ExtremeEnviroment.Module.ImageInspector;
using System;
using System.Collections.Generic;
using System.Linq;
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

        /*
         * MouseEvent Area
         */
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

                imageCanvas.Children.Add(this.currentRectangle);
                Canvas.SetLeft(this.currentRectangle, startPoint.X);
                Canvas.SetTop(this.currentRectangle, startPoint.Y);

            }
        }
        private void BgImage_MouseMove(object sender, MouseEventArgs e)
        {
            Point posOnImage = e.GetPosition(this.imageCanvas);
            int bgImageWidth = (int)this.bgImage.RenderSize.Width;
            int bgImageHeight = (int)this.bgImage.RenderSize.Height;


            int posX = (int)posOnImage.X;
            if (posX <= 0)
            {
                posX = 0;
            }
            else if (posX >= bgImageWidth)
            {
                posX = bgImageWidth;
            }

            int posY = (int)posOnImage.Y;
            if (posY <= 0)
            {
                posY = 0;
            }
            else if (posY >= bgImageHeight)
            {
                posY = bgImageHeight;
            }

            this.locationText.Text = posX + " x " + posY;

            if (0 < posOnImage.X && posOnImage.X <= bgImageWidth
                && posOnImage.Y > 0 && posOnImage.Y <= bgImageHeight)
            {
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
            int width = (int)this.currentRectangle.Width;
            int height = (int)this.currentRectangle.Height;
            // 픽셀 계산
            GetAveragePixelColor(index, width, height);
            imageCanvas.Children.Remove(this.currentRectangle);
            Mouse.Capture(null);
        }
        private void BgImage_MouseLeave(object sender, MouseEventArgs e)
        {
            this.locationText.Text = "";
        }

        private void GetAveragePixelColor(int index, int width, int height)
        {
            
            BitmapSource bitmapSource = (BitmapSource)this.bgImage.Source;

            PixelFormat pixelFormat = bitmapSource.Format;

            Int32Rect rect = new Int32Rect((int)this.startPoint.X, (int)this.startPoint.Y, (int)this.currentRectangle.Width, (int)this.currentRectangle.Height);
            int bytesPerPixel = (width * pixelFormat.BitsPerPixel + 7) / 8;
            int numPixels = width * height;

            MainWindow mainWindow = ExtremeEnviroment.MainWindow._mainWindow;
            if(pixelFormat == PixelFormats.Indexed8)
            {
                byte[] pixelBuffer = new byte[numPixels];
                bitmapSource.CopyPixels(rect, pixelBuffer, bytesPerPixel, 0);
                int sum = 0;
                for (int i = 0; i < pixelBuffer.Length; ++i)
                {
                    sum += pixelBuffer[i];
                }

                mainWindow.ImageInspector.AddRow(index, sum / pixelBuffer.Length);
            }
            else
            {
                byte[] pixelBuffer = new byte[numPixels * bytesPerPixel];

                bitmapSource.CopyPixels(rect, pixelBuffer, width * bytesPerPixel, 0);

                long blue = 0;
                long green = 0;
                long red = 0;

                for (int i = 0; i < pixelBuffer.Length; i += bytesPerPixel)
                {
                    blue += pixelBuffer[i];
                    green += pixelBuffer[i + 1];
                    red += pixelBuffer[i + 2];
                }
                mainWindow.ImageInspector.AddRow(index, numPixels, blue/pixelBuffer.Length + "," + green/pixelBuffer.Length + "," + red/pixelBuffer.Length);
            }
            
        }

    }
}
