using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Runtime.Serialization;
using System.ServiceModel.Activation;
using io.Data;

namespace ioauth.Modules.Authentication.DataContracts
{
    [Serializable()]
    [DataContract()]
    public class CreditialData : IUIControllerData
    {
        private UIData<string> _email = new UIData<string>(Types.String);
        private UIData<string> _password = new UIData<string>(Types.String);
        private UIData<string> _token = new UIData<string>(Types.String);
        private UIData<string> _firstName = new UIData<string>(Types.String);
        private UIData<string> _lastName = new UIData<string>(Types.String);

        public bool _IsValid { get; private set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            if (_email == null)
                _email = new UIData<string>(Types.String);
            if (_password == null)
                _password = new UIData<string>(Types.String);
            if (_token == null)
                _token = new UIData<string>(Types.String);
            if (_firstName == null)
                _firstName = new UIData<string>(Types.String);
            if (_lastName == null)
                _lastName = new UIData<string>(Types.String);

            Validate();
        }

        public void Validate() 
        {
            try
            {
                _email.TrueType = Types.String;
                _password.TrueType = Types.String;
                _token.TrueType = Types.String;
                _firstName.TrueType = Types.String;
                _lastName.TrueType = Types.String;

                _email.Validate(Validation.StringFormat.Email, "Required", 255);
                _password.Validate(Validation.StringFormat.Plain, "Required", 20);
                
                // Default to valid
                _IsValid = io.Constants.YES;

                // Catch any InValid
                if (!_email.IsValid)
                    _IsValid = io.Constants.NO;

                if (!_password.IsValid)
                    _IsValid = io.Constants.NO;
            }
            catch
            {
                _IsValid = io.Constants.NO;
            }
        }

        public bool IsValid()
        {
            return _IsValid;
        }

        public CreditialData()
        { }

        [DataMember()]
        public UIData<string> Email
        {
            get { return _email; }
            set { _email = value; }
        }

        [DataMember()]
        public UIData<string> Password
        {
            get { return _password; }
            set { _password = value; }
        }

        [DataMember()]
        public UIData<string> Token
        {
            get { return _token; }
            set { _token = value; }
        }

        [DataMember()]
        public UIData<string> FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        [DataMember()]
        public UIData<string> LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
    }
}
