using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Runtime.Serialization;
using System.ServiceModel.Activation;
using io.Data;
using ioauth.Modules.Authentication;

namespace ioauth.Modules.Authentication.UIControllers
{
    [ServiceContract(Namespace = "")]
    public interface ILogin
    {
        [OperationContract()]
        [WebInvoke()]
        UIControllerData.Result<Modules.Authentication.DataContracts.CredentialData> Authenticate(Modules.Authentication.DataContracts.CredentialData credentialData);
   }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Login : ILogin
    {
        public UIControllerData.Result<Modules.Authentication.DataContracts.CredentialData> Authenticate(Modules.Authentication.DataContracts.CredentialData credentialData)
        {
            return Modules.Authentication.Authenticate.Login(credentialData).ToUIControllerResult();
        }
   }
}