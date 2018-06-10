﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ChatLAN.Objects
{
    [Serializable]
    public class ObjUser
    {
        //todo реализовать наследование в котором нет passHash и Messages
        public event EventHandler<Message> SendMessage; 
        [XmlAttribute]
        public string passHash;
        [XmlAttribute]
        public string login;
        [XmlElement("Message")]
        public List<Message> Messages;

        public byte[] Avatar;

        public ObjUser()
        {
        }

        public ObjUser(string passHash, string login)
        {
            this.passHash = passHash;
            this.login = login;
        }
    }
}