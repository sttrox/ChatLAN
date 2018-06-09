using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ChatLAN.Utils
{
    public static class StreamOfClient
    {
        public static void Join(NetworkStream stream, Signature clien)
        {

            Util.SerializeObject(clien, stream);
           // stream.Write(Encoding.UTF8.GetBytes("\r\n"),0, Encoding.UTF8.GetBytes("\r\n").Length);

        }

    }
}