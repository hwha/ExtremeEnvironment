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
using ExtremeEnviroment.Model;
using ExtremeEnviroment.Module.ImageView;
using ExtremeEnviroment.Module.ImagePropView;
using MetadataExtractor.Formats.Exif;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using System.Linq;
using ExtremeEnviroment.Module.ImageInspector;
using ExtremeEnviroment.Module.DataList;
using System.Collections;
using System.Collections.ObjectModel;
using System.Net.Security;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using ExtremeEnviroment.Utils;

namespace ExtremeEnviroment.Module.ImageList
{
    /// <summary>
    /// ImageListControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImageListControl : UserControl
    {
        public List<ImageData> imageDataList;
        public ImageData currentImage;

        public ImageListControl()
        {
            InitializeComponent();
            imageDataList = new List<ImageData>();
        }

        public void LoadProjectDataList(List<ProjectImage> LoadedProjectDataList)
        {
            imageDataList = new List<ImageData>();

            foreach (ProjectImage projectImage in LoadedProjectDataList)
            {
                BitmapImage bitmapImage= this.LoadLocalImage(CommonUtils.GetProjectImageFilePath(CommonUtils.GetProjectName(), projectImage.ImageName));
                imageDataList.Add(new ImageData
                {
                    ImageTreeViewItem = this.CreateTreeViewItem(bitmapImage),
                    Image = bitmapImage,
                    ImageName = projectImage.ImageName,
                    ImageProps = projectImage.ImageProps,
                    InspectorItems = projectImage.InspectorItems,
                    DataListItem = projectImage.DataListItem
                });
            }

            this.RefreshRelativeControls();
        }

        public ImageData SelectedImageData
        {
            get
            {
                return this.imageDataList.Find(data => data.ImageTreeViewItem == this.ImageTree.SelectedItem);
            }

        }

        public BitmapImage SelectedItemImage
        {
            get
            {
                if (this.ImageTree.SelectedItem == null)
                {
                    return null;
                }

                ImageData imageData = this.imageDataList.Find(data => 
                    data.ImageTreeViewItem == this.ImageTree.SelectedItem);
                if (imageData == null)
                {
                    return null;
                }
                return imageData.Image;
            }
        }

        public List<ImageData> GetImageDataList()
        {
            return this.imageDataList;
        }

        public ImageData GetCurrentImageData()
        {
            return this.currentImage;
        }

        private TreeViewItem AddItem(string imagePath)
        {
            return this.AddItem(this.LoadLocalImage(imagePath));
        }

        private TreeViewItem AddItem(BitmapImage bitmapImage)
        {
            if (bitmapImage != null)
            {
                TreeViewItem imageTreeViewItem = this.CreateTreeViewItem(bitmapImage);
                this.ImageTree.Items.Add(imageTreeViewItem);

                // append metadata tree
                Dictionary<string, string> metaData = this.GetImageMetadata(imageTreeViewItem);
                this.AppendMetadataToTreeItem(imageTreeViewItem, metaData);

                // Add Image and metadata listed on tree
                ImageData imageData = new ImageData
                {
                    ImageTreeViewItem = imageTreeViewItem,
                    Image = bitmapImage,
                    ImageName = Path.GetFileName(bitmapImage.UriSource.AbsolutePath),
                    ImageProps = metaData,
                    DataListItem = new DataListItem
                    {
                        FILE_NAME = Path.GetFileName(bitmapImage.UriSource.AbsolutePath),
                        NUM_PIXEL = 0,
                        AVG_TEMP = "0",
                        MAX_TEMP = "0",
                        MIN_TEMP = "0",
                        STD_DEV = "0"
                    }
                };

                this.imageDataList.Add(imageData);
                this.RefreshRelativeControls();

                return imageTreeViewItem;
            }
            return null;
        }

        private TreeViewItem CreateTreeViewItem(BitmapImage bitmapImage) {
            TreeViewItem newItem = new TreeViewItem();
            string imagePath = bitmapImage.UriSource.OriginalString;
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(imagePath);

            Image iconImage = new Image
            {
                Source = bitmapImage,
                Width = 16,
                Height = 16
            };

            TextBlock textBlock = new TextBlock();
            textBlock.Inlines.Add(iconImage);
            textBlock.Inlines.Add(fileNameWithoutExtension);
            newItem.Header = textBlock;

            return newItem;
        }

        public bool SetTreeItem(Dictionary<string, string> imageProps)
        {
            if (this.currentImage == null || imageProps == null)
            {
                return false;
            }
            this.currentImage.ImageProps = imageProps;
            this.RefreshRelativeControls();
            return true;
        }

        private Dictionary<string, string> GetImageMetadata(TreeViewItem treeViewItem)
        {
            TextBlock textBlock = (TextBlock)treeViewItem.Header;
            Image thumnailImage = (Image)((InlineUIContainer)textBlock.Inlines.FirstInline).Child;
            BitmapImage imageSource = (BitmapImage)thumnailImage.Source;
            return this.GetImageMetadata(imageSource);
        }

        private Dictionary<string, string> GetImageMetadata(BitmapImage bitmapImage)
        {

            Dictionary<string, string> result = new Dictionary<string, string>();
            
            string imagePath = bitmapImage.UriSource.OriginalString;

            IReadOnlyList<Directory> metadataList = ImageMetadataReader.ReadMetadata(imagePath);
            GpsDirectory gpsMetadata = ImageMetadataReader.ReadMetadata(imagePath).OfType<GpsDirectory>().FirstOrDefault();

            foreach (Directory metadata in metadataList)
            {
                IReadOnlyList<Tag> tags = metadata.Tags;
                foreach (Tag tag in metadata.Tags)
                {
                    if (!result.ContainsKey(tag.Name))
                    {
                        result.Add(tag.Name.ToString(), tag.Description.ToString());
                    }
                }
            }
            if (gpsMetadata != null && gpsMetadata.GetGeoLocation() != null && !gpsMetadata.GetGeoLocation().IsZero)
            {
                result.Add("Latitude", gpsMetadata.GetGeoLocation().Latitude.ToString());
                result.Add("Longitude", gpsMetadata.GetGeoLocation().Longitude.ToString());
            }
            return result;
        }

        private BitmapImage LoadLocalImage(string imagePath)
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

        private void RefreshImageTree()
        {
            this.ImageTree.Items.Clear();
            if(this.imageDataList != null && this.imageDataList.Count > 0)
            {
                foreach (ImageData imageData in imageDataList) {

                    TreeViewItem treeViewItem = imageData.ImageTreeViewItem;
                    treeViewItem.Items.Clear();
                    this.ImageTree.Items.Add(treeViewItem);

                    // append metadata tree
                    Dictionary<string, string> metaData = this.GetImageMetadata(treeViewItem);
                    this.AppendMetadataToTreeItem(treeViewItem, imageData.ImageProps);
                }
            }
        }

        private void RefreshRelativeControls()
        {
            MainWindow mainWindow = ExtremeEnviroment.MainWindow._mainWindow;
            // Refresh ImageList
            this.RefreshImageTree();
            // Redraw map markers
            mainWindow.MapViewer.Refresh();
            // Refresh DataListGrid
            this.RefreshDataListGrid();
        }

        // Tree DoubleClick Handler
        private void OnTreeViewItemDoubleClick(object sender, RoutedEventArgs e)
        {
            this.currentImage = this.SelectedImageData;
            if(currentImage != null)
            {
                MainWindow mainWindow = ExtremeEnviroment.MainWindow._mainWindow;
                // set imageview
                ImageViewControl imageViewControl = mainWindow.GetImageViewControl();
                imageViewControl.SetImageSource(currentImage.Image);

                // set imagePropView
                ImagePropViewControl imagePropViewControl =  mainWindow.GetImagePropViewControl();
                imagePropViewControl.SetImageProps(currentImage.ImageProps);

                // set imageInspector
                if(currentImage.InspectorItems == null || currentImage.InspectorItems.Count == 0)
                {
                    currentImage.InspectorItems = new ObservableCollection<InspectorItem>();
                }
                ImageInspectorControl imageInspectorControl = mainWindow.GetImageInspectorControl();
                imageInspectorControl.SetIspectItemList(currentImage.InspectorItems);

                if (currentImage.ImageProps.ContainsKey("Latitude") && currentImage.ImageProps.ContainsKey("Longitude"))
                {
                    mainWindow.MapViewer.DrawMarker(double.Parse(currentImage.ImageProps.GetValueOrDefault("Latitude")), double.Parse(currentImage.ImageProps.GetValueOrDefault("Longitude")));
                }
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
                string[] fileAbsolutePaths = openFileDialog.FileNames;
                foreach (string fileAbsolutePath in fileAbsolutePaths)
                {
                    this.AddItem(fileAbsolutePath);
                }
            }
        }
        // Remove Button Handler
        private void BtnRemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.ImageTree.SelectedItem != null)
            {
                this.ImageTree.Items.Remove(this.ImageTree.SelectedItem);
                // TODO: 삭제되는 아이템이랑 연결된 컨트롤도 초기화 필요
            }
        }

        private void AppendMetadataToTreeItem(TreeViewItem treeViewItem, Dictionary<string, string> metadataMap)
        {
            if (treeViewItem != null)
            {
                foreach (KeyValuePair<string, string> keyValuePair in metadataMap)
                {
                    if (!keyValuePair.Key.Contains("Unknown tag"))
                    {
                        treeViewItem.Items.Add(new TreeViewItem { Header = $"[{keyValuePair.Key}]{keyValuePair.Value}" });
                    }
                }
            }
        }

        public void RefreshDataListGrid()
        {
            // Refresh DataListGrid
            ExtremeEnviroment.MainWindow._mainWindow.DataList.SetDataListItems(this.imageDataList);
        }
    }
}

