using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLAN.Serializable
{
    [Serializable]
   public class ObjChat
    {
        public event EventHandler<Message> sendMessage;
        public List<Message> Messages =new List<Message>();

        public void SendMessage(Message message)
        {
            Messages.Add(message);
            sendMessage?.Invoke(null,message);
        }
    }
}
