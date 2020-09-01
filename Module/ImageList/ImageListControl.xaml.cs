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
using Path = System.IO.Path;

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
        }

        public void AddItem(TreeViewItem treeViewItem)
        {
            ImageTree.Items.Add(treeViewItem);
        }

        public BitmapImage GetSelectedItem()
        {
            BitmapImage imageSource = null;
            TreeViewItem selectedItem = (TreeViewItem)this.ImageTree.SelectedItem;

            if (selectedItem != null)
            {
                TextBlock textBlock = (TextBlock)selectedItem.Header;
                Image thumnailImage = (Image)((InlineUIContainer)textBlock.Inlines.FirstInline).Child;
                imageSource = (BitmapImage)thumnailImage.Source;
            }
            return imageSource;
        }

        private void OnTreeViewItemDoubleClick(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmapImage = this.GetSelectedItem();
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(bitmapImage.UriSource.AbsolutePath);
            MessageBox.Show(fileNameWithoutExtension);
        }

        public void AddImageToTreeView(string imagePath)
        {
            BitmapImage bitmapImage = this.GetLocalImage(imagePath);
            this.AddItem(this.CreateTreeItem(bitmapImage));
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Multiselect = true,
                Filter = "BMP|*.bmp|GIF|*.gif|JPG|*.jpg;*.jpeg|PNG|*.png|TIFF|*.tif;*.tiff|All files(*.*)|*.*"
            };

            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                string[] fileNames = openFileDialog.FileNames;
                foreach (string fileName in fileNames)
                {
                    this.AddImageToTreeView(fileName);
                }
            }
        }

        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private TreeViewItem CreateTreeItem(BitmapImage bitmapImage)
        {
            TreeViewItem treeViewItem = null;
            if (bitmapImage != null)
            {
                treeViewItem = new TreeViewItem();
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(bitmapImage.UriSource.AbsolutePath);

                Image iconImage = new Image
                {
                    Source = bitmapImage,
                    Width = 16,
                    Height = 16
                };

                TextBlock textBlock = new TextBlock();
                textBlock.Inlines.Add(iconImage);
                textBlock.Inlines.Add(fileNameWithoutExtension);

                treeViewItem.Header = textBlock;
            }

            return treeViewItem;
        }

        private BitmapImage GetLocalImage(string imagePath)
        {
            BitmapImage bitmapImage = new BitmapImage();
            try
            {
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(imagePath, UriKind.Absolute);
                bitmapImage.EndInit();
            }
            catch (IOException e)
            {
                if (e.Source != null)
                    Console.WriteLine("IOException source: {0}", e.Source);
                throw e;
            }
            return bitmapImage;
        }
    }
}
