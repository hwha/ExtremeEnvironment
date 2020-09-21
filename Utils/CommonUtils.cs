using System;
using System.Collections.Generic;
using System.Text;

namespace ExtremeEnviroment.Utils
{
    class CommonUtils
    {
        public static String GetAppPath() 
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        public static String GetDataPath()
        { 
            return GetAppPath() + @"\data";
        }

        public static String GetProjectDataPath(String projectName)
        {
            return GetDataPath() + "\\" + projectName;
        }

        public static String GetProjectImageDataPath(String projectName)
        {
            return GetProjectDataPath(projectName) + @"\images";
        }
    }
}
