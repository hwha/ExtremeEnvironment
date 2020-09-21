using System;
using System.Collections.Generic;
using System.Text;

namespace ExtremeEnviroment.Model
{
    public class ProjectData
    {
        public List<ProjectImage> images { get; set;  }
    }

    public class ProjectImage
    {
        public string imageName { get; set; }
        public int index { get; set; }

        public Dictionary<string, string> metadata { get; set; }
    }
}
