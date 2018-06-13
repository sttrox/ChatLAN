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

namespace ChatLAN.Client.Pages.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ControlMessagRevers.xaml
    /// </summary>
    public partial class ControlMessagRevers : UserControl
    {
        public ControlMessagRevers(Message message)
        {
            InitializeComponent();
            Text = message.Text;
            File = message.File;
        }
        private File File
        {
            set
            {
                if (value.Data == null) return;
                ControlFile.Visibility = Visibility;
                ControlFile.FileName = value?.Name;
            }
        }

        private string Text
        {
            set => TbText.Text = value;
        }
    }
}
