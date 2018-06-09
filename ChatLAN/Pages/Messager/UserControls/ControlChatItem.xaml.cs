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

namespace ChatLAN.Pages.Messager.UsecControls
{
    /// <summary>
    /// Логика взаимодействия для ControlChatItem.xaml
    /// </summary>
    public partial class ControlChatItem : UserControl
    {
        public string SourceAvatar { get; set; }
        public string LastMesagge { get; set; }
        public string Login { get; set; }
        public ControlChatItem()
        {
            InitializeComponent();
        }
    }
}
