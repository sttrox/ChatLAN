using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using ChatLAN.Utils;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace ChatLAN.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageMain.xaml
    /// </summary>
    public partial class PageMain : Page
    {

        public PageMain()
        {
            InitializeComponent();
            Serialized.Clients.Add("81DC9BDB52D04DC20036DBD8313ED055", new ObjClient()
            {
                login = "Login",
                passHash = "81DC9BDB52D04DC20036DBD8313ED055"
            });
        }

        public bool ValidationAdress(string text)
        {
            foreach (var ch in text)
                if (!(char.IsDigit(ch) | ch == '.'))
                    return false;

            string[] digits = text.Split('.');
            if (digits.Length != 4) return false;

            foreach (var s in digits)
             if (s == string.Empty) return false;
            
            return true;
        }

        private void BtnStart_OnClick(object sender, RoutedEventArgs e)
        {
            //todo port
            FrameStatic.OpenPage("Pages/Server.xaml");
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

        private void BtnSign_OnClick(object sender, RoutedEventArgs e)
        {
            PanelOfClient.Visibility = Visibility.Collapsed;
            ProgressRing.Visibility = Visibility.Visible;

            if (!ValidationAdress(TbAdress.Text))
            {
                PrintAndReturnButton("Ошибка", "Не верный ip адрес");
                return;
            }

            Auth auth = new Auth();
            auth.Error += (o, s) => PrintAndReturnButton("Ошибка", s);
            auth.Join += (o, s) => PrintAndReturnButton("Juicy", "Auth OK");
            auth.SingIn(getIpAdress(TbAdress.Text), (int)NumPort.Value, TbLogin.Text, TbPass.Text);
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