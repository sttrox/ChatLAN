using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ChatLAN.Pages;
using MahApps.Metro.Controls;

namespace ChatLAN.Utils
{
    class Auth
    {
        public event EventHandler<string> Error;
        public event EventHandler<string> Join;

        public void SingIn(byte[] ipAdress, int port, string login, string pass)
        {
            Client client = Client.InicializeClient(ipAdress, port);
            string _login = login;
            string _pass = pass;

            
               
            new Thread(() =>
            {
                bool badRequest = false;

                Util.UnhandledException += (o, args) =>
                    badRequest = true;

                if (client.Sign(
                    client.GetNetworkStream(),
                    new Signature(
                        _login,
                        Util.GetMD5(_pass),
                        Util.TypeMessage.SignIn)))
                {
                    Join.Invoke(null, "Всё хорошо");
                    Client.InicializeClient(ipAdress,port).ReceiveMessage();
                    // FrameStatic.Frame.Invoke(() => FrameStatic.Frame.NavigationService.Navigate(new PageMessager()));
                    return;
                }


                if (badRequest) { 
                    Error?.Invoke(null, "Сервер ответил не верно");
                    return;
                }
                Error?.Invoke(null, "Неправильное имя пользователя или пароль. Проверьте введённые данные");
            }).Start();
        }
    }
}