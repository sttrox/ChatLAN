using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ChatLAN.Objects;

namespace ChatLAN.Server.Utils
{
    static class DataClients
    {
        public static Dictionary<string, ObjClient> Clients = new Dictionary<string, ObjClient>();

        public static bool HasItemLogin(string login)
        { 
            foreach (var client in Clients)
                if (client.Value.login == login) return true;
            return false;
        }
        static DataClients()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<ObjClient>));

            using (FileStream fs = new FileStream("clients.xml", FileMode.OpenOrCreate))
            {
                try
                {
                    List<ObjClient> newClients = (List<ObjClient>) formatter.Deserialize(fs);
                    foreach (var newClient in newClients)
                    {
                        Clients.Add(newClient.passHash, newClient);
                    }
                }
                catch (InvalidOperationException e)
                {
                    formatter.Serialize(fs, new List<ObjClient>());
                }
            }
        }
    }
}