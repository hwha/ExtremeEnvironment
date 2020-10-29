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

namespace ExtremeEnviroment
{
    /// <summary>
    /// AppWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AppWindow : Window
    {
        public AppWindow()
        {
            InitializeComponent();
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

                    this.Hide();
                    MainWindow mainWindow = new MainWindow(split[0], loadedProjectData);
                    mainWindow.Show();
                }
                
            }
        }
    }
}
