using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brush = System.Drawing.Brush;

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
            set
            {
                if (File.Exists($"{Environment.CurrentDirectory}/{value}"))

                    Ellipse.Fill = new ImageBrush(new BitmapImage(new Uri(value, UriKind.Relative)));
//ImageBrush.ImageSource = new BitmapImage(new Uri(value, UriKind.Relative));
                else
                {
                    TbHideText.Text = new string(Login.ToCharArray(0, 2));
                    Ellipse.Fill = new VisualBrush(TbHideText)
                    {
                        Viewbox = Rect.Parse("0.1,0.1,0.8,0.8")
                    };
                }
            }
        }

        public string BackgroundRound
        {

            set
            {
                byte[] color = new byte[3];
                for (int i = 0; i < 3; i++)
                {
                    color[i]= Byte.Parse(value.Split(',')[i]);
                }
                TbHideText.Background = new SolidColorBrush(Color.FromRgb(color[0],color[1],color[2]));
        }
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


        public ControlChatItem(string uriImage = null, string login = null, string text = null) : this()
        {
            
            Login = login;
            UriImage = uriImage;
            Text = text;
        }
    }
}