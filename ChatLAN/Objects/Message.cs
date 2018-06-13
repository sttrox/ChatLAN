using System.Windows.Forms;
using System.Xml.Serialization;

namespace ChatLAN.Objects
{
    public class Message
    {
        public string Data;
        public string Text;
        public string Name;
        public File File;

        public Message()
        {
            File = new File();
        }
    }
}
