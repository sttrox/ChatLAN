using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace ChatLAN
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
            TObject temp = default(TObject);
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TObject));
                using (MemoryStream memory = new MemoryStream(bytes))
                    temp = (TObject) serializer.Deserialize(memory);
            }
            catch (InvalidOperationException e)
            {
                UnhandledException?.Invoke(null, new UnhandledExceptionEventArgs(e, false));
            }

            return temp;
        }

        public static TObject ReadObject<TObject>(MemoryStream stream)
        {
            TObject obj = DeserializeObject<TObject>(stream.ToArray());
            return obj;
        }

        public static TObject ReadObject<TObject>(TcpClient tcpClient)
        {
            TObject obj = DeserializeObject<TObject>(ReadAllBytes(tcpClient));
            return obj;
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

        private enum TypeMessageFromServer
        {
            Message,
            New
        }

        public enum TypeSoketMessage
        {
            Connect,
            Disconnect,
            SignIn,
            SingUp,
            Ok,
            Bad
        }

        public static string ReadString(NetworkStream stream, int size)
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bufferSize = 0;
            do
            {
                int tempSize = stream.Read(data, 0, data.Length);
                bufferSize += tempSize;
                builder.Append(Encoding.UTF8.GetString(data, 0, tempSize));
            } while (bufferSize != size);

            return builder.ToString(); //вывод сообщения
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