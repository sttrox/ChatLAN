using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace ChatLAN.Utils
{
    public class Client
    {
        private static Client _client;

        private readonly TcpClient _tcpClient;

        private Client(byte[] ipAdress, int port)
        {
            _tcpClient = new TcpClient();
            _tcpClient.Connect(new IPAddress(ipAdress), port);
        }

        public static Client InicializeClient(byte[] ipAdress, int port)
        {
            if (_client == null)
                _client = new Client(ipAdress, port);
            return _client;
        }

        public NetworkStream GetNetworkStream()
        {
            return _tcpClient.GetStream();
        }

        public bool Sign(NetworkStream stream, Signature clien)
        {
            Util.SerializeObject(clien, stream);
            if (Util.TypeMessage.Ok ==
                Util.DeserializeObject<Util.TypeMessage>(Util.ReadAllBytes(_tcpClient)))
                return true;
            return false;
        }
        
        public void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    MessageFromServer messageFromServer = Util.DeserializeObject<MessageFromServer>(Util.ReadAllBytes(_tcpClient));

                   // messageFromServer

                    //byte[] data = new byte[64]; // буфер для получаемых данных
                    //StringBuilder builder = new StringBuilder();
                    //int bytes = 0;
                    //do
                    //{
                    //    bytes = stream.Read(data, 0, data.Length);
                    //    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    //} while (stream.DataAvailable);

                    //string message = builder.ToString();
                    //MessageBox.Show(message);

                    //Console.WriteLine(message); //вывод сообщения
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Подключение прервано!"); //соединение было прервано
                    Console.ReadLine();
                   // Disconnect();
                }
            }
        }
    }
 [Serializable]
    public class MessageFromServer
    {

    }
    enum TypeMessageFromServer
    {
        Message,
        New

    }
    [Serializable]
    public class Signature
    {
        public string Login;
        public string HashPass;
        public Util.TypeMessage TypeMessage;

        public Signature(string login, string hashPass, Util.TypeMessage typeMessage)
        {
            Login = login;
            HashPass = hashPass;
            TypeMessage = typeMessage;
        }

        public Signature()
        {
        }
    }

   
}