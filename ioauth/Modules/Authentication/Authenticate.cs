using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using io.Data;
using iocontacts.Databases.io_contacts.Views;
using ioauth.Databases.io_auth.Tables;
using ioauth.Databases.io_auth.Views;

namespace ioauth.Modules.Authentication
{
    public static class Authenticate
    {
        internal static io.Data.Return<DataContracts.CredentialData> Login(DataContracts.CredentialData credentialData)
        {
            var functionName = "ioauth.Modules.Authentication.Authenticate.Login()";

            if (!credentialData.IsValid())
                return new io.Data.Return<DataContracts.CredentialData>(io.Constants.FAILURE, @"Invalid Login Id or Password", "", credentialData);

            using (var users = new iocontacts.Databases.io_contacts.Views.DataContracts.Login.GetUser(credentialData.Email.Value))
            {
                if (users.QueryResult.Failed)
                    users.QueryResult.LogResult(1, 1, 1, 0, 101, functionName);

                if (users.Count ==0)
                    return new io.Data.Return<DataContracts.CredentialData>(io.Constants.FAILURE, @"Invalid Login Id or Password", "", credentialData);

                if (users[0].Password != GenerateHash(credentialData.Password.Value, users[0].EntityContactKey.ToString()))
                    return new io.Data.Return<DataContracts.CredentialData>(io.Constants.FAILURE, @"Invalid Login Id or Password", "", credentialData);
                
                if (users[0].Active == false)
                    return new io.Data.Return<DataContracts.CredentialData>(io.Constants.FAILURE, @"Account not active", "", credentialData);
                
                using (var sessions = new GetActiveSessions(credentialData.UserAgent.Value, users[0].EntityContactKey))
                {
                    foreach (GetActiveSessions.ActiveSession session in sessions)
                        session.Active = false;

                    credentialData.FirstName.Value = users[0].FirstName;
                    credentialData.LastName.Value = users[0].LastName;
                    credentialData.Email.Value = users[0].Email;

                    GetActiveSessions.ActiveSession newSession = null;
                    newSession = sessions.NewActiveSession();
                    newSession.EntityContactKey = users[0].EntityContactKey;
                    newSession.Active = true;
                    newSession.LastActivity = DateTime.Now.ToString();
                    newSession.UserAgent = credentialData.UserAgent.Value.ToString();

                    io.Data.Return<bool> updateResult = sessions.Update();

                    if (updateResult.Failed)
                        return new io.Data.Return<DataContracts.CredentialData>(io.Constants.FAILURE, "Failed to create session", updateResult.Message, credentialData).LogResult(1, 1, 1, 0, 100, functionName);

                    var newSessions = Databases.io_auth.Tables.UserSessions.GetObjectWithKey(newSession.UserSessionKey);
                    if (newSessions.Failed)
                        return new io.Data.Return<DataContracts.CredentialData>(io.Constants.FAILURE, "Failed to create session", updateResult.Message, credentialData).LogResult(1, 1, 1, 0, 100, functionName);

                    credentialData.Token.Value = newSessions.Value.Token;
                    credentialData.Token.IsValid = true;

                    return new io.Data.Return<DataContracts.CredentialData>(io.Constants.SUCCESS, "", "", credentialData);
                }
            }
        }

        public static io.Data.Return<UserSession> UserSession(string token)
        {
            UserSession usersSession = null;
            Guid tokenGUID;

            if (!Guid.TryParse(token, out tokenGUID))
                return new Return<UserSession>(io.Constants.FAILURE, Constants.CONST_NOTLOGGEDIN, "", null);
            
            using (var sessions = new Databases.io_auth.Views.GetSession(token))
            {
                if (sessions.QueryResult.Failed || sessions.Count == 0)
                {
                    usersSession = new UserSession(0, 0, 0, 0);
                    return new Return<UserSession>(io.Constants.FAILURE, Constants.CONST_NOTLOGGEDIN, "", null);
                }
                
                sessions[0].Active = true;
                sessions[0].LastActivity = DateTime.Now.ToString();

                io.Data.Return<bool> updateResult = sessions[0].UpdateRow();

                if (updateResult.Success)
                {
                    usersSession = new UserSession(sessions[0].UserSessionKey, sessions[0].EntityContactKey, sessions[0].EntityLocationKey, sessions[0].EntityKey);
                    return new io.Data.Return<UserSession>(io.Constants.SUCCESS, "", "", usersSession);
                }
                else
                {
                    usersSession = new UserSession(0, 0, 0, 0);
                    return new Return<UserSession>(io.Constants.FAILURE, Constants.CONST_SESSIONEXPIRED, "", usersSession);
                }
            }
        }

        private static string GenerateHash(string value, string salt)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(salt + value);
            data = System.Security.Cryptography.MD5.Create().ComputeHash(data);
            return Convert.ToBase64String(data);
        }
    }
}
