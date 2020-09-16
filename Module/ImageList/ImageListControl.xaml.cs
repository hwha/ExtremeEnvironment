using MetadataExtractor;
using Directory = MetadataExtractor.Directory;
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
using ExtremeEnviroment.Module.ImageView;
using ExtremeEnviroment.Module.ImagePropView;

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
                
                if (selectedItem != null && selectedItem.Header is TextBlock textBlock)
                {
                    Image thumnailImage = (Image)((InlineUIContainer)textBlock.Inlines.FirstInline).Child;
                    imageSource = (BitmapImage)thumnailImage.Source;
                }
                return imageSource;
            }
        }

        private TreeViewItem CreateTreeItem(BitmapImage bitmapImage)
        {
            TreeViewItem imageTreeViewItem = null;
            if (bitmapImage != null)
            {
                imageTreeViewItem = new TreeViewItem();
                string imageAbsolutePath = bitmapImage.UriSource.AbsolutePath;
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imageAbsolutePath);

                Image iconImage = new Image
                {
                    Source = bitmapImage,
                    Width = 16,
                    Height = 16
                };

                TextBlock textBlock = new TextBlock();
                textBlock.Inlines.Add(iconImage);
                textBlock.Inlines.Add(fileNameWithoutExtension);

                imageTreeViewItem.Header = textBlock;

                // append metadata tree
                this.AppendMetadataToTreeItem(imageTreeViewItem, this.GetImageMetadata(imageAbsolutePath));
            }

            return imageTreeViewItem;
        }

        public void UpdateTreeItem(Dictionary<string, string> metadataMap) 
        {
            foreach (KeyValuePair<String, String> entry in metadataMap)
            {
                System.Diagnostics.Debug.WriteLine(entry.Key + " :: " + entry.Value);
            }
            
        }

        private Dictionary<string, string> GetImageMetadata(string filePath)
        {
            Dictionary<string, string> metadataMap = new Dictionary<string, string>();
            IReadOnlyList<Directory> readOnlyLists = ImageMetadataReader.ReadMetadata(filePath);
            foreach (var dir in readOnlyLists)
            {
                foreach (var tag in dir.Tags)
                {
                    metadataMap.Add($"{tag.Name}", $"{tag.Description}");
                }
            }
            return metadataMap;
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
            if(bitmapImage != null)
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                // set imageview
                ImageViewControl imageViewControl = mainWindow.GetImageViewControl();
                imageViewControl.SetImageSource(bitmapImage);

                // set imagePropView
                ImagePropViewControl imagePropViewControl =  mainWindow.GetImagePropViewControl();
                string imageAbsolutePath = bitmapImage.UriSource.AbsolutePath;
                imagePropViewControl.SetImageProps(this.GetImageMetadata(imageAbsolutePath));
            }
        }

        // Add Button Handler
        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Multiselect = true,
                Filter = "Image Files|*.bmp;*.gif;*.jpg;*.jpeg;*.png;*.tif;*.tiff|All files(*.*)|*.*"
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
        private void BtnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedItem != null)
            {
                this.ImageTree.Items.Remove(this.SelectedItem);
                // TODO: 삭제되는 아이템이랑 연결된 컨트롤도 초기화 필요
            }
        }

        private void AppendMetadataToTreeItem(TreeViewItem treeViewItem, Dictionary<string, string> metadataMap)
        {
            if (treeViewItem != null)
            {
                foreach (KeyValuePair<string, string> keyValuePair in metadataMap)
                {
                    if (keyValuePair.Key.IndexOf("Unknown tag") < 0)
                    {
                        treeViewItem.Items.Add(new TreeViewItem { Header = $"[{keyValuePair.Key}]{keyValuePair.Value}" });
                    }
                }
            }
        }
    }
}

