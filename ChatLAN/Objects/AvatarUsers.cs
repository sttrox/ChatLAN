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

        public AvatarUsers(IEnumerable<ObjUser> listUsers)
        {
            foreach (var user in listUsers)
                AddImage(user.login);
        }

        public AvatarUsers()
        {
        }

        public void AddImage(string login)
        {
            byte[] bytes;
            string path = $"{Environment.CurrentDirectory}{Const.ServerPathToAvatarUsers}{login}.jpg";

            if (!System.IO.File.Exists(path))
                bytes = new byte[] {0};
            else
                using (FileStream fileStream = new FileStream(path, FileMode.Open))
                    bytes = Util.ReadAllByte(fileStream);

            ListAvatarUsers.Add(new AvatarUser(bytes, login));
        }

        public void AddAllImage(IEnumerable<string> listUsers)
        {
            foreach (var user in listUsers)
                AddImage(user);
        }

        public void AddAllImageAndClear(IEnumerable<string> listUsers)
        {
            ListAvatarUsers.Clear();
            AddAllImage(listUsers);
        }
    }
}