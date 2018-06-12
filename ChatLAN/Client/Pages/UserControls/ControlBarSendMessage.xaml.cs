using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using ChatLAN.Objects;
using Microsoft.Win32;

namespace ChatLAN.Client.Pages.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ControlBarSendMessage.xaml
    /// </summary>
    public partial class ControlBarSendMessage : UserControl
    {
        private Message _message = new Message();

        public ControlBarSendMessage()
        {
            InitializeComponent();
        }

        #region Property

        public string File
        {
            set => _message.File.Data = Util.ReadAllBytes(value);
        }

        public string Text
        {
            set => _message.Text = value;
        }

        #endregion


        #region Button

        private void SendMessage(object sender, RoutedEventArgs e)
        {
            ClientCore.SendMessage(_message);
            _message = new Message();
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Text = TbText.Text;
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if ((bool) dialog.ShowDialog())
                File = dialog.FileName;
        }

        #endregion
    }
}