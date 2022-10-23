using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiTranslate.Domain.Interfaces.Apis
{
    public interface IGoogleApi
    {
        UserCredential GetAccessTokenAccount();
        UserCredential RevokeTokenAccount();
    }
}
