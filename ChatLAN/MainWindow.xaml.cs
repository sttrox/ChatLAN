using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;
using ChatLAN.Annotations;
using ChatLAN.Pages;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace ChatLAN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public static void ShowMessage(string title, string message)
        {
            _metroWindow.Dispatcher.Invoke(() =>
            {
                _metroWindow.ShowMessageAsync(title, message);
                _metroWindow.Show();
            }, DispatcherPriority.Normal);
        }

        public static event EventHandler Close;

        private static MetroWindow _metroWindow;

        public MainWindow()
        {
            InitializeComponent();
            _metroWindow = this;
            FrameStatic.Frame = this.Frame;
            this.Closed += (sender, args) => Close?.Invoke(sender, null);
        }
    }
}