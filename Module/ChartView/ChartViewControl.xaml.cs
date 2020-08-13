using LiveCharts;
using LiveCharts.Wpf;
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

namespace ExtremeEnviroment.Module.ChartView
{
    /// <summary>
    /// Interaction logic for ChartViewControl.xaml
    /// </summary>
    public partial class ChartViewControl : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public enum LineSmooth
        {
            Smooth, Straight
        }

        public ChartViewControl()
        {
            InitializeComponent();
            InitControl();
        }

        private void InitControl()
        {
            this.SeriesCollection = new SeriesCollection();

            this.AddLineSeries("Series 1", new List<double> { 4, 6, 5, 2, 4 }, LineSmooth.Straight);
            this.AddLineSeries("Series 2", new List<double> { 6, 7, 3, 4, 6 }, LineSmooth.Straight);
            this.AddLineSeries("Series 3", new List<double> { 4, 2, 7, 2, 7 }, LineSmooth.Straight);
            this.AddLineSeries("Series 4", new List<double> { 5, 3, 2, 4 }, LineSmooth.Smooth);

            this.Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            this.YFormatter = value => value.ToString("C");

            //modifying any series values will also animate and update the chart
            SeriesCollection[3].Values.Add(5d);

            DataContext = this;

        }

        public bool AddLineSeries(string title, List<double> values, LineSmooth lineSmooth)
        {
            try
            {
                LineSeries lineSeries = new LineSeries
                {
                    Title = title,
                    Values = new ChartValues<double>(values),
                    LineSmoothness = lineSmooth == LineSmooth.Smooth ? 1 : 0
                };

                this.SeriesCollection.Add(lineSeries);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        
    }
}
