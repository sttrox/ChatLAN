using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using ChatLAN.Objects;

namespace ChatLAN.Server.Utils
{
    static class DataClients
    {
        public static Dictionary<string, ObjUser> Users = new Dictionary<string, ObjUser>();

        public static bool HasItemLogin(string login)
        { 
            foreach (var client in Users)
                if (client.Value.login == login) return true;
            return false;
        }
        static DataClients()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<ObjUser>));

            using (FileStream fs = new FileStream("DataChats.xml", FileMode.OpenOrCreate))
            {
                try
                {
                    List<ObjUser> newClients = (List<ObjUser>) formatter.Deserialize(fs);
                    foreach (var newClient in newClients)
                    {
                        Users.Add(newClient.passHash, newClient);
                    }
                }
                catch (InvalidOperationException e)
                {
                    formatter.Serialize(fs, new List<ObjUser>());
                }
            }

            if (!HasItemLogin("Чат")) Users.Add("", new ObjUser("", "Чат"));
        }
    }
}