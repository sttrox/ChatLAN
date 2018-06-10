using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ChatLAN.Client;
using ChatLAN.Objects;
using ChatLAN.Server;
using ChatLAN.Server.Utils;

namespace ChatLAN
{
    /// <summary>
    /// Логика взаимодействия для PageMain.xaml
    /// </summary>
    public partial class PageMain : Page
    {
        public PageMain()
        {
            InitializeComponent();
            DataClients.Clients.Add("81DC9BDB52D04DC20036DBD8313ED055", new ObjClient()
            {
                login = "Login",
                passHash = "81DC9BDB52D04DC20036DBD8313ED055"
            });
        }

        private void BtnSign_OnClick(object sender, RoutedEventArgs e)
        {
            PanelOfClient.Visibility = Visibility.Collapsed;
            ProgressRing.Visibility = Visibility.Visible;

            if (!ValidationAdress(TbAdress.Text))
            {
                PrintAndReturnButton("Ошибка", "Не верный ip адрес");
                return;
            }

            ClientCore.Auth auth = new ClientCore.Auth();
            auth.Error += (o, s) => PrintAndReturnButton("Ошибка", s); //todo remove method
            //auth.Error -= (o, s) => PrintAndReturnButton("Ошибка", s);
            auth.Join += (o, s) =>
                MainWindow.OpenPage("Client/Pages/PageMessager.xaml");
            //PrintAndReturnButton("Juicy", "Auth OK");
            auth.SingIn(getIpAdress(TbAdress.Text), (int) NumPort.Value, TbLogin.Text, TbPass.Text);
        }

        private void BtnStart_OnClick(object sender, RoutedEventArgs e)
        {
            ServerCore server = ServerCore.InicilizeServer((int) NumPort.Value);
            server.Error -= MessageOnError;
            server.Error += MessageOnError;
            server.ServerStart -= OpenPageServer;
            server.ServerStart += OpenPageServer;
            server.Start();
        }

        private void OpenPageServer(object sender, TcpListener e)
        {
            MainWindow.OpenPage(new Server.Pages.Server());
        }

        private void MessageOnError(object sender, string e)
        {
            MainWindow.ShowMessage("Ошибка", e);
        }

        private bool ValidationAdress(string text)
        {
            foreach (var ch in text)
                if (!(char.IsDigit(ch) | ch == '.'))
                    return false;

            string[] digits = text.Split('.');
            if (digits.Length != 4) return false;

            foreach (var s in digits)
                if (s == string.Empty)
                    return false;

            return true;
        }


        private byte[] getIpAdress(string ipAdress)
        {
            byte[] bytes = new byte[4];
            byte inc = 0;
            foreach (var bit in ipAdress.Split('.'))
            {
                bytes[inc] = byte.Parse(bit);
                inc++;
            }

            return bytes;
        }


        private void PrintAndReturnButton(string title, string message)
        {
            MainWindow.ShowMessage(title, message);
            this.Dispatcher.Invoke(() =>
            {
                ProgressRing.Visibility = Visibility.Collapsed;
                PanelOfClient.Visibility = Visibility.Visible;
            }, DispatcherPriority.Normal);
        }
    }
}