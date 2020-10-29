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
using ExtremeEnviroment.Module.DataList;
using System.Collections.ObjectModel;

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

            this.SeriesCollection = new SeriesCollection();
            this.Labels = new[] { "Avg Temp", "Max Temp", "Min Temp", "Std Dev" };
            this.YFormatter = value => value + "°";
            DataContext = this;
        }

        public void addDataList(string fileName, List<double> ChartData)
        {
            this.AddLineSeries(fileName, ChartData, LineSmooth.Straight);
        }

        public void RefreshChart(ObservableCollection<DataListItem> dataListItems)
        {
            this.SeriesCollection.Clear();

            foreach (DataListItem dataListItem in dataListItems)
            {

                string fileName = dataListItem.FILE_NAME;
                double avgTemp = Convert.ToDouble(dataListItem.AVG_TEMP);
                double maxTemp = Convert.ToDouble(dataListItem.MAX_TEMP);
                double minTemp = Convert.ToDouble(dataListItem.MIN_TEMP);
                double stdDev = Convert.ToDouble(dataListItem.STD_DEV);
                this.addDataList(fileName, new List<double> { avgTemp, maxTemp, minTemp, stdDev });
            }
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }


    }

}
