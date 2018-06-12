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

        private List<string> _listUserName = new List<string>();
        private readonly List<Message> _listMessage;
        private event EventHandler<TcpClient> ClientOnlineAdd;
        private TcpListener _tcpListener;
        private List<TcpClient> _tcpClientsOnline = new List<TcpClient>();
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

            _listMessage = Serializer.DeserializeMessage();
        }

        public static ServerCore InicilizeServer(int port)
        {
            if (_serverCore == null)
                _serverCore = new ServerCore(port);
            return _serverCore;
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
                    Message mess = Util.DeserializeTypeObject<Message>(Util.ReadAllBytes(e)).TObj;
                    Server.Pages.Server.PrintText(mess.Text);
                }
            }).Start();
        }

        private void AddBadClient(TcpClient client)
        {
            new Thread(() =>
            {
                while (true)
                    if (ValidationUserName(client))
                        break;
            }).Start();
        }

        private bool ValidationUserName(TcpClient tcpClient)
        {
            var client = Util.DeserializeTypeObject<string>(Util.ReadAllBytes(tcpClient));
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
            _tcpClientsOnline.Add(client);
            ClientOnlineAdd?.Invoke(null, client);
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
                    //var client = Util.DeserializeTypeObject<string>(Util.ReadAllBytes(tcpClient));
                    if (ValidationUserName(tcpClient)) continue;
                    //string userName = client.TObj;
                    //if (client.TypeSoketMessage == Util.TypeSoketMessage.Connect &&
                    //    !_listUserName.Contains(userName))
                    //{
                    //    Util.SerializeTypeObject(Util.TypeSoketMessage.Ok, "Авторизация прошла успешно",
                    //        tcpClient.GetStream());
                    //    AddClientOnline(tcpClient, userName);
                    //    Pages.Server.PrintText("В чат вошёл " + userName); //IP Adress
                    //    //DataClients.Users.Add(userName, new ObjUser(client.HashPass, client.Login));
                    //    continue;
                    //}

                    //Thread.Sleep(7420);
                    AddBadClient(tcpClient);
                    Server.Pages.Server.PrintText("Что-то пошло не так " );
                    Util.SerializeTypeObject(Util.TypeSoketMessage.Bad, "Данные отклонены", tcpClient.GetStream());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        private void Disconnect()
        {
            _tcpListener.Stop(); //остановка сервера

            foreach (var client in _tcpClientsOnline)
                client.Close();

            Environment.Exit(0); //завершение процесса
        }
    }
}