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

namespace ChatLAN.Pages.Messager
{
    /// <summary>
    /// Логика взаимодействия для ControlListChat.xaml
    /// </summary>
    public partial class ControlListChat : UserControl
    {
        public void AddChat(UIElement element)
        {
            ItemsControl.Items.Add(element);
        }

        public void RemoveChat(UIElement element)
        {
            ItemsControl.Items.Remove(element);
        }

        public ControlListChat()
        {
            InitializeComponent();
        }
    }
}