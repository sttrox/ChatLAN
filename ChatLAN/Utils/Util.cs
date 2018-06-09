using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Serialization;

namespace ChatLAN.Utils
{
    public static class Util
    {
        public static event UnhandledExceptionEventHandler UnhandledException;
        public static void SerializeObject<TObject>(TObject objSerializ, NetworkStream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TObject));
            serializer.Serialize(stream, objSerializ);
        }

        //public static TObject DeserializeObject<TObject>(MemoryStream memory)
        //{
        //    XmlSerializer serializer = new XmlSerializer(typeof(TObject));
        //    using (FileStream fs = new FileStream(".xml", FileMode.OpenOrCreate))
        //        fs.Write(memory.ToArray(), 0, memory.ToArray().Length);

        //    using (FileStream fs = new FileStream(".xml", FileMode.OpenOrCreate))
        //        return (TObject) serializer.Deserialize(fs);

        //}
        public static TObject DeserializeObject<TObject>(byte[] bytes)
        {
            TObject temp= default(TObject);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TObject));
                using (MemoryStream memory = new MemoryStream(bytes))
                {
                   temp = (TObject)serializer.Deserialize(memory);
                }
            }
            catch (InvalidOperationException e)
            {
               UnhandledException?.Invoke(null,new UnhandledExceptionEventArgs(e,false));
            }

            return temp;
        }

        public static string GetMD5(string @string)
        {
            MD5 md5 = MD5.Create();
            byte[] input = Encoding.UTF8.GetBytes(@string);
            byte[] hash = md5.ComputeHash(input);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var @byte in hash)
                stringBuilder.Append(@byte.ToString("X2"));

            return stringBuilder.ToString();
        }

        public enum TypeObject
        {
            Signature
        }

        public enum TypeMessage
        {
            Connect,
            Disconnect,
            SignIn,
            SingUp,
            Ok,
            Bad
        }

        public static MemoryStream ReadAllByte(TcpClient tcpClient)
        {
            MemoryStream streamOut = new MemoryStream();
            NetworkStream streamIn = tcpClient.GetStream();
            byte[] buffer = new byte[64];
            int size = 0;
            do
            {
                size = streamIn.Read(buffer, 0, buffer.Length);
                streamOut.Write(buffer, 0, size);
            } while (streamIn.DataAvailable);

            return streamOut;
        }

        public static byte[] ReadAllBytes(TcpClient tcpClient)
            => ReadAllByte(tcpClient).ToArray();
    }
}