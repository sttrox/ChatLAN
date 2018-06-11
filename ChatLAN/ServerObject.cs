/*using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;
using ChatLAN.Pages;
using ChatLAN.Server.Utils;
using ChatLAN;
using ChatLAN.Client;
using ChatLAN.Pages;
using Client = ChatLAN.Pages.Client;

namespace ChatLAN
{
    public class ServerObject
    {
        static TcpListener tcpListener; // сервер для прослушивания
        List<ClientObject> clients = new List<ClientObject>(); // все подключения

        protected internal void AddConnection(ClientObject clientObject)
        {
            clients.Add(clientObject);
        }

        protected internal void RemoveConnection(string id)
        {
            // получаем по id закрытое подключение
            ClientObject client = clients.FirstOrDefault(c => c.Id == id);
            // и удаляем его из списка подключений
            if (client != null)
                clients.Remove(client);
        }
*/
        //private bool ValidationConnect(NetworkStream stream)
        //{
        //    byte[] bufer = new byte[2];
        //    stream.Read(bufer, 0, 2);
        //    if (Encoding.UTF8.GetString(bufer) == "\n\n")
        //        return true;
        //    return false;
        //}

        //// прослушивание входящих подключений
        //protected internal void Listen()
        //{
        //    MainWindow.Close += (sender, args) => Disconnect();
        //    try
        //    {
        //        //todo порт занят 
        //        tcpListener = new TcpListener(IPAddress.Any, 8888);
        //        tcpListener.JoinServer();
        //        Server.Pages.Server.PrintText("Сервер запущен. Ожидание подключений...");
        //        while (true)
        //        {
        //            TcpClient tcpClient = tcpListener.AcceptTcpClient();
        //            Signature client = Util.ReadObject<Signature>(Util.ReadAllByte(tcpClient));

        //            if (client.TypeSoketMessage == Util.TypeSoketMessage.SingUp)
        //                if (!AvatarUsers.HasItemLogin(client.Login))
        //                {
        //                    Util.SerializeObject(Util.TypeSoketMessage.Ok, tcpClient.GetStream());
        //                    //начинаем слушать тут
        //                    clients.Add(new ClientObject(tcpClient));
        //                    AvatarUsers.AvatarUsers.Add(client.HashPass, new ObjUser(client.HashPass, client.Login));
        //                    continue;
        //                }


        //            if (client.TypeSoketMessage == Util.TypeSoketMessage.SignIn)
        //                if (AvatarUsers.AvatarUsers.ContainsKey(client.HashPass) &
        //                    AvatarUsers.AvatarUsers[client.HashPass].login == client.Login)
        //                {
        //                    Util.SerializeObject(Util.TypeSoketMessage.Ok, tcpClient.GetStream());
        //                    Server.Pages.Server.PrintText("Login " + client.Login);
        //                    clients.Add(new ClientObject(tcpClient));
        //                    continue;
        //                }

        //            //Thread.Sleep(7420);

        //            // Server.PrintText(client.Login);
        //            Server.Pages.Server.PrintText("Бяда");
        //            Util.SerializeObject(Util.TypeSoketMessage.Bad, tcpClient.GetStream());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        Disconnect();
        //    }
        //}

   /*     private void ReceiveClient(TcpClient tcpClient)
        {
            NetworkStream netStream = tcpClient.GetStream();
            while (true)
            {
                string temp = Util.ReadString(netStream, tcpClient.Available);
            }
        }
 */       //public string readMessage(NetworkStream stream, int size)
        //{
        //    try
        //    {
        //        byte[] data = new byte[64]; // буфер для получаемых данных
        //        StringBuilder builder = new StringBuilder();
        //        int bufferSize = 0;
        //        do
        //        {
        //            int tempSize = stream.Read(data, 0, data.Length);
        //            bufferSize += tempSize;
        //            builder.Append(Encoding.UTF8.GetString(data, 0, tempSize));
        //        } while (bufferSize != size);

        //        return builder.ToString(); //вывод сообщения
        //    }
        //    catch
        //    {
        //        Console.WriteLine("Подключение прервано!"); //соединение было прервано
        //        Console.ReadLine();
        //        Disconnect();
        //        return null;
        //    }
        //}

        // трансляция сообщения подключенным клиентам
  /*      protected internal void BroadcastMessage(string message, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            for (int i = 0; i < clients.Count; i++)
            {
                if (clients[i].Id != id) // если id клиента не равно id отправляющего
                {
                    clients[i].Stream.Write(data, 0, data.Length); //передача данных
                }
            }
        }
*/
        // отключение всех клиентов
        //protected internal void Disconnect()
        //{
        //    tcpListener.Stop(); //остановка сервера

        //    for (int i = 0; i < clients.Count; i++)
        //    {
        //        clients[i].Close(); //отключение клиента
        //    }

        //    Environment.Exit(0); //завершение процесса
        //}
//    }
//}