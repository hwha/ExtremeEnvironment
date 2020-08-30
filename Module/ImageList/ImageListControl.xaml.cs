using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
            TreeViewItem treeViewItem = this.CreateItem("Root", "images.jpg");
            treeViewItem.Items.Add(this.CreateItem("CHILD1"));
            treeViewItem.Items.Add(this.CreateItem("CHILD2"));
            treeViewItem.Items.Add(this.CreateItem("CHILD3"));
            return treeViewItem;
        }

        public void AddItem(TreeViewItem treeViewItem)
        {
            ImageTree.Items.Add(treeViewItem);
        }

        private TreeViewItem CreateItem(string itemName)
        {
            return CreateItem(itemName, null);
        }

        private TreeViewItem CreateItem(string itemName, string imagePath)
        {
            TextBlock textBlock = new TextBlock();

            if (imagePath == null)
            {
                textBlock.Inlines.Add(itemName);
            }
            else
            {
                // load imagesource
                BitmapImage bitmapImage = new BitmapImage();
                try
                {
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri("pack://application:,,/Resources/" + imagePath);
                    bitmapImage.EndInit();
                }
                catch (IOException e)
                {
                    if (e.Source != null)
                        Console.WriteLine("IOException source: {0}", e.Source);
                }

                // set imagesource
                Image iconImage = new Image
                {
                    Source = bitmapImage,
                    Width = 16,
                    Height = 16
                };

                // create item textblock
                textBlock.Inlines.Add(iconImage);
                textBlock.Inlines.Add(itemName);
            }


            TreeViewItem treeViewItem = new TreeViewItem
            {
                Header = textBlock
            };

            return treeViewItem;
        }

        private void OnTreeViewItemDoubleClick(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = this.ImageTree.SelectedItem as TreeViewItem;

            if (selectedItem != null)
            {
                TextBlock textBlock = selectedItem.Header as TextBlock;
                MessageBox.Show(textBlock.Text);
            }
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|All files(*.*)|*.*";

            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string[] fileNames = openFileDialog.FileNames;
                foreach (string fileName in fileNames)
                {
                    MessageBox.Show(fileName);
                    this.AddImageToTreeView(fileName);
                }
            }
        }

        public void AddImageToTreeView(string imagePath)
        {

        }


        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
