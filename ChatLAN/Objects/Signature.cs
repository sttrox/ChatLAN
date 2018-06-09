using System;

namespace ChatLAN.Objects
{
   
        [Serializable]
        public class Signature
        {
            public string Login;
            public string HashPass;
            public Util.TypeSoketMessage TypeSoketMessage;

            public Signature(string login, string hashPass, Util.TypeSoketMessage typeSoketMessage)
            {
                Login = login;
                HashPass = hashPass;
                TypeSoketMessage = typeSoketMessage;
            }

            public Signature()
            {
            }
        }
    
}
