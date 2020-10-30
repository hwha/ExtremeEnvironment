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
using System.Windows.Shapes;
using System.IO;
using ExtremeEnviroment.Model;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using ExtremeEnviroment.Utils;

namespace ExtremeEnviroment
{
    /// <summary>
    /// AppWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AppWindow : Window
    {
        public List<LoadedProject> LoadedProjectItem { get; set; }


        public AppWindow()
        {
            InitializeComponent();
            LoadedProjectItem = new List<LoadedProject>();
            this.ReadLoadedProject();
        }
        private void ReadLoadedProject()
        {
            LoadedProjectList loadedProjectList = null;

            using (StreamReader r = new StreamReader(CommonUtils.GetLoadedProjecListPath()))
            {
                string json = r.ReadToEnd();
                loadedProjectList = JsonConvert.DeserializeObject<LoadedProjectList>(json);
                System.Diagnostics.Debug.WriteLine("!!");
            }

            if (loadedProjectList == null) { return; }
            
            foreach (LoadedProject project in loadedProjectList.loadedProjectLists)
            {
                this.LoadedProjectItem.Add(project);
            }

            LoadedProjectItemControl.ItemsSource = LoadedProjectItem;

        }

        public void SaveLoadedProject(string projectName)
        {
            try
            {
                DirectoryInfo appDataDirectory = new DirectoryInfo(CommonUtils.GetDataPath()) ;
                if (!appDataDirectory.Exists) { Directory.CreateDirectory(CommonUtils.GetDataPath()); }
                LoadedProjectList loadedProjectList = null;
                FileInfo loadedProjectListFile = new FileInfo(CommonUtils.GetLoadedProjecListPath());

                if (!loadedProjectListFile.Exists)
                {
                    var file = File.CreateText(CommonUtils.GetLoadedProjecListPath());
                    file.Close();
                }
                
                using (StreamReader r = new StreamReader(CommonUtils.GetLoadedProjecListPath()))
                {
                    string json = r.ReadToEnd();
                    loadedProjectList = JsonConvert.DeserializeObject<LoadedProjectList>(json);
                }

                if (loadedProjectList == null)
                {
                    loadedProjectList = new LoadedProjectList();
                    loadedProjectList.loadedProjectLists = new List<LoadedProject>();
                }

                loadedProjectList.loadedProjectLists.Insert(0, new LoadedProject {
                    PROJECT_NAME = projectName,
                    LOAD_DATETIME = DateTime.Now.ToString("yyyy-MMM-dd dddd, hh:mm:ss")
                });

                using (StreamWriter file = new StreamWriter(CommonUtils.GetLoadedProjecListPath(), false))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serialize object directly into file stream
                    serializer.Serialize(file, loadedProjectList);
                }
            }
            catch (Exception exception)
            {
                System.Diagnostics.Debug.WriteLine("Error :: " + exception);
            }
        }

        private void BtnNewProject_Click(object sender, RoutedEventArgs e)
        {
            NewProjectWindow newProjectWindow = new NewProjectWindow(this);
            if (newProjectWindow.ShowDialog() == true) { }
        }

        private void BtnLoadProject_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Multiselect = false,
                Filter = "Image Files|*.data"
            };

            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                ProjectData loadedProjectData = null;
                string fileAbsolutePath = openFileDialog.FileName;

                using (StreamReader r = new StreamReader(fileAbsolutePath))
                {
                    string json = r.ReadToEnd();
                    loadedProjectData = JsonConvert.DeserializeObject<ProjectData>(json);
                }

                if (loadedProjectData != null)
                {
                    string[] split = openFileDialog.SafeFileName.Split(".");
                    this.SaveLoadedProject(split[0]);
                    this.Hide();
                    MainWindow mainWindow = new MainWindow(split[0], loadedProjectData);
                    mainWindow.Show();
                }
                
            }
        }

        private void BtnLoadSingleProject_Click(object sender, RoutedEventArgs e)
        {
            string ProjectName = ((Button)sender).Tag.ToString();

            if (!string.IsNullOrEmpty(ProjectName))
            {
                ProjectData loadedProjectData = null;
                string fileAbsolutePath = CommonUtils.GetProjectDataFilePath(ProjectName);

                using (StreamReader r = new StreamReader(fileAbsolutePath))
                {
                    string json = r.ReadToEnd();
                    loadedProjectData = JsonConvert.DeserializeObject<ProjectData>(json);
                }

                if (loadedProjectData != null)
                {
                    this.SaveLoadedProject(ProjectName);
                    this.Hide();
                    MainWindow mainWindow = new MainWindow(ProjectName, loadedProjectData);
                    mainWindow.Show();
                }

            }
        }
    }

    public class LoadedProjectList
    {
        public List<LoadedProject> loadedProjectLists { get; set; }
    }

    public class LoadedProject
    {
        public LoadedProject()
        {
        }
        public LoadedProject(Dictionary<string, string> keyValuePairs)
        {
            this.FromDictionary(keyValuePairs);
        }
        public string PROJECT_NAME { get; set; }
        public string LOAD_DATETIME { get; set; }

        public void FromDictionary(Dictionary<string, string> keyValuePairs)
        {
            this.PROJECT_NAME = keyValuePairs.GetValueOrDefault("PROJECT_NAME").ToString();
            this.LOAD_DATETIME = keyValuePairs.GetValueOrDefault("LOAD_DATETIME").ToString();
        }

    }
}
