using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;
using System.Xml.Serialization;
using ChatLAN.Objects;
using Encoder = System.Drawing.Imaging.Encoder;
using File = System.IO.File;

namespace ChatLAN
{
    public static class Util
    {
        public static event UnhandledExceptionEventHandler UnhandledException;
        public static event EventHandler<string> Error;

        private static void SerializeObject<TObject>(TObject objSerializ, NetworkStream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TObject));
            serializer.Serialize(stream, objSerializ);
            using (StreamWriter memoryStream = new StreamWriter($"{Environment.CurrentDirectory}/log.xml", true))
            {
                serializer.Serialize(memoryStream, objSerializ);
            }
        }

        public static void SerializeTypeObject<TObject>(Util.TypeSoketMessage message, TObject objSerializ,
            NetworkStream stream)
        {
            SerializeObject(new TypeMessage<TObject>(objSerializ, message), stream);
            Thread.Sleep(50);
        }

        public static TypeMessage<TObject> DeserializeTypeObject<TObject>(byte[] bytes)
        {
            Thread.Sleep(50);
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
                Error?.Invoke(null, $"Принятое сообщение содержит ошибку");
                return temp;
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
            Message,
            Connect,
            Disconnect,
            ConflictName,
            Ok,
            Bad,
            ListMessage
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
            try
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

                    using (FileStream memoryStream =
                        new FileStream($"{Environment.CurrentDirectory}/logRead.xml", FileMode.OpenOrCreate))
                        memoryStream.Write(streamOut.ToArray(), 0, streamOut.ToArray().Length);
                    return streamOut;
                }
            }
            catch (Exception e)
            {
                return new MemoryStream();
                //Console.WriteLine(e);
                //throw;
            }
        }

        public static byte[] ReadAllBytes(TcpClient tcpClient)
            => ReadAllByte(tcpClient).ToArray();

        public static byte[] ReadAllBytes(string pathToFile)
        {
            if (File.Exists(pathToFile))
                using (FileStream stream = new FileStream(pathToFile, FileMode.Open))
                {
                    return ReadAllByte(stream);
                }

            Error?.Invoke(null, $"Файл {pathToFile} не найден ");
            return null;
        }
    }
}