using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatLAN.Objects
{
    [Serializable]
    public class AvatarUsers
    {
        public List<AvatarUser> ListAvatarUsers = new List<AvatarUser>();

        public AvatarUsers(IEnumerable<string> listUsers)
        {
            AddAllImage(listUsers);
        }

        public AvatarUsers()
        {
        }

        public void AddAllImage(IEnumerable<string> listUsers)
        {
            foreach (var user in listUsers)
            {
                byte[] bytes;
                string path =  $"{Environment.CurrentDirectory}{Const.ServerPathToAvatarUsers}{user}.jpg";

                if (!System.IO.File.Exists(path))
                    bytes = new byte[] {0};
                else
                    using (FileStream fileStream = new FileStream(path, FileMode.Open))
                        bytes = Util.ReadAllByte(fileStream);


                ListAvatarUsers.Add(new AvatarUser(bytes, user));
            }
        }

        public void AddAllImageAndClear(IEnumerable<string> listUsers)
        {
            ListAvatarUsers.Clear();
            AddAllImage(listUsers);
        }
    }
}