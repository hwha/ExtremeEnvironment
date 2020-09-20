using System;
using System.Collections.Generic;
using System.Text;

namespace ExtremeEnviroment.Model
{
    public class ImageProp
    {

        public String Prop { get; set; }
        public String Val { get; set; }
        public ImageProp(String _Prop, String _Val)
        {
            Prop = _Prop;
            Val = _Val;
        }
    }

    
}
