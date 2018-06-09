using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ChatLAN.Serializable
{
    public class Message
    {
        public string Data;
        public string Text;
        public string Name;

        [XmlIgnore]
        public File File;
    }
}
