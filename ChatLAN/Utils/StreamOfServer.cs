using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ChatLAN.Serializable;

namespace ChatLAN.Utils
{
    public static class StreamOfServer
    {
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

        public static TObject ReadObject<TObject>(MemoryStream stream)
        {
                TObject obj = Util.DeserializeObject<TObject>(stream.ToArray());
                // MessageBox.Show(ReadString(stream, tcpClient.Available));
                return obj;
           
        }
    }
}