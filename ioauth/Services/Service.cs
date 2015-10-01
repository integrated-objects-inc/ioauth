using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ioauth.Services
{
    public static class Service
    {
        public static io.Data.Return<bool> Start()
        {
            var result = ExpireSessions().AddTrace("Start()").AddTrace("ioauth.Services.Service");
            return result;
        }

        private static io.Data.Return<bool> ExpireSessions()
        {
            string sql = @"UPDATE tUserSessions
                       SET Active = 0, Expired = 1, LoggedOut = GETDATE()
                       WHERE (DATEDIFF(n, LastActivity, GETDATE()) > CAST ((SELECT SettingValue FROM tSettings WHERE (SettingKey = 1)) AS int)) AND (Active = 1)";

            var expireSessions = Databases.io_auth.Database.RunNonQuery(sql);

            return expireSessions;
        }
    }
}
