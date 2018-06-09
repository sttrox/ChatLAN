using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatLAN.Server.Utils;

namespace ChatLAN.Server
{
    public class ServerCore
    {
        public event EventHandler<TcpListener> ServerStart;
        public event EventHandler<String> Error;

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
                object temp = new object();
                ServerStart?.Invoke(temp, _tcpListener);
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
            _tcpListener = new TcpListener(IPAddress.Any, port);
            MainWindow.Close += (sender, args) => Disconnect();

            //Запускаем прослушивание клиентов
            ClientOnlineAdd += ListenClient;
        }

        public static ServerCore InicilizeServer(int port)
        {
            if (_serverCore == null)
                _serverCore = new ServerCore(port);
            return _serverCore;
        }

        //todo
        private void ListenClient(object sender, TcpClient e)
        {
            while (true)
            {
            }
        }

        private void AddClientOnline(TcpClient client)
        {
            _tcpClientsOnline.Add(client);
            ClientOnlineAdd?.Invoke(null, client);
        }


        // прослушивание входящих подключений
        private void Listen()
        {
            try
            {
                //todo порт занят 

                Server.Pages.Server.PrintText("Сервер запущен. Ожидание подключений...");
                while (true)
                {
                    TcpClient tcpClient = _tcpListener.AcceptTcpClient();
                    Objects.Signature client = Util.ReadObject<Objects.Signature>(tcpClient);

                    if (client.TypeSoketMessage == Util.TypeSoketMessage.SingUp &&
                        !DataClients.HasItemLogin(client.Login))
                    {
                        Util.SerializeObject(Util.TypeSoketMessage.Ok, tcpClient.GetStream());
                        AddClientOnline(tcpClient);
                        Server.Pages.Server.PrintText("Зарегистрировался " + client.Login);
                        DataClients.Clients.Add(client.HashPass, new ObjClient(client.HashPass, client.Login));
                        continue;
                    }


                    if (client.TypeSoketMessage == Util.TypeSoketMessage.SignIn &&
                        DataClients.Clients.ContainsKey(client.HashPass) &&
                        DataClients.Clients[client.HashPass].login == client.Login)
                    {
                        Util.SerializeObject(Util.TypeSoketMessage.Ok, tcpClient.GetStream());
                        Server.Pages.Server.PrintText("Авторизировался " + client.Login);
                        AddClientOnline(tcpClient);
                        continue;
                    }

                    //Thread.Sleep(7420);

                    Server.Pages.Server.PrintText("Что-то пошло не так \n Имя клиента " + client.Login);
                    Util.SerializeObject(Util.TypeSoketMessage.Bad, tcpClient.GetStream());
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