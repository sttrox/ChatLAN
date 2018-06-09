using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace ChatLAN.Pages
{
    public static class  FrameStatic
    {
        public static Frame Frame
        {
            get => _frame;
            set => _frame = value;
        } 
        
        private static Frame _frame;

        public static void OpenPage(string pathToPage)
        {
            Uri u = new Uri(pathToPage, UriKind.Relative);
            _frame.Source = u;
        }
    }
}
