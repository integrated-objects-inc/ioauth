using System;
using System.Text;
using System.Linq;
using System.Data;
using io.Data;
using io;

namespace ioauth.Databases.io_auth.Views
{
    [System.ComponentModel.DesignerCategory("")]
    public class GetSession : View
    {
        public enum Fields
            {
                 Active = 0,
                 DateCreated = 1,
                 EntityContactKey = 2,
                 EntityKey = 3,
                 EntityLocationKey = 4,
                 Expired = 5,
                 LastActivity = 6,
                 LoggedOut = 7,
                 Token = 8,
                 UserAgent = 9,
                 UserSessionKey = 10
            }

            public const Int16 TOKEN_MAXLENGTH = 36;
            public const Int16 USERAGENT_MAXLENGTH = 500;

            private Session DefaultValues(Session row)
            {
                return row;
            }

            private string _token = "";

            private void Init()
            {
                var sql = new StringBuilder(@"SELECT dbo.tUserSessions.UserSessionKey, dbo.tUserSessions.EntityContactKey, dbo.v_contacts_tEntityContacts.EntityLocationKey, dbo.tUserSessions.Token, dbo.tUserSessions.Active, dbo.tUserSessions.UserAgent, dbo.tUserSessions.LastActivity, dbo.tUserSessions.LoggedOut, dbo.tUserSessions.Expired, dbo.tUserSessions.DateCreated, dbo.v_contacts_tEntityContacts.EntityKey FROM dbo.tUserSessions INNER JOIN dbo.v_contacts_tEntityContacts ON dbo.tUserSessions.EntityContactKey = dbo.v_contacts_tEntityContacts.EntityContactKey WHERE (dbo.tUserSessions.Active = 1) AND (dbo.tUserSessions.Token = @TOKEN)");

                base.AddParameterValue("@TOKEN", _token);

                _view = sql.ToString();
                _source = "tUserSessions";
                _id = "UserSessionKey";
                _ioSystem = ioauth.Common.IOSystem;
                _connectionIndex = Convert.ToInt32(Databases.io_auth.Database.ActiveConnection());
                base.Query();
            }

            public GetSession(string token)
            {
                _token = token;
                Init(); 
            }

            public Session this[int index]
            {
                get { return (Session)this.Rows[index]; }
            }

            protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
            {
                return new Session(builder, this);
            }

            protected override Type GetRowType()
            {
                return typeof(Session);
            }

            public Session NewSession()
            {
                Session row = (Session)this.NewRow();
                row = DefaultValues(row);
                this.Rows.Add(row);
                return row;
            }

            public class Session : io.Data.ViewRow
            {

                internal Session(DataRowBuilder rb, IView view) : base(rb, view)
                {
                }

                public bool Active
                {
                    get { return this.DBBoolean(Fields.Active.ToString()); }
                    set { this.SetDBBoolean(Fields.Active.ToString(), value); }
                }

                public string DateCreated
                {
                    get { return this.DBDate(Fields.DateCreated.ToString()); }
                }

                public int EntityContactKey
                {
                    get { return this.DBInteger(Fields.EntityContactKey.ToString()); }
                }

                public int EntityKey
                {
                    get { return this.DBInteger(Fields.EntityKey.ToString()); }
                }

                public int EntityLocationKey
                {
                    get { return this.DBInteger(Fields.EntityLocationKey.ToString()); }
                }

                public bool Expired
                {
                    get { return this.DBBoolean(Fields.Expired.ToString()); }
                }

                public string LastActivity
                {
                    get { return this.DBDate(Fields.LastActivity.ToString(), true); }
                    set { this.SetDBDate(Fields.LastActivity.ToString(), Convert.ToDateTime(value)); }
                }

                public string LoggedOut
                {
                    get { return this.DBDate(Fields.LoggedOut.ToString()); }
                }

                public string Token
                {
                    get { return this.DBString(Fields.Token.ToString()); }
                }

                public string UserAgent
                {
                    get { return this.DBString(Fields.UserAgent.ToString()); }
                }

                public int UserSessionKey
                {
                    get { return this.DBInteger(Fields.UserSessionKey.ToString()); }
                }
            }
        }
    }
