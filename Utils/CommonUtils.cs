using System;
using System.Collections.Generic;
using System.Text;

namespace ExtremeEnviroment.Utils
{
    class CommonUtils
    {

        public static String GetProjectName()
        {
           return ExtremeEnviroment.MainWindow._mainWindow.MainWindowTitle;
        }
        public static String GetAppPath() 
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static String GetDataPath()
        {
            return GetAppPath() + "data";
        }

        public static String GetLoadedProjecListPath()
        {
            return GetDataPath() + "\\projectlist.data";
        }

        public static String GetProjectDataFolderPath(String projectName)
        {
            return GetDataPath() + "\\" + projectName;
        }

        public static String GetProjectDataFilePath(String projectName)
        {
            return GetDataPath() + "\\" + projectName + "\\" + projectName + ".data";
        }

        public static String GetProjectImageDataFolderPath(String projectName)
        {
            return GetProjectDataFolderPath(projectName) + @"\images";
        }

        public static String GetProjectImageFilePath(String projectName, string fileName)
        {            
            return GetProjectImageDataFolderPath(projectName) + "\\" + fileName;
        }
    }
}
