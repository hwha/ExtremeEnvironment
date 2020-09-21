using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ExtremeEnviroment.Module.ImageList;
using System.Linq;
using ExtremeEnviroment.Utils;
using ExtremeEnviroment.Model;
using Newtonsoft.Json;

namespace ExtremeEnviroment.Module.Menu
{
    /// <summary>
    /// MenuControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MenuControl : UserControl
    {

        public MenuControl()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = ExtremeEnviroment.MainWindow._mainWindow;
            String projectName = mainWindow.MainWindowTitle;
            String appProjectDataPath = CommonUtils.GetProjectDataPath(projectName);
            String appProjectImageDataPath = CommonUtils.GetProjectImageDataPath(projectName);

            if (mainWindow != null)
            {
                ImageListControl imgeListControl = mainWindow.GetImageListControl();
                List<ImageData> ImageDataList = imgeListControl.GetImageDataList();
                DirectoryInfo appImageDataDirectory = new DirectoryInfo(appProjectImageDataPath);

                if (ImageDataList.Count == 0)
                {
                    return;
                }

                if (!appImageDataDirectory.Exists) 
                {
                    Directory.CreateDirectory(appProjectImageDataPath);
                }
                List<ProjectImage> projectImageList = new List<ProjectImage>();

                foreach (ImageData imageData in ImageDataList)
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(imageData.Image));

                    using (FileStream fileStream = new System.IO.FileStream(appProjectImageDataPath + "\\" + imageData.ImageName, System.IO.FileMode.Create))
                    {
                        encoder.Save(fileStream);
                    }


                    ProjectImage projectImage = new ProjectImage();
                    projectImage.index = ImageDataList.IndexOf(imageData);
                    projectImage.imageName = imageData.ImageName;
                    projectImage.metadata = imageData.ImageProps;
                    projectImageList.Add(projectImage);

                }


                ProjectData projectData = new ProjectData();
                projectData.images = projectImageList;

                using (StreamWriter file = File.CreateText(appProjectDataPath + "\\" + projectName + ".data"))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serialize object directly into file stream
                    serializer.Serialize(file, projectData);
                }
            }
        }
    }
}
