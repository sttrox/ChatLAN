using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using ChatLAN.Objects;

namespace ChatLAN
{
    public static class Util
    {
        public static event UnhandledExceptionEventHandler UnhandledException;

        private static void SerializeObject<TObject>(TObject objSerializ, NetworkStream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TObject));
            serializer.Serialize(stream, objSerializ);
            using (StreamWriter memoryStream = new StreamWriter($"{Environment.CurrentDirectory}/log.xml",true))
            {
                serializer.Serialize(memoryStream, objSerializ);
            }

        }

        public static void SerializeTypeObject<TObject>(Util.TypeSoketMessage message, TObject objSerializ,
            NetworkStream stream)
        {
            SerializeObject(new TypeMessage<TObject>(objSerializ, message), stream);
        }

        public static TypeMessage<TObject> DeserializeTypeObject<TObject>(byte[] bytes)
        {
            return DeserializeObject<TypeMessage<TObject>>(bytes);
        }

        private static TObject DeserializeObject<TObject>(byte[] bytes)
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

        public enum TypeSoketMessage
        {
            Disconnect,
            SignIn,
            SingUp,
            Ok,
            Bad,
            ListAvatar
        }

        public static string ReadString(MemoryStream stream)
        {
            throw new NotImplementedException("Реализовать лог в файл");
            using (StreamReader streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEnd();
            }
            byte[] data = new byte[8192]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bufferSize = 0;
            do
            {
                int tempSize = stream.Read(data, 0, data.Length);
                bufferSize += tempSize;
                builder.Append(Encoding.UTF8.GetString(data, 0, tempSize));
            } while (stream.CanRead);

            string t = builder.ToString();
            return builder.ToString(); //вывод сообщения
        }

        public static byte[] ReadAllByte(FileStream stream)
        {
            byte[] array = new byte[stream.Length];
            stream.Read(array, 0, array.Length);
            return array;
        }

        public static MemoryStream ReadAllByte(TcpClient tcpClient)
        {
            using (MemoryStream streamOut = new MemoryStream())
            {
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
        }

        public static byte[] ReadAllBytes(TcpClient tcpClient)
            => ReadAllByte(tcpClient).ToArray();
    }
}