using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ChatLAN.Serializable
{
    class Message
    {
        public string Data;
        public string Text;
        public string Name;
        public string Key;
        [XmlIgnore]
        public File File;
    }
}
