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
using System.Windows.Shapes;
using ExtremeEnviroment.Utils;

namespace ExtremeEnviroment
{
    /// <summary>
    /// NewProjectWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NewProjectWindow : Window
    {
        readonly private AppWindow _parent;
        readonly private String appDataPath = CommonUtils.GetDataPath();

        public NewProjectWindow(AppWindow appWindow)
        {
            InitializeComponent();
            Owner = appWindow;
            _parent = appWindow;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            String newProjectName = TextNewProjectName.Text;

            if (String.IsNullOrEmpty(newProjectName))
            {
                System.Windows.MessageBox.Show("프로젝트명을 입력하세요.", "프로젝트명을 입력하세요", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                DirectoryInfo appDataDirectory = new DirectoryInfo(appDataPath);
                if (!appDataDirectory.Exists) { Directory.CreateDirectory(appDataPath); }

                String projectDataPath = appDataPath + "\\" + newProjectName;
                DirectoryInfo projectDataDirectory = new DirectoryInfo(projectDataPath);
                if (projectDataDirectory.Exists)
                {
                    System.Windows.MessageBox.Show("동일한 프로젝트명이 이미 존재합니다.", "동일한 프로젝트명이 이미 존재합니다", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else 
                {
                    Directory.CreateDirectory(projectDataPath);
                    this.Close();
                    _parent.Hide();

                    MainWindow mainWindow = new MainWindow(newProjectName);
                    mainWindow.Show();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error :: " + exception);
            }
        }
    }
}
