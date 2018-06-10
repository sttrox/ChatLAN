using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLAN.Objects
{
    [Serializable]
    public class TypeMessage<TObject>
    {
        public TObject TObj;
        public Util.TypeSoketMessage TypeSoketMessage;

        public TypeMessage(TObject obj, Util.TypeSoketMessage typeSoketMessage)
        {
            TObj = obj;
            TypeSoketMessage = typeSoketMessage;
        }

        public TypeMessage()
        {
        }
    }
}
