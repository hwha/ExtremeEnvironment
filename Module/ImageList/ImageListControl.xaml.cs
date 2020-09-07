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
        }

        public void AddItem(TreeViewItem treeViewItem)
        {
            ImageTree.Items.Add(treeViewItem);
        }

        public void AddItem(string imagePath)
        {
            BitmapImage bitmapImage = this.GetLocalImage(imagePath);
            this.AddItem(this.CreateTreeItem(bitmapImage));
        }

        public TreeViewItem SelectedItem
        {
            get
            {
                object selectedItem = this.ImageTree.SelectedItem;
                if (selectedItem == null)
                {
                    return null;
                }

                return (TreeViewItem)selectedItem;
            }
        }

        public BitmapImage SelectedItemImage
        {
            get
            {
                BitmapImage imageSource = null;
                TreeViewItem selectedItem = this.SelectedItem;

                if (selectedItem != null)
                {
                    TextBlock textBlock = (TextBlock)selectedItem.Header;
                    Image thumnailImage = (Image)((InlineUIContainer)textBlock.Inlines.FirstInline).Child;
                    imageSource = (BitmapImage)thumnailImage.Source;
                }
                return imageSource;
            }
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

        // Tree DoubleClick Handler
        private void OnTreeViewItemDoubleClick(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmapImage = this.SelectedItemImage;
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(bitmapImage.UriSource.AbsolutePath);
        }

        // Add Button Handler
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
                    this.AddItem(fileName);
                }
            }
        }
        // Remove Button Handler
        private void btnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedItem != null)
            {
                this.ImageTree.Items.Remove(this.SelectedItem);
                // TODO: 삭제되는 아이템이랑 연결된 컨트롤도 초기화 필요
            }
        }
    }
}

