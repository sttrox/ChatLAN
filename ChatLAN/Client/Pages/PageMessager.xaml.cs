﻿using System;
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
                PanelMessage.Invoke(() => { PanelMessage.Children.Add(new ControlMessag(message)
                    { Margin = new Thickness(5), Foreground = Brushes.LightBlue}); });

            new Thread(() => clientCore.ReceiveMessage()).Start();
        }
    }
}