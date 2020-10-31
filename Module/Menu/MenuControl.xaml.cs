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
using Newtonsoft.Json.Linq;

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

        public void SaveProjectData()
        {
            MainWindow mainWindow = ExtremeEnviroment.MainWindow._mainWindow;
            String projectName = mainWindow.MainWindowTitle;
            String appProjectDataPath = CommonUtils.GetProjectDataFolderPath(projectName);
            String appProjectImageDataPath = CommonUtils.GetProjectImageDataFolderPath(projectName);

            ProjectData projectData = this.GetCurrentProjectData(appProjectImageDataPath);

            using (StreamWriter file = File.CreateText(appProjectDataPath + "\\" + projectName + ".data"))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, projectData);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            this.SaveProjectData();
        }

        private ProjectData GetCurrentProjectData(string appProjectImageDataPath)
        {            
            ProjectData projectData = new ProjectData();

            MainWindow mainWindow = ExtremeEnviroment.MainWindow._mainWindow;
            if (mainWindow != null)
            {
                ImageListControl imgeListControl = mainWindow.GetImageListControl();
                List<ImageData> ImageDataList = imgeListControl.GetImageDataList();
                DirectoryInfo appImageDataDirectory = new DirectoryInfo(appProjectImageDataPath);

                if (!appImageDataDirectory.Exists)
                {
                    Directory.CreateDirectory(appProjectImageDataPath);
                }
                List<ProjectImage> projectImageList = new List<ProjectImage>();

                foreach (ImageData imageData in ImageDataList)
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(imageData.Image));

                    string imageFilePath = appProjectImageDataPath + "\\" + imageData.ImageName;
                    FileInfo fileInfo = new FileInfo(imageFilePath);

                    if (fileInfo.Exists)
                    {
                        using (FileStream fileStream = new System.IO.FileStream(imageFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            encoder.Save(fileStream);
                        }
                    }
                    else
                    {
                        using (FileStream fileStream = new System.IO.FileStream(imageFilePath, FileMode.Create))
                        {
                            encoder.Save(fileStream);
                        }
                    }


                    ProjectImage projectImage = new ProjectImage();
                    projectImage.index = ImageDataList.IndexOf(imageData);
                    projectImage.ImageName = imageData.ImageName;
                    projectImage.ImageProps = imageData.ImageProps;
                    //projectImage.ImageTreeViewItem = imageData.ImageTreeViewItem;
                    projectImage.InspectorItems = imageData.InspectorItems;
                    projectImage.DataListItem = imageData.DataListItem;
                    projectImageList.Add(projectImage);

                }
                
                projectData.images = projectImageList;
            }
            return projectData;
        }


        public Boolean hasModifiedContents()
        {
            MainWindow mainWindow = ExtremeEnviroment.MainWindow._mainWindow;
            String projectName = mainWindow.MainWindowTitle;
            String appProjectDataPath = CommonUtils.GetProjectDataFolderPath(projectName);
            String appProjectImageDataPath = CommonUtils.GetProjectImageDataFolderPath(projectName);

            ProjectData projectData = this.GetCurrentProjectData(appProjectImageDataPath);


            string projectDataFileAbsolutePath = CommonUtils.GetProjectDataFilePath(CommonUtils.GetProjectName());
            ProjectData lastSavedProjectData;

            using (StreamReader r = new StreamReader(projectDataFileAbsolutePath))
                {
                string json = r.ReadToEnd();
                lastSavedProjectData = JsonConvert.DeserializeObject<ProjectData>(json);
            }


            bool result = new Comparator<ProjectData>().Equals(projectData, lastSavedProjectData);


            System.Diagnostics.Debug.WriteLine(result);
            return result;
        }

        private class Comparator<T> : IEqualityComparer<T>
        {
            public bool Equals(T x, T y)
            {
                return JsonConvert.SerializeObject(x) == JsonConvert.SerializeObject(y);
            }

            public int GetHashCode(T obj)
            {
                return JsonConvert.SerializeObject(obj).GetHashCode();
            }
        }
    }
}
