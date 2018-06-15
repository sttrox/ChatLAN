using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatLAN.Objects;
using ChatLAN.Server.Utils;

namespace ChatLAN.Server
{
    public class ServerCore
    {
        public event EventHandler<TcpListener> ServerStart;
        public event EventHandler<String> Error;
        public event EventHandler<Message> MessageReceived;

        private List<string> _listUserName = new List<string>();
        private readonly List<Message> _listMessage;
        private TcpListener _tcpListener;
        private Dictionary<string, TcpClient> _tcpClientsOnline = new Dictionary<string, TcpClient>();
        private static ServerCore _serverCore;


        public void RemoveServer()
        {
            _tcpListener.Stop();
            _serverCore = null;
        }

        public void Start()
        {
            try
            {
                _tcpListener.Start();
                ServerStart?.Invoke(null, _tcpListener);
                new Thread(Listen).Start();
            }
            catch (SocketException e)
            {
                RemoveServer();
                Error?.Invoke(e, "Порт занят!");
                Console.WriteLine(e);
            }
        }

        private ServerCore(int port)
        {
            Util.UnhandledException += (sender, args) => Pages.Server.PrintText(args.ToString());
            _tcpListener = new TcpListener(IPAddress.Any, port);
            MainWindow.Close += (sender, args) => Disconnect();

            //загрузка из памяти сообщений
            _listMessage = Serializer.DeserializeMessage();
        }

        public static ServerCore InicilizeServer(int port)
        {
            if (_serverCore == null)
                _serverCore = new ServerCore(port);
            return _serverCore;
        }

        private void SendingMessages(Message message)
        {
            foreach (var client in _tcpClientsOnline)
                Util.SerializeTypeObject(Util.TypeSoketMessage.Message, message, client.Value.GetStream());
        }

        private void ReceivedMessage(Message message)
        {
            MessageReceived?.Invoke(null, message);
            _listMessage.Add(message);
            SendingMessages(message);
        }

        //todo
        private void ListenClient(TcpClient e, string nameUser)
        {
            new Thread(() =>
            {
                Thread.Sleep(500);
                Util.SerializeTypeObject(Util.TypeSoketMessage.ListMessage, _listMessage, e.GetStream());

                while (true)
                {
                    var message = Util.DeserializeTypeObject<Message>(Util.ReadAllBytes(e));
                    if (message == null)
                    {
                        RemoveUser(nameUser);
                        return;
                    }

                    Server.Pages.Server.PrintText(message.TObj.Text);
                    if (message.TypeSoketMessage == Util.TypeSoketMessage.Message)
                        ReceivedMessage(message.TObj);
                }
            }).Start();
        }

        private void AddBadClient(TcpClient client)
        {
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(500);

                    if (ValidationUserName(client))
                        break;
                }
            }).Start();
        }

        private bool ValidationUserName(TcpClient tcpClient)
        {
            var client = Util.DeserializeTypeObject<string>(Util.ReadAllBytes(tcpClient));
            if (client == null) return true;
            if (client.TypeSoketMessage == Util.TypeSoketMessage.Connect && !_listUserName.Contains(client.TObj))
            {
                Util.SerializeTypeObject(Util.TypeSoketMessage.Ok, "Авторизация прошла успешно", tcpClient.GetStream());
                AddClientOnline(tcpClient, client.TObj);
                Pages.Server.PrintText("В чат вошёл " + client.TObj); //IP Adress
                return true;
            }

            return false;
        }

        private void AddClientOnline(TcpClient client, string nameUser)
        {
            _listUserName.Add(nameUser);
            _tcpClientsOnline.Add(nameUser, client);
            ListenClient(client, nameUser);
        }


        // прослушивание входящих подключений
        private void Listen()
        {
            try
            {
                Server.Pages.Server.PrintText("Сервер запущен. Ожидание подключений...");
                while (true)
                {
                    TcpClient tcpClient = _tcpListener.AcceptTcpClient();
                    if (ValidationUserName(tcpClient)) continue;

                    AddBadClient(tcpClient);
                    Server.Pages.Server.PrintText("Что-то пошло не так ");
                    Util.SerializeTypeObject(Util.TypeSoketMessage.ConflictName, String.Empty, tcpClient.GetStream());
                }
            }
            catch (Exception ex)
            {
                new Thread(Listen).Start();
            }
        }

        private void RemoveUser(string nameUser)
        {
            Pages.Server.PrintText($"Клиент {nameUser} отключился");
            _tcpClientsOnline[nameUser].Close();
            _tcpClientsOnline.Remove(nameUser);
            _listUserName.Remove(nameUser);
        }

        private void Disconnect()
        {
            _tcpListener.Stop(); //остановка сервера

            foreach (var client in _tcpClientsOnline)
                client.Value.Close();

            Environment.Exit(0); //завершение процесса
        }
    }
}