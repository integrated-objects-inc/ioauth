using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioauth.Modules.Authentication
{
    public class UserSession
    {
        public int UserSessionKey { get; private set; }
        public int EntityContactKey { get; private set; }
        public int EntityLocationKey {get; private set; }
        public int EntityKey { get; private set; }
        
        internal UserSession(int userSessionKey, int entityContactKey, int entityLocationKey, int entityKey)
        {
            UserSessionKey = userSessionKey;
            EntityContactKey = entityContactKey;
            EntityLocationKey = entityLocationKey;
            EntityKey = entityKey;
        }
    }
}