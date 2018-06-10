using System;

namespace ChatLAN.Objects
{
   
        [Serializable]
        public class Signature
        {
            public string Login;
            public string HashPass;

            public Signature(string login, string hashPass)
            {
                Login = login;
                HashPass = hashPass;
         
            }

            public Signature()
            {
            }
        }
    
}
