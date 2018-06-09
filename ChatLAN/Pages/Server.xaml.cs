using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatLAN.Pages
{
    /// <summary>
    /// Логика взаимодействия для Server.xaml
    /// </summary>
    public partial class Server : Page
    {
        private static StackPanel _stackPanel;

        static ServerObject server; // сервер
        static Thread listenThread; // потока для прослушивания
        public Server()
        {
            InitializeComponent();
            _stackPanel = StackPanel;
      
        
            try
            {
                PrintText("Start Server");
                server = new ServerObject();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start(); //старт потока
            }
            catch (Exception ex)
            {
                server.Disconnect();
                PrintText(ex.Message);
                Console.WriteLine(ex.Message);
            }
        
            //new ChatLAN.ServerChat(new IPEndPoint(IPAddress.Any, 2345)).Start();
        }

        public static void PrintText(string text) =>
            _stackPanel.Dispatcher.Invoke(()=>_stackPanel.Children.Add(new TextBox() {Text = text}));

    }
}
