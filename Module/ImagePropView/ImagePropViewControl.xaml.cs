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

namespace ExtremeEnviroment.Module.ImagePropView
{
    /// <summary>
    /// ImagePropViewControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImagePropViewControl : UserControl
    {
        public ImagePropViewControl()
        {
            InitializeComponent();
        }

        private void DgImageProp_Loaded(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("PROP", typeof(string));
            dataTable.Columns.Add("VALUE", typeof(string));

            dataTable.Rows.Add(new object[] { "파일명", "sample.png" });
            dataTable.Rows.Add(new object[] { "위도", "23.19827" });
            dataTable.Rows.Add(new object[] { "경도", "32.17623" });
            dataTable.Rows.Add(new object[] { "생성일시", "2020-08-13 23:21:08" });
            dataTable.Rows.Add(new object[] { "파일크기", "1,298,029" });
            dataTable.Rows.Add(new object[] { "속성1", "속성1.." });
            dataTable.Rows.Add(new object[] { "속성2", "속성2.." });
            dataTable.Rows.Add(new object[] { "속성3", "속성3.." });

            DgImageProp.ItemsSource = dataTable.DefaultView;
        }
    }
}
