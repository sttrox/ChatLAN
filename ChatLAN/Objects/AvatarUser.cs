using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLAN.Objects
{
    [Serializable]
    public class AvatarUser
    {
        public AvatarUser(byte[] image, string name)
        {
            Image = image;
            Name = name;
        }

        public byte[] Image;
        public string Name;

        public AvatarUser()
        {
        }
    }
}
