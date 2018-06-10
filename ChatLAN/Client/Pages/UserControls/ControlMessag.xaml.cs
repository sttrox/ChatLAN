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

namespace ChatLAN.Client.Pages.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ControlMessag.xaml
    /// </summary>
    public partial class ControlMessag : UserControl
    {
        public string Text
        {
            set => TextBlock.Text=value;
        }
        public ControlMessag()
        {
            InitializeComponent();
        }
    }
}
