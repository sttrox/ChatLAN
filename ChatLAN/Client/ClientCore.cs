using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using ChatLAN.Objects;

namespace ChatLAN.Client
{
    public class ClientCore
    {
        public event EventHandler Join;
        public event EventHandler<Message> ChatAdd;
        public event EventHandler<Message> AddMessage;

        public static event EventHandler<Dictionary<string, Message>> RefreshListChat;
        private event EventHandler<ChatMessage> AcceptMessage;
        private List<Message> _listMessage;

        public static Dictionary<string, Message> Chats = new Dictionary<string, Message>();
        public static List<string> ListAllUsers = new List<string>();
        public static Dictionary<string, Message> ListAllChatMessage = new Dictionary<string, Message>();
        public event EventHandler<string> Error;

        private static ClientCore _client;
        private readonly TcpClient _tcpClient;

        private ClientCore(byte[] ipAdress, int port)
        {
            AcceptMessage += OnAcceptMessage;
            RefreshListChat?.Invoke(null, ListAllChatMessage);
            try
            {
                _tcpClient = new TcpClient();
                _tcpClient.Connect(new IPAddress(ipAdress), port);
            }
            catch (SocketException e)
            {
                _tcpClient = null;

                Error?.Invoke(null, "Сокет занят");
            }

            catch (ObjectDisposedException e)
            {
                _tcpClient = null;
                Error?.Invoke(null, "Соединение разорвано");
            }
            finally
            {
                MainWindow.Close += (sender, args) => Disconnect();
            }
        }

        private void OnAcceptMessage(object sender, ChatMessage e)
        {
            if (ListAllChatMessage.ContainsKey(e.Login))
                //todo Data
                ListAllChatMessage[e.Login].Text = e.Message.Text;
            else
                ListAllChatMessage.Add(e.Login, e.Message);
        }


        public static ClientCore InicializeClient(byte[] ipAdress, int port)
        {
            if (_client == null) _client = new ClientCore(ipAdress, port);
            return _client;
        }

        public static ClientCore GetCore()
        {
            if (_client == null) throw new NullReferenceException("Необходимо инициализировать клиент");
            return _client;
        }

        public void JoinServer(string login)
        {
            Util.SerializeTypeObject(Util.TypeSoketMessage.Connect, login, _tcpClient.GetStream());
            byte[] b = Util.ReadAllBytes(_tcpClient);
            var k = Util.DeserializeTypeObject<string>(b);
            if (Util.TypeSoketMessage.Ok ==
                k
                    .TypeSoketMessage)
                Join?.Invoke(null, null);
            else
                Error?.Invoke(null, "Не удалось подключиться");
        }

        public static void SendMessage(Message message)
        {
            Util.SerializeTypeObject(Util.TypeSoketMessage.Message, message, _client._tcpClient.GetStream());
        }

        public void ReceiveMessage()
        {
            _listMessage = Util.DeserializeTypeObject<List<Message>>(Util.ReadAllBytes(_tcpClient)).TObj;
            foreach (var message in _listMessage)
            {
                AddMessage?.Invoke(null, message);
            }

            //while (true)
            //{
            //    try
            //    {
            //        //MessageFromServer messageFromServer =
            //        //    Util.DeserializeObject<MessageFromServer>(Util.ReadAllBytes(_tcpClient));
            //                   AcceptMessage?.Invoke(null, new ChatMessage(null,null));
            //        // messageFromServer

            //        //byte[] data = new byte[64]; // буфер для получаемых данных
            //        //StringBuilder builder = new StringBuilder();
            //        //int bytes = 0;
            //        //do
            //        //{
            //        //    bytes = stream.Read(data, 0, data.Length);
            //        //    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            //        //} while (stream.DataAvailable);

            //        //string message = builder.ToString();
            //        //MessageBox.Show(message);

            //        //Console.WriteLine(message); //вывод сообщения
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //        Console.WriteLine("Подключение прервано!"); //соединение было прервано
            //        Console.ReadLine();
            //        // Disconnect();
            //    }
            //}
        }

        static void Disconnect()
        {
            _client._tcpClient?.Close(); //отключение клиента
            Environment.Exit(0); //завершение процесса
        }
    }

    internal class ChatMessage
    {
        public string Login;
        public Message Message;

        public ChatMessage(string login, Message message)
        {
            Login = login;
            Message = message;
        }
    }
}