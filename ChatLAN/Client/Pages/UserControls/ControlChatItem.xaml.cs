using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChatLAN.Client.Pages.Messager.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ControlChatItem.xaml
    /// </summary>
    public partial class ControlChatItem : UserControl
    {
        public ControlChatItem()
        {
            InitializeComponent();
            UriImage = "/Data/Image/B6pW1T4c8MQ.jpg";
            Login = "Lf";
            Text = "dfd";
        }

        public string UriImage
        {
            set => ImageBrush.ImageSource = new BitmapImage(new Uri(value, UriKind.Relative));
        }

        public string Login
        {
            get => TbLogin.Text;
            set => TbLogin.Text = value;
        }


        public string Text
        {
            get => TbMessage.Text;
            set => TbMessage.Text = value;
        }


        public ControlChatItem(string uriImage = null, string login = null, string text = null):this()
        {
            UriImage = uriImage;
            Login = login;
            Text = text;
        }

       
    }
}