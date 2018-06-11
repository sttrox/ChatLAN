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
    /// Логика взаимодействия для ControlFile.xaml
    /// </summary>
    public partial class ControlFile : UserControl
    {
        public string FileName
        {
            set => TbName.Text = value;
            get => TbName.Text;
        }

        private bool _boolClick = false;

        public event EventHandler<string> Click;

        public ControlFile()
        {
            InitializeComponent();
        }

        public void ChangeFile(File file)
        {
            FileName = file.Name;
        }

        private void ControlFile_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_boolClick) Click?.Invoke(null, FileName);
        }

        private void ControlFile_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) =>
            _boolClick = true;


        private void ControlFile_OnMouseLeave(object sender, MouseEventArgs e) =>
            _boolClick = false;
    }
}