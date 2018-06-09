using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ChatLAN.Serializable;

namespace ChatLAN.Client.Pages.Messager.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ControlListChat.xaml
    /// </summary>
    public partial class ControlListChat : UserControl
    {
      
        private List<ControlChatItem> _chatItems = new List<ControlChatItem>();

        public void AddChat(Message message)
        {

           // _chatItems.Add(new ControlChatItem(message.UriAvatar));
        }
        public ControlListChat()
        {
            InitializeComponent();
            ItemsControl.ItemsSource = ClientCore.Chats;
        }
    }
}