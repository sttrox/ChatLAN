using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ChatLAN.Serializable;

namespace ChatLAN
{
    [Serializable]
    public class ObjClient
    {
        public event EventHandler<Message> SendMessage; 
        [XmlAttribute]
        public string passHash;
        [XmlAttribute]
        public string login;
        [XmlElement("Message")]
        public List<Message> Messages;

        public byte[] Avatar;

        public ObjClient()
        {
        }

        public ObjClient(string passHash, string login)
        {
            this.passHash = passHash;
            this.login = login;
        }
    }
}
