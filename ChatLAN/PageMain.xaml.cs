using System;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Threading;
using ChatLAN.Client;
using ChatLAN.Client.Pages;
using ChatLAN.Server;

namespace ChatLAN
{
    public partial class PageMain
    {
        public PageMain()
        {
            InitializeComponent();
        }

        private void BtnJoinOnClick(object sender, RoutedEventArgs e)
        {
            PanelOfClient.Visibility = Visibility.Collapsed;
            ProgressRing.Visibility = Visibility.Visible;

            if (!ValidationAdress(TbAdress.Text))
            {
                PrintAndReturnButton("Ошибка", "Неверный IP адрес");
                return;
            }

            byte[] ipAdress = getIpAdress(TbAdress.Text);
            ClientCore client = ClientCore.InicializeClient(ipAdress, int.Parse(NumPort.Value.ToString()));
            if (client == null)
            {
                ClientCore.RemoveClient();
                MessageOnError(null, "Что-то пошло не так, попробуйте снова");
                return;
            }

            client.Error -= MessageOnError;
            client.Error += MessageOnError;
            client.Join -= OpenPageServer;
            client.Join += OpenPageServer;
            client.JoinServer(TbLogin.Text);
        }

        private void BtnStart_OnClick(object sender, RoutedEventArgs e)
        {
            ServerCore server = ServerCore.InicilizeServer(int.Parse(NumPort.Value.ToString()));
            server.Error -= MessageOnError;
            server.Error += MessageOnError;
            server.ServerStart -= OpenPageServer;
            server.ServerStart += OpenPageServer;
            server.Start();
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
            Dispatcher.Invoke(() =>
            {
                ProgressRing.Visibility = Visibility.Collapsed;
                PanelOfClient.Visibility = Visibility.Visible;
            }, DispatcherPriority.Normal);
        }

        private void OpenPageServer(object sender, EventArgs e)
        {
            MainWindow.OpenPage(new PageMessager());
        }

        private void OpenPageServer(object sender, TcpListener e)
        {
            MainWindow.OpenPage(new Server.Pages.Server());
        }

        private void MessageOnError(object sender, string e)
        {
            MainWindow.ShowMessage("Ошибка", e);
            Dispatcher.Invoke(() =>
            {
                ProgressRing.Visibility = Visibility.Collapsed;
                PanelOfClient.Visibility = Visibility.Visible;
            }, DispatcherPriority.Normal);
        }
    }
}