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
using System.Data;

namespace ExtremeEnviroment.Module.DataList
{
    /// <summary>
    /// DataListControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DataListControl : UserControl
    {
        public DataListControl()
        {
            InitializeComponent();
        }

        private void DgDataList_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("NUM", typeof(int));
            dataTable.Columns.Add("NUM_PIXEL", typeof(int));
            dataTable.Columns.Add("AVG_TEMP", typeof(float));
            dataTable.Columns.Add("MAX_TEMP", typeof(float));
            dataTable.Columns.Add("MIN_TEMP", typeof(float));
            dataTable.Columns.Add("STD_TEMP", typeof(float));

            dataTable.Rows.Add(new object[] { "1", "25", "32.123", "49.092", "21.112", "0.928" });
            dataTable.Rows.Add(new object[] { "2", "1", "20.121", "20.121", "20.121", "0.928" });
            dataTable.Rows.Add(new object[] { "3", "1", "29.982", "29.982", "29.982", "0.928" });
            dataTable.Rows.Add(new object[] { "4", "25", "32.123", "49.092", "21.112", "0.928" });

            DgDataList.ItemsSource = dataTable.DefaultView;
        }
    }
}
