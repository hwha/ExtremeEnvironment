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

namespace ExtremeEnviroment.Module.ImageList
{
    /// <summary>
    /// ImageListControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageListControl : UserControl
    {
        public ImageListControl()
        {
            InitializeComponent();
            InitControl();
        }
        void InitControl()
        {
            this.AddItem(this.GetTestItem());
        }

        private TreeViewItem GetTestItem()
        {
            TreeViewItem treeViewItem = new TreeViewItem();
            treeViewItem.Header = "Root";

            TreeViewItem treeViewItem1 = new TreeViewItem();
            treeViewItem1.Header = "CHILD1";
            TreeViewItem treeViewItem2 = new TreeViewItem();
            treeViewItem2.Header = "CHILD2";
            TreeViewItem treeViewItem3 = new TreeViewItem();
            treeViewItem3.Header = "CHILD3";

            treeViewItem.Items.Add(treeViewItem1);
            treeViewItem.Items.Add(treeViewItem2);
            treeViewItem.Items.Add(treeViewItem3);

            return treeViewItem;
        }

        public void AddItem(TreeViewItem treeViewItem)
        {
            ImageTree.Items.Add(treeViewItem);
        }
    }
}
