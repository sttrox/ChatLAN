using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using ChatLAN.Objects;

namespace ChatLAN.Client.Pages.Messager.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ControlListChat.xaml
    /// </summary>
    public partial class ControlListChat : UserControl
    {
      
        private List<ControlChatItem> _chatItems = new List<ControlChatItem>();

        public Dictionary<string, Message> ListChat
        {
            set
            {
                foreach (var chat in value)
                {
                    //todo Data
                    _chatItems.Add(new ControlChatItem(
                        $"{Environment.CurrentDirectory}/{Const.ClientPathToAvatarUsers}{chat.Key}.jpg",
                        chat.Key,
                        chat.Value.Text));
                }
            }
        }
        public void AddChat(Message message)
        {

            //_chatItems.Add(new ControlChatItem(message.UriAvatar));
        }
        public ControlListChat()
        {
            InitializeComponent();
            ClientCore.RefreshListChat += (sender, listChat) => ListChat = listChat;
            ListChat = ClientCore.Chats;
            ItemsControl.ItemsSource = _chatItems;
        }
    }
}