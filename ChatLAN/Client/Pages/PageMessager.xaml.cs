using System;
using System.Collections.Generic;
using System.Linq;
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
using ChatLAN.Client.Pages.UserControls;
using MahApps.Metro.Controls;

namespace ChatLAN.Client.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageMessager.xaml
    /// </summary>
    public partial class PageMessager : Page
    {
        public PageMessager()
        {
            InitializeComponent();
            ClientCore clientCore = ClientCore.GetCore();
            clientCore.AddMessage += (sender, message) =>
                PanelMessage.Invoke(() =>
                {
                    UserControl element;
                    if (message.Name == ClientCore._nameUser)
                        element = new ControlMessagRevers(message){Foreground = new SolidColorBrush(Color.FromArgb(255,255,218,187)) };
                    else
                        element = new ControlMessag(message){Foreground =Foreground = Brushes.LightBlue };
                    element.Margin = new Thickness(5);
               
                    PanelMessage.Children.Add(element);
                });

            new Thread(() => clientCore.ReceiveMessage()).Start();
        }
    }
}