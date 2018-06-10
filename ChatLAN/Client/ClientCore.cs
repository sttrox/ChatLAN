using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using ChatLAN.Objects;

namespace ChatLAN.Client
{
    public class ClientCore
    {
        public event EventHandler<Message> ChatAdd;
        public static event EventHandler<Dictionary<string, Message>> RefreshListChat; 
        private event EventHandler<ChatMessage> AcceptMessage;

        public static Dictionary<string, Message> Chats = new Dictionary<string, Message>();
        public static List<string> ListAllUsers = new List<string>();
        public static Dictionary<string, Message> ListAllChatMessage = new Dictionary<string, Message>();
        public event EventHandler<string> Error;

        private static ClientCore _client;
        private readonly TcpClient _tcpClient;

       

        private ClientCore(byte[] ipAdress, int port)
        {
            AcceptMessage += OnAcceptMessage;
            Chats.Add("Azzara",
                new Message() {Data = "3 Января", Name = "Azzara", Text = "Этелон килограмма равен 5и"});
            Chats.Add("Pato",
                new Message() {Data = "3 Января", Name = "Azzara", Text = "Этелон килограмма равен 5и"});
            Chats.Add("Lain",
                new Message() {Data = "3 Января", Name = "Azzara", Text = "Этелон килограмма равен 5и"});
            Chats.Add("AntiLoop",
                new Message() {Data = "3 Января", Name = "Azzara", Text = "Этелон килограмма равен 5и"});
            Chats.Add("Withay",
                new Message() {Data = "3 Января", Name = "Azzara", Text = "Этелон килограмма равен 5и"});
            RefreshListChat?.Invoke(null,ListAllChatMessage);
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
            if (_client == null)
                _client = new ClientCore(ipAdress, port);

            return _client;
        }

        public NetworkStream GetNetworkStream() => _tcpClient.GetStream();


        public class Auth
        {
            public event EventHandler<string> Error;
            public event EventHandler<string> Join;

            public void SingIn(byte[] ipAdress, int port, string login, string pass)
            {
                ClientCore client = InicializeClient(ipAdress, port);

                new Thread(() =>
                {
                    bool badRequest = false;

                    Util.UnhandledException += (o, args) =>
                        badRequest = true;

                    if (client.Sign(client.GetNetworkStream(),
                        new Signature(login, Util.GetMD5(pass)), Util.TypeSoketMessage.SignIn))
                    {
                        Join?.Invoke(null, "Всё хорошо");
                        ClientCore.InicializeClient(ipAdress, port).ReceiveMessage();
                        // FrameStatic.Frame.Invoke(() => FrameStatic.Frame.NavigationService.Navigate(new PageMessager()));
                        return;
                    }


                    if (badRequest)
                    {
                        Error?.Invoke(null, "Сервер ответил не верно");
                        return;
                    }

                    Error?.Invoke(null, "Неправильное имя пользователя или пароль. Проверьте введённые данные");
                }).Start();
            }

            public void SingOut(byte[] ipAdress, int port, string login, string pass)
            {
                throw new NotImplementedException();
            }
        }


        public void AddChat(string login, Message message)
        {
            Chats.Add(login, message);
            ChatAdd?.Invoke(null, message);
        }


        public bool Sign(NetworkStream stream, Signature clien, Util.TypeSoketMessage typeSoketMessage)
        {
            Util.SerializeTypeObject(typeSoketMessage, clien, stream);
            if (Util.TypeSoketMessage.Ok ==
                Util.DeserializeTypeObject<string>(Util.ReadAllBytes(_tcpClient))
                    .TypeSoketMessage)
                return true;
            return false;
        }

        public void ReceiveMessage()
        {
            var p = Util.DeserializeTypeObject<AvatarUsers>(Util.ReadAllBytes(_tcpClient));
            foreach (AvatarUser avatarUser in p.TObj.ListAvatarUsers)
            {
                //todo лишнее хранить все аватары в памяти
                ListAllUsers.Add(avatarUser.Name);
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

       
    } internal class ChatMessage
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