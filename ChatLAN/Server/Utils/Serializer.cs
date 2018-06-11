using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ChatLAN.Objects;
using File = System.IO.File;

namespace ChatLAN.Server.Utils
{
    public class Serializer
    {
        public static void SerializeMessage(List<Message> objSerializ)
        {
            using (FileStream stream = new FileStream($"{Const.ServerPathToMessage}messages.xml", FileMode.Create))
                new XmlSerializer(typeof(List<Message>)).Serialize(stream, objSerializ);
        }

        public static List<Message> DeserializeMessage()
        {
            if (File.Exists($"{Const.ServerPathToMessage}messages.xml"))
                using (FileStream stream = new FileStream($"{Const.ServerPathToMessage}messages.xml", FileMode.Open))
                    return (List<Message>) new XmlSerializer(typeof(List<Message>)).Deserialize(stream);
            return new List<Message>();
        }
    }
}