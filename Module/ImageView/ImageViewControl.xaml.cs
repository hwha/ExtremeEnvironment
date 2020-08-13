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

namespace ExtremeEnviroment.Module.ImageView
{
    /// <summary>
    /// Interaction logic for ImageViewControl.xaml
    /// </summary>
    public partial class ImageViewControl : UserControl
    {
        private Point startPoint;
        private Rectangle rectangle;

        public ImageViewControl()
        {
            InitializeComponent();
            InitControl();
        }

        private void InitControl()
        {
            // load imagesource
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri("pack://application:,,/Resources/sample_image.jpg");
            bitmapImage.EndInit();
            this.bgImage.Source = bitmapImage;
        }

        private void imageCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                this.startPoint = e.GetPosition(this);

                this.rectangle = new Rectangle
                {
                    Stroke = Brushes.Black,
                    StrokeThickness = 2
                };
                Canvas.SetLeft(this.rectangle, startPoint.X);
                Canvas.SetTop(this.rectangle, startPoint.Y);

                imageCanvas.Children.Add(this.rectangle);
            }
        }

        private void imageCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                Point pos = e.GetPosition(this);

                var x = Math.Min(pos.X, startPoint.X);
                var y = Math.Min(pos.Y, startPoint.Y);

                var w = Math.Max(pos.X, startPoint.X) - x;
                var h = Math.Max(pos.Y, startPoint.Y) - y;

                this.rectangle.Width = w;
                this.rectangle.Height = h;

                Canvas.SetLeft(this.rectangle, x);
                Canvas.SetTop(this.rectangle, y);
            }
            else
            {
                imageCanvas.Children.Remove(this.rectangle);
            }

        }
    }
}
