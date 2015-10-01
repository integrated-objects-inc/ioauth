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
    public class CredentialData : IUIControllerData
    {
        [DataMember()]
        public UIData<string> Email {get; set;}
        
        [DataMember()]
        public UIData<string> Password { get; set; }
        
        [DataMember()]
        public UIData<string> Token { get; set; }
        
        [DataMember()]
        public UIData<string> FirstName { get; set; }
        
        [DataMember()]
        public UIData<string> LastName { get; set; }

        [DataMember()]
        public UIData<string> UserAgent { get; set; }

        public bool _IsValid { get; private set; }

        public CredentialData()
        {
            InitProperties();
        }

        private void InitProperties()
        {
            if (Email == null)
                Email = new UIData<string>(Types.String);
            if (Password == null)
                Password = new UIData<string>(Types.String);
            if (Token == null)
                Token = new UIData<string>(Types.String);
            if (FirstName == null)
                FirstName = new UIData<string>(Types.String);
            if (LastName == null)
                LastName = new UIData<string>(Types.String);
            if (UserAgent == null)
                UserAgent = new UIData<string>(Types.String);

            Validate();
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            InitProperties();
        }

        public void Validate() 
        {
            try
            {
                Email.TrueType = Types.String;
                Password.TrueType = Types.String;
                Token.TrueType = Types.String;
                FirstName.TrueType = Types.String;
                LastName.TrueType = Types.String;

                Email.Validate(Validation.StringFormat.Email, "Required", 255);
                Password.Validate(Validation.StringFormat.Plain, "Required", 20);
                UserAgent.Validate(Validation.StringFormat.Plain, "Required", 500);
                
                // Default to valid
                _IsValid = io.Constants.YES;

                // Catch any InValid
                if (!Email.IsValid)
                    _IsValid = io.Constants.NO;

                if (!Password.IsValid)
                    _IsValid = io.Constants.NO;

                if (!UserAgent.IsValid)
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
    }
}
