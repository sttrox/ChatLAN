using System;
using System.Threading;
using System.Windows.Controls;

namespace ChatLAN.Server.Pages
{
    /// <summary>
    /// Логика взаимодействия для Server.xaml
    /// </summary>
    public partial class Server : Page
    {
        private static StackPanel _stackPanel;

        public Server()
        {
            InitializeComponent();
            _stackPanel = StackPanel;
        }

        public static void PrintText(string text) =>
            _stackPanel.Dispatcher.Invoke(() => _stackPanel.Children.Add(new TextBox() {Text = text}));
    }
}