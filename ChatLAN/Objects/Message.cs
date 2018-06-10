using System.Xml.Serialization;

namespace ChatLAN.Objects
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
