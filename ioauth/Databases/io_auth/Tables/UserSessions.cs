using System;
using System.Text;
using System.Linq;
using System.Data;
using io.Data;
using io;

namespace ioauth.Databases.io_auth.Tables
{
    [System.ComponentModel.DesignerCategory("")]
    public class UserSessions : View
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

            private UserSession DefaultValues(UserSession row)
            {
                return row;
            }

            public static io.Data.Return<UserSessions.UserSession> GetObjectWithKey(int key)
            { 
                using(var objects = new UserSessions(key))
                {
                    if (objects.QueryResult.Success && objects.Count != 0)
                        return new io.Data.Return<UserSessions.UserSession>(io.Constants.SUCCESS,"","", objects[0]);
                    else
                        return new io.Data.Return<UserSessions.UserSession>(io.Constants.FAILURE, "", "", null);
                }
            }

            private void Init()
            {
                _view = @"tUserSessions";
                _source = @"tUserSessions";
                _id = @"UserSessionKey";
                _ioSystem = ioauth.Common.IOSystem;
                _connectionIndex = Convert.ToInt32(Databases.io_auth.Database.ActiveConnection());

                base.Query();
            }

            public UserSessions(int userSessionKey, params Fields[] selectFields)
            {
                _where = "UserSessionKey = " + userSessionKey;
                _selectFields = selectFields.ToArray();
                Init();
            }

            public UserSessions(string where, params Fields[] selectFields)
            {
                _where = where;
                _selectFields = selectFields.ToArray();
                Init();
            }

            public UserSessions(string where, string orderBy, params Fields[] selectFields)
            {
                _where = where;
                _orderBy = orderBy;
                _selectFields = selectFields.ToArray();
                Init();
            }

            public UserSessions(int top, string where, string orderBy, params Fields[] selectFields)
            {
                _top = top;
                _where = where;
                _orderBy = orderBy;
                _selectFields = selectFields.ToArray();
                Init();
            }

            public UserSession this[int index]
            {
                get { return (UserSession)this.Rows[index]; }
            }

            protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
            {
                return new UserSession(builder, this);
            }

            protected override Type GetRowType()
            {
                return typeof(UserSession);
            }

            public UserSession NewUserSession()
            {
                UserSession row = (UserSession)this.NewRow();
                row = DefaultValues(row);
                this.Rows.Add(row);
                return row;
            }

            public class UserSession : io.Data.ViewRow
            {

                internal UserSession(DataRowBuilder rb, IView view)
                    : base(rb, view)
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
                    set { this.SetDBBoolean(Fields.Expired.ToString(), value); }
                }

                public string LastActivity
                {
                    get { return this.DBDate(Fields.LastActivity.ToString()); }
                    set { this.SetDBDate(Fields.LastActivity.ToString(), Convert.ToDateTime(value)); }
                }

                public string LoggedOut
                {
                    get { return this.DBDate(Fields.LoggedOut.ToString()); }
                    set { this.SetDBDate(Fields.LoggedOut.ToString(), Convert.ToDateTime(value)); }
                }

                public string Token
                {
                    get { return this.DBString(Fields.Token.ToString()); }
                    set { this.SetDBString(Fields.Token.ToString(), value, TOKEN_MAXLENGTH); }
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
