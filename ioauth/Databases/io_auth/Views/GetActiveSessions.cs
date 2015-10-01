using System;
using System.Text;
using System.Linq;
using System.Data;
using io.Data;
using io;

namespace ioauth.Databases.io_auth.Views
{
    [System.ComponentModel.DesignerCategory("")]
    public class GetActiveSessions : View
    {
        public enum Fields
            {
                 Active = 0,
                 DateCreated = 1,
                 EntityContactKey = 2,
                 Expired = 3,
                 LastActivity = 4,
                 LoggedOut = 5,
                 Token = 6,
                 UserAgent = 7,
                 UserSessionKey = 8
            }

            public const Int16 TOKEN_MAXLENGTH = 36;
            public const Int16 USERAGENT_MAXLENGTH = 500;

            private ActiveSession DefaultValues(ActiveSession row)
            {
                return row;
            }

            private string _userAgent = "";
            private int _entityContactKey = 0;

            private void Init()
            {
                var sql = new StringBuilder(@"SELECT UserSessionKey, EntityContactKey, Token, Active, UserAgent, LastActivity, LoggedOut, Expired, DateCreated FROM dbo.tUserSessions WHERE (UserAgent = @USERAGENT) AND (Active = 1) AND (EntityContactKey = @ENTITYCONTACTKEY)");

                base.AddParameterValue("@USERAGENT", _userAgent);
                base.AddParameterValue("@ENTITYCONTACTKEY", _entityContactKey.ToString());

                _view = sql.ToString();
                _source = "tUserSessions";
                _id = "UserSessionKey";
                _ioSystem = ioauth.Common.IOSystem;
                _connectionIndex = Convert.ToInt32(Databases.io_auth.Database.ActiveConnection());
                base.Query();
            }

            public GetActiveSessions(string userAgent, int entityContactKey)
            {
                _userAgent = userAgent;
                _entityContactKey = entityContactKey;
                Init(); 
            }

            public ActiveSession this[int index]
            {
                get { return (ActiveSession)this.Rows[index]; }
            }

            protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
            {
                return new ActiveSession(builder, this);
            }

            protected override Type GetRowType()
            {
                return typeof(ActiveSession);
            }

            public ActiveSession NewActiveSession()
            {
                ActiveSession row = (ActiveSession)this.NewRow();
                row = DefaultValues(row);
                this.Rows.Add(row);
                return row;
            }

            public class ActiveSession : io.Data.ViewRow
            {

                internal ActiveSession(DataRowBuilder rb, IView view) : base(rb, view)
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
                    set { this.SetDBInteger(Fields.EntityContactKey.ToString(), value); }
                }

                public bool Expired
                {
                    get { return this.DBBoolean(Fields.Expired.ToString()); }
                }

                public string LastActivity
                {
                    get { return this.DBDate(Fields.LastActivity.ToString()); }
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
                    set { this.SetDBString(Fields.UserAgent.ToString(), value, USERAGENT_MAXLENGTH); }
                }

                public int UserSessionKey
                {
                    get { return this.DBInteger(Fields.UserSessionKey.ToString()); }
                }
            }
        }
    }
