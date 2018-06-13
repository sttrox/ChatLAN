using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using ChatLAN.Client;
using ChatLAN.Client.Pages;
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

            ClientCore client = ClientCore.InicializeClient(getIpAdress(TbAdress.Text), (int) NumPort.Value);
            if (client == null) {MessageOnError(null,"Сервер не найден");return;}
            client.Error -= MessageOnError; //todo remove method
            client.Error += MessageOnError; //todo remove method
            client.Join -= OpenPageServer;
            client.Join += OpenPageServer;
            client.JoinServer(TbLogin.Text);

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
            this.Dispatcher.Invoke(() =>
            {
                ProgressRing.Visibility = Visibility.Collapsed;
                PanelOfClient.Visibility = Visibility.Visible;
            }, DispatcherPriority.Normal);
        }
    }
}