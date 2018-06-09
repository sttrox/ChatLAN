using System;
using System.Windows.Controls;
using System.Windows.Threading;
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

        public static void OpenPage(object obj)
        {
            
            _frame.Navigate(obj);
        }
        public static void OpenPage(string pathToPage)
        {
            Uri u = new Uri(pathToPage, UriKind.Relative);
            _frame.Invoke(() => _frame.Source = u);
        }

        public new static event EventHandler Close;

        private static MetroWindow _metroWindow;
        private static Frame _frame;
        public MainWindow()
        {
            InitializeComponent();
            _metroWindow = this;
            _frame = this.Frame;
            this.Closed += (sender, args) => Close?.Invoke(sender, null);
        }
    }
}