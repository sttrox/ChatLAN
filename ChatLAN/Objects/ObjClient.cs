using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ChatLAN.Objects
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
